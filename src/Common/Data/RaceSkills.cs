// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Common.Data
{
	public class RaceSkillInfo
	{
		public uint MonsterId;
		public ushort SkillId;
		public byte Rank;
	}

	internal class RaceSkillDb : DataManager<RaceSkillInfo>
	{
		public List<RaceSkillInfo> FindAll(uint id)
		{
			return this.Entries.FindAll(a => a.MonsterId == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 2);
		}

		protected override void CsvToEntry(RaceSkillInfo info, List<string> csv, int currentLine)
		{
			info.MonsterId = Convert.ToUInt32(csv[0]);
			info.SkillId = Convert.ToUInt16(csv[1]);

			var rank = Convert.ToByte(csv[2], 16);
			info.Rank = Convert.ToByte(16 - rank);
		}
	}
}
