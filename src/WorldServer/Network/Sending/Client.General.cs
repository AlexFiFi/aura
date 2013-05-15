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
	}
}
