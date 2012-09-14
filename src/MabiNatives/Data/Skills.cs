// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;

namespace MabiNatives
{
	public class SkillInfo
	{
		public ushort Id;
		public string DescName;
		public ushort MasterTitle;

		public Dictionary<string, List<SkillRankInfo>> LevelInfo = new Dictionary<string, List<SkillRankInfo>>();

		public void AddLevelInfo(string race, SkillRankInfo detail)
		{
			if (!this.LevelInfo.ContainsKey(race))
				this.LevelInfo.Add(race, new List<SkillRankInfo>());

			var info = this.LevelInfo[race];
			for (int i = 0; i < info.Count(); ++i)
			{
				if (info[i].Level == detail.Level)
				{
					info.RemoveAt(i);
					break;
				}
			}

			info.Add(detail);
		}

		public SkillRankInfo GetLevelInfo(byte level, string race = "base")
		{
			if (!this.LevelInfo.ContainsKey(race))
			{
				if (!this.LevelInfo.ContainsKey(race = "base"))
					return null;
			}

			return this.LevelInfo[race].FirstOrDefault(a => a.Level == level);
		}

		public SkillRankInfo GetLevelInfo(byte level, uint race)
		{
			string sRace = "base";

			if (race == 9001 || race == 9002)
				sRace = "elf";
			else if (race == 8001 || race == 8002)
				sRace = "giant";

			return this.GetLevelInfo(level, sRace); ;
		}
	}

	public class SkillRankInfo
	{
		public byte Level;
		public uint PrepareTime;
		public uint CharPrepareTime;
		public uint Cooltime;
		public byte ApCost;

		public float StaminaCosts;
		public float StaminaModPrepare;
		public float StaminaModPerSecond;
		public float StaminaModDoing;

		public float ManaCosts;
		public float ManaModPrepare;
		public float ManaModPerSecond;
		public float ManaModDoing;

		public float CombatPower;
		public byte Stacks;

		public float Var1, Var2, Var3, Var4, Var5, Var6, Var7, Var8, Var9;

		public float Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
	}

