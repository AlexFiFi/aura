// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Common.Data
{
	public class SkillInfo
	{
		public ushort Id;
		public string Name;
		public ushort MasterTitle;

		public List<SkillRankInfo> RankInfo = new List<SkillRankInfo>();

		public SkillRankInfo GetRankInfo(byte level, uint race)
		{
			race = race & ~(uint)3;

			SkillRankInfo info;
			if ((info = this.RankInfo.FirstOrDefault(a => a.Rank == level && a.Race == race)) == null)
			{
				if ((info = this.RankInfo.FirstOrDefault(a => a.Rank == level && a.Race == 0)) == null)
				{
					return null;
				}
			}

			return info;
		}
	}

	public class SkillDb : DataManager<SkillInfo>
	{
		public SkillInfo Find(ushort id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 3);
		}

		protected override void CsvToEntry(SkillInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.Id = Convert.ToUInt16(csv[i++]);
			info.Name = csv[i++];
			info.MasterTitle = Convert.ToUInt16(csv[i++]);

			var ranks = MabiData.SkillRankDb.Entries.FindAll(a => a.SkillId == info.Id);
			foreach (var rank in ranks)
			{
				info.RankInfo.Add(rank);
			}
		}
	}
}
