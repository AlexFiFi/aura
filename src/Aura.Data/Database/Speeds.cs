// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Data
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
	/// Indexed by group identification.
	/// </summary>
	public class SpeedDb : DatabaseCSVIndexed<string, SpeedInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 2)
				throw new FieldCountException(2);

			var info = new SpeedInfo();
			info.Ident = entry.ReadString();
			info.Speed = entry.ReadFloat();

			if (this.Entries.ContainsKey(info.Ident))
				throw new DatabaseWarningException("Duplicate: " + info.Ident);
			this.Entries.Add(info.Ident, info);
		}
	}
}
