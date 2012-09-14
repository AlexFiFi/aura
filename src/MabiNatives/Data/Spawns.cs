// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MabiNatives
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

		public override void LoadFromXml(string path)
		{
			throw new NotImplementedException();
		}

		public override void LoadFromJson(string path)
		{
			base.LoadFromJson(path);

			foreach (var entry in this.Entries)
			{
				entry.Id = _spawnId++;
			}
		}

		public override void ExportToJson(string path)
		{
			throw new NotImplementedException();
		}
	}
}