	public class SkillDb : DataManager<SkillInfo>
	{
		public SkillInfo Find(ushort id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromXml(string dbFolderPath)
		{
			string skillInfoPath = dbFolderPath + "/skill/skillinfo.xml";
			if (!File.Exists(skillInfoPath))
				throw new FileNotFoundException("File not found: " + skillInfoPath);

			string skillLevelPath = dbFolderPath + "/skill/skillleveldescription.xml";
			if (!File.Exists(skillLevelPath))
				throw new FileNotFoundException("File not found: " + skillLevelPath);

			using (var reader = new XmlTextReader(skillInfoPath))
			{
				while (reader.ReadToFollowing("Skill"))
				{
					var skillInfo = new SkillInfo();
					skillInfo.Id = Convert.ToUInt16(reader.GetAttribute("SkillID"));
					skillInfo.MasterTitle = Convert.ToUInt16(reader.GetAttribute("MasterTitle"));
					skillInfo.DescName = reader.GetAttribute("DescName");

					this.Entries.Add(skillInfo);
				}
			}

			foreach (var skill in this.Entries)
			{
				// FIXME: Slow as hell... it would be faster to do it the other way around o.o
				using (var reader = new XmlTextReader(skillLevelPath))
				{
					while (reader.ReadToFollowing(skill.DescName))
					{
						var race = reader.GetAttribute("race");

						using (var reader2 = reader.ReadSubtree())
						{
							while (reader2.ReadToFollowing("SkillLevelDetail"))
							{
								var detail = new SkillRankInfo();
								detail.Level = Convert.ToByte(reader2.GetAttribute("SkillLevel"));
								detail.PrepareTime = Convert.ToUInt16(reader2.GetAttribute("PrepareTime"));
								detail.CharPrepareTime = Convert.ToUInt16(reader2.GetAttribute("CharPrepareTime"));
								detail.Cooltime = Convert.ToUInt16(reader2.GetAttribute("Cooltime"));
								detail.ApCost = Convert.ToByte(reader2.GetAttribute("SkillLevel"));

								detail.StaminaCosts = Convert.ToSingle(reader2.GetAttribute("StaminaNecessary"));
								detail.StaminaModPrepare = Convert.ToSingle(reader2.GetAttribute("StaminaModPreparing"));
								detail.StaminaModPerSecond = Convert.ToSingle(reader2.GetAttribute("StaminaModWaiting"));
								detail.StaminaModDoing = Convert.ToSingle(reader2.GetAttribute("StaminaModProcessing"));

								detail.ManaCosts = Convert.ToSingle(reader2.GetAttribute("ManaNecessary"));
								detail.ManaModPrepare = Convert.ToSingle(reader2.GetAttribute("ManaPreparing"));
								detail.ManaModPerSecond = Convert.ToSingle(reader2.GetAttribute("ManaModWaiting"));
								detail.ManaModDoing = Convert.ToSingle(reader2.GetAttribute("ManaModProcessing"));

								detail.CombatPower = Convert.ToSingle(reader2.GetAttribute("CombatPower"));
								detail.Stacks = Convert.ToByte(reader2.GetAttribute("StackPerCast"));

								detail.Var1 = Convert.ToSingle(reader2.GetAttribute("Var1"));
								detail.Var2 = Convert.ToSingle(reader2.GetAttribute("Var2"));
								detail.Var3 = Convert.ToSingle(reader2.GetAttribute("Var3"));
								detail.Var4 = Convert.ToSingle(reader2.GetAttribute("Var4"));
								detail.Var5 = Convert.ToSingle(reader2.GetAttribute("Var5"));
								detail.Var6 = Convert.ToSingle(reader2.GetAttribute("Var6"));
								detail.Var7 = Convert.ToSingle(reader2.GetAttribute("Var7"));
								detail.Var8 = Convert.ToSingle(reader2.GetAttribute("Var8"));
								detail.Var9 = Convert.ToSingle(reader2.GetAttribute("Var9"));

								detail.Life = Convert.ToSingle(reader2.GetAttribute("BonusLife"));
								detail.Mana = Convert.ToSingle(reader2.GetAttribute("BonusMana"));
								detail.Stamina = Convert.ToSingle(reader2.GetAttribute("BonusStamina"));
								detail.Str = Convert.ToSingle(reader2.GetAttribute("BonusSTR"));
								detail.Int = Convert.ToSingle(reader2.GetAttribute("BonusINT"));
								detail.Dex = Convert.ToSingle(reader2.GetAttribute("BonusDEX"));
								detail.Will = Convert.ToSingle(reader2.GetAttribute("BonusWill"));
								detail.Luck = Convert.ToSingle(reader2.GetAttribute("BonusLuck"));

								var version = reader2.GetAttribute("Version");
								if (version == null || version != "1")
								{
									// Doubles are overwritten
									// TODO: Handle "Feature" and "Version" properly.

									skill.AddLevelInfo(race, detail);
								}
							}
						}
					}
				}
			}
		}

		protected override string FormatJson(string line)
		{
			line = base.FormatJson(line);

			line = line.Replace("\"LevelInfo\": { ", "\"LevelInfo\":\r\n  {");
			line = line.Replace("\"base\": [", "\r\n    \"base\": [");
			line = line.Replace("], \"elf\": [", "\r\n    ],\r\n    \"elf\": [");
			line = line.Replace("], \"giant\": [", "\r\n    ],\r\n    \"giant\": [");
			line = line.Replace("{ \"Level\"", "\r\n      { \"Level\"");

			line = Regex.Replace(line, "\"Level\": (?<lvl>[0-9]),", "\"Level\":  ${lvl},");

			line = line.Replace("] } }", "\r\n    ]\r\n  }\r\n}");

			return line;
		}

		//public override void ExportToJSON(string filePath)
		//{
		//    File.Delete(filePath);
		//    using (var writer = File.AppendText(filePath))
		//    {
		//        this.WriteFileHeader(writer);
		//        writer.WriteLine('[');
		//        foreach (var entry in this.Entries)
		//        {
		//            string serialized = JsonConvert.SerializeObject(entry, Newtonsoft.Json.Formatting.None);
		//            serialized = this.CleanJSONLine(serialized);

		//            serialized = serialized.Replace("\"LevelInfo\": { ", "\"LevelInfo\":\r\n  {");
		//            serialized = serialized.Replace("\"base\": [", "\r\n    \"base\": [");
		//            serialized = serialized.Replace("], \"elf\": [", "\r\n    ],\r\n    \"elf\": [");
		//            serialized = serialized.Replace("], \"giant\": [", "\r\n    ],\r\n    \"giant\": [");
		//            serialized = serialized.Replace("{ \"Level\"", "\r\n      { \"Level\"");

		//            serialized = Regex.Replace(serialized, "\"Level\": (?<lvl>[0-9]),", "\"Level\":  ${lvl},");

		//            serialized = serialized.Replace("] } }", "\r\n    ]\r\n  }\r\n}");

		//            writer.WriteLine(serialized + ",\r\n");
		//        }
		//        writer.WriteLine(']');
		//    }
		//}
	}
}
