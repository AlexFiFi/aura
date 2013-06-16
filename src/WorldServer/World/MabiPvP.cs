using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.Shared.Const;
using Aura.Shared.Util;

namespace Aura.World.World
{
	public interface IDual
	{
		bool IsAttackableBy(MabiCreature target, MabiCreature attacker);
		IEnumerable<DeadMenuOptions> GetRevivalOptions(MabiCreature creature);
	}

	public abstract class ArenaPvPManager : IDual
	{
		public abstract uint LobbyRegion { get; }
		public abstract uint ArenaRegion { get; }

		protected Dictionary<MabiCreature, uint> _teams = new Dictionary<MabiCreature, uint>();
		protected Dictionary<MabiCreature, uint> _stars = new Dictionary<MabiCreature, uint>();
		protected System.Timers.Timer _roundTimer;
		protected DateTime _roundEnd;
		protected MabiVertex[] _revivalLocations = new MabiVertex[] { };

		public ArenaPvPManager(int roundLength)
		{
			_roundTimer = new System.Timers.Timer(roundLength); // 5 minutes
			_roundTimer.AutoReset = true;
			_roundTimer.Elapsed += new System.Timers.ElapsedEventHandler(_roundTimer_Elapsed);
			_roundEnd = DateTime.Now.AddMilliseconds(roundLength);
			_roundTimer.Start();
		}

		private void _roundTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_roundTimer.Stop();
			OnRoundEnd();
			_roundTimer.Start();
			_roundEnd = DateTime.Now.AddMilliseconds(_roundTimer.Interval);

			var creatures = WorldManager.Instance.GetAllPlayersInRegion(LobbyRegion).Concat(WorldManager.Instance.GetAllPlayersInRegion(ArenaRegion));
			foreach (var c in creatures)
				SendArenaRoundInfo(c);
		}

		public abstract bool IsAttackableBy(MabiCreature target, MabiCreature attacker);
		public abstract IEnumerable<DeadMenuOptions> GetRevivalOptions(MabiCreature creature);
		public virtual MabiVertex[] RevivalLocations { get { return _revivalLocations; } }

		public uint GetTeam(MabiCreature creature)
		{
			return _teams.ContainsKey(creature) ? _teams[creature] : (uint)0;
		}

		public uint GetStars(MabiCreature creature)
		{
			return _stars.ContainsKey(creature) ? _stars[creature] : (uint)0;
		}

		public abstract void EnterArena(MabiCreature creature);
		
		public virtual void EnterLobby(MabiCreature creature)
		{
			SendArenaRoundInfo(creature);
		}

		public virtual void Leave(MabiCreature creature)
		{
			creature.Client.Send(new MabiPacket(Op.ArenaRoundInfoCancel, creature.Id));
		}

		public abstract void CreatureKilled(MabiCreature creature, MabiCreature attacker);

		protected virtual void SendArenaRoundInfo(MabiCreature creature)
		{
			creature.Client.Send(new MabiPacket(Op.ArenaRoundInfo, creature.Id).PutInt((uint)(_roundEnd - DateTime.Now).TotalMilliseconds).PutShorts(0, 0));
		}

		public virtual void ReviveInArena(MabiCreature creature)
		{
			var loc = this.RevivalLocations[RandomProvider.Get().Next(this.RevivalLocations.Length)];
			creature.SetLocation(ArenaRegion, loc.X, loc.Y);
			WorldManager.Instance.BroadcastRegion(new MabiPacket(Op.SetLocation, creature.Id).PutByte(1).PutInts(ArenaRegion, loc.X, loc.Y), ArenaRegion);
			creature.Client.Send(new MabiPacket(Op.WarpUnk2, creature.Id));

			creature.Injuries += creature.LifeInjured * .2f;
			creature.Life = creature.LifeInjured / 2;
			creature.Stamina = creature.StaminaHunger / 2;
			creature.Revive();

			this.HideCreature(creature);

			creature.Client.Send(new MabiPacket(Op.Revived, creature.Id).PutInts(1, ArenaRegion, loc.X, loc.Y));
		}

		public abstract void ReviveInLobby(MabiCreature creature);

		public abstract void ReviveInWaitingRoom(MabiCreature creature);

		public virtual void HideCreature(MabiCreature creature)
		{
			WorldManager.Instance.BroadcastRegion(new MabiPacket(Op.ArenaHideOn, creature.Id), ArenaRegion);

			System.Threading.Timer t = null;
			t = new System.Threading.Timer((o) =>
			{
				if (!creature.IsDead)
					WorldManager.Instance.BroadcastRegion(new MabiPacket(Op.ArenaHideOff, creature.Id), ArenaRegion);
				GC.KeepAlive(t);
				t.Dispose();
			}, null, 2000, System.Threading.Timeout.Infinite);
		}

