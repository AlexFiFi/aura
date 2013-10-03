// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Data
{
	public class MotionInfo
	{
		public string Name;
		public ushort Category;
		public ushort Type;
		public bool Loop;
	}

	/// <summary>
	/// Indexed by motion name.
	/// </summary>
	public class MotionDb : DatabaseCSVIndexed<string, MotionInfo>
	{
		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 4)
				throw new FieldCountException(4);

			var info = new MotionInfo();
			info.Name = entry.ReadString();
			info.Category = entry.ReadUShort();
			info.Type = entry.ReadUShort();
			info.Loop = entry.ReadBool();

			this.Entries.Add(info.Name, info);
		}
	}
}
