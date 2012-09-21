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
	public class CharCardSetInfo
	{
		public uint Id;
		public uint Race;
		public uint ItemId;
		public byte Pocket;
		public uint Color1, Color2, Color3;
	}

	public class CharCardSetDb : DataManager<CharCardSetInfo>
	{
		public List<CharCardSetInfo> Find(uint setId, uint race)
		{
			return this.Entries.FindAll(a => a.Id == setId && a.Race == race);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 7);
		}

		protected override void CsvToEntry(CharCardSetInfo info, List<string> csv, int currentLine)
		{
			info.Id = Convert.ToUInt32(csv[0]);
			info.Race = Convert.ToUInt32(csv[1]);
			info.ItemId = Convert.ToUInt32(csv[2]);
			info.Pocket = Convert.ToByte(csv[3]);
			info.Color1 = Convert.ToUInt32(csv[4]);
			info.Color2 = Convert.ToUInt32(csv[5]);
			info.Color3 = Convert.ToUInt32(csv[6]);
		}
	}
}
