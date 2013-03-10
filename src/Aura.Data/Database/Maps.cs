// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Linq;

namespace Aura.Data
{
	public class MapInfo
	{
		public uint Id;
		public string Name;
	}

	/// <summary>
	/// Indexed by map name.
	/// </summary>
	public class MapDb : DatabaseCSVIndexed<string, MapInfo>
	{
		public MapInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Value.Id == id).Value;
		}

		public uint TryGetRegionId(string region, uint fallBack = 0)
		{
			uint regionId = fallBack;
			if (!uint.TryParse(region, out regionId))
			{
				var mapInfo = this.Find(region);
				if (mapInfo != null)
					regionId = mapInfo.Id;
			}

			return regionId;
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 2)
				throw new FieldCountException(2);

			var info = new MapInfo();
			info.Id = entry.ReadUInt();
			info.Name = entry.ReadString();

			this.Entries.Add(info.Name, info);
		}
	}
}
