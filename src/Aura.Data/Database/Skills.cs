// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Linq;

namespace Aura.Data
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

	/// <summary>
	/// Indexed by skill id.
	/// Depends on: SkillRankDb
	/// </summary>
	public class SkillDb : DatabaseCSVIndexed<ushort, SkillInfo>
	{
		public List<SkillInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Value.Name.ToLower().Contains(name));
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 3)
				throw new FieldCountException(3);

			var info = new SkillInfo();
			info.Id = entry.ReadUShort();
			info.Name = entry.ReadString();
			info.MasterTitle = entry.ReadUShort();

			info.RankInfo.AddRange(MabiData.SkillRankDb.Entries.FindAll(a => a.SkillId == info.Id).OrderBy(a => a.Rank));

			this.Entries.Add(info.Id, info);
		}
	}
}
