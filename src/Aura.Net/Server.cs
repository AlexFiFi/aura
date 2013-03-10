// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Aura.Net
{
	public abstract class Server<TClient> where TClient : Client, new()
	{
		protected Socket _serverSocket;
		protected List<TClient> _clients;

		public Server()
		{
			_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clients = new List<TClient>();
		}

		/// <summary>
		/// Binds server socket and starts listening.
		/// </summary>
		/// <param name="iep"></param>
		protected void StartListening(IPEndPoint iep)
		{
			this.OnServerStartUp();

			_serverSocket.Bind(iep);
			_serverSocket.Listen(10);

			_serverSocket.BeginAccept(new AsyncCallback(OnAccept), _serverSocket);
		}

		protected void StartListening(int port)
		{
			this.StartListening(new IPEndPoint(IPAddress.Any, port));
		}

		protected void StartListening(string ip, int port)
		{
			this.StartListening(new IPEndPoint(IPAddress.Parse(ip), port));
		}

		/// <summary>
		/// Closes server socket (no further connections will be accepted).
		/// </summary>
		protected void StopListening()
		{
			_serverSocket.Close();
		}

		/// <summary>
		/// Callback for when a client establishes a connection;
		/// starts listening for data.
		/// </summary>
		/// <param name="result"></param>
		protected void OnAccept(IAsyncResult result)
		{
			var client = new TClient();

			try
			{
				client.Socket = (result.AsyncState as Socket).EndAccept(result);
			}
			catch (ObjectDisposedException)
			{
				return;
			}

			try
			{
				client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), client);

				lock (_clients)
					_clients.Add(client);
				this.OnClientAccepted(client);
			}
			catch (Exception ex)
			{
				this.OnReceiveException(client, ex);
			}
			finally
			{
				_serverSocket.BeginAccept(new AsyncCallback(this.OnAccept), _serverSocket);
			}
		}

		/// <summary>
		/// Callback for when data is received from a client.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnReceive(IAsyncResult result)
		{
			var client = result.AsyncState as TClient;

			try
			{
				int bytesReceived = client.Socket.EndReceive(result);

				if (bytesReceived <= 0)
				{
					lock (_clients)
						_clients.Remove(client);
					this.OnClientDisconnect(client, DisconnectType.ClosedByClient);
					return;
				}

				if (this.ReadBuffer(client, bytesReceived))
					client.Socket.BeginReceive(client.Buffer.Front, 0, client.Buffer.Front.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);
			}
			catch (SocketException)
			{
				lock (_clients)
					_clients.Remove(client);
				this.OnClientDisconnect(client, DisconnectType.Unexpected);
			}
			//catch (ObjectDisposedException)
			//{
			//}
			//catch (Exception ex)
			//{
			//    this.OnBufferReadException(client, ex);
			//}
		}

		public virtual bool ReadBuffer(TClient client, int length)
		{
			return true;
		}

		/// <summary>
		/// Callback for when the server is started, before listening for connections.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnServerStartUp()
		{ }

		/// <summary>
		/// Callback for when a client connection was accepted successfully.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnClientAccepted(TClient client)
		{ }

		/// <summary>
		/// Callback for when a client connection was disconnected.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnClientDisconnect(TClient client, DisconnectType type)
		{ }

		/// <summary>
		/// Callback for when an exception is raised while receiving data.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnReceiveException(TClient client, Exception ex)
		{ }

		/// <summary>
		/// Callback for when an exception is raised while reading the buffer.
		/// </summary>
		/// <param name="result"></param>
		protected virtual void OnBufferReadException(TClient client, Exception ex)
		{ }
	}

	public enum DisconnectType { Unexpected, ClosedByClient }
}
