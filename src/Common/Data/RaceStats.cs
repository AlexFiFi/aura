// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Common.Tools;

namespace Common.Data
{
	internal class RaceStatInfo
	{
		public uint RaceId;
		public string Name;
		public string AI;
		public float Life;
		public uint Exp;
		public float Size;
		public uint ColorA, ColorB, ColorC;

		public int GoldMin, GoldMax;
		public List<DropInfo> Drops = new List<DropInfo>();

		public List<RaceSkillInfo> Skills = new List<RaceSkillInfo>();
	}

	public class DropInfo
	{
		public uint ItemId;
		public float Chance;
	}

	internal class RaceStatDb : DataManager<RaceStatInfo>
	{
		public RaceStatInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.RaceId == id);
		}

		public RaceStatInfo Find(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Name.ToLower() == name);
		}

		public List<RaceStatInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Name.ToLower() == name);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 11);
		}

		protected override void CsvToEntry(RaceStatInfo info, List<string> csv, int currentLine)
		{
			int j = 0;
			info.RaceId = Convert.ToUInt32(csv[j++]);
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
				var drop = new DropInfo();
				drop.ItemId = Convert.ToUInt32(csv[j++]);
				drop.Chance = float.Parse(csv[j++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

				drop.Chance /= 100;
				if (drop.Chance > 1)
					drop.Chance = 1;
				else if (drop.Chance < 0)
					drop.Chance = 0;

				info.Drops.Add(drop);
			}

			info.Skills = MabiData.RaceSkillDb.FindAll(info.RaceId);
		}
	}
}
