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
					LoadDB(MabiData.SpawnDb, dataPath + "/db/spawns.txt", reload);

					LoadDB(MabiData.AncientDropDb, dataPath + "/db/ancient_drops.txt", reload);
				}

				if ((toLoad & DataLoad.Races) != 0)
				{
					LoadDB(MabiData.RaceSkillDb, dataPath + "/db/race_skills.txt", reload);

					LoadDB(MabiData.SpeedDb, dataPath + "/db/speed.txt", reload, false);

					LoadDB(MabiData.FlightDb, dataPath + "/db/flight.txt", reload, false);

					LoadDB(MabiData.RaceDb, dataPath + "/db/races.txt", reload);
				}

				if ((toLoad & DataLoad.StatsBase) != 0)
				{
					LoadDB(MabiData.StatsBaseDb, dataPath + "/db/stats_base.txt", reload);
				}

				if ((toLoad & DataLoad.StatsLevel) != 0)
				{
					LoadDB(MabiData.StatsLevelUpDb, dataPath + "/db/stats_levelup.txt", reload);
				}

				if ((toLoad & DataLoad.Motions) != 0)
				{
					LoadDB(MabiData.MotionDb, dataPath + "/db/motions.txt", reload);
				}

				if ((toLoad & DataLoad.Cards) != 0)
				{
					LoadDB(MabiData.CharCardSetDb, dataPath + "/db/charcardsets.txt", reload, false);

					LoadDB(MabiData.CharCardDb, dataPath + "/db/charcards.txt", reload);
				}

				if ((toLoad & DataLoad.Colors) != 0)
				{
					LoadDB(MabiData.ColorMapDb, dataPath + "/db/colormap.dat", reload);
				}

				if ((toLoad & DataLoad.Items) != 0)
				{
					LoadDB(MabiData.ItemDb, dataPath + "/db/items.txt", reload);
				}

				if ((toLoad & DataLoad.Skills) != 0)
				{
					LoadDB(MabiData.SkillRankDb, dataPath + "/db/skill_ranks.txt", reload, false);

					LoadDB(MabiData.SkillDb, dataPath + "/db/skills.txt", reload);

					LoadDB(MabiData.TalentExpDb, dataPath + "/db/talent_exp.txt", reload);

					LoadDB(MabiData.TalentRankDb, dataPath + "/db/talent_ranks.txt", reload, false);
				}

				if ((toLoad & DataLoad.Regions) != 0)
				{
					LoadDB(MabiData.MapDb, dataPath + "/db/maps.txt", reload);

					LoadDB(MabiData.RegionDb, dataPath + "/db/regioninfo.dat", reload);
				}

				if ((toLoad & DataLoad.Shamala) != 0)
				{
					LoadDB(MabiData.ShamalaDb, dataPath + "/db/shamala.txt", reload);
				}

				if ((toLoad & DataLoad.PropDrops) != 0)
				{
					LoadDB(MabiData.PropDropDb, dataPath + "/db/prop_drops.txt", reload);
				}

				if ((toLoad & DataLoad.Exp) != 0)
				{
					LoadDB(MabiData.ExpDb, dataPath + "/db/exp.txt", reload);
				}

				if ((toLoad & DataLoad.Pets) != 0)
				{
					LoadDB(MabiData.PetDb, dataPath + "/db/pets.txt", reload);
				}

				if ((toLoad & DataLoad.Weather) != 0)
				{
					LoadDB(MabiData.WeatherDb, dataPath + "/db/weather.txt", reload);
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

		private static void LoadDB(IDatabase db, string path, bool reload, bool log = true)
		{
			db.Load(path, reload);
			PrintDataWarnings(db.Warnings);

			if (log)
				Logger.Info("Done loading {0} entries from {1}.", db.Count, Path.GetFileName(path));
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
