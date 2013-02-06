// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Data
{
	public class MotionInfo
	{
		public string Name;
		public ushort Category;
		public ushort Type;
	}

	public class MotionDb : DataManager<MotionInfo>
	{
		/// <summary>
		/// Searches for the entry with the given name returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MotionInfo Find(string name)
		{
			return this.Entries.FirstOrDefault(a => a.Name == name);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 3);
		}

		protected override void CsvToEntry(MotionInfo info, List<string> csv, int currentLine)
		{
			info.Name = csv[0];
			info.Category = Convert.ToUInt16(csv[1]);
			info.Type = Convert.ToUInt16(csv[2]);
		}
	}
}
