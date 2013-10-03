// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Shared.Util;

namespace Aura.Shared.Network
{
	/// <summary>
	/// This class derives from the external Server class and extends it with
	/// all methods that are required by all servers.
	/// </summary>
	/// <typeparam name="TClient"></typeparam>
	public abstract class BaseServer<TClient> : Aura.Net.Server<TClient> where TClient : Client, new()
	{
		protected Dictionary<uint, PacketHandlerFunc> _handlers;

		protected DateTime _startTime = DateTime.Now;

		public BaseServer()
			: base()
		{
			_handlers = new Dictionary<uint, PacketHandlerFunc>();
		}

		/// <summary>
		/// This method should contain the code to initiate and start the server.
		/// </summary>
		/// <param name="args"></param>
		public abstract void Run(string[] args);

		/// <summary>
		/// Sends seed to the newly connected client.
		/// </summary>
		/// <param name="client"></param>
		protected override void OnClientAccepted(TClient client)
		{
			Logger.Info("Connection established from '{0}'.", client.Address);

			client.Socket.Send(BitConverter.GetBytes(client.Crypto.Seed));
		}

		/// <summary>
		/// Called whenever a connection is closed.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="type"></param>
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

		/// <summary>
		/// Reads data from the TCP stream and passes it to HandleBuffer,
		/// once a full packet was received.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="bytesReceived"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Cuts off 4 byte value found on normal Mabi packets
		/// and updates packet length.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="length"></param>
		protected virtual void PrepareBuffer(ref byte[] buffer, int length)
		{
			// Cut 4 bytes at the end (some checksum?)
			Array.Resize(ref buffer, length -= 4);

			// Write new length into the buffer.
			BitConverter.GetBytes(length).CopyTo(buffer, 1);
		}

		/// <summary>
		/// Handles incoming data and passes actual packets that have to
		/// be handled to HandlePacket.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="buffer"></param>
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
				// First packet, skip challenge and send success msg.
				if (client.State == ClientState.Check)
				{
					client.Send(new byte[] { 0x88, 0x07, 0x00, 0x00, 0x00, 0x00, 0x07 });

					client.State = ClientState.LoggingIn;
				}
				// Actual packets
				else
				{
					var packet = new MabiPacket(buffer);
					//Logger.Debug(packet);
					this.HandlePacket(client, packet);
				}
			}
		}

		/// <summary>
		/// Passes packet to the respective handler, depending on the op.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		protected virtual void HandlePacket(TClient client, MabiPacket packet)
		{
			try
			{
				//Logger.Debug(packet);

				var handler = this.GetPacketHandler(packet.Op);
				if (handler != null)
				{
					handler.Invoke(client, packet);
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

		/// <summary>
		/// Adds the handler.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="handler"></param>
		public void RegisterPacketHandler(uint op, PacketHandlerFunc handler)
		{
			if (_handlers.ContainsKey(op))
				Logger.Info("Packet handler '{0:X8}' is being overwritten.", op);
			_handlers[op] = handler;
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
}
