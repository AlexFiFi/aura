// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Database;
using Aura.World.Events;
using Aura.World.Scripting;
using Aura.World.Util;
using Aura.World.World;

namespace Aura.World.Network
{
	public partial class WorldServer : BaseServer<WorldClient>
	{
		public static readonly WorldServer Instance = new WorldServer();
		static WorldServer() { }
		private WorldServer() : base() { }

		/// <summary>
		/// Milliseconds between tries to connect.
		/// </summary>
		private const int LoginTryTime = 10 * 1000;

		private WorldClient _loginServer;
		private Timer _shutdownTimer1, _shutdownTimer2;

		private readonly Dictionary<string, MabiServer> ServerList = new Dictionary<string, MabiServer>();

		public override void Run(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			ServerUtil.WriteHeader("World Server", ConsoleColor.DarkGreen);
			Console.Title = "* " + Console.Title;

			// Logger
			// --------------------------------------------------------------
			if (!Directory.Exists("../../logs/"))
				Directory.CreateDirectory("../../logs/");
			Logger.FileLog = "../../logs/world.txt";

			Logger.Info("Initializing server @ " + DateTime.Now);
			Logger.Info("Packet version: " + Op.Version);

			// Configuration
			// --------------------------------------------------------------
			Logger.Info("Reading configuration...");
			try
			{
				WorldConf.Load(args);
			}
			catch (FileNotFoundException)
			{
				Logger.Warning("Sorry, I couldn't find 'conf/world.conf'.");
			}
			catch (Exception ex)
			{
				Logger.Warning("There has been a problem while reading 'conf/world.conf'.");
				Logger.Exception(ex);
			}

			// Logger display filter
			// --------------------------------------------------------------
			Logger.Hide = WorldConf.ConsoleFilter;

			// Security checks
			// --------------------------------------------------------------
			ServerUtil.CheckInterPassword(WorldConf.Password);

			// Localization
			// --------------------------------------------------------------
			Logger.Info("Loading localization files (" + WorldConf.Localization + ")...");
			try
			{
				Localization.Parse(WorldConf.DataPath + "/localization/" + WorldConf.Localization);
			}
			catch (FileNotFoundException ex)
			{
				Logger.Warning("Unable to load localization: " + ex.Message);
			}

			// Database
			// --------------------------------------------------------------
			Logger.Info("Connecting to database...");
			ServerUtil.TryConnectToDatabase(WorldConf.DatabaseHost, WorldConf.DatabaseUser, WorldConf.DatabasePass, WorldConf.DatabaseDb);

			//Logger.Info("Clearing database cache...");
			//MabiDb.Instance.ClearDatabaseCache();

			// CS-S
			// --------------------------------------------------------------
			//var tmpPath = Path.Combine(Path.GetTempPath(), "CSSCRIPT");
			//if (Directory.Exists(tmpPath))
			//{
			//    Logger.Info("Clearing CSScript cache...");
			//    Directory.Delete(tmpPath, true);
			//}

			// Data
			// --------------------------------------------------------------
			Logger.Info("Loading data files...");
			ServerUtil.LoadData(WorldConf.DataPath);

			// Guilds
			// --------------------------------------------------------------
			Logger.Info("Loading guilds...");
			this.LoadGuilds();

			// Commands
			// --------------------------------------------------------------
			Logger.Info("Loading commands...");
			CommandHandler.Instance.Load();

			// Scripts (NPCs, Portals, etc.)
			// --------------------------------------------------------------
			ScriptManager.Instance.LoadScripts();

			// Monsters
			// --------------------------------------------------------------
			Logger.Info("Spawning monsters...");
			ScriptManager.Instance.LoadSpawns();

			// Setting up weather
			// --------------------------------------------------------------
			Logger.Info("Initializing weather...");
			WeatherManager.Instance.Init();

			// World
			// --------------------------------------------------------------
			WorldManager.Instance.Start();

			// Starto
			// --------------------------------------------------------------
			try
			{
				this.StartListening(new IPEndPoint(IPAddress.Any, WorldConf.ChannelPort));

				Logger.Status("World Server ready, listening on " + _serverSocket.LocalEndPoint.ToString());

				Console.Title = Console.Title.Replace("* ", "");
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Unable to set up socket; perhaps you're already running a server?");
				ServerUtil.Exit(1);
			}

			// Login server
			// --------------------------------------------------------------
			this.ConnectToLogin(true);

			// Run the channel register method once, and then subscribe to the event that's run once per minute.
			this.SendChannelStatus(null, null);
			ServerEvents.Instance.RealTimeTick += this.SendChannelStatus;

			// Console commands
			// --------------------------------------------------------------
			Logger.Info("Type 'help' for a list of console commands.");
			ServerUtil.ReadCommands(this.ParseCommand);
		}

