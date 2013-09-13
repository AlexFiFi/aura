// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Aura.Data;
using Aura.Login.Util;
using Aura.Net;
using Aura.Shared.Const;
using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.Shared.Util;

namespace Aura.Login.Network
{
	public partial class LoginServer : BaseServer<LoginClient>
	{
		public static readonly LoginServer Instance = new LoginServer();
		static LoginServer() { }
		private LoginServer() : base() { }

		private const int ChannelUpdateInterval = 30 * 1000;
		private Timer _channelUpdateTimer;

		public readonly Dictionary<string, MabiServer> ServerList = new Dictionary<string, MabiServer>();
		public readonly List<LoginClient> ChannelClients = new List<LoginClient>();

		public override void Run(string[] args)
		{
			ServerUtil.WriteHeader("Login Server", ConsoleColor.Magenta);

			// Logger
			// --------------------------------------------------------------
			if (!Directory.Exists("../../logs/"))
				Directory.CreateDirectory("../../logs/");
			Logger.FileLog = "../../logs/login.txt";

			Logger.Info("Initializing server @ " + DateTime.Now);

			// Configuration
			// --------------------------------------------------------------
			Logger.Info("Reading configuration...");
			try
			{
				LoginConf.Load(args);
			}
			catch (FileNotFoundException)
			{
				Logger.Warning("Unable to find 'conf/login.conf'.");
			}
			catch (Exception ex)
			{
				Logger.Warning("There has been a problem while reading 'conf/login.conf'.");
				Logger.Exception(ex);
			}

			Logger.Hide = LoginConf.ConsoleFilter;

			// Security checks
			// --------------------------------------------------------------
			ServerUtil.CheckInterPassword(LoginConf.Password);

			// Localization
			// --------------------------------------------------------------
			Logger.Info("Loading localization files (" + LoginConf.Localization + ")...");
			try
			{
				Localization.Parse(LoginConf.DataPath + "/localization/" + LoginConf.Localization + "/login.txt");
			}
			catch (FileNotFoundException ex)
			{
				Logger.Warning("Unable to load localization: " + ex.Message);
			}

			// Database
			// --------------------------------------------------------------
			Logger.Info("Connecting to database...");
			ServerUtil.TryConnectToDatabase(LoginConf.DatabaseHost, LoginConf.DatabaseUser, LoginConf.DatabasePass, LoginConf.DatabaseDb);

			// Data
			// --------------------------------------------------------------
			Logger.Info("Loading data files...");
			ServerUtil.LoadData(LoginConf.DataPath, DataLoad.LoginServer);

			// Configuration 2
			// --------------------------------------------------------------
			Logger.Info("Loading run-time configuration...");
			try
			{
				LoginConf.LoadRound2();
			}
			catch (Exception ex)
			{
				Logger.Warning("There has been a problem while reading the remaining conf options.");
				Logger.Exception(ex);
			}

			// Starto
			// --------------------------------------------------------------
			try
			{
				this.StartListening(LoginConf.Port);

				Logger.Status("Login Server ready, listening on " + _serverSocket.LocalEndPoint.ToString());
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Unable to set up socket; perhaps you're already running a server?");
				ServerUtil.Exit(1);
			}

			// Client update timer
			// --------------------------------------------------------------
			_channelUpdateTimer = new Timer(_ => Send.ChannelUpdate(), null, 0, ChannelUpdateInterval);

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
						Logger.Info("  auth         Sets the authority of the given user");
						Logger.Info("  addcard      Adds the specified card to an account");
						Logger.Info("  passwd       Sets password of an account");
						Logger.Info("  help         Shows this");
					}
					break;

				case "status":
					{
						Logger.Info("Online time: " + (DateTime.Now - _startTime).ToString(@"hh\:mm\:ss"));
					}
					break;

				case "auth":
					{
						if (args.Length < 3)
						{
							Logger.Error("Usage: auth <account id> <auth level>");
							break;
						}

						var accountId = args[1];
						byte level = 0;
						try
						{
							level = Convert.ToByte(args[2]);
							if (MabiDb.Instance.SetAuthority(accountId, level))
								Logger.Info("Done.");
							else
								Logger.Info("Account couldn't be found.");
						}
						catch
						{
							Logger.Error("Please specify an existing account and an authority level between 0 and 99.");
						}
					}
					break;

				case "addcard":
					{
						if (args.Length < 4)
						{
							Logger.Info("Usage: addcard <pet|character> <card id> <account id>");
							break;
						}

						var type = args[1];
						var account = args[3];

						uint cardId = 0;
						if (!uint.TryParse(args[2], out cardId))
						{
							Logger.Error("Please specify a numeric card id.");
							return;
						}

						if (!MabiDb.Instance.AccountExists(account))
						{
							Logger.Error("Please specify an existing account.");
							return;
						}

						if (type != "character" && type != "pet")
						{
							Logger.Error("Please specify a valid card type (pet/character).");
							return;
						}

						if (type == "character")
							MabiDb.Instance.AddCard(account, cardId, 0);
						else
						{
							if (!MabiData.PetDb.Has(cardId))
							{
								Logger.Error("Unknown pet card ({0}).", cardId);
								break;
							}
							MabiDb.Instance.AddCard(account, Id.PetCardType, cardId);
						}

						Logger.Info("Card added.");
					}
					break;

				case "passwd":
					{
						if (args.Length < 3)
						{
							Logger.Info("Usage: passwd <account id> <password>");
							break;
						}

						var accountName = args[1];
						var password = args[2];

						if (!MabiDb.Instance.AccountExists(accountName))
						{
							Logger.Error("Please specify an existing account.");
							return;
						}

						MabiDb.Instance.SetAccountPassword(accountName, password);

						Logger.Info("Password changed.");
					}
					break;

				case "":
					break;

				default:
					Logger.Info("Unkown command.");
					goto case "help";
			}
		}

		/// <summary>
		/// Kills client connection and checks if this was a channel.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="type"></param>
		protected override void OnClientDisconnect(LoginClient client, DisconnectType type)
		{
			base.OnClientDisconnect(client, type);

			if (this.ChannelClients.Contains(client))
			{
				lock (this.ChannelClients)
					this.ChannelClients.Remove(client);

				// Update channel
				if (client.Account != null)
				{
					lock (this.ServerList)
					{
						foreach (var server in this.ServerList.Values)
						{
							foreach (var channel in server.Channels.Values)
							{
								if (channel.FullName == client.Account.Name)
								{
									channel.State = ChannelState.Maintenance;
									break;
								}
							}
						}
					}

					Send.ChannelUpdate();
				}
			}
		}

		/// <summary>
		/// Sends packet to all connected clients.
		/// </summary>
		/// <param name="packet"></param>
		public void Broadcast(MabiPacket packet)
		{
			foreach (var client in _clients.Where(a => a.State == ClientState.LoggedIn))
			{
				client.Send(packet);
			}
		}

		/// <summary>
		/// Sends packet to all channels. If server is set, it's only sent
		/// to all channels in that server.
		/// </summary>
		/// <param name="packet"></param>
		public void BroadcastChannels(MabiPacket packet, string server = null)
		{
			lock (this.ChannelClients)
			{
				foreach (var client in this.ChannelClients.Where(a => a.Account != null))
				{
					if (server == null || client.Account.Name.EndsWith("@" + server))
						client.Send(packet);
				}
			}
		}
	}
}
