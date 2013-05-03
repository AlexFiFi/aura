// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System;

namespace Aura.Shared.Network
{
	public class MabiServer
	{
		public string Name;
		public Dictionary<string, MabiChannel> Channels = new Dictionary<string, MabiChannel>();

		public MabiServer(string name)
		{
			this.Name = name;
		}

		public void AddToPacket(MabiPacket packet)
		{
			packet.PutString(this.Name);
			packet.PutShort(0); // Server type?
			packet.PutShort(0);
			packet.PutByte(1);

			// Channels
			// ----------------------------------------------------------
			packet.PutInt((uint)this.Channels.Count);
			foreach (var channel in this.Channels.Values)
			{
				var state = channel.State;
				if ((DateTime.Now - channel.LastUpdate).TotalSeconds > 90)
					state = ChannelState.Maintenance;

				packet.PutString(channel.Name);
				packet.PutInt((uint)state);
				packet.PutInt((uint)channel.Events);
				packet.PutInt(0); // 1 for Housing? Hidden?
				packet.PutShort(channel.Stress);
			}
		}
	}

	public class MabiChannel
	{
		public string Name;
		public string FullName;
		public string IP;
		public ushort Port;
		public DateTime LastUpdate = DateTime.MinValue;

		/// <summary>
		/// 0 = Maintenance,
		/// 1 = Normal,
		/// 2 = Busy,
		/// 3 = Full,
		/// 5 = Error
		/// </summary>
		public ChannelState State;

		/// <summary>
		/// 0 = Normal,
		/// 1 = Event,
		/// 2 = PvP,
		/// 3 = Event/PvP
		/// </summary>
		public ChannelEvent Events;

		/// <summary>
		/// 0-75
		/// </summary>
		public byte Stress;

		public MabiChannel(string name, string server, string ip, ushort port)
		{
			this.Name = name;
			this.FullName = name + "@" + server;
			this.IP = ip;
			this.Port = port;

			this.State = ChannelState.Normal;
			this.Events = ChannelEvent.Normal;
		}
	}

	public enum ChannelState { Maintenance, Normal, Busy, Full, Error = 5 }
	public enum ChannelEvent { Normal, Event, PvP, EventPvP }
}