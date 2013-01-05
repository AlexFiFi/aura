// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Common.Database;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Scripting;
using World.World;
using Common.Constants;
using Common.Data;

namespace World.Network
{
	public class WorldClient : Client
	{
		public List<MabiCreature> Creatures = new List<MabiCreature>();
		public MabiCreature Character;

		public readonly NPCSession NPCSession = new NPCSession();

		public override void Kill()
		{
			if (this.State != SessionState.Dead)
			{
				foreach (var creature in this.Creatures)
					WorldManager.Instance.RemoveCreature(creature);

				if (this.Account != null)
					MabiDb.Instance.SaveAccount(this.Account);

				if (this.Socket != null)
				{
					this.Socket.Shutdown(SocketShutdown.Both);
					this.Socket.Close();
				}

				this.State = SessionState.Dead;
			}
			else
			{
				Logger.Warning("Client got killed multiple times." + Environment.NewLine + Environment.StackTrace);
			}
		}

		public void Disconnect(int seconds = 5)
		{
			this.Send(new MabiPacket(Op.RequestClientDisconnect, Id.World).PutSInt(seconds * 1000));
		}

		public void Warp(string region, uint x, uint y)
		{
			var regionId = MabiData.MapDb.TryGetRegionId(region);
			if (regionId > 0)
				this.Warp(regionId, x, y);
		}

		public void Warp(uint region, uint x, uint y)
		{
			var pos = this.Character.GetPosition();
			this.Send(new MabiPacket(Op.SetLocation, this.Character.Id).PutByte(1).PutInts(this.Character.Region, pos.X, pos.Y));
			this.Send(new MabiPacket(Op.WarpUnk1, this.Character.Id));
			this.Send(new MabiPacket(Op.WarpUnk2, this.Character.Id).PutByte(11));

			WorldManager.Instance.CreatureLeaveRegion(this.Character);

			if (this.Creatures.Count > 1)
			{
				this.Send(PacketCreator.EntitiesLeave(this.Creatures.Where(c => c != this.Character)));
			}

			this.Send(PacketCreator.Lock(this.Character));
			this.Character.SetLocation(region, x, y);
			this.Send(PacketCreator.EnterRegionPermission(this.Character));

			foreach (var c in this.Creatures.Where(c => c != this.Character))
			{
				this.Send(PacketCreator.Lock(c));
			}

			this.Send(new MabiPacket(Op.WarpUnk3, this.Character.Id).PutLong(0).PutInt(0));
		}
	}
}
