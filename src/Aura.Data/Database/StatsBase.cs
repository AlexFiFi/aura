// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aura.Data
{
	public class StatsBaseInfo
	{
		public byte Age;
		public ushort Race;
		public byte AP, Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
	}

	public class StatsBaseDb : DatabaseCSV<StatsBaseInfo>
	{
		/// <summary>
		/// Returns the age info (base stats) for the given race
		/// at the given age, or null.
		/// </summary>
		/// <param name="race">0 = Human, 1 = Elf, 2 = Giant</param>
		/// <param name="age">10-17</param>
		/// <returns></returns>
		public StatsBaseInfo Find(uint race, byte age)
		{
			race = (uint)(race & ~3);
			return this.Entries.FirstOrDefault(a => a.Race == race && a.Age == age);
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 11)
				throw new FieldCountException(11);

			var info = new StatsBaseInfo();
			info.Age = entry.ReadUByte();
			info.Race = entry.ReadUShort();
			info.AP = entry.ReadUByte();
			info.Life = entry.ReadUByte();
			info.Mana = entry.ReadUByte();
			info.Stamina = entry.ReadUByte();
			info.Str = entry.ReadUByte();
			info.Int = entry.ReadUByte();
			info.Dex = entry.ReadUByte();
			info.Will = entry.ReadUByte();
			info.Luck = entry.ReadUByte();

			this.Entries.Add(info);
		}
	}
}
