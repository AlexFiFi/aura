// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Data
{
	public class CharCardInfo
	{
		public uint Id;
		public string Name;
		public uint SetId;
		public List<uint> Races = new List<uint>();
	}

	public class CharCardDb : DataManager<CharCardInfo>
	{
		/// <summary>
		/// Searches for the entry with the given Id returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CharCardInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 3);
		}

		protected override void CsvToEntry(CharCardInfo info, List<string> csv, int currentLine)
		{
			info.Id = Convert.ToUInt32(csv[0]);
			info.SetId = Convert.ToUInt32(csv[1]);
			var races = Convert.ToUInt32(csv[2], 16);
			if ((races & 0x01) != 0) info.Races.Add(10001);
			if ((races & 0x02) != 0) info.Races.Add(10002);
			if ((races & 0x04) != 0) info.Races.Add(9001);
			if ((races & 0x08) != 0) info.Races.Add(9002);
			if ((races & 0x10) != 0) info.Races.Add(8001);
			if ((races & 0x20) != 0) info.Races.Add(8002);
		}
	}
}
