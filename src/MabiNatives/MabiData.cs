// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MabiNatives
{
	[Flags]
	public enum DataCats : ushort
	{
		Action = 0x01, Color = 0x02, Item = 0x04, Race = 0x08, Age = 0x10, PCCard = 0x20, Motion = 0x40, Portal = 0x80, Skill = 0x100, Monster = 0x200, Spawn = 0x400,
		All = 0xFFFF
	}

	public static class MabiData
	{
		public static ActionDb ActionDb = new ActionDb();
		public static ColorDb ColorDb = new ColorDb();
		public static ColorMapDb ColorMapDb = new ColorMapDb();

		public static ItemDb ItemDb = new ItemDb();
		public static RaceDb RaceDb = new RaceDb();
		public static AgeDb AgeDb = new AgeDb();
		public static PCCardDb PCCardDb = new PCCardDb();
		public static MotionDb MotionDb = new MotionDb();
		public static PortalDb PortalDb = new PortalDb();
		public static SkillDb SkillDb = new SkillDb();

		public static MonsterDb MonsterDb = new MonsterDb();
		public static SpawnDb SpawnDb = new SpawnDb();

		public static void LoadFromJSON(string dataPath, DataCats cats)
		{
			if (cats.HasFlag(DataCats.Action))
			{
				MabiData.ActionDb.LoadFromJson(dataPath + "/db/actions.txt");
			}
			if (cats.HasFlag(DataCats.Portal))
			{
				MabiData.PortalDb.LoadFromJson(dataPath + "/db/portals.txt");
			}
			if (cats.HasFlag(DataCats.Skill))
			{
				MabiData.SkillDb.LoadFromJson(dataPath + "/db/skills.txt");
			}
			if (cats.HasFlag(DataCats.Monster))
			{
				MabiData.MonsterDb.LoadFromJson(dataPath + "/db/monsters.txt");
			}
			if (cats.HasFlag(DataCats.Spawn))
			{
				MabiData.SpawnDb.LoadFromJson(dataPath + "/db/spawns.txt");
			}
			if (cats.HasFlag(DataCats.Color))
			{
				MabiData.ColorDb.LoadFromJson(dataPath + "/db/colors.txt");
				MabiData.ColorMapDb.LoadFromJson(dataPath + "/db/colormaps.txt.gz");
			}
			if (cats.HasFlag(DataCats.Item))
			{
				MabiData.ItemDb.LoadFromJson(dataPath + "/db/items.txt");
			}
			if (cats.HasFlag(DataCats.Age))
			{
				MabiData.AgeDb.LoadFromJson(dataPath + "/db/ages.txt");
			}
			if (cats.HasFlag(DataCats.Motion))
			{
				MabiData.MotionDb.LoadFromJson(dataPath + "/db/motions.txt");
			}
			if (cats.HasFlag(DataCats.Race))
			{
				MabiData.RaceDb.LoadFromJson(dataPath + "/db/races.txt");
			}
			if (cats.HasFlag(DataCats.PCCard))
			{
				MabiData.PCCardDb.LoadFromJson(dataPath + "/db/charcards.txt", dataPath + "/db/charcardsets.txt");
			}
		}
	}
}
