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
			this.ReadCsv(filePath, 9);
		}

		protected override void CsvToEntry(MonsterInfo info, List<string> csv, int currentLine)
		{
			info.Id = Convert.ToUInt32(csv[0]);
			info.Race = Convert.ToUInt32(csv[1]);
			info.Name = csv[2];
			info.AI = csv[3];
			info.Life = float.Parse(csv[4], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Exp = Convert.ToUInt32(csv[5]);
			info.Size = float.Parse(csv[6], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.GoldMin = Convert.ToInt32(csv[7]);
			info.GoldMax = Convert.ToInt32(csv[8]);

			var remaining = csv.Count - 9;
			if (remaining % 2 != 0)
			{
				Logger.Warning("Missing drop id or chance on line " + currentLine + " in monsters.");
				remaining = 0;
			}
			for (int j = 0; j < remaining; ++j)
			{
				var drop = new MonsterDropInfo();
				drop.ItemId = Convert.ToUInt32(csv[9 + j]);
				drop.Chance = float.Parse(csv[9 + j + 1], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

				drop.Chance /= 100;
				if (drop.Chance > 1)
					drop.Chance = 1;
				else if (drop.Chance < 0)
					drop.Chance = 0;

				++j;
			}

			info.Skills = MabiData.MonsterSkillDb.FindAll(info.Id);
		}
	}
}
