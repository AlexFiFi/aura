// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Data
{
	public class PetInfo
	{
		public uint RaceId;
		public ushort TimeLimit;
		public float ExpMultiplicator;
		public float Height, Upper, Lower;
		public float Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
		public ushort Defense;
		public float Protection;
		public uint Color1, Color2, Color3;
	}

	public class PetDb : DatabaseCSVIndexed<uint, PetInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 20)
				throw new FieldCountException(20);

			var info = new PetInfo();
			info.RaceId = entry.ReadUInt();
			entry.ReadString(); // Name
			info.TimeLimit = entry.ReadUShort();
			info.ExpMultiplicator = entry.ReadFloat();

			info.Height = entry.ReadFloat();
			info.Upper = entry.ReadFloat();
			info.Lower = entry.ReadFloat();

			info.Life = entry.ReadFloat();
			info.Mana = entry.ReadFloat();
			info.Stamina = entry.ReadFloat();
			info.Str = entry.ReadFloat();
			info.Int = entry.ReadFloat();
			info.Dex = entry.ReadFloat();
			info.Will = entry.ReadFloat();
			info.Luck = entry.ReadFloat();

			info.Defense = entry.ReadUShort();
			info.Protection = entry.ReadFloat();

			info.Color1 = entry.ReadUIntHex();
			info.Color2 = entry.ReadUIntHex();
			info.Color3 = entry.ReadUIntHex();

			this.Entries.Add(info.RaceId, info);
		}
	}
}
