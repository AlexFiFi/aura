// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.Data
{
	public class ShamalaInfo
	{
		public uint Id;
		public string Name;
		public string Category;
		public byte Rank = 1;
		public float Rate = 100;
		public byte Required = 1;
		public float Size = 1f;
		public uint Color1 = 0x808080, Color2 = 0x808080, Color3 = 0x808080;
		public List<uint> Races = new List<uint>();

		/// <summary>
		/// Returns a random race id from this transformation's races list.
		/// </summary>
		public uint Race
		{
			get
			{
				var rnd = new Random(Environment.TickCount);
				return this.Races[rnd.Next(Races.Count)];
			}
		}
	}

	/// <summary>
	/// Indexed by transformation id.
	/// </summary>
	public class ShamalaDb : DatabaseCSVIndexed<uint, ShamalaInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 11)
				throw new FieldCountException(11);

			var info = new ShamalaInfo();
			info.Id = entry.ReadUInt();
			info.Name = entry.ReadString();
			info.Category = entry.ReadString();
			info.Rank = entry.ReadUByte();
			info.Rate = entry.ReadFloat();
			info.Required = entry.ReadUByte();
			info.Size = entry.ReadFloat();
			info.Color1 = entry.ReadUIntHex();
			info.Color2 = entry.ReadUIntHex();
			info.Color3 = entry.ReadUIntHex();

			var races = entry.ReadStringList();
			foreach (var race in races)
				info.Races.Add(Convert.ToUInt32(race));

			if (this.Entries.ContainsKey(info.Id))
				throw new DatabaseWarningException("Duplicate: " + info.Id);
			this.Entries.Add(info.Id, info);
		}
	}
}
