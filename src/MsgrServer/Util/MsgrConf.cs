// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Util;

namespace Aura.Msgr.Util
{
	public static class MsgrConf
	{
		public static LogLevel ConsoleFilter = LogLevel.None;

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

			MsgrConf.ConsoleFilter = (LogLevel)_conf.GetInt("msgr.consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			MsgrConf.ConsoleFilter &= ~LogLevel.Debug;
#endif

			MsgrConf.DatabaseHost = _conf.GetString("database.host", "localhost");
			MsgrConf.DatabaseUser = _conf.GetString("database.user", "root");
			MsgrConf.DatabasePass = _conf.GetString("database.pass", "");
			MsgrConf.DatabaseDb = _conf.GetString("database.db", "aura");
		}
	}
}
