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
		public static ColorMapDb ColorMapDb = new ColorMapDb();
		public static ItemDb ItemDb = new ItemDb();
		public static RaceDb RaceDb = new RaceDb();
		public static StatsBaseDb StatsBaseDb = new StatsBaseDb();
		public static CharCardDb CharCardDb = new CharCardDb();
		public static CharCardSetDb CharCardSetDb = new CharCardSetDb();
		public static MotionDb MotionDb = new MotionDb();
		public static PortalDb PortalDb = new PortalDb();
		public static SkillDb SkillDb = new SkillDb();
		public static SkillRankDb SkillRankDb = new SkillRankDb();
		public static MonsterSkillDb MonsterSkillDb = new MonsterSkillDb();
		public static MonsterDb MonsterDb = new MonsterDb();
		public static SpawnDb SpawnDb = new SpawnDb();
	}

	public enum DataLoad { Npcs = 0x01, Data = 0x02, All = 0xFFFF }
}
