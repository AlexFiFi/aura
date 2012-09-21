// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Common.Tools;
using Common.Data;
using System.IO;

namespace Common.Network
{
	/// <summary>
	/// Main class for Login and Channels to be derived from. Contains networking code and
	/// some basic stuff that's shared between the servers.
	/// </summary>
	public abstract class Server<TClient> where TClient : Client, new()
	{
		protected Socket _serverSocket;

		public delegate void PacketHandler(TClient client, MabiPacket packet);
		protected Dictionary<uint, PacketHandler> _handlers = new Dictionary<uint, PacketHandler>();

		protected List<TClient> _clients = new List<TClient>();

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
		protected void Exit(int exitCode = 0)
		{
			Logger.Info("Exiting...");
			Console.ReadLine();
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

		public void LoadData(string dataPath, bool reload = false)
		{
			try
			{
				MabiData.SpeedDb.LoadFromCsv(dataPath + "/db/speed.txt", reload);
				MabiData.RaceDb.LoadFromCsv(dataPath + "/db/races.txt", reload);
				Logger.Info("Done loading " + MabiData.RaceDb.Entries.Count + " entries from races.txt.");

				MabiData.PortalDb.LoadFromCsv(dataPath + "/db/portals.txt", reload);
				Logger.Info("Done loading " + MabiData.PortalDb.Entries.Count + " entries from portals.txt.");

				MabiData.StatsBaseDb.LoadFromCsv(dataPath + "/db/stats_base.txt", reload);
				Logger.Info("Done loading " + MabiData.StatsBaseDb.Entries.Count + " entries from stats_base.txt.");

				MabiData.MotionDb.LoadFromCsv(dataPath + "/db/motions.txt", reload);
				Logger.Info("Done loading " + MabiData.MotionDb.Entries.Count + " entries from motions.txt.");

				MabiData.CharCardSetDb.LoadFromCsv(dataPath + "/db/charcardsets.txt", reload);
				MabiData.CharCardDb.LoadFromCsv(dataPath + "/db/charcards.txt", reload);
				Logger.Info("Done loading " + MabiData.CharCardSetDb.Entries.Count + " entries from charcardsets.txt.");

				MabiData.MonsterSkillDb.LoadFromCsv(dataPath + "/db/monster_skills.txt", reload);
				MabiData.MonsterDb.LoadFromCsv(dataPath + "/db/monsters.txt", reload);
				Logger.Info("Done loading " + MabiData.MonsterDb.Entries.Count + " entries from monsters.txt.");

				MabiData.SpawnDb.LoadFromCsv(dataPath + "/db/spawns.txt", reload);
				Logger.Info("Done loading " + MabiData.SpawnDb.Entries.Count + " entries from spawns.txt.");

				MabiData.ColorMapDb.LoadFromDat(dataPath + "/db/colormap.dat", reload);
				Logger.Info("Done loading " + MabiData.ColorMapDb.Entries.Count + " entries from colormap.dat.");

				MabiData.ItemDb.LoadFromCsv(dataPath + "/db/items.txt", reload);
				Logger.Info("Done loading " + MabiData.ItemDb.Entries.Count + " entries from items.txt.");

				MabiData.SkillRankDb.LoadFromCsv(dataPath + "/db/skill_ranks.txt", reload);
				MabiData.SkillDb.LoadFromCsv(dataPath + "/db/skills.txt", reload);
				Logger.Info("Done loading " + MabiData.SkillDb.Entries.Count + " entries from skills.txt.");
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

		protected virtual void OnClientAccepted(TClient client)
		{

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

				this.OnClientAccepted(client);

				client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), client);

				// Send seed
				client.Socket.Send(BitConverter.GetBytes(client.Seed));

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
					{
						client.Buffer.Remaining =
							(client.Buffer.Front[bytesRead + 1] << 0) +
							(client.Buffer.Front[bytesRead + 2] << 8) +
							(client.Buffer.Front[bytesRead + 3] << 16) +
							(client.Buffer.Front[bytesRead + 4] << 24);
					}

					// Copy to back buffer
					client.Buffer.Back[client.Buffer.Ptr++] = client.Buffer.Front[bytesRead++];

					// Full packet is not in back buffer yet
					if (--client.Buffer.Remaining > 0)
						continue;

					// Prepare buffer, cut the appended 4 bytes
					int len = client.Buffer.Ptr - 4;
					var buffer = new byte[len];
					Array.Copy(client.Buffer.Back, buffer, len);
					BitConverter.GetBytes(len).CopyTo(buffer, 1);

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
							var packet = new MabiPacket(buffer, (ushort)len);

							var handler = this.GetPacketHandler(packet.Op);
							if (handler != null)
							{
								//Logger.Debug("Handling packet: " + HexTool.ToString(packet.Op));
								try
								{
									handler(client, packet);
								}
								catch (Exception ex)
								{
									Logger.Exception(ex, "There has been a problem while handling '" + HexTool.ToString(packet.Op) + "'.", true);
								}
							}
							else
							{
								Logger.Warning("Unhandled packet: " + HexTool.ToString(packet.Op));
							}
						}
					}

					client.Buffer.InitBB();
				}

				if (client.State < SessionState.Dead)
					client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);
				else
					Logger.Warning("Bad client kill. Client attempted to send data after disconnect.");
			}
			catch (SocketException)
			{
				Logger.Info("Lost connection from " + client.Socket.RemoteEndPoint.ToString());
				this.OnClientDisconnect(client);
			}
		}
	}
}
