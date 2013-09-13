// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.Msgr.Util;

namespace Aura.Msgr.Network
{
	public partial class MsgrServer : BaseServer<MsgrClient>
	{
		public static readonly MsgrServer Instance = new MsgrServer();
		static MsgrServer() { }
		private MsgrServer() : base() { }

		public override void Run(string[] args)
		{
			ServerUtil.WriteHeader("Msgr Server", ConsoleColor.DarkCyan);

			// Logger
			// --------------------------------------------------------------
			if (!Directory.Exists("../../logs/"))
				Directory.CreateDirectory("../../logs/");
			Logger.FileLog = "../../logs/msgr.txt";

			Logger.Info("Initializing server @ " + DateTime.Now);

			// Configuration
			// --------------------------------------------------------------
			Logger.Info("Reading configuration...");
			try
			{
				MsgrConf.Load(args);
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
			Logger.Hide = MsgrConf.ConsoleFilter;

			// Database
			// --------------------------------------------------------------
			Logger.Info("Connecting to database...");
			ServerUtil.TryConnectToDatabase(MsgrConf.DatabaseHost, MsgrConf.DatabaseUser, MsgrConf.DatabasePass, MsgrConf.DatabaseDb);

			// Starto
			// --------------------------------------------------------------
			try
			{
				this.StartListening(8002);

				Logger.Status("Msgr Server ready, listening on " + _serverSocket.LocalEndPoint.ToString());
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Unable to set up socket; perhaps you're already running a server?");
				ServerUtil.Exit(1);
			}

			//Logger.Info("Type 'help' for a list of console commands.");
			// Command ideas: "Newsletters"
			while (Console.ReadLine() != "exit") ;
		}

		protected override void OnClientAccepted(MsgrClient client)
		{
			// do nothing (default is seeding)
		}

		protected override int GetPacketLength(byte[] buffer, int start)
		{
			return buffer[start + 3] + 4;
		}

		protected override void PrepareBuffer(ref byte[] buffer, int length)
		{ }

		protected override void HandleBuffer(MsgrClient client, byte[] buffer)
		{
			var len = buffer.Length;
			if (len < 5)
				return;

			if (client.State == ClientState.Check)
			{
				if (buffer[4] == 0x00)
					client.Socket.Send(new byte[] { 0x55, 0xfb, 0x02, 0x05, 0x00, 0x00, 0x00, 0x00, 0x40 });
				if (buffer[4] == 0x01)
					client.Socket.Send(new byte[] { 0x55, 0xff, 0x02, 0x09, 0x01, 0x1e, 0xf7, 0x5d, 0x68, 0x00, 0x00, 0x00, 0x40 });
				if (buffer[4] == 0x02)
				{
					client.Socket.Send(new byte[] { 0x55, 0x12, 0x02, 0x01, 0x02 });
					client.State = ClientState.LoggingIn;
				}
			}
			else
			{
				var packet = new MabiPacket(buffer, (ushort)buffer.Length, true);
				this.HandlePacket(client, packet);
			}
		}
	}
}
