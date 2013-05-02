// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using Aura.Data;
using Aura.Shared.Database;
using Aura.Shared.Util;

namespace Aura.Shared.Network
{
	/// <summary>
	/// Includes methods needed for all servers, mainly used on start up.
	/// </summary>
	public static class ServerUtil
	{
		/// <summary>
		/// Prints Aura's standard header.
		/// </summary>
		/// <param name="title">Name of this server. Example: "Login Server"</param>
		public static void WriteHeader(string title = null, ConsoleColor color = ConsoleColor.DarkGray)
		{
			if (title != null)
				Console.Title = "Aura : " + title;

			Console.ForegroundColor = color;
			Console.Write(@"                          __     __  __  _ __    __                             ");
			Console.Write(@"                        /'__`\  /\ \/\ \/\`'__\/'__`\                           ");
			Console.Write(@"                       /\ \L\.\_\ \ \_\ \ \ \//\ \L\.\_                         ");
			Console.Write(@"                       \ \__/.\_\\ \____/\ \_\\ \__/.\_\                        ");
			Console.Write(@"                        \/__/\/_/ \/___/  \/_/ \/__/\/_/                        ");
			Console.Write(@"                                                                                ");

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@"                         by the Aura development team                           ");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(@"________________________________________________________________________________");

			Console.WriteLine("");
		}

		/// <summary>
		/// Waits for the return key, and closes the application afterwards.
		/// </summary>
		/// <param name="exitCode"></param>
		public static void Exit(int exitCode = 0, bool wait = true)
		{
			if (wait)
			{
				Logger.Info("Press Enter to exit.");
				Console.ReadLine();
			}
			Logger.Info("Exiting...");
			Environment.Exit(exitCode);
		}

		/// <summary>
		/// Initializes MabiDb and tries to connect,
		/// calls Exit if there are any problems.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="user"></param>
		/// <param name="pass"></param>
		/// <param name="db"></param>
		public static void TryConnectToDatabase(string host, string user, string pass, string db)
		{
			try
			{
				MabiDb.Instance.Init(host, user, pass, db);
				MabiDb.Instance.TestConnection();
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to connect to database. ({0})", ex.Message);
				Exit(1);
			}
		}

