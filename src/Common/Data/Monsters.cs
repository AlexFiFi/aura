// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using Common.Tools;

namespace Common.Data
{
	public class MonsterInfo
	{
		public uint Id;
		public string Name;
		public uint Race;
		public string AI;
		public float Life;
		public uint Exp;
		public float Size;
		public uint ColorA, ColorB, ColorC;

		public int GoldMin, GoldMax;
		public List<MonsterDropInfo> Drops = new List<MonsterDropInfo>();

		public List<MonsterSkillInfo> Skills = new List<MonsterSkillInfo>();
	}

	public class MonsterDropInfo
	{
		public uint ItemId;
		public float Chance;
	}

	public class MonsterDb : DataManager<MonsterInfo>
	{
		public MonsterInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public MonsterInfo Find(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Name.ToLower() == name);
		}

		public List<MonsterInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Name.ToLower() == name);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 12);
		}

		protected override void CsvToEntry(MonsterInfo info, List<string> csv, int currentLine)
		{
			int j = 0;
			info.Id = Convert.ToUInt32(csv[j++]);
			info.Race = Convert.ToUInt32(csv[j++]);
			info.Name = csv[j++];
			info.AI = csv[j++];
			info.ColorA = Convert.ToUInt32(csv[j++].Replace("0x", ""), 16);
			info.ColorB = Convert.ToUInt32(csv[j++].Replace("0x", ""), 16);
			info.ColorC = Convert.ToUInt32(csv[j++].Replace("0x", ""), 16);
			info.Life = float.Parse(csv[j++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Exp = Convert.ToUInt32(csv[j++]);
			info.Size = float.Parse(csv[j++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.GoldMin = Convert.ToInt32(csv[j++]);
			info.GoldMax = Convert.ToInt32(csv[j++]);

			if ((csv.Count - j) % 2 != 0)
			{
				Logger.Warning("Missing drop id or chance on line " + currentLine + " in monsters.");
				j = csv.Count;
			}
			while (j < csv.Count)
			{
				var drop = new MonsterDropInfo();
				drop.ItemId = Convert.ToUInt32(csv[j++]);
				drop.Chance = float.Parse(csv[j++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

				drop.Chance /= 100;
				if (drop.Chance > 1)
					drop.Chance = 1;
				else if (drop.Chance < 0)
					drop.Chance = 0;

				info.Drops.Add(drop);
			}

			info.Skills = MabiData.MonsterSkillDb.FindAll(info.Id);
		}
	}
}
