// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.World;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Player;

namespace Aura.World.Network
{
	public static partial class Send
	{
		public static void GuildstoneLocation(Client client, MabiCreature creature)
		{
			var packet = new MabiPacket(Op.GuildstoneLocation, creature.Id);
			packet.PutByte(1);
			packet.PutInts(creature.Guild.Region);
			packet.PutInts(creature.Guild.X);
			packet.PutInts(creature.Guild.Y);

			client.Send(packet);
		}

		/// <summary>
		/// Sends GuildApplyR to creature's client.
		/// </summary>
		public static void GuildApplyR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.GuildApplyR, creature.Id);
			packet.PutByte(success);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends GuildDonateR to creature's client.
		/// </summary>
		public static void GuildDonateR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.GuildDonateR, creature.Id);
			packet.PutByte(success);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ConvertGpConfirmR to creature's client.
		/// </summary>
		public static void ConvertGpConfirmR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.ConvertGpConfirmR, creature.Id);
			packet.PutByte(success);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ConvertGpR to creature's client.
		/// </summary>
		public static void ConvertGpR(MabiCreature creature, bool success, uint amount)
		{
			var packet = new MabiPacket(Op.ConvertGpR, creature.Id);
			packet.PutByte(success);
			packet.PutInt(amount);

			creature.Client.Send(packet);
		}
	}
}
