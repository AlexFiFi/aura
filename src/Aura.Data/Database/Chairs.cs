// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.Data
{
	public class ChairInfo
	{
		public uint ItemId;
		public uint PropId, GiantPropId;
		public uint Effect;
	}

	/// <summary>
	/// Indexed by item id.
	/// </summary>
	public class ChairDb : DatabaseCSVIndexed<uint, ChairInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 5)
				throw new FieldCountException(5);

			var info = new ChairInfo();
			info.ItemId = entry.ReadUInt();
			entry.ReadString();
			info.PropId = entry.ReadUInt();
			info.GiantPropId = entry.ReadUInt();
			info.Effect = entry.ReadUInt();

			this.Entries.Add(info.ItemId, info);
		}
	}
}
