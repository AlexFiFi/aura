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
	}
}
