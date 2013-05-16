// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Aura.Data
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
		public RaceStands Stand;

		public string AI;
		public uint ColorA, ColorB, ColorC;
		public float Size;
		public float CombatPower;
		public float Life;
		public int Defense;
		public int Protection;
		public Element Element;
		public uint Exp;

		public int GoldMin, GoldMax;
		public List<DropInfo> Drops = new List<DropInfo>();

		public float SpeedRun, SpeedWalk;
		public FlightInfo FlightInfo;
		public List<RaceSkillInfo> Skills = new List<RaceSkillInfo>();

		public bool Is(RaceStands stand)
		{
			return (this.Stand & stand) != 0;
		}
	}

	public class DropInfo
	{
		public uint ItemId;
		public float Chance;
	}

	public enum RaceStands : int
	{
		KnockBackable = 0x01,
		KnockDownable = 0x02
	}

	/// <summary>
	/// Indexed by race id.
	/// Depends on: SpeedDb, FlightDb, RaceSkillDb
	/// </summary>
	public class RaceDb : DatabaseCSVIndexed<uint, RaceInfo>
	{
		public List<RaceInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Value.Name.ToLower() == name);
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 24)
				throw new FieldCountException(24);

			var info = new RaceInfo();
			info.Id = entry.ReadUInt();
			info.Name = entry.ReadString();
			info.Group = entry.ReadString();
			info.Gender = (Gender)entry.ReadUByte();
			info.VehicleType = entry.ReadUInt();
			info.DefaultState = entry.ReadUIntHex();
			info.InvWidth = entry.ReadUInt();
			info.InvHeight = entry.ReadUInt();
			info.AttackSkill = 23002; // Combat Mastery, they all use this anyway.
			info.AttackMin = entry.ReadSInt();
			info.AttackMax = entry.ReadSInt();
			info.AttackRange = entry.ReadSInt();
			info.AttackSpeed = entry.ReadSInt();
			info.KnockCount = entry.ReadSInt();
			info.Critical = entry.ReadUInt();
			info.SplashRadius = entry.ReadUInt();
			info.SplashAngle = entry.ReadUInt();
			info.SplashDamage = entry.ReadFloat();
			info.Stand = (RaceStands)entry.ReadSIntHex();

			// Stat Info
			info.AI = entry.ReadString();
			info.ColorA = entry.ReadUIntHex();
			info.ColorB = entry.ReadUIntHex();
			info.ColorC = entry.ReadUIntHex();
			info.Size = entry.ReadFloat();
			info.CombatPower = entry.ReadFloat();
			info.Life = entry.ReadFloat();
			info.Defense = entry.ReadSIntHex();
			info.Protection = (int)entry.ReadFloat();
			info.Element = (Element)entry.ReadUByte();
			info.Exp = entry.ReadUInt();
			info.GoldMin = entry.ReadSInt();
			info.GoldMax = entry.ReadSInt();

			// Optional drop information
			while (!entry.End)
			{
				// Drop format: <itemId>:<chance>, skip this drop if incorrect.
				var drop = entry.ReadString().Split(':');
				if (drop.Length != 2)
					throw new DatabaseWarningException("Incomplete drop information.");

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

			if (this.Entries.ContainsKey(info.Id))
				throw new DatabaseWarningException("Duplicate: " + info.Id.ToString());
			this.Entries.Add(info.Id, info);
		}
	}

	public enum Gender : byte { None, Female, Male, Universal }
	public enum Element : byte { None, Ice, Fire, Lightning }
}
