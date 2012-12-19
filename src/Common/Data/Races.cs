// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace Common.Data
{
	public class RaceInfo
	{
		public uint Id;
		public string Name;
		public string Group;
		public byte Gender;
		public uint VehicleType;
		public uint DefaultState;
		public uint InvWidth, InvHeight;
		public ushort AttackSkill;
		public int AttackRange;
		public int AttackMin, AttackMax;
		public int AttackSpeed;
		public int KnockCount;
		public float SpeedRun, SpeedWalk;
		public FlightInfo FlightInfo;
		public int Stand;

		// v-- Race stat info
		public string AI;
		public float Life;
		public uint Exp;
		public float Size;
		public uint ColorA, ColorB, ColorC;

		public int GoldMin, GoldMax;
		public List<DropInfo> Drops = new List<DropInfo>();

		public List<RaceSkillInfo> Skills = new List<RaceSkillInfo>();
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
			this.ReadCsv(filePath, 15);
		}

		protected override void CsvToEntry(RaceInfo info, List<string> csv, int currentLine)
		{
			int i = 0;
			info.Id = Convert.ToUInt32(csv[i++]);
			info.Name = csv[i++];
			info.Group = csv[i++];
			info.Gender = Convert.ToByte(csv[i++]);
			info.VehicleType = Convert.ToUInt32(csv[i++]);
			info.DefaultState = Convert.ToUInt32(csv[i++], 16);
			info.InvWidth = Convert.ToUInt32(csv[i++]);
			info.InvHeight = Convert.ToUInt32(csv[i++]);
			info.AttackSkill = Convert.ToUInt16(csv[i++]);
			info.AttackMin = Convert.ToInt32(csv[i++]);
			info.AttackMax = Convert.ToInt32(csv[i++]);
			info.AttackRange = Convert.ToInt32(csv[i++]);
			info.AttackSpeed = Convert.ToInt32(csv[i++]);
			info.KnockCount = Convert.ToInt32(csv[i++]);
			info.Stand = Convert.ToInt32(csv[i++], 16);

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

			var statInfo = MabiData.RaceStatDb.Find(info.Id);
			if (statInfo == null)
			{
				return;
			}


			// Maybe not the most elegant way...
			info.AI = statInfo.AI;
			info.Life = statInfo.Life;
			info.Exp = statInfo.Exp;
			info.Size = statInfo.Size;
			info.ColorA = statInfo.ColorA;
			info.ColorB = statInfo.ColorB;
			info.ColorC = statInfo.ColorC;
			info.GoldMin = statInfo.GoldMin;
			info.GoldMax = statInfo.GoldMax;
			info.Drops = statInfo.Drops;
			info.Skills = statInfo.Skills;
		}
	}
}
