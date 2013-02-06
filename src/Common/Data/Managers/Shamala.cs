// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Common.Tools;

namespace Common.Data
{
	public class ShamalaInfo
	{
		private static Random _rnd = RandomProvider.Get();

		public uint Id;
		public string Name;
		public string Category;
		public byte Rank = 1;
		public float Rate = 100;
		public byte Required = 1;
		public float Size = 1f;
		public uint Color1 = 0x808080, Color2 = 0x808080, Color3 = 0x808080;
		public List<uint> Races = new List<uint>();

		/// <summary>
		/// Returns a random race id from this transformation's races list.
		/// </summary>
		public uint Race { get { return this.Races[_rnd.Next(Races.Count)]; } }
	}

	public class ShamalaDb : DataManager<ShamalaInfo>
	{
		public ShamalaInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 11);
		}

		protected override void CsvToEntry(ShamalaInfo info, List<string> csv, int currentLine)
		{
			int i = 0;

			info.Id = Convert.ToUInt32(csv[i++]);
			info.Name = csv[i++];
			info.Category = csv[i++];
			info.Rank = Convert.ToByte(csv[i++]);
			info.Rate = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Required = Convert.ToByte(csv[i++]);
			info.Size = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Color1 = Convert.ToUInt32(csv[i++].Replace("0x", ""), 16);
			info.Color2 = Convert.ToUInt32(csv[i++].Replace("0x", ""), 16);
			info.Color3 = Convert.ToUInt32(csv[i++].Replace("0x", ""), 16);

			var races = csv[i++].Split(':');
			foreach (var race in races)
				info.Races.Add(Convert.ToUInt32(race));
		}
	}
}
