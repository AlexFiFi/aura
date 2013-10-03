// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Aura.Data
{
	public class FlightInfo
	{
		public uint RaceId;
		public float FlightSpeed, AscentSpeed, DescentSpeed;
		public byte RotationSpeed;
	}

	/// <summary>
	/// Contains information about speed of flying races (pets, monsters).
	/// Indexed by race id.
	/// </summary>
	public class FlightDb : DatabaseCSVIndexed<uint, FlightInfo>
	{
		private const double Pi2 = Math.PI * 2;
		private const float UnitsPerM = 1000f;

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 4)
				throw new FieldCountException(4);

			var info = new FlightInfo();
			info.RaceId = entry.ReadUInt();
			info.FlightSpeed = entry.ReadFloat() * UnitsPerM;
			info.AscentSpeed = entry.ReadFloat() * UnitsPerM;
			info.DescentSpeed = entry.ReadFloat() * UnitsPerM;
			info.RotationSpeed = (byte)((entry.ReadFloat() * 256) / Pi2);

			this.Entries.Add(info.RaceId, info);
		}
	}
}
