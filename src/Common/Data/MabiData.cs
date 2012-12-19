// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
	public static class MabiData
	{
		public static SpeedDb SpeedDb = new SpeedDb();
		public static FlightDb FlightDb = new FlightDb();
		public static ColorMapDb ColorMapDb = new ColorMapDb();
		public static ItemDb ItemDb = new ItemDb();
		public static RaceDb RaceDb = new RaceDb();
		public static StatsBaseDb StatsBaseDb = new StatsBaseDb();
		public static StatsLevelUpDb StatsLevelUpDb = new StatsLevelUpDb();
		public static CharCardDb CharCardDb = new CharCardDb();
		public static CharCardSetDb CharCardSetDb = new CharCardSetDb();
		public static MotionDb MotionDb = new MotionDb();
		public static PortalDb PortalDb = new PortalDb();
		public static SkillDb SkillDb = new SkillDb();
		public static SkillRankDb SkillRankDb = new SkillRankDb();
		internal static RaceSkillDb RaceSkillDb = new RaceSkillDb();
		internal static RaceStatDb RaceStatDb = new RaceStatDb();
		public static SpawnDb SpawnDb = new SpawnDb();
		public static MapDb MapDb = new MapDb();
	}
}
