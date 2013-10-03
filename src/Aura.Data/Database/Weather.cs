// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System;

namespace Aura.Data
{
	public class WeatherInfo
	{
		public uint Region { get; internal set; }
		public WeatherInfoType Type { get; internal set; }
		public List<float> Values { get; internal set; }
	}

	public enum WeatherInfoType { Official, Custom, Pattern, OWM }

	/// <summary>
	/// Indexed by region.
	/// </summary>
	public class WeatherDb : DatabaseCSVIndexed<uint, WeatherInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 3)
				throw new FieldCountException(3);

			// Read everything first, we might need it for multiple regions.
			var regions = entry.ReadStringList();
			var type = (WeatherInfoType)entry.ReadUByte();
			var values = new List<float>();
			while (!entry.End)
				values.Add(entry.ReadFloat());

			// Every type has at least 1 value.
			if (values.Count < 1)
				throw new DatabaseWarningException("Too few values.");

			foreach (var region in regions)
			{
				var info = new WeatherInfo();
				info.Region = Convert.ToUInt32(region);
				info.Type = type;
				info.Values = values;

				this.Entries[info.Region] = info;
			}
		}
	}
}
