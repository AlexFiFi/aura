// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Tools;
using Common.Data;

namespace Login.Tools
{
	public static class LoginConf
	{
		public static Logger.LogLevel ConsoleFilter = Logger.LogLevel.None;

		public static string DataPath;

		public static string DatabaseHost;
		public static string DatabaseUser;
		public static string DatabasePass;
		public static string DatabaseDb;

		public static bool ConsumeCards;
		public static bool NewAccounts;

		public static uint SpawnRegion, SpawnX, SpawnY;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/login.conf");
			_conf.ReadArguments(args, "../../");

			LoginConf.ConsoleFilter = (Logger.LogLevel)_conf.GetInt("login_consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			LoginConf.ConsoleFilter &= ~Logger.LogLevel.Debug;
#endif

			LoginConf.DataPath = _conf.GetString("data_path", "../../data");

			LoginConf.DatabaseHost = _conf.GetString("database_host", "localhost");
			LoginConf.DatabaseUser = _conf.GetString("database_user", "root");
			LoginConf.DatabasePass = _conf.GetString("database_pass", "");
			LoginConf.DatabaseDb = _conf.GetString("database_db", "aura");

			LoginConf.ConsumeCards = _conf.GetBool("login_consumecards", true);
			LoginConf.NewAccounts = _conf.GetBool("login_newaccounts", true);
		}


		/// <summary>
		/// Option parsing that has to be done after data loading.
		/// </summary>
		public static void LoadRound2()
		{
			// Spawn point
			// Default: Tir square
			LoginConf.SpawnRegion = 1;
			LoginConf.SpawnX = 12800;
			LoginConf.SpawnY = 38100;

			var spawn = _conf.GetString("login_spawn").Split(',');
			if (spawn.Length == 3)
			{
				uint region = 0;

				if (!uint.TryParse(spawn[0], out region))
				{
					// Not numeric, check map names
					var mapInfo = MabiData.MapDb.Find(spawn[0]);
					if (mapInfo != null)
						region = mapInfo.Id;
					else
						Logger.Warning("login_spawn : Map '" + spawn[0] + "' not found.");
				}

				if (region > 0)
				{
					LoginConf.SpawnRegion = region;
					if (!uint.TryParse(spawn[1], out LoginConf.SpawnX))
						Logger.Warning("login_spawn : Invalid format for 'X'.");
					if (!uint.TryParse(spawn[2], out LoginConf.SpawnY))
						Logger.Warning("login_spawn : Invalid format for 'Y'.");
				}
			}
		}
	}
}
