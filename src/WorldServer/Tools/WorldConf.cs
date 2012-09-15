// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.IO;
using Common.Tools;

namespace World.Tools
{
	public static class WorldConf
	{
		public static Logger.LogLevel ConsoleFilter;

		public static string DataPath;

		public static string DatabaseHost;
		public static string DatabaseUser;
		public static string DatabasePass;
		public static string DatabaseDb;

		public static string ServerName;
		public static string ChannelName;
		public static string ChannelHost;
		public static ushort ChannelPort;

		public static char CommandPrefix;

		public static string ScriptPath;

		public static string Motd;

		public static uint SightRange;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/world.conf");
			_conf.ReadArguments(args, "../../");

			WorldConf.ConsoleFilter = (Logger.LogLevel)_conf.GetInt("world_consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			WorldConf.ConsoleFilter &= ~Logger.LogLevel.Debug;
#endif

			WorldConf.DataPath = _conf.GetString("data_path", "../../data");

			WorldConf.DatabaseHost = _conf.GetString("database_host", "localhost");
			WorldConf.DatabaseUser = _conf.GetString("database_user", "root");
			WorldConf.DatabasePass = _conf.GetString("database_pass", "");
			WorldConf.DatabaseDb = _conf.GetString("database_db", "aura");

			WorldConf.ServerName = _conf.GetString("world_servername", "Dummy");
			WorldConf.ChannelName = _conf.GetString("world_channelname", "Ch1");
			WorldConf.ChannelHost = _conf.GetString("world_channelhost", "127.0.0.1");
			WorldConf.ChannelPort = (ushort)_conf.GetInt("world_channelport", 11020);

			WorldConf.CommandPrefix = _conf.GetString("commands_prefix", ">")[0];

			WorldConf.ScriptPath = _conf.GetString("script_path", "../../scripts");

			WorldConf.SightRange = _conf.Get<uint>("world_sightrange", 3000);

			try
			{
				WorldConf.Motd = File.ReadAllText("../../conf/motd.txt");
			}
			catch (FileNotFoundException)
			{
				Logger.Warning("'motd.txt' not found.");
				WorldConf.Motd = "";
			}
		}
	}
}
