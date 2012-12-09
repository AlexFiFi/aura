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
using Common.Database;
using World.Skills;

namespace World.World
{
	public partial class WorldManager
	{
		public readonly static WorldManager Instance = new WorldManager();

		private WorldManager()
		{
			EntityEvents.Instance.CreatureLevelsUp += this.CreatureLevelsUp;
			EntityEvents.Instance.CreatureStatUpdates += this.CreatureStatsUpdate;
			EntityEvents.Instance.CreatureStatusEffectUpdate += this.CreatureStatusEffectsChange;
			EntityEvents.Instance.CreatureItemUpdate += this.CreatureItemUpdate;
			EntityEvents.Instance.CreatureDropItem += this.CreatureDropItem;
			EntityEvents.Instance.CreatureSkillUpdate += this.CreatureSkillUpdate;
		}

		private List<MabiCreature> _creatures = new List<MabiCreature>();
		private List<WorldClient> _clients = new List<WorldClient>();
		private List<MabiItem> _items = new List<MabiItem>();
		private List<MabiProp> _props = new List<MabiProp>();

		private int _lastRlHour = -1, _lastRlMinute = -1;
		private int _overloadCounter = 0;

		private DateTime last = DateTime.MaxValue;

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

			if ((now - last).TotalMilliseconds > 1700)
			{
				if (++_overloadCounter >= 2)
				{
					Logger.Warning("Server took longer than expected for ErinnTimeTick. (Overloaded?)");
					_overloadCounter = 0;
				}
			}

			last = now;

			byte erinnHour = (byte)((serverTicks / 90000) % 24);
			byte erinnMinute = (byte)((serverTicks / 1500) % 60);

			TimeEventArgs args = new TimeEventArgs(erinnHour, erinnMinute);

			// Erinn time event, every Erinn minute
			ServerEvents.Instance.OnErinnTimeTick(this, args);

			// Erinn time event, every 12 Erinn hours (6AM/6PM specifically)
			if (((erinnHour == 6 || erinnHour == 18) && erinnMinute == 0) || _lastRlHour == -1) //LastRLHour is so we *always* run it on server startup.
			{
				ServerEvents.Instance.OnErinnDaytimeTick(this, args);
				this.Broadcast(PacketCreator.Notice((args.IsNight ? "Eweca is rising.\nMana is starting to fill the air all around." : "Eweca has disappeared.\nThe surrounding Mana is starting to fade away."), NoticeType.MIDDLE_TOP), SendTargets.All, null);
			}

			if (erinnHour == 0 && erinnMinute == 0)
			{
				ServerEvents.Instance.OnErinnMidnightTick(this, args);
			}