		protected abstract void OnRoundEnd();

	}

	public class AlbyPvPManager : ArenaPvPManager
	{
		public override uint LobbyRegion
		{
			get { return 28; }
		}

		public override uint ArenaRegion
		{
			get { return 29; }
		}

		public AlbyPvPManager()
			: base(300000)
		{
			_revivalLocations = new MabiVertex[]
			{
				new MabiVertex(1800, 4644),
				new MabiVertex(1800, 1845),
				new MabiVertex(4675, 1845),
				new MabiVertex(4675, 4644)
			};
		}

		public override bool IsAttackableBy(MabiCreature target, MabiCreature attacker)
		{
			return true;
		}

		public override IEnumerable<DeadMenuOptions> GetRevivalOptions(MabiCreature creature)
		{
			if (creature.Region == ArenaRegion)
			{
				return new DeadMenuOptions[]
				{
					DeadMenuOptions.ArenaLobby,
					DeadMenuOptions.ArenaSide,
					DeadMenuOptions.WaitForRescue
				};
			}
			else if (creature.Region == LobbyRegion)
			{
				return new DeadMenuOptions[]
				{
					DeadMenuOptions.ArenaLobby,
					DeadMenuOptions.WaitForRescue
				};
			}
			else
				return new DeadMenuOptions[] { };
		}

		public override void EnterArena(MabiCreature creature)
		{
			if (!_teams.ContainsKey(creature))
				_teams.Add(creature, 0);

			if (!_stars.ContainsKey(creature))
				_stars.Add(creature, 1);

			_teams[creature] = 0;
			_stars[creature] = 1;

			this.SendArenaRoundInfo(creature);

			WorldManager.Instance.BroadcastRegion(PacketCreator.PvPInfoChanged(creature), ArenaRegion);
		}

		protected override void OnRoundEnd()
		{
			var creatures = WorldManager.Instance.GetAllPlayersInRegion(this.ArenaRegion);
			var toRemove = new List<MabiCreature>();

			foreach (var creature in creatures)
			{
				if (!_stars.ContainsKey(creature)) // If not in stars
					if (_teams.ContainsKey(creature)) // But in teams
						_teams.Remove(creature); // GTFO

				if (!_teams.ContainsKey(creature)) // If not in teams
					if (_stars.ContainsKey(creature)) // But in stars
						_stars.Remove(creature); // GTFO

				if (_stars.ContainsKey(creature) && _stars[creature] == 0)
					toRemove.Add(creature); // I SAID GTFO
			}

			foreach (var c in toRemove)
			{
				try
				{
					var client = c.Client as WorldClient;
					client.Warp(LobbyRegion, 1150, 3545); // AND THEY'RE GONE
					if (c.IsDead)
					{
						WorldServer.Instance.HandleDeadMenu(client, null);
					}
				} catch
				{}

				if (_stars.ContainsKey(c))
					_stars.Remove(c);
				if (_teams.ContainsKey(c))
					_teams.Remove(c);
			}

			creatures = creatures.Except(toRemove);

			foreach (var c in creatures)
				if (!c.IsDead)
				{
					c.FullHeal();
					WorldManager.Instance.BroadcastRegion(new MabiPacket(Op.RankUp, c.Id).PutShort(1), c.Region);
				}
		}

		public override void CreatureKilled(MabiCreature creature, MabiCreature attacker)
		{
			if (_stars.ContainsKey(creature) && _stars[creature] != 0)
			{
				_stars[creature]--;

				if (_stars.ContainsKey(attacker))
					_stars[attacker]++;
			}

			WorldManager.Instance.BroadcastRegion(PacketCreator.PvPInfoChanged(creature), ArenaRegion);
			WorldManager.Instance.BroadcastRegion(PacketCreator.PvPInfoChanged(attacker), ArenaRegion);
		}

		public override void ReviveInLobby(MabiCreature creature)
		{
			try
			{
				var c = creature.Client as WorldClient;
				c.Warp(LobbyRegion, 1150, 3545);
				creature.FullHeal();
				creature.Revive();
				c.Send(new MabiPacket(Op.Revived, creature.Id).PutInts(1, LobbyRegion, 1150, 3545));
			}
			catch { }
		}

		public override void ReviveInWaitingRoom(MabiCreature creature)
		{
			throw new NotImplementedException();
		}
	}
}
