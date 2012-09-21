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
		public uint SetId;
		public uint Race;
		public uint ItemId;
		public byte Pocket;
		public uint Color1, Color2, Color3;
	}

	public class CharCardSetDb : DataManager<CharCardSetInfo>
	{
		public List<CharCardSetInfo> Find(uint setId, uint race)
		{
			return this.Entries.FindAll(a => a.SetId == setId && a.Race == race);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 7);
		}

		protected override void CsvToEntry(CharCardSetInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.SetId = Convert.ToUInt32(csv[i++]);
			info.Race = Convert.ToUInt32(csv[i++]);
			info.ItemId = Convert.ToUInt32(csv[i++]);
			info.Pocket = Convert.ToByte(csv[i++]);
			info.Color1 = Convert.ToUInt32(csv[i++]);
			info.Color2 = Convert.ToUInt32(csv[i++]);
			info.Color3 = Convert.ToUInt32(csv[i++]);
		}
	}
}
