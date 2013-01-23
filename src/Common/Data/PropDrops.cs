// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using Common.Tools;

namespace Common.Data
{
	public class PropDropInfo
	{
		public uint Type;
		public List<PropDropItemInfo> Items = new List<PropDropItemInfo>();

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

	public class PropDropDb : DataManager<PropDropItemInfo>
	{
		/// <summary>
		/// Entries indexed by the drop type.
		/// </summary>
		public Dictionary<uint, PropDropInfo> OrderedEntries = new Dictionary<uint, PropDropInfo>();

		public PropDropInfo Find(uint type)
		{
			PropDropInfo result = null;
			this.OrderedEntries.TryGetValue(type, out result);
			return result;
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 3);

			foreach (var entry in this.Entries)
			{
				if (!this.OrderedEntries.ContainsKey(entry.Type))
					this.OrderedEntries.Add(entry.Type, new PropDropInfo(entry.Type));
				this.OrderedEntries[entry.Type].Items.Add(entry);
			}
		}

		protected override void CsvToEntry(PropDropItemInfo info, List<string> csv, int currentLine)
		{
			int i = 0;

			info.Type = Convert.ToUInt32(csv[i++]);
			info.ItemClass = Convert.ToUInt32(csv[i++]);
			info.Amount = Convert.ToUInt16(csv[i++]);
			info.Chance = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

			var ii = MabiData.ItemDb.Find(info.ItemClass);
			if (ii == null)
			{
				Logger.Warning("Unknown item id '{0}'.", info.ItemClass);
				return;
			}

			if (info.Amount > ii.StackMax)
			{
				Logger.Warning("Amount exceeds max stack for item '{0}' on line {1}.", info.ItemClass, currentLine);
				info.Amount = ii.StackMax;
			}
		}
	}
}
