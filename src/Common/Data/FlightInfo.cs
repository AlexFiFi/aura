// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Data
{
	public class FlightInfo
	{
		public uint RaceId;
		public float FlightSpeed, AscentSpeed, DecentSpeed;
		public byte RotationSpeed;
	}

	public class FlightDb : DataManager<FlightInfo>
	{
		private const double twoPi = Math.PI * 2;
		private const float mabiUnitsPerMeter = 1000f;

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
			info.FlightSpeed = Convert.ToSingle(csv[i++]) * mabiUnitsPerMeter;
			info.AscentSpeed = Convert.ToSingle(csv[i++]) * mabiUnitsPerMeter;
			info.DecentSpeed = Convert.ToSingle(csv[i++]) * mabiUnitsPerMeter;
			info.RotationSpeed = (byte)((Convert.ToSingle(csv[i++]) * 256) / twoPi);
		}
	}
}
