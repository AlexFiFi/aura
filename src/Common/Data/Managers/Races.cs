// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Constants;
using Common.Tools;

namespace Common.Data
{
	public class RaceInfo
	{
		public uint Id;
		public string Name;
		public string Group;
		public Gender Gender;
		public uint VehicleType;
		public uint DefaultState;
		public uint InvWidth, InvHeight;
		public ushort AttackSkill;
		public int AttackRange;
		public int AttackMin, AttackMax;
		public int AttackSpeed;
		public int KnockCount;
		public uint Critical;
		public uint SplashRadius, SplashAngle;
		public float SplashDamage;
		public int Stand;

		public string AI;
		public uint ColorA, ColorB, ColorC;
		public float Size;
		public float CombatPower;
		public float Life;
		public uint Defense;
		public float Protection;
		public Element Element;
		public uint Exp;

		public int GoldMin, GoldMax;
		public List<DropInfo> Drops = new List<DropInfo>();

		public float SpeedRun, SpeedWalk;
		public FlightInfo FlightInfo;
		public List<RaceSkillInfo> Skills = new List<RaceSkillInfo>();
	}

	public class DropInfo
	{
		public uint ItemId;
		public float Chance;
	}

	public class RaceDb : DataManager<RaceInfo>
	{
		/// <summary>
		/// Searches for the entry with the given Id and returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public RaceInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public List<RaceInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Name.ToLower() == name);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 24);
		}

		protected override void CsvToEntry(RaceInfo info, List<string> csv, int currentLine)
		{
			int i = 0;

			info.Id = Convert.ToUInt32(csv[i++]);
			info.Name = csv[i++];
			info.Group = csv[i++];
			info.Gender = (Gender)Convert.ToByte(csv[i++]);
			info.VehicleType = Convert.ToUInt32(csv[i++]);
			info.DefaultState = Convert.ToUInt32(csv[i++], 16);
			info.InvWidth = Convert.ToUInt32(csv[i++]);
			info.InvHeight = Convert.ToUInt32(csv[i++]);
			//info.AttackSkill = Convert.ToUInt16(csv[i++]);
			info.AttackSkill = (ushort)SkillConst.MeleeCombatMastery; // They all use this anyway.
			info.AttackMin = Convert.ToInt32(csv[i++]);
			info.AttackMax = Convert.ToInt32(csv[i++]);
			info.AttackRange = Convert.ToInt32(csv[i++]);
			info.AttackSpeed = Convert.ToInt32(csv[i++]);
			info.KnockCount = Convert.ToInt32(csv[i++]);
			info.Critical = Convert.ToUInt32(csv[i++]);
			info.SplashRadius = Convert.ToUInt32(csv[i++]);
			info.SplashAngle = Convert.ToUInt32(csv[i++]);
			info.SplashDamage = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Stand = Convert.ToInt32(csv[i++], 16);

			// Stat Info
			info.AI = csv[i++];
			info.ColorA = Convert.ToUInt32(csv[i++], 16);
			info.ColorB = Convert.ToUInt32(csv[i++], 16);
			info.ColorC = Convert.ToUInt32(csv[i++], 16);
			info.Size = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.CombatPower = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Life = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			info.Defense = Convert.ToUInt32(csv[i++], 16);
			info.Protection = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")) / 100f;
			info.Element = (Element)Convert.ToByte(csv[i++]);
			info.Exp = Convert.ToUInt32(csv[i++]);
			info.GoldMin = Convert.ToInt32(csv[i++]);
			info.GoldMax = Convert.ToInt32(csv[i++]);

			// Optional drop information
			for (; i < csv.Count; ++i)
			{
				// Drop format: <itemId>:<chance>, skip this drop if incorrect.
				var drop = csv[i].Split(':');
				if (drop.Length < 2)
				{
					Logger.Warning("Incomplete drop information for one line {0} in races db.", currentLine);
					continue;
				}

				var di = new DropInfo();
				di.ItemId = Convert.ToUInt32(drop[0]);
				di.Chance = float.Parse(drop[1], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

				di.Chance /= 100;
				if (di.Chance > 1)
					di.Chance = 1;
				else if (di.Chance < 0)
					di.Chance = 0;

				info.Drops.Add(di);
			}

			// External information from other dbs
			SpeedInfo actionInfo;
			if ((actionInfo = MabiData.SpeedDb.Find(info.Group + "/walk")) != null)
				info.SpeedWalk = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(info.Group + "/*")) != null)
				info.SpeedWalk = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(Regex.Replace(info.Group, "/.*$", "") + "/*/walk")) != null)
				info.SpeedWalk = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(Regex.Replace(info.Group, "/.*$", "") + "/*/*")) != null)
				info.SpeedWalk = actionInfo.Speed;
			else
				info.SpeedWalk = 207.6892f;

			if ((actionInfo = MabiData.SpeedDb.Find(info.Group + "/run")) != null)
				info.SpeedRun = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(info.Group + "/*")) != null)
				info.SpeedRun = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(Regex.Replace(info.Group, "/.*$", "") + "/*/run")) != null)
				info.SpeedRun = actionInfo.Speed;
			else if ((actionInfo = MabiData.SpeedDb.Find(Regex.Replace(info.Group, "/.*$", "") + "/*/*")) != null)
				info.SpeedRun = actionInfo.Speed;
			else
				info.SpeedRun = 373.850647f;

			info.FlightInfo = MabiData.FlightDb.Find(info.Id);

			info.Skills = MabiData.RaceSkillDb.FindAll(info.Id);
		}
	}

	public enum Gender : byte { None, Female, Male, Universal }
	public enum Element : byte { None, Ice, Fire, Lightning }
}
