// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Common.Database;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Scripting;
using World.World;

namespace World.Network
{
	public class WorldClient : Client
	{
		public List<MabiCreature> Creatures = new List<MabiCreature>();
		public MabiCreature Character;

		public readonly NPCSession NPCSession = new NPCSession();

		public override void Kill()
		{
			// TODO: There is some kind of client disconnect packet, isn't there?
			if (this.State != SessionState.Dead)
			{
				foreach (var creature in this.Creatures)
					WorldManager.Instance.RemoveCreature(creature);

				if (this.Account != null)
					MabiDb.Instance.SaveAccount(this.Account);

				this.Socket.Shutdown(SocketShutdown.Both);
				this.Socket.Close();

				this.State = SessionState.Dead;
			}
			else
			{
				Logger.Warning("Client got killed multiple times." + Environment.NewLine + Environment.StackTrace);
			}
		}

		public void Warp(uint region, uint x, uint y)
		{
			// TODO: Find a better place for this. Do we even need to send information about all creatures here?

			// Despawn client's creatures and move them
			foreach (var creature in this.Creatures)
			{
				WorldManager.Instance.CreatureLeaveRegion(creature);
				creature.SetLocation(region, x, y);
			}

			// Tell the client to change location
			this.Send(PacketCreator.EnterRegionPermission(this.Character));
		}
	}
}
