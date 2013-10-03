// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.World;

namespace Aura.World.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Broadcasts pvp information for creature in range.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void PvPInformation(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.PvPInformation, creature.Id);
			packet.AddPvPInfo(creature);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		/// <summary>
		/// Broadcasts pvp information for creature in region.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="region"></param>
		public static void PvPInformation(MabiCreature creature, uint region)
		{
			var packet = new MabiPacket(Op.PvPInformation, creature.Id);
			packet.AddPvPInfo(creature);

			WorldManager.Instance.BroadcastRegion(packet, region);
		}

		private static void AddPvPInfo(this MabiPacket packet, MabiCreature creature)
		{
			var arena = creature.ArenaPvPManager != null;

			packet.PutByte(arena); // ArenaPvP
			packet.PutInt(arena ? creature.ArenaPvPManager.GetTeam(creature) : (uint)0);
			packet.PutByte(creature.TransPvPEnabled);
			packet.PutInt(arena ? creature.ArenaPvPManager.GetStars(creature) : 0);
			packet.PutByte(creature.EvGEnabled);
			packet.PutByte(creature.EvGSupportRace);
			packet.PutByte(1); // IsPvPMode
			packet.PutLong(creature.PvPWins);
			packet.PutLong(creature.PvPLosses);
			packet.PutInt(0);// PenaltyPoints
			packet.PutByte(1);  // unk

			// [170300] ?
			{
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
			}
		}
	}
}