			// Real time event, every Real minute
			// Some caching is needed here, since this method will be called
			// multiple times during this minute.
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
			lock (_creatures)
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
			lock (_creatures)
			{
				foreach (var creature in _creatures)
				{
					foreach (var entity in entities)
					{
						if (creature == entity)
							continue;

						var creaturePos = creature.GetPosition();
						var entityPos = entity.GetPosition();

						var entityCreature = (entity as MabiCreature);

						// In sight range...
						if (InRange(creaturePos, entityPos))
						{
							// but previously not in sight range, or invisible.
							if ((!InRange(creature.PrevPosition, entity.PrevPosition) && (entityCreature == null || !entityCreature.Conditions.A.HasFlag(CreatureConditionA.Invisible))) || (entityCreature != null && !entityCreature.Conditions.A.HasFlag(CreatureConditionA.Invisible) && entityCreature.PrevConditions.A.HasFlag(CreatureConditionA.Invisible)))
							{
								if (creature.Client != null)
									creature.Client.Send(PacketCreator.EntityAppears(entity));
							}
							// Invisible now, but not before.
							else if (entityCreature != null && entityCreature.Conditions.A.HasFlag(CreatureConditionA.Invisible) && !entityCreature.PrevConditions.A.HasFlag(CreatureConditionA.Invisible))
							{
								if (creature.Client != null)
									creature.Client.Send(PacketCreator.EntityLeaves(entity));
							}
						}

						// Not in range...
						else
						{
							// but was in range.
							if (InRange(creature.PrevPosition, entity.PrevPosition))
							{
								if (creature.Client != null)
									creature.Client.Send(PacketCreator.EntityLeaves(entity));
							}
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

				if (entity is MabiCreature)
					(entity as MabiCreature).PrevConditions = (entity as MabiCreature).Conditions;
			}
		}


		/// <summary>
		/// This is Aura's Kernel Panic. Don't ever call it yourself unless
		/// you're stopping the server under extreme conditions.
		/// </summary>
		public void EmergencyShutdown()
		{
			// We need to be *very* careful in here, as we're most likely
			// running under unstable/exceptional conditions.
			try
			{
				Logger.Info("Salvaging and saving connected clients (" + _clients.Count + ").");
			}
			catch { }
			for (int i = _clients.Count - 1; i >= 0; i--)
			{
				try
				{
					// Saving is more important than a clean disconnect.
					MabiDb.Instance.SaveAccount(_clients[i].Account);
					_clients[i].Disconnect(0);
				}
				catch { }
				try
				{
					Logger.ClearLine();
					Logger.Info("Saved " + (i + 1));
				}
				catch { }
			}
			try
			{
				Logger.Info("All saved and disconnected.");
			}
			catch { }
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

		public List<MabiCreature> GetAttackableCreaturesInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			return _creatures.FindAll(a => a != entity && a is MabiNPC && ((a as MabiNPC).State & CreatureStates.GoodNpc) == 0 && !a.IsDead() && InRange(a, entity, range));
		}

		public List<MabiCreature> GetPlayersInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			return _creatures.FindAll(a => a != entity && a.IsPlayer() && a.Region == entity.Region && InRange(a, entity, range));
		}

		public List<MabiItem> GetItemsInRegion(uint region)
		{
			return _items.FindAll(a => a.Region == region);
		}

		public IEnumerable<MabiCreature> GetAllPlayers()
		{
			return _creatures.Where(c => c is MabiPC);
		}

		public IEnumerable<MabiCreature> GetAllPlayersInRegion(uint region)
		{
			return this.GetAllPlayers().Where(a => a.Region == region);
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
					this.ActivateMobs(creature, creature.GetPosition(), creature.GetPosition());
				}
			}
			if (creature.Client != null)
			{
				lock (_clients)
				{
					if (!_clients.Contains((WorldClient)creature.Client))
						_clients.Add((WorldClient)creature.Client);
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
				creature.Dispose();
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
			if (creature.Client != null)
			{
				lock (_clients)
				{
					_clients.Remove((WorldClient)creature.Client);
				}
			}

			this.CreatureLeaveRegion(creature);
			creature.Dispose();
		}

		/// <summary>
		/// Returns the first character with the given name, or null if it doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MabiCreature GetCharacterByName(string name, bool forcePC = true)
		{
			if (forcePC)
				return _creatures.FirstOrDefault(a => a.Name.Equals(name) && a is MabiPC);
			else
				return _creatures.FirstOrDefault(a => a.Name.Equals(name));
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

			var appears = new MabiPacket(Op.ItemAppears, 0x3000000000000000);
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

			var disappears = new MabiPacket(Op.ItemDisappears, 0x3000000000000000);
			disappears.PutLong(item.Id);
			this.Broadcast(disappears, SendTargets.Range, item);

			ServerEvents.Instance.OnEntityLeavesRegion(item);

			item.Dispose();
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
			var p = new MabiPacket(Op.ChangedTitle, creature.Id);
			p.PutShort(creature.Title);
			p.PutShort(0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureSetTarget(MabiCreature creature, MabiCreature target)
		{
			var p = new MabiPacket(Op.CombatTargetSet, creature.Id);
			p.PutLong(target != null ? target.Id : 0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureMove(MabiCreature creature, MabiVertex from, MabiVertex to, bool walking = false)
		{
			var p = new MabiPacket((!walking ? (uint)Op.Running : (uint)Op.Walking), creature.Id);
			p.PutInt(from.X);
			p.PutInt(from.Y);
			p.PutInt(to.X);
			p.PutInt(to.Y);

			this.Broadcast(p, SendTargets.Range, creature);

			if (creature.Client != null)
				ActivateMobs(creature, from, to);

			ServerEvents.Instance.OnCreatureMoves(creature, new MoveEventArgs(from, to));
		}

		private void ActivateMobs(MabiCreature creature, MabiVertex from, MabiVertex to)
		{
			IEnumerable<MabiCreature> mobsInRange = _creatures.Where(c =>
				c.Region == creature.Region
				&& c is MabiNPC
				&& ((MabiNPC)c).AIScript != null);

			long leftX, rightX, topY, bottomY; //Bounding rectangle coordinates

			if (from.X < to.X) //Moving right
			{
				leftX = from.X - 2600;
				rightX = to.X + 2600;
			}
			else
			{
				leftX = to.X - 2600;
				rightX = from.X + 2600;
			}

			if (from.Y < to.Y) //Moving up
			{
				bottomY = from.Y - 2600;
				topY = to.Y + 2600;
			}
			else
			{
				bottomY = to.Y - 2600;
				topY = from.Y + 2600;
			}


			//Linear movement equation
			double slope;
			if (to.Y == from.Y)
			{
				slope = .001; //double.MinValue produces infinity in B
			}
			else
			{
				slope = ((double)to.Y - from.Y) / ((double)to.X - from.X);
			}
			double b = from.Y - slope * from.X;

			mobsInRange = mobsInRange.Where((c) =>
			{
				var pos = c.GetPosition();
				return (leftX < pos.X && pos.X < rightX && bottomY < pos.Y && pos.Y < topY && (Math.Abs(pos.Y - (long)(slope * pos.X + b)) < 2600));
			});

			double dist = Math.Sqrt(((to.X - from.X) * (to.X - from.X)) + ((to.Y - from.Y) * (to.Y - from.Y)));

			uint time = (uint)Math.Ceiling(dist / creature.GetSpeed());

			foreach (var mob in mobsInRange)
			{
				((MabiNPC)mob).AIScript.Activate(time);
			}
		}

		public void CreatureSwitchSet(MabiCreature creature)
		{
			var p = new MabiPacket(Op.SwitchedSet, creature.Id);
			p.PutByte(creature.WeaponSet);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureSitDown(MabiCreature creature)
		{
			var p = new MabiPacket(Op.Resting, creature.Id);
			p.PutByte(creature.RestPose);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureStandUp(MabiCreature creature)
		{
			var p = new MabiPacket(Op.StandUp, creature.Id);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureUseMotion(MabiCreature creature, uint category, uint type, bool loop = false, bool cancel = true)
		{
			MabiPacket p;

			if (cancel)
			{
				// Cancel motion
				p = new MabiPacket(Op.MotionCancel, creature.Id);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);
			}

			// Do motion
			p = new MabiPacket(Op.Motions, creature.Id);
			p.PutInt(category);
			p.PutInt(type);
			p.PutByte(loop);
			p.PutShort(0);
			this.Broadcast(p, SendTargets.Range, creature);

			ServerEvents.Instance.OnCreatureUsesMotion(creature, new MotionEventArgs(category, type, loop));
		}

		public void CreatureChangeStance(MabiCreature creature, byte unk = 1)
		{
			var p = new MabiPacket(Op.ChangesStance, creature.Id);
			p.PutByte(creature.BattleState);
			p.PutByte(unk);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureUnequip(MabiCreature creature, Pocket from)
		{
			var p = new MabiPacket(Op.EquipmentMoved, creature.Id);
			p.PutByte((byte)from);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureEquip(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.EquipmentChanged, creature.Id);
			p.PutBin(item.Info);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureDropItem(object sender, ItemEventArgs e)
		{
			var creature = sender as MabiCreature;
			if (creature == null)
				return;

			var pos = creature.GetPosition();
			var rand = RandomProvider.Get();
			e.Item.Region = creature.Region;
			e.Item.Info.X = (uint)(pos.X + rand.Next(-100, 101));
			e.Item.Info.Y = (uint)(pos.Y + rand.Next(-100, 101));
			e.Item.Info.Pocket = (byte)Pocket.None;
			e.Item.DisappearTime = DateTime.Now.AddSeconds((int)Math.Max(60, (e.Item.OptionInfo.Price / 100) * 60));

			WorldManager.Instance.AddItem(e.Item);
		}

		public void CreatureItemUpdate(object sender, ItemUpdateEventArgs ie)
		{
			var creature = sender as MabiCreature;
			if (creature.Client == null)
				return;

			if (!ie.IsNew)
			{
				// Update or remove, depending on type and amount
				if (ie.Item.StackType == BundleType.Sac || ie.Item.Info.Amount > 0)
				{
					creature.Client.Send(PacketCreator.ItemAmount(creature, ie.Item));
				}
				else
				{
					creature.Client.Send(PacketCreator.ItemRemove(creature, ie.Item));
				}
			}
			else
			{
				// Send info about a new item
				creature.Client.Send(PacketCreator.ItemInfo(creature, ie.Item));
			}
		}

		public void CreatureSkillUpdate(object sender, SkillUpdateEventArgs args)
		{
			var creature = sender as MabiCreature;
			if (args.IsNew && creature.Client != null)
			{
				creature.Client.Send(new MabiPacket(Op.SkillInfo, creature.Id).PutBin(args.Skill.Info));
			}
			else
			{
				if (creature.Client != null)
					creature.Client.Send(new MabiPacket(Op.SkillRankUp, creature.Id).PutByte(1).PutBin(args.Skill.Info).PutFloat(0));
				WorldManager.Instance.Broadcast(new MabiPacket(Op.RankUp, creature.Id).PutShorts(args.Skill.Info.Id, 1), SendTargets.Range, creature);
			}
		}

		public void CreatureLeaveRegion(MabiCreature creature)
		{
			this.Broadcast(PacketCreator.EntityLeaves(creature), SendTargets.Range, creature);

			ServerEvents.Instance.OnEntityLeavesRegion(creature);
		}

		public void CreatureTalk(MabiCreature creature, string message, byte type = 0)
		{
			var p = new MabiPacket(Op.Chat, creature.Id);
			p.PutByte(type);
			p.PutString(creature.Name);
			p.PutString(message);

			this.Broadcast(p, SendTargets.Range, creature);

			ServerEvents.Instance.OnCreatureTalks(creature, new ChatEventArgs(message));
		}

		public void CreatureStatsUpdate(MabiCreature creature)
		{
			var pub = new MabiPacket(Op.StatUpdatePublic, creature.Id);
			pub.PutByte(4);
			creature.AddPublicStatData(pub);
			this.Broadcast(pub, SendTargets.Range, creature);

			if (creature.Client != null)
			{
				var priv = new MabiPacket(Op.StatUpdatePrivate, creature.Id);
				priv.PutByte(3);
				creature.AddPrivateStatData(priv);
				creature.Client.Send(priv);
			}
		}

		public void CreatureStatsUpdate(object sender, EntityEventArgs e)
		{
			this.CreatureStatsUpdate(e.Entity as MabiCreature);
		}

		public void CreatureStatusEffectsChange(object sender, EntityEventArgs e)
		{
			var creature = e.Entity as MabiCreature;

			var p = new MabiPacket(Op.StatusEffectUpdate, creature.Id);
			p.PutLong((ulong)creature.Conditions.A);
			p.PutLong((ulong)creature.Conditions.B);
			p.PutLong((ulong)creature.Conditions.C);
			p.PutLong((ulong)creature.Conditions.D);
			p.PutInt(0);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void CreatureStatusEffectsChange(MabiCreature creature)
		{
			this.CreatureStatusEffectsChange(creature, new EntityEventArgs(creature));
		}

		public void CreatureLevelsUp(object sender, EntityEventArgs e)
		{
			var creature = e.Entity as MabiCreature;

			var p = new MabiPacket(Op.LevelUp, creature.Id);
			p.PutShort((ushort)creature.Level);

			this.Broadcast(p, SendTargets.Range, creature);

			var publ = new MabiPacket(Op.StatUpdatePublic, creature.Id);
			publ.PutByte(4);
			publ.PutInt(1);
			publ.PutInt((uint)Stat.LifeMax);
			publ.PutFloat(creature.LifeMax);
			for (int i = 0; i < 7; ++i)
				publ.PutInt(0);

			this.Broadcast(publ, SendTargets.Range, creature);

			if (creature.Client != null)
			{
				var priv = new MabiPacket(Op.StatUpdatePrivate, creature.Id);
				priv.PutByte(3);
				priv.PutInt(9);
				priv.PutInt((uint)Stat.AbilityPoints);
				priv.PutInt(creature.AbilityPoints);
				priv.PutInt((uint)Stat.LifeMax);
				priv.PutFloat(creature.LifeMax);
				priv.PutInt((uint)Stat.ManaMax);
				priv.PutFloat(creature.ManaMax);
				priv.PutInt((uint)Stat.StaminaMax);
				priv.PutFloat(creature.StaminaMax);
				priv.PutInt((uint)Stat.Str);
				priv.PutFloat(creature.StrBase);
				priv.PutInt((uint)Stat.Int);
				priv.PutFloat(creature.IntBase);
				priv.PutInt((uint)Stat.Dex);
				priv.PutFloat(creature.DexBase);
				priv.PutInt((uint)Stat.Will);
				priv.PutFloat(creature.WillBase);
				priv.PutInt((uint)Stat.Luck);
				priv.PutFloat(creature.LuckBase);
				for (int i = 0; i < 7; ++i)
					priv.PutInt(0);

				creature.Client.Send(priv);
			}
		}

		public void Effect(MabiCreature creature, uint effect, uint region, uint x, uint y)
		{
			var p = new MabiPacket(Op.Effect, creature.Id);
			p.PutInt(effect);
			p.PutInt(region);
			p.PutFloat((float)x);
			p.PutFloat((float)y);
			p.PutByte(1);

			this.Broadcast(p, SendTargets.Range, creature);
		}

		public void VehicleBind(MabiCreature creature, MabiCreature vehicle)
		{
			var bind1 = new MabiPacket(Op.VehicleBond, creature.Id);
			bind1.PutInt(vehicle.RaceInfo.VehicleType);
			bind1.PutInt(7);
			bind1.PutLong(vehicle.Id);
			bind1.PutInt(0);
			bind1.PutByte(1);
			bind1.PutLong(creature.Id);

			var bind2 = new MabiPacket(Op.VehicleBond, vehicle.Id);
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
				p = new MabiPacket(Op.VehicleBond, creature.Id);
				p.PutInt(0);
				p.PutInt(0);
				p.PutLong(0);
				p.PutInt(0);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);

				p = new MabiPacket(Op.VehicleBond, creature.Id);
				p.PutInt(0);
				p.PutInt(5);
				p.PutLong(creature.Id);
				p.PutInt(32);
				p.PutByte(0);
				this.Broadcast(p, SendTargets.Range, creature);
			}

			p = new MabiPacket(Op.VehicleBond, vehicle.Id);
			p.PutInt(0);
			p.PutInt(1);
			p.PutLong(vehicle.Id);
			p.PutInt(32);
			p.PutByte(0);
			this.Broadcast(p, SendTargets.Range, vehicle);
		}

		public void CreatureSkillCancel(MabiCreature creature)
		{
			if (creature.ActiveSkillId > 0)
			{
				MabiSkill skill; SkillHandler handler;
				SkillManager.CheckOutSkill(creature, creature.ActiveSkillId, out skill, out handler);
				if (skill == null || handler == null)
					return;

				var result = handler.Cancel(creature, skill);

				if ((result & SkillResults.Okay) == 0)
					return;

				creature.Client.Send(new MabiPacket(Op.SkillStackUpdate, creature.Id).PutBytes(0, 1, 0).PutShort(creature.ActiveSkillId));
				creature.Client.Send(new MabiPacket(Op.SkillCancel, creature.Id).PutBytes(0, 1));
			}

			creature.ActiveSkillId = 0;
			creature.ActiveSkillStacks = 0;
		}

		public void CreatureRevive(MabiCreature creature)
		{
			creature.Revive();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead1, creature.Id), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead2, creature.Id), SendTargets.Range, creature);

			WorldManager.Instance.CreatureStatsUpdate(creature);
		}

		public void CreatureCombatAction(MabiCreature source, MabiCreature target, CombatEventArgs combatArgs)
		{
			var combatPacket = new MabiPacket(Op.CombatActionBundle, 0x3000000000000000);
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
				var actionPacket = new MabiPacket(Op.CombatAction, action.Creature.Id);
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
					var enemy = action.Target as MabiCreature;
					var enemyPos = enemy.GetPosition();

					if (action.ActionType.HasFlag(CombatActionType.Defense))
					{
						this.CreatureSkillCancel(action.Creature);

						actionPacket.PutLong(enemy.Id);
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
					actionPacket.PutInt(action.ReactionDelay);
					actionPacket.PutLong(enemy.Id);

					if (action.Finish)
					{
						// Exp
						if (enemy.LevelingEnabled)
						{
							// Give exp
							var exp = action.Creature.BattleExp * WorldConf.ExpRate;
							enemy.GiveExp((ulong)exp);

							// If the creature is controlled by a client
							// it probably wants to get some information.
							if (enemy.Client != null)
							{
								var client = enemy.Client;
								client.Send(PacketCreator.CombatMessage(enemy, "+" + exp.ToString() + " EXP"));
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
									gold.Info.Amount = (ushort)amount;
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
									item.Info.Amount = 1;
									item.Info.Region = npc.Region;
									item.Info.X = (uint)(action.OldPosition.X + rnd.Next(-50, 51));
									item.Info.Y = (uint)(action.OldPosition.Y + rnd.Next(-50, 51));
									item.DisappearTime = DateTime.Now.AddSeconds(60);

									this.AddItem(item);
								}
							}
						}

						// Set finisher?
						var finishPacket = new MabiPacket(Op.CombatSetFinisher, action.Creature.Id);
						finishPacket.PutLong(enemy.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// Clear target
						WorldManager.Instance.CreatureSetTarget(enemy, null);

						// Finish this finisher part?
						finishPacket = new MabiPacket(Op.CombatSetFinisher2, action.Creature.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// TODO: There appears to be something missing to let it lay there for finish, if we don't kill it with the following packets.
						// TODO: Check for finishing.

						// Make it dead
						finishPacket = new MabiPacket(Op.IsNowDead, action.Creature.Id);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						// Remove finisher?
						finishPacket = new MabiPacket(Op.CombatSetFinisher, action.Creature.Id);
						finishPacket.PutLong(0);
						WorldManager.Instance.Broadcast(finishPacket, SendTargets.Range, action.Creature);

						if (action.Creature.ActiveSkillId > 0)
						{
							this.CreatureSkillCancel(action.Creature);
						}

						if (action.Creature.Owner != null)
						{
							WorldManager.Instance.Broadcast(new MabiPacket(Op.DeadFeather, action.Creature.Id).PutShort(1).PutInt(10).PutByte(0), SendTargets.Range, action.Creature);
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
						this.CreatureSkillCancel(action.Creature);
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
			var p = new MabiPacket(Op.CombatActionEnd, 0x3000000000000000);
			p.PutInt(actionId);

			this.Broadcast(p, SendTargets.Range, source);
		}

		public void Broadcast(MabiPacket packet, SendTargets targets, MabiEntity source, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			bool excludeSender = targets.HasFlag(SendTargets.ExcludeSender);
			WorldClient sourceClient = null;
			if (source != null && source is MabiCreature)
				sourceClient = (WorldClient)((MabiCreature)source).Client;
			lock (_clients)
			{
				if (targets.HasFlag(SendTargets.All))
				{
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient)) //Don't send if we're excluding the sender AND the sender is the client
							client.Send(packet);
					}
				}
				else if (targets.HasFlag(SendTargets.Region))
				{
					var region = source.Region;
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient))
							if (region == client.Character.Region)
								client.Send(packet);
					}
				}
				else if (targets.HasFlag(SendTargets.Range))
				{
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient))
							if (InRange(client.Character, source, range))
								client.Send(packet);
					}
				}
			}
		}
	}

	[Flags]
	public enum SendTargets : byte { All = 1, Region = 2, Range = 4, Party = 8, Guild = 16, ExcludeSender = 32 }
}
