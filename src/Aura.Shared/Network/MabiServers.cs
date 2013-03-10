// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;

namespace Aura.Shared.Network
{
	public class MabiServers
	{
		public string Name;
		public List<MabiChannel> Channels = new List<MabiChannel>();

		public MabiServers(string name)
		{
			this.Name = name;
		}
	}

	public class MabiChannel
	{
		public string Name;
		public string IP;
		public ushort Port;

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

		public MabiChannel(string name, string ip, ushort port, ChannelState state = 0, ChannelEvent events = 0, byte stress = 0)
		{
			this.Name = name;
			this.IP = ip;
			this.Port = port;

			this.State = state;
			this.Events = events;
			this.Stress = stress;
		}
	}

	public enum ChannelState { Maintenance, Normal, Busy, Full, Error = 5 }
	public enum ChannelEvent { Normal, Event, PvP, EventPvP }
}