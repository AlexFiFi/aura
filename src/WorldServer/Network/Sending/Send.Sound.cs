// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.World;
using Aura.Shared.Network;

namespace Aura.World.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Plays sound in range of source.
		/// </summary>
		/// <param name="file">e.g. "data/sound/Glasgavelen_blowaway_endure.wav"</param>
		public static void PlaySound(string file, MabiEntity source)
		{
			var packet = new MabiPacket(Op.PlaySound, source.Id);
			packet.PutString(file);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, source);
		}
	}
}
