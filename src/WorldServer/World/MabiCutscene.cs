// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;

namespace Aura.World.World
{
	/// <summary>
	/// A cutscene is a pre-defined sequence of events, found in the
	/// client's data. The name and ids for actors can be fetched there.
	/// </summary>
	public class MabiCutscene
	{
		public List<Tuple<string, byte[]>> Actors = new List<Tuple<string, byte[]>>();

		public MabiCreature Leader { get; protected set; }
		public string Name { get; protected set; }

		public Action<WorldClient> OnComplete = null;

		public MabiCutscene(MabiCreature leader, string cutsceneName)
		{
			this.Leader = leader;
			this.Name = cutsceneName;
		}

		public void AddActor(string name, MabiCreature creature)
		{
			var packet = new MabiPacket(0);
			packet.AddCreatureInfo(creature, Send.CreaturePacketType.Public);
			AddActor(name, packet.Build(false));
		}

		public void AddActor(string name, byte[] creatureData)
		{
			this.Actors.Add(new Tuple<string, byte[]>(name, creatureData));
		}

		/// <summary>
		/// Starts the cutscene.
		/// Sends: EntityDisappears, EntitiesDisappear, Lock, CutsceneStart
		/// </summary>
		/// <param name="client"></param>
		public void Play(WorldClient client)
		{
			client.Character.CurrentCutscene = this;

			Send.EntityDisappears(client.Character);
			Send.EntitiesDisappear(client, WorldManager.Instance.GetCreaturesInRange(client.Character));
			Send.CharacterLock(client, client.Character);
			Send.CutsceneStart(client, this);
		}
	}
}
