﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Aura.Shared.Util
{
	public class MabiTime
	{
		/// <summary>
		/// 1,500ms (1.5 seconds)
		/// </summary>
		protected const int TICKS_PER_MINUTE = 15000000;
		/// <summary>
		/// 90,000ms (1.5 minutes)
		/// </summary>
		protected const int TICKS_PER_HOUR = TICKS_PER_MINUTE * 60;

		/// <summary>
		/// Erinn months in English, starting on Imbolic (Sunday).
		/// </summary>
		protected static readonly string[] Months = new string[] { "Imbolic", "Alban Eiler", "Baltane", "Alban Heruin", "Lughnasadh", "Alban Elved", "Samhain" };

		/// <summary>
		/// Release of KR.
		/// </summary>
		protected static readonly DateTime BeginOfTime = DateTime.Parse("2004-06-22");

		/// <summary>
		/// Erinn hour of this instance.
		/// </summary>
		public int Hour { get; protected set; }
		/// <summary>
		/// Erinn minute of this instance.
		/// </summary>
		public int Minute { get; protected set; }
		/// <summary>
		/// Erinn year of this instance.
		/// </summary>
		public int Year { get; protected set; }
		/// <summary>
		/// Erinn month of this instance.
		/// </summary>
		public int Month { get; protected set; }
		/// <summary>
		/// Erinn day of this instance.
		/// </summary>
		public int Day { get; protected set; }

		/// <summary>
		/// DateTime object used by this instance.
		/// </summary>
		public DateTime DateTime { get; protected set; }

		/// <summary>
		/// Time stamp used in packets.
		/// </summary>
		public long MabiTimeStamp { get { return this.DateTime.Ticks / 10000; } }

		/// <summary>
		/// Returns a new MabiTime instance based on the current time.
		/// </summary>
		public static MabiTime Now { get { return new MabiTime(); } }

		/// <summary>
		/// Returns true if the Erinn hour of this instance is between 6:00pm and 5:59am.
		/// </summary>
		public bool IsNight { get { return (this.Hour >= 18 || this.Hour < 6); } }

		/// <summary>
		/// Returns true if time of this instance is 0:00am.
		/// </summary>
		public bool IsMidnight { get { return (this.Hour == 0 && this.Minute == 0); } }

		public MabiTime() : this(DateTime.Now) { }

		public MabiTime(DateTime dt)
		{
			this.DateTime = dt;
			this.Hour = (byte)((this.DateTime.Ticks / TICKS_PER_HOUR) % 24);
			this.Minute = (byte)((this.DateTime.Ticks / TICKS_PER_MINUTE) % 60);

			// Based on the theory that 1 year (1 week realtime) consts of
			// 7 months (7 days) with 40 days (1440 / 36 min) each.
			this.Year = (int)Math.Floor((this.DateTime.Ticks - BeginOfTime.Ticks) / TICKS_PER_MINUTE / 60 / 24 / 280f);
			this.Month = (int)this.DateTime.DayOfWeek;
			this.Day = (int)Math.Ceiling((this.DateTime.Hour * 60 + this.DateTime.Minute) / 36f);
		}

		/// <summary>
		/// Returns a string with the Erinn time of this instance in AM/PM.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.ToString("ampm");
		}

		/// <summary>
		/// Returns a string with the Erinn time of this instance.
		/// Formats: "24", "ampm"
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToString(string format)
		{
			switch (format)
			{
				default:
				case "24":
					return this.Hour.ToString("00") + ":" + this.Minute.ToString("00");
				case "ampm":
					var h = this.Hour % 12;
					if (this.Hour == 12)
						h = 12;
					return h.ToString("00") + ":" + this.Minute.ToString("00") + (this.Hour < 12 ? " A.M." : " P.M.");
			}
		}
	}
}
