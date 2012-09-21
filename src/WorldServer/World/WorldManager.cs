// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using Common.Events;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Network;
using World.Scripting;
using World.Tools;

namespace World.World
{
	public partial class WorldManager
	{
		public readonly static WorldManager Instance = new WorldManager();

		private WorldManager()
		{
			EntityEvents.Instance.CreatureLevelsUp += this.CreatureLevelsUp;
			EntityEvents.Instance.CreatureStatUpdates += this.CreatureStatsUpdate;
		}

		private List<MabiCreature> _creatures = new List<MabiCreature>();
		private List<MabiItem> _items = new List<MabiItem>();
		private List<MabiProp> _props = new List<MabiProp>();

		private int _lastRlHour = -1, _lastRlMinute = -1;

		/// <summary>
		/// This is a general method that's run once every 1500ms (1 Erinn minute).
		/// It's used to raise the Erinn and Real time events (once per Erinn/Real minute).
		/// Possibly, it could also be used for other things,
		/// if it's not enough to just subscribe those, to the time events.
		/// TODO: Running this every few Erinn minutes might be enough.
		/// </summary>
		/// <param name="state"></param>
		public void Heartbeat(object state)
		{
			var now = DateTime.Now;
			long serverTicks = now.Ticks / 10000;

			byte erinnHour = (byte)((serverTicks / 90000) % 24);
			byte erinnMinute = (byte)((serverTicks / 1500) % 60);

			// Erinn time event, every Erinn minute
			ServerEvents.Instance.OnErinnTimeTick(this, new TimeEventArgs(erinnHour, erinnMinute));

			// Erinn time event, every 12 Erinn hours (6AM/6PM specifically)
			if ((erinnHour == 6 || erinnHour == 18) && erinnMinute == 0)
			{
				ServerEvents.Instance.OnErinnDaytimeTick(this, new TimeEventArgs(erinnHour, erinnMinute));
			}

			// Real time event, every Real minute
			// Some caching is needed here, since this method will be called
			// multiple times dzring this minute.
			int rlHour = now.Hour, rlMinute = now.Minute;
			if ((rlHour != _lastRlHour || rlMinute != _lastRlMinute))
			{
				ServerEvents.Instance.OnRealTimeTick(this, new TimeEventArgs((byte)(_lastRlHour = rlHour), (byte)(_lastRlMinute = rlMinute)));
			}
		}

		public void CreatureUpdates(object state)
		{
			// TODO: Not good... >_>
			var entities = new List<MabiEntity>();
			entities.AddRange(_creatures);
			entities.AddRange(_items);
			entities.AddRange(_props);

			// Remove dead entites
			var toRemove = new List<MabiEntity>();
			foreach (var entity in entities)
			{
				if (entity.DisappearTime > DateTime.MinValue && DateTime.Now > entity.DisappearTime)
				{
					toRemove.Add(entity);
				}
			}
			foreach (var entity in toRemove)
			{
				if (entity is MabiCreature)
				{
					this.RemoveCreature(entity as MabiCreature);

					// Respawn
					var npc = entity as MabiNPC;
					if (npc != null && npc.SpawnId > 0)
					{
						NPCManager.Instance.Spawn(npc.SpawnId, 1);
					}
				}
				else if (entity is MabiItem)
				{
					this.RemoveItem(entity as MabiItem);
				}

				entities.Remove(entity);
			}

			// Update visible creatures
			foreach (var creature in _creatures)
			{
				foreach (var entity in entities)
				{
					if (creature == entity)
						continue;

					var creaturePos = creature.GetPosition();
					var entityPos = entity.GetPosition();

					// In sight range
					if (InRange(creaturePos, entityPos))
					{
						// but previously not in sight range
						if (!InRange(creature.PrevPosition, entity.PrevPosition))
						{
							if (creature.Client != null)
								creature.Client.Send(PacketCreator.EntityAppears(entity));
						}
					}

					// Not in range
					else
					{
						// but was in range
						if (InRange(creature.PrevPosition, entity.PrevPosition))
						{
							if (creature.Client != null)
								creature.Client.Send(PacketCreator.EntityLeaves(entity));
						}
					}
				}
			}

			// Update previous position
			foreach (var entity in entities)
			{
				var pos = entity.GetPosition();
				entity.PrevPosition.X = pos.X;
				entity.PrevPosition.Y = pos.Y;
			}
		}

