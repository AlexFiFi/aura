// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Linq;
using System;

namespace Aura.Data
{
	public class AncientDropDb : DatabaseCSV<DropInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count <2)
				throw new FieldCountException(2);

			var info = new DropInfo();

			info.ItemId = entry.ReadUInt(0);
			info.Chance = entry.ReadFloat(1);

			this.Entries.Add(info);
		}
	}
}