		protected void ParseCommand(string[] args, string command)
		{
			switch (args[0])
			{
				case "help":
					{
						Logger.Info("Commands:");
						Logger.Info("  status       Shows some status information about the channel");
						Logger.Info("  shutdown     Announces and executes server shutdown");
						Logger.Info("  help         Shows this");
					}
					break;

				case "status":
					{
						Logger.Info("Creatures: " + WorldManager.Instance.GetCreatureCount());
						Logger.Info("Items: " + WorldManager.Instance.GetItemCount());
						Logger.Info("Online time: " + (DateTime.Now - _startTime).ToString(@"hh\:mm\:ss"));
					}
					break;

				case "shutdown":
					{
						this.StopListening();

						// Seconds till players are dced, 10s min.
						int dcSeconds = 10;
						if (args.Length > 1)
							int.TryParse(args[1], out dcSeconds);
						if (dcSeconds < 10)
							dcSeconds = 10;

						// Seconds till the server shuts down.
						int exitSeconds = dcSeconds + 30;

						// Broadcast a notice.
						WorldManager.Instance.Broadcast(PacketCreator.Notice("The server will shutdown in " + dcSeconds + " seconds, please log out before that time, for your own safety.", NoticeType.TopRed), SendTargets.All);

						// Set a timer when to send the dc request all remaining players.
						_shutdownTimer1 = new Timer(_ =>
						{
							var dc = new MabiPacket(Op.RequestClientDisconnect, Id.World).PutSInt((dcSeconds - (dcSeconds - 10)) * 1000);
							WorldManager.Instance.Broadcast(dc, SendTargets.All, null);
						},
							null, (dcSeconds - 10) * 1000, Timeout.Infinite
						);
						Logger.Info("Disconnecting players in " + dcSeconds + " seconds.");

						// Set a timer when to exit this server.
						_shutdownTimer2 = new Timer(_ =>
						{
							ServerUtil.Exit(0, false);
						},
							null, exitSeconds * 1000, Timeout.Infinite
						);
						Logger.Info("Shutting down in " + exitSeconds + " seconds.");
					}
					break;

				case "":
					break;

				default:
					Logger.Info("Unkown command.");
					goto case "help";
			}
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Logger.Error("Oh no! Ferghus escaped his memory block and infected the rest of the server! We're going doooooown!!! ... Uh yeah, about that. Aura has encountered an unexpected and unrecoverable error. We're going to try to save as much as we can.");
			}
			catch { }
			try
			{
				this.StopListening();
			}
			catch { }
			try
			{
				WorldManager.Instance.EmergencyShutdown();
			}
			catch { }
			try
			{
				Logger.Exception((Exception)e.ExceptionObject, null, true);
				Logger.Status("Closing the server.");
			}
			catch { }
			ServerUtil.Exit(1, false);
		}

		protected void LoadGuilds()
		{
			var guilds = WorldDb.Instance.LoadGuilds();

			foreach (var guild in guilds)
			{
				var p = new MabiProp(guild.Region, guild.Area);
				p.Info.Class = guild.StoneClass;
				p.Info.Direction = guild.Rotation;
				p.Info.Region = guild.Region;
				p.Info.X = guild.X;
				p.Info.Y = guild.Y;
				p.Title = guild.Name;
				p.ExtraData = string.Format("<xml guildid=\"{0}\" {1}/>", guild.WorldId,
					guild.HasOption(GuildOptionFlags.Warp) ? "gh_warp=\"true\"" : "");

				WorldManager.Instance.AddProp(p);
				WorldManager.Instance.SetPropBehavior(new MabiPropBehavior(p, GuildstoneTouch));
			}

			Logger.ClearLine();
			Logger.Info("Done loading {0} guilds.", guilds.Count);
		}

