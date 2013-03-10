// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using Aura.Data;
using Aura.Shared.Database;
using Aura.Shared.Util;

namespace Aura.Shared.Network
{
	public abstract class Server<TClient> : Aura.Net.Server<TClient> where TClient : Client, new()
	{
		protected Dictionary<uint, PacketHandlerFunc> _handlers;

		protected DateTime _startTime = DateTime.Now;

		public Server()
			: base()
		{
			_handlers = new Dictionary<uint, PacketHandlerFunc>();
		}

		/// <summary>
		/// Prints Aura's standard header.
		/// </summary>
		/// <param name="title">Name of this server. Example: "Login Server"</param>
		public void WriteHeader(string title = null, ConsoleColor color = ConsoleColor.DarkGray)
		{
			if (title != null)
				Console.Title = "Aura : " + title;

			Console.ForegroundColor = color;
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

		protected void TryConnectToDatabase(string host, string user, string pass, string db)
		{
			try
			{
				MabiDb.Instance.Init(host, user, pass, db);
				MabiDb.Instance.TestConnection();
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to connect to database. ({0})", ex.Message);
				this.Exit(1);
			}
		}

		public void LoadData(string dataPath, DataLoad toLoad = DataLoad.All, bool reload = false)
		{
			try
			{
				if ((toLoad & DataLoad.Spawns) != 0)
				{
					MabiData.SpawnDb.Load(dataPath + "/db/spawns.txt", reload);
					this.PrintDataWarnings(MabiData.SpawnDb.Warnings);

					Logger.Info("Done loading " + MabiData.SpawnDb.Count + " entries from spawns.txt.");
				}

				if ((toLoad & DataLoad.Races) != 0)
				{
					MabiData.RaceSkillDb.Load(dataPath + "/db/race_skills.txt", reload);
					this.PrintDataWarnings(MabiData.RaceSkillDb.Warnings);

					Logger.Info("Done loading " + MabiData.RaceSkillDb.Count + " entries from race_skills.txt.");

					MabiData.SpeedDb.Load(dataPath + "/db/speed.txt", reload);
					this.PrintDataWarnings(MabiData.SpeedDb.Warnings);

					MabiData.FlightDb.Load(dataPath + "/db/flight.txt", reload);
					this.PrintDataWarnings(MabiData.FlightDb.Warnings);

					MabiData.RaceDb.Load(dataPath + "/db/races.txt", reload);
					this.PrintDataWarnings(MabiData.RaceDb.Warnings);

					Logger.Info("Done loading " + MabiData.RaceDb.Count + " entries from races.txt.");
				}

				if ((toLoad & DataLoad.StatsBase) != 0)
				{
					MabiData.StatsBaseDb.Load(dataPath + "/db/stats_base.txt", reload);
					this.PrintDataWarnings(MabiData.StatsBaseDb.Warnings);

					Logger.Info("Done loading " + MabiData.StatsBaseDb.Count + " entries from stats_base.txt.");
				}

				if ((toLoad & DataLoad.StatsLevel) != 0)
				{
					MabiData.StatsLevelUpDb.Load(dataPath + "/db/stats_levelup.txt", reload);
					this.PrintDataWarnings(MabiData.StatsLevelUpDb.Warnings);

					Logger.Info("Done loading " + MabiData.StatsLevelUpDb.Count + " entries from stats_levelup.txt.");
				}

				if ((toLoad & DataLoad.Motions) != 0)
				{
					MabiData.MotionDb.Load(dataPath + "/db/motions.txt", reload);
					this.PrintDataWarnings(MabiData.MotionDb.Warnings);

					Logger.Info("Done loading " + MabiData.MotionDb.Count + " entries from motions.txt.");
				}

				if ((toLoad & DataLoad.Cards) != 0)
				{
					MabiData.CharCardSetDb.Load(dataPath + "/db/charcardsets.txt", reload);
					this.PrintDataWarnings(MabiData.CharCardSetDb.Warnings);

					MabiData.CharCardDb.Load(dataPath + "/db/charcards.txt", reload);
					this.PrintDataWarnings(MabiData.CharCardDb.Warnings);

					Logger.Info("Done loading " + MabiData.CharCardSetDb.Count + " entries from charcardsets.txt.");
				}

				if ((toLoad & DataLoad.Colors) != 0)
				{
					MabiData.ColorMapDb.Load(dataPath + "/db/colormap.dat", reload);
					this.PrintDataWarnings(MabiData.ColorMapDb.Warnings);

					Logger.Info("Done loading " + MabiData.ColorMapDb.Count + " entries from colormap.dat.");
				}

				if ((toLoad & DataLoad.Items) != 0)
				{
					MabiData.ItemDb.Load(dataPath + "/db/items.txt", reload);
					this.PrintDataWarnings(MabiData.ItemDb.Warnings);

					Logger.Info("Done loading " + MabiData.ItemDb.Count + " entries from items.txt.");
				}

				if ((toLoad & DataLoad.Skills) != 0)
				{
					MabiData.SkillRankDb.Load(dataPath + "/db/skill_ranks.txt", reload);
					this.PrintDataWarnings(MabiData.SkillRankDb.Warnings);

					MabiData.SkillDb.Load(dataPath + "/db/skills.txt", reload);
					this.PrintDataWarnings(MabiData.SkillDb.Warnings);

					Logger.Info("Done loading " + MabiData.SkillDb.Count + " entries from skills.txt.");
				}

				if ((toLoad & DataLoad.Regions) != 0)
				{
					MabiData.MapDb.Load(dataPath + "/db/maps.txt", reload);
					this.PrintDataWarnings(MabiData.MapDb.Warnings);

					Logger.Info("Done loading " + MabiData.MapDb.Count + " entries from maps.txt.");

					MabiData.RegionDb.Load(dataPath + "/db/regioninfo.dat", reload);
					this.PrintDataWarnings(MabiData.RegionDb.Warnings);

					Logger.Info("Done loading " + MabiData.RegionDb.Count + " entries from regioninfo.dat.");
				}

				if ((toLoad & DataLoad.Shamala) != 0)
				{
					MabiData.ShamalaDb.Load(dataPath + "/db/shamala.txt", reload);
					this.PrintDataWarnings(MabiData.ShamalaDb.Warnings);

					Logger.Info("Done loading " + MabiData.ShamalaDb.Count + " entries from shamala.txt.");
				}

				if ((toLoad & DataLoad.PropDrops) != 0)
				{
					MabiData.PropDropDb.Load(dataPath + "/db/prop_drops.txt", reload);
					this.PrintDataWarnings(MabiData.PropDropDb.Warnings);

					Logger.Info("Done loading " + MabiData.PropDropDb.Count + " entries from prop_drops.txt.");
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

		protected void PrintDataWarnings(List<DatabaseWarningException> warnings)
		{
			foreach (var ex in warnings)
			{
				Logger.Warning(ex.ToString());
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

		/// <summary>
		/// Called whenever a command is used. Should contain the handling.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="command"></param>
		protected virtual void ParseCommand(string[] args, string command)
		{ }

		/// <summary>
		/// This method should contain the code to initiate and start the server.
		/// </summary>
		/// <param name="args"></param>
		public abstract void Run(string[] args);

		protected override void OnClientAccepted(TClient client)
		{
			Logger.Info("Connection established from '{0}'.", client.Address);

			client.Socket.Send(BitConverter.GetBytes(client.Crypto.Seed));
		}

		protected override void OnClientDisconnect(TClient client, Aura.Net.DisconnectType type)
		{
			if (type == Aura.Net.DisconnectType.ClosedByClient)
				// Connection closed normally.
				Logger.Info("Connection closed from '{0}'.", client.Address);
			else
				// Connection lost unexpectedly (process killed, cable pulled, etc.)
				Logger.Info("Connection lost from '{0}'.", client.Address);

			client.Kill();
		}

		public override bool ReadBuffer(TClient client, int bytesReceived)
		{
			int bytesRead = 0;

			while (bytesRead < bytesReceived)
			{
				// New packet
				if (client.Buffer.Remaining < 1)
					client.Buffer.Remaining = this.GetPacketLength(client.Buffer.Front, bytesRead);

				// Copy 1 byte from front to back buffer.
				client.Buffer.Back[client.Buffer.Ptr++] = client.Buffer.Front[bytesRead++];

				// Check if back buffer contains full packet.
				if (--client.Buffer.Remaining > 0)
					continue;

				// Preparations for the buffer.
				this.PrepareBuffer(ref client.Buffer.Back, client.Buffer.Ptr);

				// Turn buffer into packet and handle it.
				this.HandleBuffer(client, client.Buffer.Back);

				client.Buffer.InitBB();
			}

			return (client.State < ClientState.Dead);
		}

		/// <summary>
		/// Reads the length of a new packet from the buffer.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="ptr"></param>
		/// <returns></returns>
		protected virtual int GetPacketLength(byte[] buffer, int ptr)
		{
			// <0x??><int:length><...>
			return
				(buffer[ptr + 1] << 0) +
				(buffer[ptr + 2] << 8) +
				(buffer[ptr + 3] << 16) +
				(buffer[ptr + 4] << 24);
		}

		protected virtual void PrepareBuffer(ref byte[] buffer, int length)
		{
			// Cut 4 bytes at the end (some checksum?)
			Array.Resize(ref buffer, length -= 4);

			// Write new length into the buffer.
			BitConverter.GetBytes(length).CopyTo(buffer, 1);
		}

		protected virtual void HandleBuffer(TClient client, byte[] buffer)
		{
			// Decrypt packet if crypt flag isn't 3.
			if (buffer[5] != 0x03)
				client.Crypto.DecodePacket(ref buffer);

			// Flag 1 is a ping or something, encode and send back.
			if (buffer[5] == 0x01)
			{
				client.Crypto.EncodePacket(ref buffer);
				client.Socket.Send(buffer);
			}
			else
			{
				// First packet, answer to the seed
				if (client.State == ClientState.Check)
				{
					client.Send(new byte[] { 0x88, 0x07, 0x00, 0x00, 0x00, 0x00, 0x07 });

					client.State = ClientState.LoggingIn;
				}
				// Actual packets
				else
				{
					var packet = new MabiPacket(buffer);
					this.HandlePacket(client, packet);
				}
			}
		}

		protected virtual void HandlePacket(TClient client, MabiPacket packet)
		{
			try
			{
				var handler = this.GetPacketHandler(packet.Op);
				if (handler != null)
				{
					handler(client, packet);
				}
				else
				{
					Logger.Unimplemented("Unhandled packet: " + packet.Op.ToString("X"));
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, true, "There has been a problem while handling '{0}'.", packet.Op.ToString("X"));
			}
		}

		protected void RegisterPacketHandler(uint op, PacketHandlerFunc handler)
		{
			_handlers.Add(op, handler);
		}

		/// <summary>
		/// Retrieves a packet handler from the list of registered handlers.
		/// </summary>
		/// <param name="op"></param>
		/// <returns>Returns null if there is no handler for this op.</returns>
		public PacketHandlerFunc GetPacketHandler(uint op)
		{
			if (_handlers.ContainsKey(op))
				return _handlers[op];

			return null;
		}

		public delegate void PacketHandlerFunc(TClient client, MabiPacket packet);
	}

	public enum DataLoad
	{
		Spawns = 0x01,
		Skills = 0x02,
		Races = 0x04,
		StatsBase = 0x08,
		StatsLevel = 0x10,
		Motions = 0x20,
		Cards = 0x40,
		Colors = 0x80,
		Items = 0x100,
		Regions = 0x200,
		Shamala = 0x400,
		PropDrops = 0x800,

		All = 0xFFFF,

		LoginServer = Skills | Races | StatsBase | StatsLevel | Cards | Colors | Items | Regions,
		WorldServer = All,
		Npcs = Races | Spawns,
	}
}
