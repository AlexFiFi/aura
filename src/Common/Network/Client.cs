// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Common.Tools;
using Common.World;

namespace Common.Network
{
	public enum SessionState { ClientCheck, Login, LoggedIn, Dead }

	public struct SocketBuffer
	{
		public byte[] Front, Back;
		public int Ptr, Read, Remaining;

		public void InitBB()
		{
			this.Back = new byte[2048];
			this.Ptr = 0;
			this.Remaining = 0;
		}

		public void InitFB()
		{
			this.Front = new byte[2048];
			this.Read = 0;
		}
	}

	public class Client
	{
		public Socket Socket;
		public SocketBuffer Buffer;
		public SessionState State;
		public MabiAccount Account;

		public MabiCrypto Crypto;
		public readonly uint Seed;

		public Client()
		{
			this.Seed = (uint)RandomProvider.Get().Next();
			this.Crypto = new MabiCrypto(this.Seed);

			this.Buffer.InitBB();
			this.Buffer.InitFB();
		}

		public void Send(byte[] raw)
		{
			if (this.State == SessionState.Dead)
				return;

			if (raw[5] != 0x03)
				this.Crypto.EncodePacket(raw);

			try
			{
				this.Socket.Send(raw);
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to send packet to " + this.Socket.RemoteEndPoint.ToString() + ". (" + ex.Message + ")");
			}
		}

		public void Send(MabiPacket packet)
		{
			this.Send(packet.Build());
		}

		public void Send(params MabiPacket[] packets)
		{
			foreach (var packet in packets)
			{
				this.Send(packet.Build());
			}
		}

		public virtual void Kill()
		{
			if (this.State != SessionState.Dead)
			{
				try
				{
					if (this.Socket.Connected)
					{
						this.Socket.Shutdown(SocketShutdown.Both);
					}
				}
				catch (Exception ex) { Logger.Error("Failed to shutdown socket: " + ex.Message); }
				try
				{
					this.Socket.Close();
				}
				catch (Exception ex)
				{ Logger.Error("Failed to close socket: " + ex.Message); }
			}

			this.State = SessionState.Dead;
		}
	}

	public static class ClientExtenstion
	{
		public static void SendTo(this MabiPacket p, params Client[] clients)
		{
			foreach (var client in clients)
				client.Send(p);
		}
	}
}