		public static void GuildstoneTouch(WorldClient client, MabiCreature creature, MabiProp p)
		{
			string gid = p.ExtraData.Substring(p.ExtraData.IndexOf("guildid=\""));
			gid = gid.Substring(9);
			gid = gid.Substring(0, gid.IndexOf("\""));
			ulong bid = MabiGuild.GetBaseId(ulong.Parse(gid));

			var g = WorldDb.Instance.GetGuild(bid);
			if (creature.Guild != null)
			{
				if (g.BaseId == creature.Guild.BaseId)
					client.Send(new MabiPacket(Op.OpenGuildPanel, creature.Id).PutLong(g.WorldId).PutBytes(0, 0, 0)); // 3 Unknown bytes...
				else
					client.Send(new MabiPacket(Op.GuildInfo, creature.Id).PutLong(g.WorldId).PutStrings(g.Name, g.LeaderName)
						.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < (byte)GuildMemberRank.Applied))
						.PutString(g.IntroMessage));
			}
			else
				client.Send(new MabiPacket(Op.GuildInfoNoGuild, creature.Id).PutLong(g.WorldId).PutStrings(g.Name, g.LeaderName)
					.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < (byte)GuildMemberRank.Applied))
					.PutString(g.IntroMessage));
		}

		/// <summary>
		/// Tries to connect to login server, keeps trying every 10 seconds
		/// till there is a success. Blocking.
		/// </summary>
		private void ConnectToLogin(bool firstTime)
		{
			Logger.Write("");
			if (firstTime)
				Logger.Info("Trying to connect to login server at {0}:{1}...", WorldConf.LoginHost, WorldConf.LoginPort);
			else
			{
				Logger.Info("Trying to re-connect to login server in {0} seconds.", LoginTryTime / 1000);
				Thread.Sleep(LoginTryTime);
			}

			bool success = false;
			while (!success)
			{
				try
				{
					if (_loginServer != null)
					{
						try
						{
							_loginServer.Socket.Shutdown(SocketShutdown.Both);
							_loginServer.Socket.Close();
						}
						catch
						{ }
					}

					_loginServer = new WorldClient();
					_loginServer.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_loginServer.Socket.Connect(WorldConf.LoginHost, WorldConf.LoginPort);

					var buffer = new byte[255];

					// Recv Seed, send back empty packet to get done with the challenge.
					_loginServer.Socket.Receive(buffer);
					_loginServer.Crypto = new MabiCrypto(BitConverter.ToUInt32(buffer, 0));
					_loginServer.Send(new MabiPacket(0, 0));

					// Challenge end
					_loginServer.Socket.Receive(buffer);

					// Inject login server to the normal data receiving.
					_loginServer.Socket.BeginReceive(_loginServer.Buffer.Front, 0, _loginServer.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), _loginServer);

					// Identify
					_loginServer.State = ClientState.LoggingIn;
					_loginServer.Send(new MabiPacket(Op.Internal.ServerIdentify).PutString(BCrypt.HashPassword(WorldConf.Password, BCrypt.GenerateSalt(10))));

					success = true;
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to connect to login server. ({1})", "xyz", ex.Message);
					Logger.Info("Trying again in {0} seconds.", LoginTryTime / 1000);
					Thread.Sleep(LoginTryTime);
				}
			}

			Logger.Info("Connection to login server esablished.");
			Logger.Write("");
		}

		/// <summary>
		/// Sends channel status to login server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void SendChannelStatus(object sender, TimeEventArgs args)
		{
			// Let's asume 20 users would be a lot for now.
			// TODO: Option for max users.
			uint count = WorldManager.Instance.GetCharactersCount();
			byte stress = (byte)Math.Min(75, Math.Ceiling(75 / 20.0f * count));

			if (_loginServer.State == ClientState.LoggedIn)
			{
				_loginServer.Send(
					new MabiPacket(Op.Internal.ChannelStatus)
					.PutString(WorldConf.ServerName)
					.PutString(WorldConf.ChannelName)
					.PutString(WorldConf.ChannelHost)
					.PutShort(WorldConf.ChannelPort)
					.PutByte(stress)
				);
			}
		}

		/// <summary>
		/// Kills client and checks if we have to reconnect to login,
		/// if the client in question was the login server's.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="type"></param>
		protected override void OnClientDisconnect(WorldClient client, Net.DisconnectType type)
		{
			base.OnClientDisconnect(client, type);

			if (client == _loginServer)
			{
				Logger.Info("Lost connection to login server.");
				this.ConnectToLogin(false);
			}
		}
	}
}
