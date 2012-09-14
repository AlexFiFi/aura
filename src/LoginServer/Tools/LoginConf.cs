// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Tools;

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
			LoginConf.DatabaseDb = _conf.GetString("database_database", "aura");

			LoginConf.ConsumeCards = _conf.GetBool("login_consumecards", true);
			LoginConf.NewAccounts = _conf.GetBool("login_newaccounts", true);
		}
	}
}
