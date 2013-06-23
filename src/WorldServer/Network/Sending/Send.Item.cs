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
		/// <summary>
		/// Sends item box packet for regular dyed item to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="itemId"></param>
		/// <param name="selected"></param>
		public static void AcquireDyedItem(Client client, MabiCreature creature, ulong itemId, byte selected)
		{
			var packet = new MabiPacket(Op.AcquireInfo2, creature.Id);
			packet.PutString("<xml type='dyeing' objectid='{0}' selected='{1}'/>", itemId, selected);
			packet.PutInt(3000);

			client.Send(packet);
		}

		/// <summary>
		/// Sends item box packet for fixed dyed item to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="itemId"></param>
		public static void AcquireDyedItem(Client client, MabiCreature creature, ulong itemId)
		{
			var packet = new MabiPacket(Op.AcquireInfo2, creature.Id);
			packet.PutString("<xml type='fixed_color_dyeing' objectid='{0}'/>", itemId);
			packet.PutInt(3000);

			client.Send(packet);
		}
	}
}
