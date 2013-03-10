// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.Data
{
	public class PropDropInfo
	{
		public uint Type;
		public List<PropDropItemInfo> Items = new List<PropDropItemInfo>();

		public PropDropInfo()
		{ }

		public PropDropInfo(uint type)
		{
			this.Type = type;
		}

		/// <summary>
		/// Returns a random item id from the list, based on the weight (chance).
		/// </summary>
		/// <param name="rand"></param>
		/// <returns></returns>
		public PropDropItemInfo GetRndItem(Random rand)
		{
			float total = 0;
			foreach (var cls in this.Items)
				total += cls.Chance;

			var rand_val = rand.NextDouble() * total;
			int i = 0;
			for (; rand_val > 0; ++i)
				rand_val -= this.Items[i].Chance;

			return this.Items[i - 1];
		}
	}

	public class PropDropItemInfo
	{
		public uint Type;
		public uint ItemClass;
		public ushort Amount;
		public float Chance;
	}

	public class PropDropDb : DatabaseCSVIndexed<uint, PropDropInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 3)
				throw new FieldCountException(3);

			var info = new PropDropItemInfo();
			info.Type = entry.ReadUInt();
			info.ItemClass = entry.ReadUInt();
			info.Amount = entry.ReadUShort();
			info.Chance = entry.ReadFloat();

			var ii = MabiData.ItemDb.Find(info.ItemClass);
			if (ii == null)
				throw new Exception(string.Format("Unknown item id '{0}'.", info.ItemClass));

			if (info.Amount > ii.StackMax)
				info.Amount = ii.StackMax;

			// The file contains PropDropItemInfo, here we organize it into PropDropInfo structs.
			if (!this.Entries.ContainsKey(info.Type))
				this.Entries.Add(info.Type, new PropDropInfo(info.Type));
			this.Entries[info.Type].Items.Add(info);
		}
	}
}
