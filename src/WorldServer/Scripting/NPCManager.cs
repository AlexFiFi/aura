// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;
using Common.Tools;
using CSScriptLibrary;
using World.Tools;
using World.World;
using MabiNatives;
using Common.Constants;

namespace World.Scripting
{
	public class NPCManager
	{
		public readonly static NPCManager Instance = new NPCManager();
		static NPCManager() { }
		private NPCManager() { }

		public void LoadNPCs()
		{
			uint npcsLoaded = 0;
			try
			{
				var listPath = Path.Combine(WorldConf.ScriptPath, "npc/enabled.txt");

				if (File.Exists(listPath))
				{
					// TODO: JSON?

					FileReader.DoEach(listPath, (string line) =>
					{
						var filePath = Path.Combine(WorldConf.ScriptPath, "npc", line);

						Logger.ClearLine();
						Logger.Info("Loading '" + line + "'...", false);
						if (!File.Exists(filePath))
						{
							Logger.Warning("NPC script missing: " + line);
							return;
						}

						try
						{
							// TODO: Maybe try to do this some other way, without script and NPC needing each other. But this is the most comfortable way I guess.

							//var script = (NPCScript)CSScript.Load(filePath).CreateObject("*");
							var script = (NPCScript)CSScript.LoadCode(File.ReadAllText(filePath)).CreateObject("*");
							var npc = new MabiNPC();
							npc.Script = script;
							npc.ScriptPath = filePath;

							script.NPC = npc;
							script.OnLoad();

							npc.LoadDefault();

							WorldManager.Instance.AddCreature(npc);

							npcsLoaded++;
						}
						catch (Exception ex)
						{
							Console.WriteLine("");
							Logger.Exception(ex, "Unable to load '" + line + "'.", true);
						}
					});
				}
				else
				{
					Logger.Warning("NPC list not found.");
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, null, true);
			}


			Logger.ClearLine();
			Logger.Info("Loaded " + npcsLoaded.ToString() + " NPCs.");
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
