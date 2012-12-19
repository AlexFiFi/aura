// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Common.Data;
using Common.Tools;

namespace Common.Network
{
	public class DoNotCatchException : Exception { public DoNotCatchException(string msg) : base(msg) { } }

	/// <summary>
	/// Main class for Login and Channels to be derived from. Contains networking code and
	/// some basic stuff that's shared between the servers.
	/// </summary>
	public abstract class Server<TClient> where TClient : Client, new()
	{
		protected Socket _serverSocket;

		protected DateTime _startTime = DateTime.Now;

		public delegate void PacketHandler(TClient client, MabiPacket packet);
		protected Dictionary<uint, PacketHandler> _handlers = new Dictionary<uint, PacketHandler>();

		//protected List<TClient> _clients = new List<TClient>();

		/// <summary>
		/// Prints the standard header for the emulator.
		/// </summary>
		/// <param name="title">Name of this server. Example: "Login Server"</param>
		public void WriteHeader(string title = null, ConsoleColor color = ConsoleColor.DarkGray)
		{
			if (title != null)
				Console.Title = "Aura : " + title;

			Console.ForegroundColor = color;

			//Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@"                          __     __  __  _ __    __                             ");
			Console.Write(@"                        /'__`\  /\ \/\ \/\`'__\/'__`\                           ");
			Console.Write(@"                       /\ \L\.\_\ \ \_\ \ \ \//\ \L\.\_                         ");
			Console.Write(@"                       \ \__/.\_\\ \____/\ \_\\ \__/.\_\                        ");
			Console.Write(@"                        \/__/\/_/ \/___/  \/_/ \/__/\/_/                        ");
			Console.Write(@"                                                                                ");

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@"                         by the Aura development team                           ");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(@"________________________________________________________________________________");
			Console.WriteLine("");
		}

		/// <summary>
		/// Waits for the return key, and closes the application afterwards.
		/// </summary>
		/// <param name="exitCode"></param>
		protected void Exit(int exitCode = 0, bool wait = true)
		{
			if (wait)
			{
				Logger.Info("Press Enter to exit.");
				Console.ReadLine();
			}
			Logger.Info("Exiting...");
			Environment.Exit(exitCode);
		}

		/// <summary>
		/// This method should contain the code to initiate and start the server.
		/// </summary>
		/// <param name="args"></param>
		public abstract void Run(string[] args);

		/// <summary>
		/// Called automatically in StartListening.
		/// Should contain the registering for the packet handlers.
		/// </summary>
		protected abstract void InitPacketHandlers();

		protected void RegisterPacketHandler(uint op, PacketHandler handler)
		{
			if (_handlers == null)
				_handlers = new Dictionary<uint, PacketHandler>();

			_handlers.Add(op, handler);
		}

		/// <summary>
		/// Retrieves a packet handler from the list of registered handlers.
		/// </summary>
		/// <param name="op"></param>
		/// <returns>Returns null if there is no handler for this op.</returns>
		public PacketHandler GetPacketHandler(uint op)
		{
			if (_handlers.ContainsKey(op))
				return _handlers[op];

			return null;
		}

