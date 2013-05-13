// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Linq;
using System.Collections.Generic;
using System;

namespace Aura.Data
{
	public class TalentExpInfo
	{
		public ushort SkillId;
		public byte Race, SkillRank;
		public Dictionary<byte, uint> Exps = new Dictionary<byte, uint>();
	}

	public class TalentExpDb : DatabaseCSV<TalentExpInfo>
	{
		public TalentExpInfo Find(ushort skillId, byte rank)
		{
			return this.Entries.FirstOrDefault(a => a.SkillId == skillId && a.SkillRank == rank);
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 5)
				throw new FieldCountException(5);

			if (entry.Count % 2 != 1)
				throw new Exception("Mismatched talent Id and talent exp");

			var info = new TalentExpInfo();
			int i = 0;
			info.SkillId = entry.ReadUShort(i++);
			info.SkillRank = entry.ReadUByte(i++);
			info.Race = entry.ReadUByte(i++);
			for (; i < entry.Count; )
			{
				var talentId = entry.ReadUByte(i++);
				var talentExp = entry.ReadUInt(i++);
				info.Exps.Add(talentId, talentExp);
			}

			this.Entries.Add(info);
		}
	}
}
