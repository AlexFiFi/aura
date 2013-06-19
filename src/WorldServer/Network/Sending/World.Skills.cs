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

	public static class WorldSkillsSendingExt
	{
		/// <summary>
		/// Sends skill init flash effect to all players in range of creature.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="str"></param>
		public static void SendFlash(this WorldManager wm, MabiCreature creature)
		{
			SendSkillInitEffect(wm, creature, "flashing");
		}

		/// <summary>
		/// Sends skill init effect to all players in range of creature.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="str"></param>
		public static void SendSkillInitEffect(this WorldManager wm, MabiCreature creature, string str = "")
		{
			var p = new MabiPacket(Op.Effect, creature.Id);
			p.PutInt(Effect.SkillInit);
			p.PutString(str);

			wm.Broadcast(p, SendTargets.Range, creature);
		}
	}
}
