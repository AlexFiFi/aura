// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.Constants;
using Common.Tools;
using CSScriptLibrary;
using World.Tools;
using World.World;
using Common.Data;

namespace World.Scripting
{
	public class NPCManager
	{
		public readonly static NPCManager Instance = new NPCManager();
		static NPCManager() { }
		private NPCManager() { }

		public readonly string NpcFolder = Path.Combine(WorldConf.ScriptPath, "npc");
		public readonly string NpcCompiled = Path.Combine(Path.Combine(WorldConf.ScriptPath, "npc"), "compiled");

		public void LoadNPCs()
		{
			
			loadDefs(NpcFolder);//The first thing to do is to load our DEF files, as some of the NPCs derive from them.

			Logger.Info("Loading NPCs...");

			try
			{
				var listPath = Path.Combine(NpcFolder ,"npclist.txt");
				List<string> npcListContents = new List<string>();

				if (File.Exists(listPath))
				{
					FileReader.DoEach(listPath, (string line) =>
					{
						npcListContents.Add(Path.Combine(WorldConf.ScriptPath, "npc", line));
					});
				}
				else
				{
					Logger.Warning("NPC list not found!");
				}

				_loadedNpcs = 0;

				List<Task> loadNpcTaskList = new List<Task>();

				List<string> npcsToLoad;
				if (WorldConf.UseNpcListAsExclude)
					npcsToLoad = Directory.GetFiles(Path.Combine(WorldConf.ScriptPath, "npc"), "*.cs", SearchOption.AllDirectories).Except(npcListContents).ToList();
				else
					npcsToLoad = npcListContents;

				foreach (string npc in npcsToLoad)
					loadNpcTaskList.Add(Task.Factory.StartNew(new Action<object>(loadNPC), npc));

				Task.WaitAll(loadNpcTaskList.ToArray());

			}
			catch (Exception ex)
			{
				Logger.Exception(ex, null, true);
			}

			Logger.ClearLine();
			Logger.Info("Loaded " + _loadedNpcs + " NPCs");
		}

		private void loadDefs(string npcFolder)
		{
			_loadedDefs = 0;
			Logger.Info("Loading NPC defs...");
			string[] defs = Directory.GetFiles(npcFolder, "*.def", SearchOption.AllDirectories);
			Task[] loadDefTasks = new Task[defs.Length];
			for (int i = 0; i < defs.Length; i++)
				loadDefTasks[i] = Task.Factory.StartNew(new Action<object>(loadDef), defs[i], TaskCreationOptions.PreferFairness);

			Task.WaitAll(loadDefTasks);
			Logger.ClearLine();
			Logger.Info("Loaded " + _loadedDefs + " NPC defs");

		}

		private int _loadedNpcs, _loadedDefs;

		private void loadDef(object scriptPath) //Wrapper for Action<object>
		{
			loadDef(scriptPath as string);
		}

		private void loadDef(string scriptPath)
		{
			try
			{
				Assembly asm = loadScript(scriptPath);
				System.Threading.Interlocked.Increment(ref _loadedDefs);

				Logger.Info(Logger.ClearLineString + "Loaded " + _loadedDefs + " NPC defs", false);

			}
			catch (Exception ex)
			{
				try
				{
					File.Decrypt(Path.ChangeExtension(scriptPath, ".compiled"));
				}
				catch { }
				Logger.Error("While processing: " + scriptPath + " .... " + ex.Message);
			}

		}

		private void loadNPC(object scriptPath) //Wrapper for Action<object>
		{
			loadNPC(scriptPath as string);
		}

		private void loadNPC(string scriptPath)
		{
			//Logger.Info("Loading " + scriptPath + " ..."); //Bad idea to enable this for anything but debugging
			try
			{
				Assembly asm = loadScript(scriptPath);

				NPCScript script = (NPCScript)asm.CreateObject("*");
				var npc = new MabiNPC();
				npc.Script = script;
				npc.ScriptPath = scriptPath;
				script.NPC = npc;
				script.OnLoad();
				npc.LoadDefault();

				WorldManager.Instance.AddCreature(npc);

				System.Threading.Interlocked.Increment(ref _loadedNpcs);

				Logger.Info(Logger.ClearLineString + "Loaded " + _loadedNpcs + " NPCs", false);

			}
			catch (Exception ex)
			{
				try
				{
					File.Delete(Path.ChangeExtension(scriptPath.Replace(NpcFolder, NpcCompiled), ".compiled"));
				}
				catch { }
				Logger.Error("While processing: " + scriptPath + " .... " + ex.Message);
			}
		}

