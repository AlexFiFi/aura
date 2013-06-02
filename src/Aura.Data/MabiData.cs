﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Data
{
	/// <summary>
	/// Easy access static wrapper for all file databases.
	/// </summary>
	public static class MabiData
	{
		public static AncientDropDb AncientDropDb = new AncientDropDb();
		public static CharCardDb CharCardDb = new CharCardDb();
		public static CharCardSetDb CharCardSetDb = new CharCardSetDb();
		public static ColorMapDb ColorMapDb = new ColorMapDb();
		public static ExpDb ExpDb = new ExpDb();
		public static FlightDb FlightDb = new FlightDb();
		public static ItemDb ItemDb = new ItemDb();
		public static MapDb MapDb = new MapDb();
		public static MotionDb MotionDb = new MotionDb();
		public static PetDb PetDb = new PetDb();
		public static PropDropDb PropDropDb = new PropDropDb();
		public static QuestDb QuestDb = new QuestDb();
		public static RaceDb RaceDb = new RaceDb();
		public static RaceSkillDb RaceSkillDb = new RaceSkillDb();
		public static RegionDb RegionDb = new RegionDb();
		public static ShamalaDb ShamalaDb = new ShamalaDb();
		public static SkillDb SkillDb = new SkillDb();
		public static SkillRankDb SkillRankDb = new SkillRankDb();
		public static SpawnDb SpawnDb = new SpawnDb();
		public static SpeedDb SpeedDb = new SpeedDb();
		public static StatsBaseDb StatsBaseDb = new StatsBaseDb();
		public static StatsLevelUpDb StatsLevelUpDb = new StatsLevelUpDb();
		public static TalentExpDb TalentExpDb = new TalentExpDb();
		public static TalentRankDb TalentRankDb = new TalentRankDb();
		public static WeatherDb WeatherDb = new WeatherDb();
	}
}
