// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Common.Data
{
	public class StatsBaseInfo
	{
		public byte Age;
		public ushort Race;
		public Dictionary<string, byte> BaseStats = new Dictionary<string, byte>();
	}

	public class StatsBaseDb : DataManager<StatsBaseInfo>
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

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 11);
		}

		protected override void CsvToEntry(StatsBaseInfo info, List<string> csv, int currentLine)
		{
			info.Age = Convert.ToByte(csv[0]);
			info.Race = Convert.ToUInt16(csv[1]);
			info.BaseStats["AP"] = Convert.ToByte(csv[2]);
			info.BaseStats["Life"] = Convert.ToByte(csv[3]);
			info.BaseStats["Mana"] = Convert.ToByte(csv[4]);
			info.BaseStats["Stamina"] = Convert.ToByte(csv[5]);
			info.BaseStats["Str"] = Convert.ToByte(csv[6]);
			info.BaseStats["Int"] = Convert.ToByte(csv[7]);
			info.BaseStats["Dex"] = Convert.ToByte(csv[8]);
			info.BaseStats["Will"] = Convert.ToByte(csv[9]);
			info.BaseStats["Luck"] = Convert.ToByte(csv[10]);
		}
	}
}
