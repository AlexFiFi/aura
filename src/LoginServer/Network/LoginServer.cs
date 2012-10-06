// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;
using System.Net;
using Common.Database;
using Common.Network;
using Common.Tools;
using Login.Tools;

namespace Login.Network
{
	public partial class LoginServer : Server<LoginClient>
	{
		public static readonly LoginServer Instance = new LoginServer();
		static LoginServer() { }
		private LoginServer() : base() { }

		public override void Run(string[] args)
		{
			this.WriteHeader("Login Server", ConsoleColor.Magenta);

			// Logger
			// --------------------------------------------------------------
			if (!Directory.Exists("../../logs/"))
				Directory.CreateDirectory("../../logs/");
			Logger.FileLog = "../../logs/login.txt";

			Logger.Info("Initializing server @ " + DateTime.Now);
			Logger.Info("Packet version: " + Op.Version);

			// Configuration
			// --------------------------------------------------------------
			Logger.Info("Reading configuration...");
			try
			{
				LoginConf.Load(args);
			}
			catch (FileNotFoundException)
			{
				Logger.Warning("Sorry, I couldn't find 'conf/login.conf'.");
			}
			catch (Exception ex)
			{
				Logger.Warning("There has been a problem while reading 'conf/login.conf'.");
				Logger.Exception(ex);
			}

			// Logger display filter
			// --------------------------------------------------------------
			Logger.Hide = LoginConf.ConsoleFilter;

			// Database
			// --------------------------------------------------------------
			Logger.Info("Connecting to database...");
			try
			{
				MabiDb.Instance.Init(LoginConf.DatabaseHost, LoginConf.DatabaseUser, LoginConf.DatabasePass, LoginConf.DatabaseDb);
				MabiDb.Instance.TestConnection();
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to connect to database. (" + ex.Message + ")");
				this.Exit(1);
			}

			Logger.Info("Clearing database cache...");
			MabiDb.Instance.ClearDatabaseCache();

			// Data
			// --------------------------------------------------------------
			Logger.Info("Loading data files...");
			this.LoadData(LoginConf.DataPath);

			// Starto
			// --------------------------------------------------------------
			try
			{
				this.StartListening(new IPEndPoint(IPAddress.Any, 11000));

				Logger.Status("Login Server ready, listening on " + _serverSocket.LocalEndPoint.ToString());
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Unable to set up socket; perhaps you're already running a server?");
				this.Exit(1);
			}

			Logger.Info("Type 'help' for a list of console commands.");
			this.ReadCommands();
		}

		protected override void ParseCommand(string[] args, string command)
		{
			switch (args[0])
			{
				case "help":
					{
						Logger.Info("Commands:");
						Logger.Info("  status       Shows some status information about the channel");
						Logger.Info("  auth         Sets the authority of the given user");
						Logger.Info("  addcard      Adds the specified card to an account");
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

						int card = 0;
						if (!int.TryParse(args[2], out card))
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

						MabiDb.Instance.AddCard(type + "_cards", account, card);

						Logger.Info("Card added.");
					}
					break;

				case "":
					break;

				default:
					Logger.Info("Unkown command.");
					goto case "help";
			}
		}
	}
}
