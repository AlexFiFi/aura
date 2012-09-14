// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MabiNatives
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

	public class MonsterSkillInfo
	{
		public ushort SkillId;
		public byte Rank;
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

		public override void LoadFromXml(string filePath)
		{
			throw new NotImplementedException();
		}

		public override void LoadFromJson(string path)
		{
			base.LoadFromJson(path);

			foreach (var entry in this.Entries)
			{
				foreach (var drop in entry.Drops)
				{
					drop.Chance /= 100;

					if (drop.Chance > 1)
						drop.Chance = 1;
					else if (drop.Chance < 0)
						drop.Chance = 0;
				}

				// Fix ranks coming from text
				foreach (var skill in entry.Skills)
				{
					if (skill.Rank > 18)
						skill.Rank = (byte)(16 - (skill.Rank >> 4));
				}
			}
		}

		protected override string ParseJson(string path)
		{
			var content = base.ParseJson(path);

			// Replace ranks in form of "[0-9A-F]" with hex, to make them parseable.
			content = Regex.Replace(content, "\"Rank\" ?: ?\"([0-9a-fA-F])\"", "\"Rank\": 0x$1" + "F");

			return content;
		}

		public override void ExportToJson(string path)
		{
			throw new NotImplementedException();
		}
	}
}
