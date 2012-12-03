// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Common.Data
{
	public class MapInfo
	{
		public uint Id;
		public string Name;
	}

	public class MapDb : DataManager<MapInfo>
	{
		public MapInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public MapInfo Find(string name)
		{
			return this.Entries.FirstOrDefault(a => a.Name == name);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 2);
		}

		protected override void CsvToEntry(MapInfo info, List<string> csv, int currentLine)
		{
			info.Id = Convert.ToUInt32(csv[0]);
			info.Name = csv[1];
		}
	}
}
