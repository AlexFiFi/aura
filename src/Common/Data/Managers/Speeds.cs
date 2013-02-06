// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Data
{
	public class SpeedInfo
	{
		public string Ident;
		public float Speed;
	}

	/// <summary>
	/// Contains Information about walking speed of several races.
	/// This is for information purposes only, actually changing
	/// the speed would require client modifications.
	/// </summary>
	public class SpeedDb : DataManager<SpeedInfo>
	{
		/// <summary>
		/// Returns the action info for the given identifier or null.
		/// </summary>
		/// <param name="classifier"></param>
		/// <returns></returns>
		public SpeedInfo Find(string classifier)
		{
			return this.Entries.FirstOrDefault(a => a.Ident == classifier);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 2);
		}

		protected override void CsvToEntry(SpeedInfo info, List<string> csv, int currentLine)
		{
			info.Ident = csv[0];
			info.Speed = float.Parse(csv[1], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
		}
	}
}
