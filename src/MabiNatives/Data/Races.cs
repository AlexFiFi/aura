// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace MabiNatives
{
	public class RaceInfo
	{
		public uint Id;
		public string ClassName, EnglishName;

		public byte Gender;
		public uint InvWidth;
		public uint InvHeight;
		public int MeleeAttackRange;
		public ushort CombatSkill;

		public int DefaultAttackSpeed;
		public int DefaultDownHitCount;

		public string RaceDesc;

		public float SpeedRun, SpeedWalk;

		public uint VehicleType;
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

		public override void LoadFromXml(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found: " + filePath);

			using (var itemInfoReader = new XmlTextReader(filePath))
			{
				while (itemInfoReader.Read())
				{
					if (itemInfoReader.NodeType != XmlNodeType.Element)
						continue;

					if (itemInfoReader.Name != "Race")
						continue;

					if (string.IsNullOrEmpty(itemInfoReader.GetAttribute("ID")))
						continue;

					var race = new RaceInfo();
					race.Id = uint.Parse(itemInfoReader.GetAttribute("ID"));
					race.ClassName = itemInfoReader.GetAttribute("ClassName");
					race.EnglishName = itemInfoReader.GetAttribute("EnglishName");
					race.Gender = byte.Parse(itemInfoReader.GetAttribute("Gender"));
					race.InvWidth = uint.Parse(itemInfoReader.GetAttribute("InventorySizeX"));
					race.InvHeight = uint.Parse(itemInfoReader.GetAttribute("InventorySizeY"));
					race.MeleeAttackRange = int.Parse(itemInfoReader.GetAttribute("MeleeAttackRange"));
					race.CombatSkill = ushort.Parse(itemInfoReader.GetAttribute("CombatSkill"));

					race.DefaultAttackSpeed = int.Parse(itemInfoReader.GetAttribute("DefaultAttackSpeed"));
					race.DefaultDownHitCount = int.Parse(itemInfoReader.GetAttribute("DefaultDownHitCount"));

					race.RaceDesc = itemInfoReader.GetAttribute("RaceDesc");

					var extraData = itemInfoReader.GetAttribute("ExtraData");
					if (extraData != null)
					{
						var match = Regex.Match(extraData, "vehicle type=\"([0-9]+)\"", RegexOptions.IgnoreCase);
						if (match.Success)
						{
							race.VehicleType = Convert.ToUInt32(match.Groups[1].Value);
						}
					}

					ActionInfo actionInfo;
					if ((actionInfo = MabiData.ActionDb.Find(race.RaceDesc + "/walk")) != null)
						race.SpeedWalk = actionInfo.Speed;
					else if ((actionInfo = MabiData.ActionDb.Find(Regex.Replace(race.RaceDesc, "/.*$", "") + "/*/walk")) != null)
						race.SpeedWalk = actionInfo.Speed;
					else
						race.SpeedWalk = 207.6892f;

					if ((actionInfo = MabiData.ActionDb.Find(race.RaceDesc + "/run")) != null)
						race.SpeedRun = actionInfo.Speed;
					else if ((actionInfo = MabiData.ActionDb.Find(Regex.Replace(race.RaceDesc, "/.*$", "") + "/*/run")) != null)
						race.SpeedRun = actionInfo.Speed;
					else
						race.SpeedRun = 373.850647f;

					this.Entries.Add(race);
				}
			}
		}
	}
}
