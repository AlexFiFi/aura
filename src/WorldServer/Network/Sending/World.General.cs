// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.World;
using Aura.Shared.Network;
using Aura.Shared.Const;

namespace Aura.World.Network
{
	// These extensions are an experiment.

	public static class WorldGeneralSendingExt
	{
		/// <summary>
		/// Position update.
		/// </summary>
		/// <param name="wm"></param>
		/// <param name="creature"></param>
		public static void SendStopMove(this WorldManager wm, MabiCreature creature)
		{
			var pos = creature.GetPosition();

			var p = new MabiPacket(Op.RunTo, creature.Id);
			p.PutInts(pos.X, pos.Y); // From
			p.PutInts(pos.X, pos.Y); // To
			p.PutBytes(1, 0);

			wm.Broadcast(p, SendTargets.Range, creature);
		}

		public static void SendStatusEffectUpdate(this WorldManager wm, MabiCreature creature)
		{
			var p = new MabiPacket(Op.StatusEffectUpdate, creature.Id);
			p.PutLong((ulong)creature.Conditions.A);
			p.PutLong((ulong)creature.Conditions.B);
			p.PutLong((ulong)creature.Conditions.C);
			p.PutLong((ulong)creature.Conditions.D);
			p.PutInt(0);

			wm.Broadcast(p, SendTargets.Range, creature);
		}
	}
}
