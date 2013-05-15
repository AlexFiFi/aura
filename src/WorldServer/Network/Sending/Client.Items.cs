// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.World;
using Aura.Shared.Const;
using Aura.Shared.Network;

namespace Aura.World.Network
{
	// These extensions are an experiment.

	public static class ClientItemsSendingExt
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="itemId"></param>
		/// <param name="selected"></param>
		public static void SendAcquireDyedItem(this Client client, MabiCreature creature, ulong itemId, byte selected)
		{
			var p = new MabiPacket(Op.AcquireInfo2, creature.Id);
			p.PutString("<xml type='dyeing' objectid='{0}' selected='{1}'/>", itemId, selected);
			p.PutInt(3000);

			client.Send(p);
		}

		public static void SendAcquireDyedItem(this Client client, MabiCreature creature, ulong itemId)
		{
			var p = new MabiPacket(Op.AcquireInfo2, creature.Id);
			p.PutString("<xml type='fixed_color_dyeing' objectid='{0}'/>", itemId);
			p.PutInt(3000);

			client.Send(p);
		}
	}
}
