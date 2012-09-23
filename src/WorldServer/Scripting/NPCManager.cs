// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
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

		private int _loadedNpcs, _cached;

		public void LoadNPCs()
		{
			Logger.Info("Loading NPCs...");

			_loadedNpcs = _cached = 0;

			// NPCs
			var listPath = Path.Combine(Path.Combine(WorldConf.ScriptPath, "npc"), "npclist.txt");
			var npcListContents = new List<string>();

			if (File.Exists(listPath))
			{
				try
				{
					npcListContents = FileReader.GetAllLines(listPath);
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

			var scriptFiles = npcListContents.ToArray();

			foreach (var line in scriptFiles)
			{
				var file = line;
				bool virt = false;
				if (file.StartsWith("virtual:"))
				{
					virt = true;
					file = file.Replace("virtual:", "").Trim();
				}
				LoadNPC(Path.Combine(WorldConf.ScriptPath, "npc", file), virt);
			}

			Logger.Info("Done loading " + _loadedNpcs + " NPCs.");
			if (!WorldConf.DisableScriptCaching)
				Logger.Info("Cached " + _cached + " NPC scripts.");
		}

		/// <summary>
		/// Loads the script file at the given path and adds the NPC to the world.
		/// </summary>
		/// <param name="path"></param>
		private void LoadNPC(string scriptPath, bool virtualLoad = false)
		{
			try
			{
				var script = this.LoadScript(scriptPath).CreateObject("*") as NPCScript;
				if (!virtualLoad)
				{
					var npc = new MabiNPC();
					npc.Script = script;
					npc.ScriptPath = scriptPath;
					script.NPC = npc;
					script.LoadFlags = NPCLoadFlags.Real;
					script.OnLoad();
					npc.LoadDefault();

					WorldManager.Instance.AddCreature(npc);
				}
				else
				{
					script.LoadFlags = NPCLoadFlags.Virtual;
				}

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
			if (!WorldConf.DisableScriptCaching && File.Exists(compiledPath) && File.GetLastWriteTime(compiledPath) >= File.GetLastWriteTime(scriptPath))
			{
				asm = Assembly.LoadFrom(compiledPath);
			}
			else
			{
				asm = CSScript.LoadCode(File.ReadAllText(scriptPath));
				if (!WorldConf.DisableScriptCaching)
				{
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