		private Assembly loadScript(string scriptPath)
		{
			Assembly asm;
			string compiledScriptPath = Path.ChangeExtension(scriptPath.Replace(NpcFolder, NpcCompiled), ".compiled");
			if (!File.Exists(compiledScriptPath) || File.GetLastWriteTime(scriptPath) > File.GetLastWriteTime(compiledScriptPath))
			{
				asm = CSScript.LoadCode(File.ReadAllText(scriptPath));
				try
				{
					if (File.Exists(compiledScriptPath))
						File.Delete(compiledScriptPath);
					else
						Directory.CreateDirectory(Path.GetDirectoryName(compiledScriptPath));

					File.Copy(asm.Location, compiledScriptPath);
				}
				catch (Exception e)
				{
					Logger.Warning(e.Message);
				}
			}
			else
			{
				asm = Assembly.LoadFrom(compiledScriptPath);
			}
			
			return asm;
		}

		public void LoadSpawns()
		{
			int count = 0;
			foreach (var spawnInfo in MabiData.SpawnDb.Entries)
			{
				count += this.Spawn(spawnInfo);
			}

			Logger.Info("Spawned " + count.ToString() + " monsters.");
		}

		public int Spawn(SpawnInfo info, int amount = 0)
		{
			var result = 0;

			var rand = RandomProvider.Get();

			var monsterInfo = MabiData.MonsterDb.Find(info.MonsterId);
			if (monsterInfo == null)
			{
				Logger.Warning("Monster not found: " + info.MonsterId.ToString());
				return 0;
			}

			var aiFilePath = Path.Combine(WorldConf.ScriptPath, "ai", monsterInfo.AI + ".cs");
			if (!File.Exists(aiFilePath))
			{
				Logger.Warning("AI script '" + monsterInfo.AI + ".cs' couldn't be found.");
				aiFilePath = null;
			}

			if (amount == 0)
				amount = info.Amount;

			for (int i = 0; i < amount; ++i)
			{
				var monster = new MabiNPC();
				monster.SpawnId = info.Id;
				monster.Name = monsterInfo.Name;
				monster.SetLocation(info.Region, (uint)rand.Next(info.X1, info.X2), (uint)rand.Next(info.Y1, info.Y2));
				monster.Height = monsterInfo.Size;
				monster.LifeMaxBase = monsterInfo.Life;
				monster.Life = monsterInfo.Life;
				monster.Race = monsterInfo.Race;
				monster.BattleExp = monsterInfo.Exp;
				monster.Direction = (byte)rand.Next(256);
				monster.Status &= ~CreatureStates.GoodNpc; // Use race default?
				monster.GoldMin = monsterInfo.GoldMin;
				monster.GoldMax = monsterInfo.GoldMax;
				monster.Drops = monsterInfo.Drops;

				foreach (var skill in monsterInfo.Skills)
				{
					monster.Skills.Add(new Common.World.MabiSkill(skill.SkillId, skill.Rank, monster.Race));
				}

				monster.LoadDefault();

				if (aiFilePath != null)
				{
					//monster.AIScript = (AIScript)CSScript.Load(aiFilePath).CreateObject("*");
					monster.AIScript = (AIScript)CSScript.LoadCode(File.ReadAllText(aiFilePath)).CreateObject("*");
					monster.AIScript.Creature = monster;
					monster.AIScript.OnLoad();
				}

				WorldManager.Instance.AddCreature(monster);
				result++;
			}

			return result;
		}

		public void Spawn(uint spawnId, int amount = 0)
		{
			var info = MabiData.SpawnDb.Find(spawnId);
			if (info == null)
			{
				Logger.Warning("Unknown spawn Id '" + spawnId.ToString() + "'.");
				return;
			}

			this.Spawn(info, amount);
		}
	}
}
