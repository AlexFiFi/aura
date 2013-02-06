// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.IO;
using Common.Tools;

namespace World.Tools
{
	public static class WorldConf
	{
		public static Logger.LogLevel ConsoleFilter;

		// Data
		public static string DataPath;

		// Database
		public static string DatabaseHost;
		public static string DatabaseUser;
		public static string DatabasePass;
		public static string DatabaseDb;

		// World
		public static string ServerName;
		public static string ChannelName;
		public static string ChannelHost;
		public static ushort ChannelPort;

		// Commands
		public static char CommandPrefix;

		// Scripting
		public static string ScriptPath;
		public static bool DisableScriptCaching;

		// Motd
		public static string Motd;

		// Player
		public static uint SightRange;

		// GM
		public static bool AutoSendGMCP;
		public static byte MinimumGMCP;

		// Exp
		public static float ExpRate;

		// Drops
		public static float DropRate, GoldDropRate, PropDropRate;

		// Features
		public static bool EnableVisual;
		public static int MailExpires;
		public static bool EnableItemShop;

		// Skills
		public static bool BunshinSouls;

		// Shops
		public static bool ColorChange;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/world.conf");

			if (args != null)
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
			WorldConf.DisableScriptCaching = _conf.GetBool("script_disable_cache", false);

			WorldConf.SightRange = _conf.Get<uint>("world_sightrange", 3000);

			WorldConf.AutoSendGMCP = _conf.GetBool("world_auto_gmcp", false);
			WorldConf.MinimumGMCP = _conf.Get<byte>("world_minimum_gmcp", 50);

			WorldConf.ExpRate = _conf.Get<float>("world_exp_rate", 100f) / 100.0f;

			WorldConf.DropRate = _conf.Get<float>("world_drop_rate", 100f) / 100.0f;
			WorldConf.GoldDropRate = _conf.Get<float>("world_gold_drop_rate", 30f) / 100.0f;
			WorldConf.PropDropRate = _conf.Get<float>("world_prop_drop_rate", 30f) / 100.0f;

			WorldConf.EnableItemShop = _conf.GetBool("world_enable_itemshop", false);
			WorldConf.MailExpires = _conf.GetInt("world_mail_expires", 30);
			WorldConf.EnableVisual = _conf.GetBool("world_enable_visual", true);

			WorldConf.BunshinSouls = _conf.GetBool("world_bunshinsouls", true);

			WorldConf.ColorChange = _conf.GetBool("world_colorchange", true);

			try
			{
				WorldConf.Motd = File.ReadAllText("../../conf/motd.txt");
			}
			catch (FileNotFoundException)
			{
				Logger.Warning("'motd.txt' not found.");
				WorldConf.Motd = string.Empty;
			}
		}
	}
}
