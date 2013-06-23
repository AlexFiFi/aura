using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;

namespace Aura.World.World
{
	public class MabiCutscene
	{
		private MabiCreature _leader;
		private string _name;
		private List<Tuple<string, byte[]>> _actors = new List<Tuple<string, byte[]>>();

		public Action<WorldClient> OnComplete = null;

		public MabiCutscene(MabiCreature leader, string cutsceneName)
		{
			_leader = leader;
			_name = cutsceneName;
		}

		public void AddActor(string name, MabiCreature creature)
		{
			var p = new MabiPacket(0);
			creature.AddToPacket(p);
			AddActor(name, p.Build(false));
		}

		public void AddActor(string name, byte[] creatureData)
		{
			_actors.Add(new Tuple<string, byte[]>(name, creatureData));
		}

		public bool IsLeader(MabiCreature creature)
		{
			return _leader == creature;
		}

		public void Send(WorldClient client)
		{
			Aura.World.Network.Send.EntityDisappears(client.Character);
			Aura.World.Network.Send.EntitiesDisappear(client, WorldManager.Instance.GetCreaturesInRange(client.Character));
			Aura.World.Network.Send.CharacterLock(client, client.Character);

			var p = new MabiPacket(Op.CutsceneStart, Id.World);
			p.PutLongs(client.Character.Id, _leader.Id);
			p.PutString(_name);
			p.PutInt((uint)_actors.Count);
			foreach (var c in _actors)
			{
				p.PutString(c.Item1);
				p.PutShort((ushort)c.Item2.Length);
				p.PutBin(c.Item2);
			}

			p.PutInt(1);
			p.PutLong(client.Character.Id);

			client.Character.CurrentCutscene = this;

			client.Send(p);
		}
	}
}
