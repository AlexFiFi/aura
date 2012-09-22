// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Constants;
using Common.Data;
using Common.Tools;
using CSScriptLibrary;
using World.Tools;
using World.World;

namespace World.Scripting
{
	public class NPCManager
	{
		public readonly static NPCManager Instance = new NPCManager();
		static NPCManager() { }
		private NPCManager() { }

		public readonly string NpcFolder = Path.Combine(WorldConf.ScriptPath, "npc");
		public readonly string NpcCompiled = Path.Combine(Path.Combine(WorldConf.ScriptPath, "npc"), "compiled");

		private int _loadedNpcs, _loadedDefs, _cached;

		public void LoadNPCs()
		{
			Logger.Info("Loading NPCs...");

			_loadedDefs = _loadedNpcs = _cached = 0;

			// The first thing to do is to load our DEF files,
			// as some of the NPCs derive from them.
			// Disabled for now while not needed.
			//var defFiles = Directory.GetFiles(this.NpcFolder, "*.def", SearchOption.AllDirectories);
			//this.LoadScriptFiles(defFiles, LoadDef);
			//Logger.Info("Done loading " + _loadedDefs + " NPC defs.");

			// NPCs
			var listPath = Path.Combine(this.NpcFolder, "npclist.txt");
			var npcListContents = new List<string>();

			if (File.Exists(listPath))
			{
				try
				{
					FileReader.DoEach(listPath, (string line) =>
					{
						npcListContents.Add(Path.Combine(WorldConf.ScriptPath, "npc", line));
					});
				}
				catch (Exception ex)
				{
					Logger.Warning("Unable to fully parse " + listPath + ". Error: " + ex.Message);
				}
			}
			else
			{
				Logger.Warning("Unable to find NPC list '" + listPath + "'.");
			}

			string[] scriptFiles;
			if (!WorldConf.UseNpcListAsExclude)
				scriptFiles = npcListContents.ToArray();
			else
				scriptFiles = Directory.GetFiles(Path.Combine(WorldConf.ScriptPath, "npc"), "*.cs", SearchOption.AllDirectories).Except(npcListContents).ToArray();

			this.LoadScriptFiles(scriptFiles, LoadNPC);
			Logger.Info("Done loading " + _loadedNpcs + " NPCs.");
			Logger.Info("Cached " + _cached + " NPC scripts.");
		}

		/// <summary>
		/// Runs loadMethod for files, using tasks.
		/// </summary>
		/// <param name="files"></param>
		/// <param name="loadMethod"></param>
		private void LoadScriptFiles(string[] files, Action<object> loadMethod)
		{
			var tasks = new List<Task>();
			foreach (var file in files)
				tasks.Add(Task.Factory.StartNew(loadMethod, file, TaskCreationOptions.PreferFairness));

			Task.WaitAll(tasks.ToArray());
		}

		/// <summary>
		/// Loads the def file at the given path.
		/// </summary>
		/// <param name="path"></param>
		private void LoadDef(object path)
		{
			var scriptPath = path as string;

			try
			{
				var asm = this.LoadScript(scriptPath);
				Interlocked.Increment(ref _loadedDefs);
			}
			catch (Exception ex)
			{
				try
				{
					File.Delete(Path.ChangeExtension(scriptPath, ".compiled"));
				}
				catch { }
				Logger.Error("While processing: " + scriptPath + " ... " + ex.Message);
			}

		}

		/// <summary>
		/// Loads the script file at the given path and adds the NPC to the world.
		/// </summary>
		/// <param name="path"></param>
		private void LoadNPC(object path)
		{
			var scriptPath = path as string;

			try
			{
				var script = this.LoadScript(scriptPath).CreateObject("*") as NPCScript;
				var npc = new MabiNPC();
				npc.Script = script;
				npc.ScriptPath = scriptPath;
				script.NPC = npc;
				script.OnLoad();
				npc.LoadDefault();

				WorldManager.Instance.AddCreature(npc);

				Interlocked.Increment(ref _loadedNpcs);
			}
			catch (Exception ex)
			{
				try
				{
					File.Delete(this.GetCompiledPath(scriptPath));
				}
				catch { }
				Logger.Error("While processing: " + scriptPath + " ... " + ex.Message);
			}
		}

		/// <summary>
		/// Adds "cache" to the given path.
		/// Example: data/script/folder/script.cs > data/script/cache/folder/script.cs
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private string GetCompiledPath(string filePath)
		{
			return Path.ChangeExtension(filePath.Replace(WorldConf.ScriptPath, Path.Combine(WorldConf.ScriptPath, "cache")), ".compiled");
		}

		/// <summary>
		/// Loads the given script from the cache, or recaches it,
		/// if necessary. Returns script assembly.
		/// </summary>
		/// <param name="scriptPath"></param>
		/// <returns></returns>
		private Assembly LoadScript(string scriptPath)
		{
			var compiledPath = this.GetCompiledPath(scriptPath);

			Assembly asm;
			if (File.Exists(compiledPath) && File.GetLastWriteTime(compiledPath) >= File.GetLastWriteTime(scriptPath))
			{
				asm = Assembly.LoadFrom(compiledPath);
			}
			else
			{
				asm = CSScript.LoadCode(File.ReadAllText(scriptPath));
				try
				{
					if (File.Exists(compiledPath))
						File.Delete(compiledPath);
					else
						Directory.CreateDirectory(Path.GetDirectoryName(compiledPath));

					File.Copy(asm.Location, compiledPath);

					Interlocked.Increment(ref _cached);
				}
				catch (UnauthorizedAccessException)
				{
					// Thrown if file can't be copied. Happens if script was
					// initially loaded from cache.
				}
				catch (Exception ex)
				{
					Logger.Warning(ex.Message);
				}
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
	}
}
