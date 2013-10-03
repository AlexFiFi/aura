// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Net.Sockets;
using Aura.Shared.Util;

namespace Aura.Shared.Network
{
	/// <summary>
	/// This class derives from the external Client class and extends it with
	/// all methods that are required by all clients. Might be used directly,
	/// if no further customization is required.
	/// </summary>
	public class Client : Aura.Net.Client
	{
		/// <summary>
		/// Current state of client/connection.
		/// </summary>
		public ClientState State { get; set; }

		/// <summary>
		/// Encryption object for this client.
		/// </summary>
		public MabiCrypto Crypto { get; set; }

		public Client()
		{
			this.Crypto = new MabiCrypto(0x41757261); // 0xAura
		}

		/// <summary>
		/// Encodes array if necessary and sends it to the client.
		/// </summary>
		/// <param name="raw"></param>
		public virtual void Send(byte[] raw)
		{
			if (this.State == ClientState.Dead)
				return;

			this.Encode(raw);

			try
			{
				this.Socket.Send(raw);
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to send packet to '{0}'. ({1})", this.Address, ex.Message);
			}
		}

		/// <summary>
		/// Encodes array if necessary.
		/// </summary>
		/// <param name="raw"></param>
		public virtual void Encode(byte[] raw)
		{
			if (raw[5] != 0x03)
				this.Crypto.EncodePacket(ref raw);
		}

		/// <summary>
		/// Builts packet and sends it.
		/// </summary>
		/// <param name="packet"></param>
		public virtual void Send(MabiPacket packet)
		{
			//Logger.Debug(packet);

			this.Send(packet.Build());
		}

		/// <summary>
		/// Sends all packets.
		/// </summary>
		/// <param name="packets"></param>
		public void Send(params MabiPacket[] packets)
		{
			foreach (var packet in packets)
				this.Send(packet);
		}

		/// <summary>
		/// Shuts down client socket.
		/// </summary>
		public override void Kill()
		{
			if (this.State != ClientState.Dead)
			{
				try
				{
					if (this.Socket.Connected)
						this.Socket.Shutdown(SocketShutdown.Both);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to shutdown socket: " + ex.Message);
				}
				try
				{
					this.Socket.Close();
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to close socket: " + ex.Message);
				}

				this.State = ClientState.Dead;
			}
		}
	}

	public enum ClientState { Check, LoggingIn, LoggedIn, Dead }
}
