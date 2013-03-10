// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Aura.Net
{
	public enum SessionState { ClientCheck, Login, LoggedIn, Dead }

	public struct SocketBuffer
	{
		/// <summary>
		/// Buffer used by the socket to receive and read data.
		/// </summary>
		public byte[] Front;
		/// <summary>
		/// Eventually contains full packet in bytes.
		/// </summary>
		public byte[] Back;

		/// <summary>
		/// Current position in the back buffer.
		/// </summary>
		public int Ptr { get; set; }
		/// <summary>
		/// Remaining bytes till back buffer contains full packet.
		/// </summary>
		public int Remaining { get; set; }

		/// <summary>
		/// Initiates back buffer.
		/// </summary>
		public void InitBB()
		{
			this.Back = new byte[2048];
			this.Ptr = 0;
			this.Remaining = 0;
		}

		/// <summary>
		/// Initiates front buffer.
		/// </summary>
		public void InitFB()
		{
			this.Front = new byte[2048];
		}
	}

	public class Client
	{
		public Socket Socket;
		public SocketBuffer Buffer;

		/// <summary>
		/// Returns .Socket.RemoteEndPoint or empty string if socket is null.
		/// </summary>
		public string Address
		{
			get { return (this.Socket != null ? this.Socket.RemoteEndPoint.ToString() : string.Empty); }
		}

		public string IP
		{
			get { return (this.Socket != null ? (this.Socket.RemoteEndPoint as IPEndPoint).Address.ToString() : string.Empty); }
		}

		public Client()
		{
			this.Buffer.InitBB();
			this.Buffer.InitFB();
		}

		public virtual void Kill()
		{
			try
			{
				if (this.Socket.Connected)
					this.Socket.Shutdown(SocketShutdown.Both);
				this.Socket.Close();
			}
			catch (Exception)
			{ }
		}
	}
}
