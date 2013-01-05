// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
	public class FlightInfo
	{
		public uint raceId;
		public float FlightSpeed, AscentSpeed, DecentSpeed;
		public byte RotationSpeed;
	}
	public class FlightDb : DataManager<FlightInfo>
	{
		public FlightInfo Find(uint raceId)
		{
			return this.Entries.FirstOrDefault(i => i.raceId == raceId);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 4);
		}

		private const double twoPi = Math.PI * 2;
		private const float MabiUnitsPerMeter = 1000f;
		protected override void CsvToEntry(FlightInfo info, List<string> csv, int currentLine)
		{
			int i = 0;
			info.raceId = Convert.ToUInt32(csv[i++]);
			info.FlightSpeed = Convert.ToSingle(csv[i++]) * MabiUnitsPerMeter;
			info.AscentSpeed = Convert.ToSingle(csv[i++]) * MabiUnitsPerMeter;
			info.DecentSpeed = Convert.ToSingle(csv[i++]) * MabiUnitsPerMeter;

			float rotationInRad = Convert.ToSingle(csv[i++]);

			info.RotationSpeed = (byte)((rotationInRad * 256) / twoPi);
		}
	}
}