		// Is in range
		// ==================================================================
		// TODO: This gotta go somewhere else...

		public static bool InRange(MabiEntity c1, MabiEntity c2, uint range = 0)
		{
			return InRange(c1.GetPosition(), c2.GetPosition(), range);
		}

		public static bool InRange(MabiVertex loc1, MabiVertex loc2, uint range = 0)
		{
			return InRange(loc1.X, loc1.Y, loc2.X, loc2.Y, range);
		}

		public static bool InRange(MabiEntity entity, uint x, uint y, uint range = 0)
		{
			var pos = entity.GetPosition();
			return InRange(pos.X, pos.Y, x, y, range);
		}

		public static bool InRange(uint x1, uint y1, uint x2, uint y2, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			return ((Math.Pow(((double)x1 - (double)x2), 2) + Math.Pow(((double)y1 - (double)y2), 2)) <= Math.Pow(range, 2));
		}

		// Entity Management
		// ==================================================================

		/// <summary>
		/// Returns a list of entities (NPCs, Items, Props, etc) that are within the specified range
		/// of the specified and region and coordinates. Currently this method ignores the actual range,
		/// only pays attention to the region.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public List<MabiEntity> GetEntitiesInRange(uint region, uint x, uint y, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			var result = new List<MabiEntity>();

			lock (_creatures)
				result.AddRange(_creatures.FindAll(a => a.Region == region && InRange(a, x, y, range)));

			lock (_items)
				result.AddRange(_items.FindAll(a => a.Region == region && InRange(a, x, y, range)));

			lock (_props)
				result.AddRange(_props.FindAll(a => a.Region == region && InRange(a, x, y, range)));

			return result;
		}

