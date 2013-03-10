// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;

namespace Aura.Data
{
	public class CharCardInfo
	{
		public uint Id;
		public string Name;
		public uint SetId;
		public List<uint> Races = new List<uint>();
	}

	/// <summary>
	/// Indexed by char card id.
	/// </summary>
	public class CharCardDb : DatabaseCSVIndexed<uint, CharCardInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 3)
				throw new FieldCountException(3);

			var info = new CharCardInfo();
			info.Id = entry.ReadUInt();
			info.SetId = entry.ReadUInt();

			var races = entry.ReadUIntHex();
			if ((races & 0x01) != 0) info.Races.Add(10001);
			if ((races & 0x02) != 0) info.Races.Add(10002);
			if ((races & 0x04) != 0) info.Races.Add(9001);
			if ((races & 0x08) != 0) info.Races.Add(9002);
			if ((races & 0x10) != 0) info.Races.Add(8001);
			if ((races & 0x20) != 0) info.Races.Add(8002);

			this.Entries.Add(info.Id, info);
		}
	}
}
