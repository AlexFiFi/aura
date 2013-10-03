// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;

namespace Aura.Data
{
	public class RaceSkillInfo
	{
		public uint MonsterId;
		public ushort SkillId;
		public byte Rank;
	}

	public class RaceSkillDb : DatabaseCSV<RaceSkillInfo>
	{
		public List<RaceSkillInfo> FindAll(uint id)
		{
			return this.Entries.FindAll(a => a.MonsterId == id);
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 3)
				throw new FieldCountException(3);

			var info = new RaceSkillInfo();
			info.MonsterId = entry.ReadUInt();
			info.SkillId = entry.ReadUShort();
			info.Rank = (byte)(16 - entry.ReadUByteHex());

			this.Entries.Add(info);
		}
	}
}
