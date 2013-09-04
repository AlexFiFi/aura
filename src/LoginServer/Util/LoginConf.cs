// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Util;
using Aura.Data;

namespace Aura.Login.Util
{
	public static class LoginConf
	{
		public static LogLevel ConsoleFilter = LogLevel.None;

		// Inter
		public static string Password;

		// Data
		public static string DataPath;
		public static string Localization;

		// Database
		public static string DatabaseHost;
		public static string DatabaseUser;
		public static string DatabasePass;
		public static string DatabaseDb;

		// Login
		public static ushort Port;
		public static bool ConsumeCards;
		public static bool NewAccounts;
		public static uint SpawnRegion, SpawnX, SpawnY;
		public static int DeletionWait;
		public static bool EnableSecondaryPassword;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/login.conf");

			if (args != null)
				_conf.ReadArguments(args, "../../");

			LoginConf.ConsoleFilter = (LogLevel)_conf.GetInt("login.consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			LoginConf.ConsoleFilter &= ~LogLevel.Debug;
#endif

			LoginConf.Password = _conf.GetString("inter.password", "aura");

			LoginConf.DataPath = _conf.GetString("data.path", "../../data");
			LoginConf.Localization = _conf.GetString("data.localization", "us");

			LoginConf.DatabaseHost = _conf.GetString("database.host", "localhost");
			LoginConf.DatabaseUser = _conf.GetString("database.user", "root");
			LoginConf.DatabasePass = _conf.GetString("database.pass", "");
			LoginConf.DatabaseDb = _conf.GetString("database.db", "aura");

			LoginConf.Port = _conf.Get<ushort>("login.port", 11000);

			LoginConf.ConsumeCards = _conf.GetBool("login.consumecards", true);
			LoginConf.NewAccounts = _conf.GetBool("login.newaccounts", true);

			LoginConf.DeletionWait = _conf.Get<int>("login.deletewait", 107);
			if (LoginConf.DeletionWait < 0 || (LoginConf.DeletionWait > 23 && LoginConf.DeletionWait < 100) || LoginConf.DeletionWait > 123)
			{
				Logger.Warning("Invalid format for 'login.deletewait', setting to 0.");
				LoginConf.DeletionWait = 0;
			}

			LoginConf.EnableSecondaryPassword = _conf.GetBool("login.enable_sec", true);
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

			var spawn = _conf.GetString("login.spawn").Split(',');
			if (spawn.Length == 3)
			{
				uint region = 0;

				if (!uint.TryParse(spawn[0], out region))
				{
					// Not numeric, check map names
					region = MabiData.RegionDb.TryGetRegionId(spawn[0]);
					if (region == 0)
						Logger.Warning("login.conf, login.spawn : Map '{0}' not found.", spawn[0]);
				}

				if (region > 0)
				{
					LoginConf.SpawnRegion = region;
					if (!uint.TryParse(spawn[1], out LoginConf.SpawnX))
						Logger.Warning("login.conf, login.spawn : Invalid format for 'X'.");
					if (!uint.TryParse(spawn[2], out LoginConf.SpawnY))
						Logger.Warning("login.conf, login.spawn : Invalid format for 'Y'.");
				}
			}
		}
	}
}
