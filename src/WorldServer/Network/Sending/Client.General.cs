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

	public static class ClientGeneralSendingExt
	{
		public static void SendSystemMsg(this Client client, MabiCreature creature, string format, params object[] args)
		{
			var p = PacketCreator.SystemMessage(creature, format, args);

			client.Send(p);
		}

		public static void SendNotice(this Client client, string format, params object[] args)
		{
			var p = PacketCreator.Notice(string.Format(format, args));

			client.Send(p);
		}

		public static void SendLock(this Client client, MabiCreature creature, uint lockType = 0xEFFFFFFE)
		{
			var p = new MabiPacket(Op.CharacterLock, creature.Id);
			p.PutInt(lockType);
			p.PutInt(0);

			client.Send(p);
		}

		public static void SendUnlock(this Client client, MabiCreature creature, uint lockType = 0xEFFFFFFE)
		{
			var p = new MabiPacket(Op.CharacterUnlock, creature.Id);
			p.PutInt(lockType);

			client.Send(p);
		}

		public static void SendEnterRegionPermission(this Client client, MabiCreature creature, bool permission = true)
		{
			var p = new MabiPacket(Op.EnterRegionPermission, Id.World);
			var pos = creature.GetPosition();

			p.PutLong(creature.Id);
			if (permission)
			{
				p.PutByte(1);
				p.PutInt(creature.Region);
				p.PutInt(pos.X);
				p.PutInt(pos.Y);
			}
			else
			{
				p.PutByte(0);
			}

			client.Send(p);
		}
	}
}
