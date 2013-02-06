// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Data
{
	public class FlightInfo
	{
		public uint RaceId;
		public float FlightSpeed, AscentSpeed, DescentSpeed;
		public byte RotationSpeed;
	}

	public class FlightDb : DataManager<FlightInfo>
	{
		private const double PI2 = Math.PI * 2;
		private const float UNITS_PER_M = 1000f;

		public FlightInfo Find(uint raceId)
		{
			return this.Entries.FirstOrDefault(i => i.RaceId == raceId);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 4);
		}

		protected override void CsvToEntry(FlightInfo info, List<string> csv, int currentLine)
		{
			int i = 0;

			info.RaceId = Convert.ToUInt32(csv[i++]);
			info.FlightSpeed = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture) * UNITS_PER_M;
			info.AscentSpeed = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture) * UNITS_PER_M;
			info.DescentSpeed = Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture) * UNITS_PER_M;
			info.RotationSpeed = (byte)((Convert.ToSingle(csv[i++], CultureInfo.InvariantCulture) * 256) / PI2);
		}
	}
}
