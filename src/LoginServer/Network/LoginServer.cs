// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using Common.Database;
using Common.Network;
using Common.Tools;
using Common.World;
using Login.Tools;
using Common.Constants;
using Common.Data;

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
				Logger.Exception(ex, "Unable to connect to database.");
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

			Console.ReadLine();
		}
	}
}
