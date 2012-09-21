// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System;
using Common.Tools;

namespace Common.Data
{
	public class PortalInfo
	{
		public ulong Id;
		public uint Region, X, Y;
		public uint ToRegion, ToX, ToY;
	}

	/// <summary>
	/// Contains information about portals, where they are, and where they go.
	/// </summary>
	public class PortalDb : DataManager<PortalInfo>
	{
		/// <summary>
		/// Returns the portal with the given Id or null.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PortalInfo Find(ulong id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 7);
		}

		protected override void CsvToEntry(PortalInfo info, List<string> csv, int currentLine)
		{
			info.Id = Convert.ToUInt64(csv[0]);
			info.Region = Convert.ToUInt32(csv[1]);
			info.X = Convert.ToUInt32(csv[2]);
			info.Y = Convert.ToUInt32(csv[3]);
			info.ToRegion = Convert.ToUInt32(csv[4]);
			info.ToX = Convert.ToUInt32(csv[5]);
			info.ToY = Convert.ToUInt32(csv[6]);
		}
	}
}
