// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.IO;
using Aura.Shared.Util;

namespace Aura.World.Util
{
	public static class WorldConf
	{
		public static LogLevel ConsoleFilter;

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

		// World
		public static string ServerName;
		public static string ChannelName;
		public static string ChannelHost;
		public static ushort ChannelPort;
		public static string LoginHost;
		public static ushort LoginPort;
		public static string CachePath;

		// Commands
		public static char CommandPrefix;

		// Scripting
		public static string ScriptPath;
		public static bool DisableScriptCaching;
		public static bool ScriptStrictMode;

		// Motd
		public static string Motd;

		// Player
		public static uint SightRange;

		// GM
		public static bool AutoSendGMCP;
		public static byte MinimumGMCP;
		public static byte MinimumGMCPSummon;
		public static byte MinimumGMCPCharWarp;
		public static byte MinimumGMCPMove;
		public static byte MinimumGMCPRevive;
		public static byte MinimumGMCPInvisible;
		public static byte MinimumGMCPExpel;
		public static byte MinimumGMCPBan;

		// Exp
		public static float ExpRate;

		// Drops
		public static float DropRate, GoldDropRate, PropDropRate;

		// Features
		public static bool EnableVisual;
		public static int MailExpires;
		public static bool EnableItemShop;
		public static int ChalkOnDeath;
		public static bool SafeDye;

		// Skills
		public static bool BunshinSouls;
		public static bool PerfectPlay;
		public static bool DkSoundFix;

		// Shops
		public static bool ColorChange;

		// Combat
		public static bool DynamicCombat;

		// Mobs
		public static int TimeBeforeAncient;
		public static float AncientRate;

		// NPC
		public static bool NpcIntroOnce;

		private static Configuration _conf;

		public static void Load(string[] args)
		{
			_conf = new Configuration();
			_conf.ReadFile("../../conf/world.conf");

			if (args != null)
				_conf.ReadArguments(args, "../../");

			WorldConf.ConsoleFilter = (LogLevel)_conf.GetInt("world.consolefilter", 0);
#if DEBUG
			// Enable debug regardless of configuration in debug builds.
			WorldConf.ConsoleFilter &= ~LogLevel.Debug;
#endif

			WorldConf.Password = _conf.GetString("inter.password", "aura");

			WorldConf.DataPath = _conf.GetString("data.path", "../../data");
			WorldConf.Localization = _conf.GetString("data.localization", "us");

			WorldConf.DatabaseHost = _conf.GetString("database.host", "localhost");
			WorldConf.DatabaseUser = _conf.GetString("database.user", "root");
			WorldConf.DatabasePass = _conf.GetString("database.pass", "");
			WorldConf.DatabaseDb = _conf.GetString("database.db", "aura");

			WorldConf.ServerName = _conf.GetString("world.servername", "Dummy");
			WorldConf.ChannelName = _conf.GetString("world.channelname", "Ch1");
			WorldConf.ChannelHost = _conf.GetString("world.channelhost", "127.0.0.1");
			WorldConf.ChannelPort = (ushort)_conf.GetInt("world.channelport", 11020);
			WorldConf.CachePath = _conf.GetString("world.cache", "../../cache");

			WorldConf.LoginHost = _conf.GetString("world.loginhost", "127.0.0.1");
			WorldConf.LoginPort = (ushort)_conf.GetInt("world.loginport", 11000);

			WorldConf.CommandPrefix = _conf.GetString("commands.prefix", ">")[0];

			WorldConf.ScriptPath = _conf.GetString("script.path", "../../scripts");
			WorldConf.DisableScriptCaching = _conf.GetBool("script.disable_cache", false);
			WorldConf.ScriptStrictMode = _conf.GetBool("script.strict_mode", false);

			WorldConf.SightRange = _conf.Get<uint>("world.sightrange", 3000);

			WorldConf.AutoSendGMCP = _conf.GetBool("world.auto_gmcp", false);
			WorldConf.MinimumGMCP = _conf.Get<byte>("world.minimum_gmcp", 50);
			WorldConf.MinimumGMCPSummon = _conf.Get<byte>("world.minimum_gmcp_summon", 50);
			WorldConf.MinimumGMCPCharWarp = _conf.Get<byte>("world.minimum_gmcp_char_warp", 50);
			WorldConf.MinimumGMCPMove = _conf.Get<byte>("world.minimum_gmcp_move", 50);
			WorldConf.MinimumGMCPRevive = _conf.Get<byte>("world.minimum_gmcp_revive", 50);
			WorldConf.MinimumGMCPInvisible = _conf.Get<byte>("world.minimum_gmcp_invisible", 50);
			WorldConf.MinimumGMCPExpel = _conf.Get<byte>("world.minimum_gmcp_expel", 50);
			WorldConf.MinimumGMCPBan = _conf.Get<byte>("world.minimum_gmcp_ban", 50);

			WorldConf.ExpRate = _conf.Get<float>("world.exp_rate", 100f) / 100.0f;

			WorldConf.DropRate = _conf.Get<float>("world.drop_rate", 100f) / 100.0f;
			WorldConf.GoldDropRate = _conf.Get<float>("world.gold_drop_rate", 30f) / 100.0f;
			WorldConf.PropDropRate = _conf.Get<float>("world.prop_drop_rate", 30f) / 100.0f;

			WorldConf.EnableItemShop = _conf.GetBool("world.enable_itemshop", false);
			WorldConf.MailExpires = _conf.GetInt("world.mail_expires", 30);
			WorldConf.EnableVisual = _conf.GetBool("world.enable_visual", true);
			WorldConf.SafeDye = _conf.GetBool("world.safe_dye", false);

			WorldConf.BunshinSouls = _conf.GetBool("world.bunshinsouls", true);
			WorldConf.PerfectPlay = _conf.GetBool("world.perfectplay", false);
			WorldConf.DkSoundFix = _conf.GetBool("world.dk_sound_fix", true);

			WorldConf.ColorChange = _conf.GetBool("world.colorchange", true);

			WorldConf.DynamicCombat = _conf.GetBool("world.dynamic_combat", true);

			WorldConf.TimeBeforeAncient = _conf.GetInt("world.time_before_ancient", 300);
			WorldConf.AncientRate = _conf.Get<float>("world.ancient_rate", .33f);

			WorldConf.NpcIntroOnce = _conf.GetBool("world.npc_intro_once", true);

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
