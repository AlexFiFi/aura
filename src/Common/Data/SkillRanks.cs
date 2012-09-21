// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace Common.Data
{
	public class SkillRankInfo
	{
		public uint SkillId;
		public uint Race;
		public byte Rank;
		public byte Ap;
		public float Cp;
		public byte Stack;
		public uint LoadTime, NewLoadTime, CoolDown;
		public float StaminaCost, StaminaPrepare, StaminaLoad, StaminaUse;
		public float ManaCost, ManaPrepare, ManaLoad, ManaUse;
		public float Life, Mana, Stamina, Str, Int, Dex, Will, Luck;
		public float Var1, Var2, Var3, Var4, Var5, Var6, Var7, Var8, Var9;
	}

	public class SkillRankDb : DataManager<SkillRankInfo>
	{
		public SkillRankInfo Find(ushort id, byte rank)
		{
			return this.Entries.FirstOrDefault(a => a.SkillId == id && a.Rank == rank);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 34);
		}

		protected override void CsvToEntry(SkillRankInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.SkillId = Convert.ToUInt32(csv[i++]);
			info.Race = Convert.ToUInt32(csv[i++]);
			info.Rank = Convert.ToByte(csv[i++]);
			info.Ap = Convert.ToByte(csv[i++]);
			info.Cp = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Stack = Convert.ToByte(csv[i++]);
			info.LoadTime = Convert.ToUInt32(csv[i++]);
			info.NewLoadTime = Convert.ToUInt32(csv[i++]);
			info.CoolDown = Convert.ToUInt32(csv[i++]);
			info.StaminaCost = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.StaminaPrepare = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.StaminaLoad = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.StaminaUse = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.ManaCost = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.ManaPrepare = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.ManaLoad = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.ManaUse = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Life = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Mana = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Stamina = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Str = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Int = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Dex = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Will = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Luck = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var1 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var2 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var3 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var4 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var5 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var6 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var7 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var8 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Var9 = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
		}
	}
}
