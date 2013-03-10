// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;

namespace Aura.Data
{
	public class CharCardSetInfo
	{
		public uint SetId;
		public uint Race;
		public uint Class;
		public byte Pocket;
		public uint Color1, Color2, Color3;
	}

	public class CharCardSetDb : DatabaseCSV<CharCardSetInfo>
	{
		public List<CharCardSetInfo> Find(uint setId, uint race)
		{
			return this.Entries.FindAll(a => a.SetId == setId && a.Race == race);
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 7)
				throw new FieldCountException(7);

			var info = new CharCardSetInfo();
			info.SetId = entry.ReadUInt();
			info.Race = entry.ReadUInt();
			info.Class = entry.ReadUInt();
			info.Pocket = entry.ReadUByte();
			info.Color1 = entry.ReadUIntHex();
			info.Color2 = entry.ReadUIntHex();
			info.Color3 = entry.ReadUIntHex();

			this.Entries.Add(info);
		}
	}
}
