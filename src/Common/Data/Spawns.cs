// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Data
{
	public class SpawnInfo
	{
		public uint Id;
		public uint MonsterId;
		public uint Region;
		public int X1, Y1, X2, Y2;
		public byte Amount;
	}

	public class SpawnDb : DataManager<SpawnInfo>
	{
		private uint _spawnId = 1;

		public SpawnInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 7);
		}

		protected override void CsvToEntry(SpawnInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.Id = _spawnId++;
			info.MonsterId = Convert.ToUInt32(csv[i++]);
			info.Region = Convert.ToUInt32(csv[i++]);
			info.X1 = Convert.ToInt32(csv[i++]);
			info.Y1 = Convert.ToInt32(csv[i++]);
			info.X2 = Convert.ToInt32(csv[i++]);
			info.Y2 = Convert.ToInt32(csv[i++]);
			info.Amount = Convert.ToByte(csv[i++]);
		}
	}
}
