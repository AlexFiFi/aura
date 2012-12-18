// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Tools;

namespace Msgr.Tools
{
	public static class MsgrConf
	{
		public static Logger.LogLevel ConsoleFilter = Logger.LogLevel.None;

		public static string DatabaseHost;
		public static string DatabaseUser;
		public static string DatabasePass;
		public static string DatabaseDb;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/msgr.conf");

			if (args != null)
				_conf.ReadArguments(args, "../../");

			MsgrConf.ConsoleFilter = (Logger.LogLevel)_conf.GetInt("msgr_consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			MsgrConf.ConsoleFilter &= ~Logger.LogLevel.Debug;
#endif

			MsgrConf.DatabaseHost = _conf.GetString("database_host", "localhost");
			MsgrConf.DatabaseUser = _conf.GetString("database_user", "root");
			MsgrConf.DatabasePass = _conf.GetString("database_pass", "");
			MsgrConf.DatabaseDb = _conf.GetString("database_db", "aura");
		}
	}
}