		public List<MabiEntity> GetEntitiesInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			var pos = entity.GetPosition();
			return this.GetEntitiesInRange(entity.Region, pos.X, pos.Y, range);
		}

		public List<MabiCreature> GetCreaturesInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			return _creatures.FindAll(a => a != entity && InRange(a, entity, range));
		}

		public List<MabiCreature> GetPlayersInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			return _creatures.FindAll(a => a != entity && a.IsPlayer() && InRange(a, entity, range));
		}

		public List<MabiItem> GetItemsInRegion(uint region)
		{
			return _items.FindAll(a => a.Region == region);
		}

		/// <summary>
		/// Adds a creature to the world, and raises the EnterRegion event,
		/// to notifiy clients about it.
		/// </summary>
		/// <param name="creature"></param>
		public void AddCreature(MabiCreature creature)
		{
			lock (_creatures)
			{
				if (!_creatures.Contains(creature))
				{
					_creatures.Add(creature);
				}
			}

			this.Broadcast(PacketCreator.EntityAppears(creature), SendTargets.Range | SendTargets.ExcludeSender, creature);

			ServerEvents.Instance.OnEntityEntersRegion(creature);
		}

		public void RemoveAllNPCs()
		{
			var toRemove = new List<MabiCreature>();
			foreach (var creature in _creatures)
			{
				if (creature is MabiNPC)
					toRemove.Add(creature);
			}

			foreach (var creature in toRemove)
			{
				this.RemoveCreature(creature);
			}
		}

		/// <summary>
		/// Removes a creature from the world, and raises the LeaveRegion event,
		/// to notifiy clients about it.
		/// </summary>
		/// <param name="creature"></param>
		public void RemoveCreature(MabiCreature creature)
		{
			lock (_creatures)
			{
				_creatures.Remove(creature);
			}

			this.CreatureLeaveRegion(creature);
			creature.Dispose();
		}

		/// <summary>
		/// Returns the first character with the given name, or null if it doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MabiCharacter GetCharacterByName(string name)
		{
			return (MabiCharacter)_creatures.FirstOrDefault(a => a.Name.Equals(name) && a is MabiCharacter);
		}

		/// <summary>
		/// Returns the first creature with the given name, or null if there isn't one.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MabiCreature GetCreatureByName(string name)
		{
			return _creatures.FirstOrDefault(a => a.Name.Equals(name));
		}

		/// <summary>
		/// Returns the creature with the given Id, or null if it couldn't be found.
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public MabiCreature GetCreatureById(ulong Id)
		{
			return _creatures.FirstOrDefault(a => a.Id == Id);
		}

		/// <summary>
		/// Returns the amount of creatures (Players, NPCs, Mobs, etc) in the world.
		/// </summary>
		/// <returns></returns>
		public int GetCreatureCount()
		{
			return _creatures.Count();
		}

		/// <summary>
		/// Returns the amount of players in the world.
		/// </summary>
		/// <returns></returns>
		public uint GetCharactersCount()
		{
			return (uint)_creatures.Count(a => a is MabiPC);
		}

		/// <summary>
		/// Adds an item to the world, and raises the EnterRegion event,
		/// to notify clients about it.
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(MabiItem item)
		{
			lock (_items)
				_items.Add(item);

			var appears = new MabiPacket(0x5211, 0x3000000000000000);
			appears.PutLong(item.Id);
			appears.PutByte(1);
			appears.PutBin(item.Info);
			appears.PutBytes(1, 0, 0, 2);
			this.Broadcast(appears, SendTargets.Range, item);

			ServerEvents.Instance.OnEntityEntersRegion(item);
		}

		/// <summary>
		/// Removes an item from the world, and raises the LeaveRegion event,
		/// to notify clients about it.
		/// </summary>
		/// <param name="item"></param>
		public void RemoveItem(MabiItem item)
		{
			lock (_items)
				_items.Remove(item);

			var disappears = new MabiPacket(0x5212, 0x3000000000000000);
			disappears.PutLong(item.Id);
			this.Broadcast(disappears, SendTargets.Range, item);

			ServerEvents.Instance.OnEntityLeavesRegion(item);
		}

		/// <summary>
		/// Returns the item with the given Id, or null if it couldn't be found.
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public MabiItem GetItemById(ulong id)
		{
			return _items.FirstOrDefault(a => a.Id == id);
		}

		/// <summary>
		/// Returns amount of items in the world.
		/// </summary>
		/// <returns></returns>
		public int GetItemCount()
		{
			return _items.Count();
		}

		// Broadcasting
		// ==================================================================

		public void CreatureChangeTitle(MabiCreature creature)
		{
			var p = new MabiPacket(0x8FC5, creature.Id);
			p.PutShort(creature.Title);
			p.PutShort(0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureSetTarget(MabiCreature creature, MabiCreature target)
		{
			var p = new MabiPacket(0x791A, creature.Id);
			p.PutLong(target != null ? target.Id : 0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureMove(MabiCreature creature, MabiVertex from, MabiVertex to, bool walking = false)
		{
			var p = new MabiPacket((!walking ? (uint)0x0F44BBA3 : (uint)0x0FD13021), creature.Id);
			p.PutInt(from.X);
			p.PutInt(from.Y);
			p.PutInt(to.X);
			p.PutInt(to.Y);

			this.Broadcast(p, SendTargets.Range, creature);

			ServerEvents.Instance.OnCreatureMoves(creature, new MoveEventArgs(from, to));
		}

		public void CreatureSwitchSet(MabiCreature creature)
		{
			var p = new MabiPacket(0x5BCF, creature.Id);
			p.PutByte(creature.WeaponSet);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureSitDown(MabiCreature creature)
		{
			var p = new MabiPacket(0x6D6C, creature.Id);
			p.PutByte((byte)(creature.RestPose + (creature.RaceInfo.Gender == 2 ? 0 : 1)));

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureStandUp(MabiCreature creature)
		{
			var p = new MabiPacket(0x6D6D, creature.Id);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureUseMotion(MabiCreature creature, uint category, uint type, bool loop = false, bool cancel = true)
		{
			MabiPacket p;

			if (cancel)
			{
				// Cancel motion
				p = new MabiPacket(0x6D65, creature.Id);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);
			}

			// Do motion
			p = new MabiPacket(0x6D62, creature.Id);
			p.PutInt(category);
			p.PutInt(type);
			p.PutByte(loop);
			p.PutShort(0);
			this.Broadcast(p, SendTargets.Range, creature);

			ServerEvents.Instance.OnCreatureUsesMotion(creature, new MotionEventArgs(category, type, loop));
		}

		public void CreatureChangeStance(MabiCreature creature, byte unk = 1)
		{
			var p = new MabiPacket(0x6E2A, creature.Id);
			p.PutByte(creature.BattleState);
			p.PutByte(unk);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureMoveEquip(MabiCreature creature, Pocket from, Pocket to)
		{
			var p = new MabiPacket(0x59E7, creature.Id);
			p.PutByte((byte)from);
			p.PutByte((byte)to);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureChangeEquip(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(0x59E6, creature.Id);
			p.PutBin(item.Info);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureLeaveRegion(MabiCreature creature)
		{
			this.Broadcast(PacketCreator.EntityLeaves(creature), SendTargets.Range, creature);

			ServerEvents.Instance.OnEntityLeavesRegion(creature);
		}

		public void CreatureTalk(MabiCreature creature, string message, byte type = 0)
		{
			var p = new MabiPacket(0x526C, creature.Id);
			p.PutByte(type);
			p.PutString(creature.Name);
			p.PutString(message);

			this.Broadcast(p, SendTargets.Range, creature);

			ServerEvents.Instance.OnCreatureTalks(creature, new ChatEventArgs(message));
		}

		public void CreatureStatsUpdate(MabiCreature creature)
		{
			var pub = new MabiPacket(0x7532, creature.Id);
			pub.PutByte(4);
			creature.AddPublicStatData(pub);
			this.Broadcast(pub, SendTargets.Range, creature);

			if (creature.Client != null)
			{
				var priv = new MabiPacket(0x7530, creature.Id);
				priv.PutByte(3);
				creature.AddPrivateStatData(priv);
				creature.Client.Send(priv);
			}
		}

		public void CreatureStatsUpdate(object sender, EntityEventArgs e)
		{
			this.CreatureStatsUpdate(e.Entity as MabiCreature);
		}

		public void CreatureStatusEffectsChange(MabiCreature creature)
		{
			var p = new MabiPacket(0xA028, creature.Id);
			p.PutLong((ulong)creature.StatusEffects.A);
			p.PutLong((ulong)creature.StatusEffects.B);
			p.PutLong((ulong)creature.StatusEffects.C);
			p.PutLong((ulong)creature.StatusEffects.D);
			p.PutInt(0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureLevelsUp(object sender, EntityEventArgs e)
		{
			var creature = e.Entity as MabiCreature;
			var p = new MabiPacket(0x6D69, creature.Id);
			p.PutShort((ushort)creature.Level);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void Effect(MabiCreature creature, uint effect, uint region, uint x, uint y)
		{
			var p = new MabiPacket(0x9090, creature.Id);
			p.PutInt(effect);
			p.PutInt(region);
			p.PutFloat((float)x);
			p.PutFloat((float)y);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void VehicleBind(MabiCreature creature, MabiCreature vehicle)
		{
			var bind1 = new MabiPacket(0x1FBD4, creature.Id);
			bind1.PutInt(vehicle.RaceInfo.VehicleType);
			bind1.PutInt(7);
			bind1.PutLong(vehicle.Id);
			bind1.PutInt(0);
			bind1.PutByte(1);
			bind1.PutLong(creature.Id);

			var bind2 = new MabiPacket(0x1FBD4, vehicle.Id);
			bind2.PutInt(vehicle.RaceInfo.VehicleType);
			bind2.PutInt(0);
			bind2.PutLong(vehicle.Id);
			bind2.PutInt(32);
			bind2.PutByte(1);
			bind2.PutLong(creature.Id);

			this.Broadcast(bind1, SendTargets.Range, creature);
			this.Broadcast(bind2, SendTargets.Range, vehicle);
			//WorldManager.Instance.CreatureUseMotion(pet, 30, 0, false, false);
			//WorldManager.Instance.CreatureUseMotion(creature, 90, 0, false, false);
			this.Broadcast(bind1, SendTargets.Range, creature);
			this.Broadcast(bind2, SendTargets.Range, vehicle);
		}

		public void VehicleUnbind(MabiCreature creature, MabiCreature vehicle, bool spawn = false)
		{
			MabiPacket p;

			if (!spawn)
			{
				p = new MabiPacket(0x1FBD4, creature.Id);
				p.PutInt(0);
				p.PutInt(0);
				p.PutLong(0);
				p.PutInt(0);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);

				p = new MabiPacket(0x1FBD4, creature.Id);
				p.PutInt(0);
				p.PutInt(5);
				p.PutLong(creature.Id);
				p.PutInt(32);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);
			}

			p = new MabiPacket(0x1FBD4, vehicle.Id);
			p.PutInt(0);
			p.PutInt(1);
			p.PutLong(vehicle.Id);
			p.PutInt(32);
			p.PutByte(0);
			this.Broadcast(p, SendTargets.Range, vehicle);
		}

		public void CreatureSkillCancel(MabiCreature creature)
		{
			if (creature.Client != null)
			{
				switch ((SkillConst)creature.ActiveSkillId)
				{
					case SkillConst.Healing:
						creature.Client.Send(new MabiPacket(0x9090, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(0, 0));
						goto default;

					default:
						creature.Client.Send(new MabiPacket(0x6992, creature.Id).PutBytes(0, 1, 0).PutShort(0));
						break;
				}

				creature.Client.Send(new MabiPacket(0x6989, creature.Id).PutBytes(1, 1));
			}

			creature.ActiveSkillId = 0;
			creature.ActiveSkillStacks = 0;
		}

		public void CreatureSkillUseCancel(MabiCreature creature)
		{
			if (creature.Client != null)
			{
				switch ((SkillConst)creature.ActiveSkillId)
				{
					case SkillConst.Smash:
						creature.Client.Send(new MabiPacket(0x6986, creature.Id).PutShort(creature.ActiveSkillId).PutInts(600, 1));
						goto default;

					case SkillConst.Defense:
						creature.Client.Send(new MabiPacket(0x6986, creature.Id).PutShort(creature.ActiveSkillId).PutInts(1000, 1));
						goto default;

					default:
						creature.Client.Send(new MabiPacket(0x6992, creature.Id).PutBytes(0, 1, 0).PutShort(creature.ActiveSkillId));
						break;
				}
			}

			creature.ActiveSkillId = 0;
			creature.ActiveSkillStacks = 0;
		}

		public void CreatureRevive(MabiCreature creature)
		{
			creature.Revive();

			var alive = new MabiPacket(0x53FD, creature.Id);
			WorldManager.Instance.Broadcast(alive, SendTargets.Range, creature);

			var standUp = new MabiPacket(0x701D, creature.Id);
			WorldManager.Instance.Broadcast(standUp, SendTargets.Range, creature);

			WorldManager.Instance.CreatureStatsUpdate(creature);
		}

		public void CreatureCombatAction(MabiCreature source, MabiCreature target, CombatEventArgs combatArgs)
		{
			var combatPacket = new MabiPacket(0x7926, 0x3000000000000000);
			combatPacket.PutInt(combatArgs.CombatActionId);
			combatPacket.PutInt(combatArgs.PrevCombatActionId);
			combatPacket.PutByte(combatArgs.Hit);
			combatPacket.PutByte(combatArgs.HitsMax);
			combatPacket.PutByte(0);

			// List actions
			combatPacket.PutInt((uint)combatArgs.CombatActions.Count);
			foreach (var action in combatArgs.CombatActions)
			{
				// Sub-packet
				var actionPacket = new MabiPacket(0x7924, action.Creature.Id);
				actionPacket.PutInt(combatArgs.CombatActionId);
				actionPacket.PutLong(action.Creature.Id);
				actionPacket.PutByte((byte)action.ActionType);
				actionPacket.PutShort(action.StunTime);
				actionPacket.PutShort((ushort)action.SkillId);
				actionPacket.PutShort(0);

				// Creatures takes damage
				if (action.ActionType.HasFlag(CombatActionType.TakeDamage))
				{
					if (action.Creature.BattleState == 0)
					{
						action.Creature.BattleState = 1;
						WorldManager.Instance.CreatureChangeStance(action.Creature, 0);
					}

					var pos = action.Creature.GetPosition();
					var enemyPos = action.Enemy.GetPosition();

					if (action.ActionType.HasFlag(CombatActionType.Defense))
					{
						WorldManager.Instance.CreatureSkillUseCancel(action.Creature);

						actionPacket.PutLong(action.Enemy.Id);
						actionPacket.PutInt(0);
						actionPacket.PutByte(0);
						actionPacket.PutByte(1);
						actionPacket.PutInt(pos.X);
						actionPacket.PutInt(pos.Y);
					}

					actionPacket.PutInt(action.GetAttackOption());
					actionPacket.PutFloat(action.CombatDamage);
					actionPacket.PutFloat(0);
					actionPacket.PutInt(0);

					actionPacket.PutFloat((float)enemyPos.X - pos.X);
					actionPacket.PutFloat((float)enemyPos.Y - pos.Y);
					if (action.IsKnock())
					{
						actionPacket.PutFloat(pos.X);
						actionPacket.PutFloat(pos.Y);
					}

					actionPacket.PutByte(action.GetDefenseOption());
					actionPacket.PutInt(0);
					actionPacket.PutLong(action.Enemy.Id);

					if (action.Finish)
					{
						// Exp
						if (action.Enemy.LevelingEnabled)
						{
							// Give exp
							var exp = action.Creature.BattleExp;
							action.Enemy.GiveExp(exp);

							// If the creature is controlled by a client
							// it probably wants to get some information.
							if (action.Enemy.Client != null)
							{
								var client = action.Enemy.Client;
								client.Send(PacketCreator.CombatMessage(action.Enemy, "+" + exp.ToString() + " EXP"));
							}
						}

						var npc = action.Creature as MabiNPC;
						if (npc != null)
						{
							var rnd = RandomProvider.Get();

							// Gold
							if (rnd.NextDouble() < 0.5)
							{
								var amount = rnd.Next(npc.GoldMin, npc.GoldMax + 1);
								if (amount > 0)
								{
									var gold = new MabiItem(2000);
									gold.Info.Bundle = (ushort)amount;
									gold.Info.Region = npc.Region;
									gold.Info.X = (uint)(action.OldPosition.X + rnd.Next(-50, 51));
									gold.Info.Y = (uint)(action.OldPosition.Y + rnd.Next(-50, 51));
									gold.DisappearTime = DateTime.Now.AddSeconds(60);

									this.AddItem(gold);
								}
							}

							// Drops
							foreach (var drop in npc.Drops)
							{
								if (rnd.NextDouble() <= drop.Chance)
								{
									var item = new MabiItem(drop.ItemId);
									item.Info.Bundle = 1;
									item.Info.Region = npc.Region;
									item.Info.X = (uint)(action.OldPosition.X + rnd.Next(-50, 51));
									item.Info.Y = (uint)(action.OldPosition.Y + rnd.Next(-50, 51));
									item.DisappearTime = DateTime.Now.AddSeconds(60);

									this.AddItem(item);
								}
							}
						}

						// Set finisher?
						var finishPacket = new MabiPacket(0x7921, action.Creature.Id);
						finishPacket.PutLong(action.Enemy.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// Clear target
						WorldManager.Instance.CreatureSetTarget(action.Enemy, null);

						// Finish this finisher part?
						finishPacket = new MabiPacket(0x7922, action.Creature.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// TODO: There appears to be something missing to let it lay there for finish, if we don't kill it with the following packets.
						// TODO: Check for finishing.

						// Make it dead
						finishPacket = new MabiPacket(0x53FC, action.Creature.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// Remove finisher?
						finishPacket = new MabiPacket(0x7921, action.Creature.Id);
						finishPacket.PutLong(0);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						if (action.Creature.ActiveSkillId > 0)
						{
							Console.WriteLine(action.Creature.ActiveSkillId);
							WorldManager.Instance.CreatureSkillCancel(action.Creature);
						}

						if (action.Creature.Owner != null)
						{
							WorldManager.Instance.Broadcast(new MabiPacket(0x5403, action.Creature.Id).PutShort(1).PutInt(10).PutByte(0), SendTargets.Range, action.Creature);
							// TODO: Unmount.
						}
					}
				}
				// Creature deals damage
				else if (action.ActionType.HasFlag(CombatActionType.Hit))
				{
					var pos = action.Creature.GetPosition();

					actionPacket.PutLong(action.TargetId);
					actionPacket.PutInt(action.GetAttackOption());
					actionPacket.PutByte(0);
					actionPacket.PutByte((byte)(!action.IsKnock() ? 2 : 1)); // must be 2 for correct non knockback animation?
					actionPacket.PutInt(pos.X);
					actionPacket.PutInt(pos.Y);

					if (action.Creature.ActiveSkillId > 0)
					{
						this.CreatureSkillUseCancel(action.Creature);
					}
				}

				var actionPacketB = actionPacket.Build(false);
				combatPacket.PutInt((uint)actionPacketB.Length);
				combatPacket.PutBin(actionPacketB);
			}
			WorldManager.Instance.Broadcast(combatPacket, SendTargets.Range, source);
		}

		public void CreatureCombatSubmit(MabiCreature source, uint actionId)
		{
			var p = new MabiPacket(0x7925, 0x3000000000000000);
			p.PutInt(actionId);

			this.Broadcast(p, SendTargets.Range, source);
		}

		public void Broadcast(MabiPacket packet, SendTargets targets, MabiEntity source, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			// TODO: "Might" be more effective to not check every creature, but make a client list.

			if (targets.HasFlag(SendTargets.All))
			{
				foreach (var creature in _creatures)
				{
					if (creature.Client != null && (creature != source || !targets.HasFlag(SendTargets.ExcludeSender)))
						creature.Client.Send(packet);
				}
			}
			else if (targets.HasFlag(SendTargets.Region))
			{
				foreach (var creature in _creatures)
				{
					if (creature.Client != null && (creature != source || !targets.HasFlag(SendTargets.ExcludeSender)))
						if (creature.Region == source.Region)
							creature.Client.Send(packet);
				}
			}
			else if (targets.HasFlag(SendTargets.Range))
			{
				foreach (var creature in _creatures)
				{
					if (creature.Client != null && (creature != source || !targets.HasFlag(SendTargets.ExcludeSender)))
						if (InRange(creature, source, range))
							creature.Client.Send(packet);
				}
			}
		}
	}

	[Flags]
	public enum SendTargets : byte { All = 1, Region = 2, Range = 4, Party = 8, Guild = 16, ExcludeSender = 32 }
}