		public void LoadData(string dataPath, DataLoad toLoad = DataLoad.All, bool reload = false)
		{
			try
			{
				if (toLoad.HasFlag(DataLoad.Npcs))
				{
					MabiData.SpawnDb.LoadFromCsv(dataPath + "/db/spawns.txt", reload);
					Logger.Info("Done loading " + MabiData.SpawnDb.Entries.Count + " entries from spawns.txt.");
				}

				if (toLoad.HasFlag(DataLoad.Data))
				{
					MabiData.RaceSkillDb.LoadFromCsv(dataPath + "/db/race_skills.txt", reload);
					Logger.Info("Done loading " + MabiData.RaceSkillDb.Entries.Count + " entries from race_skills.txt.");

					MabiData.RaceStatDb.LoadFromCsv(dataPath + "/db/race_stats.txt", reload);
					Logger.Info("Done loading " + MabiData.RaceStatDb.Entries.Count + " entries from race_stats.txt.");

					MabiData.SpeedDb.LoadFromCsv(dataPath + "/db/speed.txt", reload);
					MabiData.FlightDb.LoadFromCsv(dataPath + "/db/flight.txt", reload);
					MabiData.RaceDb.LoadFromCsv(dataPath + "/db/races.txt", reload);
					Logger.Info("Done loading " + MabiData.RaceDb.Entries.Count + " entries from races.txt.");

					MabiData.PortalDb.LoadFromCsv(dataPath + "/db/portals.txt", reload);
					Logger.Info("Done loading " + MabiData.PortalDb.Entries.Count + " entries from portals.txt.");

					MabiData.StatsBaseDb.LoadFromCsv(dataPath + "/db/stats_base.txt", reload);
					Logger.Info("Done loading " + MabiData.StatsBaseDb.Entries.Count + " entries from stats_base.txt.");

					MabiData.StatsLevelUpDb.LoadFromCsv(dataPath + "/db/stats_levelup.txt", reload);
					Logger.Info("Done loading " + MabiData.StatsLevelUpDb.Entries.Count + " entries from stats_levelup.txt.");

					MabiData.MotionDb.LoadFromCsv(dataPath + "/db/motions.txt", reload);
					Logger.Info("Done loading " + MabiData.MotionDb.Entries.Count + " entries from motions.txt.");

					MabiData.CharCardSetDb.LoadFromCsv(dataPath + "/db/charcardsets.txt", reload);
					MabiData.CharCardDb.LoadFromCsv(dataPath + "/db/charcards.txt", reload);
					Logger.Info("Done loading " + MabiData.CharCardSetDb.Entries.Count + " entries from charcardsets.txt.");

					MabiData.ColorMapDb.LoadFromDat(dataPath + "/db/colormap.dat", reload);
					Logger.Info("Done loading " + MabiData.ColorMapDb.Entries.Count + " entries from colormap.dat.");

					MabiData.ItemDb.LoadFromCsv(dataPath + "/db/items.txt", reload);
					Logger.Info("Done loading " + MabiData.ItemDb.Entries.Count + " entries from items.txt.");

					MabiData.SkillRankDb.LoadFromCsv(dataPath + "/db/skill_ranks.txt", reload);
					MabiData.SkillDb.LoadFromCsv(dataPath + "/db/skills.txt", reload);
					Logger.Info("Done loading " + MabiData.SkillDb.Entries.Count + " entries from skills.txt.");

					MabiData.MapDb.LoadFromCsv(dataPath + "/db/maps.txt", reload);
					Logger.Info("Done loading " + MabiData.MapDb.Entries.Count + " entries from maps.txt.");
				}
			}
			catch (FileNotFoundException ex)
			{
				Logger.Error(ex.Message);
				this.Exit(1);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, null, true);
				this.Exit(1);
			}
		}

		/// <summary>
		/// Reads input from the console and passes it to ParseCommand,
		/// which can be overriden by deriving servers.
		/// </summary>
		/// <param name="stopCommand"></param>
		protected virtual void ReadCommands(string stopCommand = "")
		{
			var input = string.Empty;

			do
			{
				input = Console.ReadLine();
				var splitted = input.Split(' ');
				this.ParseCommand(splitted, input);
			}
			while (input != stopCommand);
		}

		protected virtual void ParseCommand(string[] args, string command)
		{
		}

		/// <summary>
		/// Binds the server, and starts the async accepting.
		/// </summary>
		protected void StartListening(IPEndPoint iep)
		{
			this.InitPacketHandlers();

			_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_serverSocket.Bind(iep);
			_serverSocket.Listen(10);

			_serverSocket.BeginAccept(new AsyncCallback(OnAccept), _serverSocket);
		}

		/// <summary>
		/// Tells the server to stop accepting new connections.
		/// </summary>
		protected void StopListening()
		{
			//_serverSocket.Shutdown(SocketShutdown.Both);
			_serverSocket.Close();
			Logger.Status("Server has stopped listening for new connections.");
		}

		protected virtual void OnClientAccepted(TClient client)
		{
			// Send seed
			client.Socket.Send(BitConverter.GetBytes(client.Seed));
		}

		protected virtual void OnClientDisconnect(TClient client)
		{
			client.Kill();
		}

		/// <summary>
		/// Callback for accepting clients.
		/// </summary>
		/// <param name="result"></param>
		protected void OnAccept(IAsyncResult result)
		{
			var client = new TClient();

			try
			{
				client.Socket = ((Socket)result.AsyncState).EndAccept(result);
			}
			catch (ObjectDisposedException)
			{
				return;
			}

			try
			{
				client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), client);

				this.OnClientAccepted(client);

				Logger.Info("Connection established from " + client.Socket.RemoteEndPoint.ToString());
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);

				this.OnClientDisconnect(client);
			}
			finally
			{
				_serverSocket.BeginAccept(new AsyncCallback(this.OnAccept), _serverSocket);
			}
		}

		/// <summary>
		/// Callback for receiving data.
		/// </summary>
		/// <param name="result"></param>
		protected void OnReceive(IAsyncResult result)
		{
			try
			{
				var client = result.AsyncState as TClient;
				try
				{
					int bytesReceived = client.Socket.EndReceive(result);
					int bytesRead = 0;

					if (bytesReceived <= 0)
					{
						Logger.Info("Connection closed from " + client.Socket.RemoteEndPoint.ToString());
						this.OnClientDisconnect(client);
						return;
					}

					// Turn buffer into packets
					while (bytesRead < bytesReceived)
					{
						// New packet
						if (client.Buffer.Remaining < 1)
							client.Buffer.Remaining = this.ReadRemainingLength(client.Buffer.Front, bytesRead);

						// Copy to back buffer
						client.Buffer.Back[client.Buffer.Ptr++] = client.Buffer.Front[bytesRead++];

						// Full packet is not in back buffer yet
						if (--client.Buffer.Remaining > 0)
							continue;

						// Prepare buffer, cut the appended 4 bytes
						var buffer = this.PrepareBuffer(client.Buffer.Back, client.Buffer.Ptr);

						this.HandleBuffer(client, buffer);

						client.Buffer.InitBB();
					}

					if (client.State < SessionState.Dead)
						client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);
				}
				catch (DoNotCatchException)
				{
					throw;
				}
				catch (SocketException)
				{
					Logger.Info("Lost connection from " + client.Socket.RemoteEndPoint.ToString());
					this.OnClientDisconnect(client);
				}
				catch (ObjectDisposedException)
				{
					if (client.State != SessionState.Dead)
						Logger.Warning("Socket of '" + client.Socket.RemoteEndPoint.ToString() + "' was disposed unexpectedly.");
				}
				catch (Exception ex)
				{
					Logger.Exception(ex, "This shouldn't have happened, please report.", true);
				}
			}
			catch (NullReferenceException)
			{
				Logger.Warning("Something went wrong in the recieve!");
			}
		}

		protected virtual int ReadRemainingLength(byte[] buffer, int start)
		{
			return
				(buffer[start + 1] << 0) +
				(buffer[start + 2] << 8) +
				(buffer[start + 3] << 16) +
				(buffer[start + 4] << 24);
		}

		protected virtual byte[] PrepareBuffer(byte[] buffer, int ptr)
		{
			ptr -= 4;
			var result = new byte[ptr];
			Array.Copy(buffer, result, ptr);
			BitConverter.GetBytes(ptr).CopyTo(result, 1);

			return result;
		}

		protected virtual void HandleBuffer(TClient client, byte[] buffer)
		{
			client.Crypto.DecodePacket(buffer);

			if (buffer[5] == 0x01)
			{
				client.Crypto.EncodePacket(buffer);
				client.Socket.Send(buffer);
			}
			else
			{
				// Challenge
				if (client.State == SessionState.ClientCheck)
				{
					client.Send(new byte[] { 0x88, 0x07, 0x00, 0x00, 0x00, 0x00, 0x07 });

					client.State = SessionState.Login;
				}
				// Actual packets
				else
				{
					var packet = new MabiPacket(buffer, (ushort)buffer.Length);
					this.HandlePacket(client, packet);
				}
			}
		}

		protected virtual void HandlePacket(TClient client, MabiPacket packet)
		{
			var handler = this.GetPacketHandler(packet.Op);
			if (handler != null)
			{
				try
				{
					handler(client, packet);
				}
				catch (DoNotCatchException)
				{
					throw;
				}
				catch (Exception ex)
				{
					Logger.Exception(ex, "There has been a problem while handling '" + HexTool.ToString(packet.Op) + "'.", true);
				}
			}
			else
			{
				Logger.Unimplemented("Unhandled packet: " + HexTool.ToString(packet.Op));
			}
		}
	}

	public enum DataLoad { Npcs = 0x01, Data = 0x02, All = 0xFFFF }
}
