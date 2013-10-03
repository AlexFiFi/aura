// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;

namespace Aura.Data
{
	public class StatsLevelUpInfo
	{
		public byte Age;
		public ushort Race;
		public ushort AP;
		public float Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
	}

	public class StatsLevelUpDb : DatabaseCSV<StatsLevelUpInfo>
	{
		/// <summary>
		/// Returns the age info for the given race
		/// at the given age, or null.
		/// </summary>
		/// <param name="race"></param>
		/// <param name="age"></param>
		/// <returns></returns>
		public StatsLevelUpInfo Find(uint race, byte age)
		{
			race = (uint)(race & ~3);
			return this.Entries.FirstOrDefault(a => a.Race == race && a.Age == Math.Min((byte)25, age));
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 11)
				throw new FieldCountException(11);

			var info = new StatsLevelUpInfo();
			info.Age = entry.ReadUByte();
			info.Race = entry.ReadUShort();
			info.AP = entry.ReadUByte();
			info.Life = entry.ReadFloat();
			info.Mana = entry.ReadFloat();
			info.Stamina = entry.ReadFloat();
			info.Str = entry.ReadFloat();
			info.Int = entry.ReadFloat();
			info.Dex = entry.ReadFloat();
			info.Will = entry.ReadFloat();
			info.Luck = entry.ReadFloat();

			this.Entries.Add(info);
		}
	}
}
