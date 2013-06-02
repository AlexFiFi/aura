// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Linq;
using System.Collections.Generic;
using System;

namespace Aura.Data
{
	public class TalentRankInfo
	{
		public byte TalentId, Rank, Race;
		public Dictionary<byte, float> Bonuses = new Dictionary<byte, float>();
	}

	public class TalentRankDb : DatabaseCSV<TalentRankInfo>
	{
		public TalentRankInfo Find(byte talentId, byte rank, byte raceMask)
		{
			return this.Entries.FirstOrDefault(a => a.TalentId == talentId && a.Rank == rank && ((raceMask & a.Race) != 0));
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 2)
				throw new FieldCountException(2);

			if (entry.Count % 2 != 1)
				throw new Exception("Mismatched stat and bonus");

			var info = new TalentRankInfo();

			info.TalentId = entry.ReadUByte();
			info.Rank = entry.ReadUByte();
			info.Race = entry.ReadUByte();
			while (!entry.End)
			{
				var stat = entry.ReadUByte();
				var amount = entry.ReadFloat();
				info.Bonuses.Add(stat, amount);
			}

			this.Entries.Add(info);
		}
	}
}