		/// <summary>
		/// (Re-)Loads the data with the data path as base, called on server
		/// start and with some reload commands. Should only load required data,
		/// e.g. Msgr Server doesn't need race data.
		/// Calls Exit if there are any problems.
		/// </summary>
		/// <param name="dataPath"></param>
		/// <param name="toLoad"></param>
		/// <param name="reload"></param>
		public static void LoadData(string dataPath, DataLoad toLoad = DataLoad.All, bool reload = false)
		{
			try
			{
				if ((toLoad & DataLoad.Spawns) != 0)
				{
					MabiData.SpawnDb.Load(dataPath + "/db/spawns.txt", reload);
					PrintDataWarnings(MabiData.SpawnDb.Warnings);

					Logger.Info("Done loading " + MabiData.SpawnDb.Count + " entries from spawns.txt.");
				}

				if ((toLoad & DataLoad.Races) != 0)
				{
					MabiData.RaceSkillDb.Load(dataPath + "/db/race_skills.txt", reload);
					PrintDataWarnings(MabiData.RaceSkillDb.Warnings);

					Logger.Info("Done loading " + MabiData.RaceSkillDb.Count + " entries from race_skills.txt.");

					MabiData.SpeedDb.Load(dataPath + "/db/speed.txt", reload);
					PrintDataWarnings(MabiData.SpeedDb.Warnings);

					MabiData.FlightDb.Load(dataPath + "/db/flight.txt", reload);
					PrintDataWarnings(MabiData.FlightDb.Warnings);

					MabiData.RaceDb.Load(dataPath + "/db/races.txt", reload);
					PrintDataWarnings(MabiData.RaceDb.Warnings);

					Logger.Info("Done loading " + MabiData.RaceDb.Count + " entries from races.txt.");
				}

				if ((toLoad & DataLoad.StatsBase) != 0)
				{
					MabiData.StatsBaseDb.Load(dataPath + "/db/stats_base.txt", reload);
					PrintDataWarnings(MabiData.StatsBaseDb.Warnings);

					Logger.Info("Done loading " + MabiData.StatsBaseDb.Count + " entries from stats_base.txt.");
				}

				if ((toLoad & DataLoad.StatsLevel) != 0)
				{
					MabiData.StatsLevelUpDb.Load(dataPath + "/db/stats_levelup.txt", reload);
					PrintDataWarnings(MabiData.StatsLevelUpDb.Warnings);

					Logger.Info("Done loading " + MabiData.StatsLevelUpDb.Count + " entries from stats_levelup.txt.");
				}

				if ((toLoad & DataLoad.Motions) != 0)
				{
					MabiData.MotionDb.Load(dataPath + "/db/motions.txt", reload);
					PrintDataWarnings(MabiData.MotionDb.Warnings);

					Logger.Info("Done loading " + MabiData.MotionDb.Count + " entries from motions.txt.");
				}

				if ((toLoad & DataLoad.Cards) != 0)
				{
					MabiData.CharCardSetDb.Load(dataPath + "/db/charcardsets.txt", reload);
					PrintDataWarnings(MabiData.CharCardSetDb.Warnings);

					MabiData.CharCardDb.Load(dataPath + "/db/charcards.txt", reload);
					PrintDataWarnings(MabiData.CharCardDb.Warnings);

					Logger.Info("Done loading " + MabiData.CharCardSetDb.Count + " entries from charcardsets.txt.");
				}

				if ((toLoad & DataLoad.Colors) != 0)
				{
					MabiData.ColorMapDb.Load(dataPath + "/db/colormap.dat", reload);
					PrintDataWarnings(MabiData.ColorMapDb.Warnings);

					Logger.Info("Done loading " + MabiData.ColorMapDb.Count + " entries from colormap.dat.");
				}

				if ((toLoad & DataLoad.Items) != 0)
				{
					MabiData.ItemDb.Load(dataPath + "/db/items.txt", reload);
					PrintDataWarnings(MabiData.ItemDb.Warnings);

					Logger.Info("Done loading " + MabiData.ItemDb.Count + " entries from items.txt.");
				}

				if ((toLoad & DataLoad.Skills) != 0)
				{
					MabiData.SkillRankDb.Load(dataPath + "/db/skill_ranks.txt", reload);
					PrintDataWarnings(MabiData.SkillRankDb.Warnings);

					MabiData.SkillDb.Load(dataPath + "/db/skills.txt", reload);
					PrintDataWarnings(MabiData.SkillDb.Warnings);

					Logger.Info("Done loading " + MabiData.SkillDb.Count + " entries from skills.txt.");
				}

				if ((toLoad & DataLoad.Regions) != 0)
				{
					MabiData.MapDb.Load(dataPath + "/db/maps.txt", reload);
					PrintDataWarnings(MabiData.MapDb.Warnings);

					Logger.Info("Done loading " + MabiData.MapDb.Count + " entries from maps.txt.");

					MabiData.RegionDb.Load(dataPath + "/db/regioninfo.dat", reload);
					PrintDataWarnings(MabiData.RegionDb.Warnings);

					Logger.Info("Done loading " + MabiData.RegionDb.Count + " entries from regioninfo.dat.");
				}

				if ((toLoad & DataLoad.Shamala) != 0)
				{
					MabiData.ShamalaDb.Load(dataPath + "/db/shamala.txt", reload);
					PrintDataWarnings(MabiData.ShamalaDb.Warnings);

					Logger.Info("Done loading " + MabiData.ShamalaDb.Count + " entries from shamala.txt.");
				}

				if ((toLoad & DataLoad.PropDrops) != 0)
				{
					MabiData.PropDropDb.Load(dataPath + "/db/prop_drops.txt", reload);
					PrintDataWarnings(MabiData.PropDropDb.Warnings);

					Logger.Info("Done loading " + MabiData.PropDropDb.Count + " entries from prop_drops.txt.");
				}

				if ((toLoad & DataLoad.Exp) != 0)
				{
					MabiData.ExpDb.Load(dataPath + "/db/exp.txt", reload);
					PrintDataWarnings(MabiData.PropDropDb.Warnings);

					Logger.Info("Done loading " + MabiData.ExpDb.Count + " levels from exp.txt.");
				}

				if ((toLoad & DataLoad.Pets) != 0)
				{
					MabiData.PetDb.Load(dataPath + "/db/pets.txt", reload);
					PrintDataWarnings(MabiData.PetDb.Warnings);

					Logger.Info("Done loading " + MabiData.PetDb.Count + " entries from pets.txt.");
				}

				if ((toLoad & DataLoad.Weather) != 0)
				{
					MabiData.WeatherDb.Load(dataPath + "/db/weather.txt", reload);
					PrintDataWarnings(MabiData.WeatherDb.Warnings);

					Logger.Info("Done loading " + MabiData.WeatherDb.Count + " entries from weather.txt.");
				}
			}
			catch (FileNotFoundException ex)
			{
				Logger.Error(ex.Message);
				Exit(1);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, null, true);
				Exit(1);
			}
		}

		/// <summary>
		/// Used by LoadData, prints the warnings potentially received by
		/// loading data.
		/// </summary>
		/// <param name="warnings"></param>
		public static void PrintDataWarnings(List<DatabaseWarningException> warnings)
		{
			foreach (var ex in warnings)
			{
				Logger.Warning(ex.ToString());
			}
		}

		/// <summary>
		/// Reads input from the console and passes it to ParseCommand,
		/// which can be overriden by deriving servers.
		/// </summary>
		/// <param name="stopCommand"></param>
		public static void ReadCommands(Action<string[], string> parseCommand)
		{
			var input = string.Empty;

			do
			{
				input = Console.ReadLine();
				var splitted = input.Split(' ');
				parseCommand(splitted, input);
			}
			while (input != "exit");
		}

		public static void CheckInterPassword(string pass)
		{
			if (pass == "aura")
			{
				Logger.Warning("Using the default inter server password is risky, please change it in 'conf/inter.conf'.");
				//Exit(1);
			}
		}
	}

	/// <summary>
	/// Used in LoadData, to specify what data files should be loaded.
	/// </summary>
	public enum DataLoad
	{
		Spawns = 0x01,
		Skills = 0x02,
		Races = 0x04,
		StatsBase = 0x08,
		StatsLevel = 0x10,
		Motions = 0x20,
		Cards = 0x40,
		Colors = 0x80,
		Items = 0x100,
		Regions = 0x200,
		Shamala = 0x400,
		PropDrops = 0x800,
		Exp = 0x1000,
		Pets = 0x2000,
		Weather = 0x4000,

		All = 0xFFFF,

		LoginServer = Skills | Races | StatsBase | StatsLevel | Cards | Colors | Items | Regions | Pets,
		WorldServer = All,
		Npcs = Races | Spawns,
	}
}
