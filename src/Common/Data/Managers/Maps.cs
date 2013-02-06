// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Tools;

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

		public uint TryGetRegionId(string region, string errorLocation = null)
		{
			uint regionId = 0;
			if (!uint.TryParse(region, out regionId))
			{
				var mapInfo = MabiData.MapDb.Find(region);
				if (mapInfo != null)
					regionId = mapInfo.Id;
				else
				{
					Logger.Warning((errorLocation != null ? errorLocation + " : " : "") + "Map '" + region + "' not found.");
				}
			}

			return regionId;
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
