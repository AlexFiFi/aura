// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace Common.Data
{
	public class StatsLevelUpInfo
	{
		public byte Age;
		public ushort Race;
		public ushort AP;
		public float Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
	}

	public class StatsLevelUpDb : DataManager<StatsLevelUpInfo>
	{
		/// <summary>
		/// Returns the age info for the given race
		/// at the given age, or null.
		/// </summary>
		/// <param name="race">0 = Human, 1 = Elf, 2 = Giant</param>
		/// <param name="age">10-17</param>
		/// <returns></returns>
		public StatsLevelUpInfo Find(uint race, byte age)
		{
			race = (uint)(race & ~3);
			return this.Entries.FirstOrDefault(a => a.Race == race && a.Age == Math.Min((byte)25, age));
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 11);
		}

		protected override void CsvToEntry(StatsLevelUpInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.Age = Convert.ToByte(csv[i++]);
			info.Race = Convert.ToUInt16(csv[i++]);
			info.AP = Convert.ToByte(csv[i++]);
			info.Life = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Mana = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Stamina = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Str = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Int = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Dex = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Will = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
			info.Luck = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture.NumberFormat);
		}
	}
}
