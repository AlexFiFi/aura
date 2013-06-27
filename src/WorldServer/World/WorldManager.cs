﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aura.Data;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Database;
using Aura.World.Events;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.Skills;
using Aura.World.Util;

namespace Aura.World.World
{
	public partial class WorldManager
	{
		public readonly static WorldManager Instance = new WorldManager();

		private WorldManager()
		{
		}

		private List<MabiCreature> _creatures = new List<MabiCreature>();
		private List<WorldClient> _clients = new List<WorldClient>();
		private List<MabiItem> _items = new List<MabiItem>();
		private List<MabiProp> _props = new List<MabiProp>();
		private List<MabiParty> _parties = new List<MabiParty>();

		private List<ArenaPvPManager> _arenaPvPs = new List<ArenaPvPManager>()
		{
			new AlbyPvPManager()
		};

		private Dictionary<ulong, MabiPropBehavior> _propBehavior = new Dictionary<ulong, MabiPropBehavior>();

		private int _lastRlHour = -1, _lastRlMinute = -1;
		private int _overloadCounter = 0;
		private bool _firstHeartbeat = true;

		private DateTime _lastHearbeat = DateTime.MaxValue;

		private Timer _worldTimer, _creatureUpdateTimer, _secondTimer;

		private Dictionary<uint, Quadtree<LinePath>> CollisionTree = new Dictionary<uint, Quadtree<LinePath>>();

		/// <summary>
		/// Starts all relevant timers.
		/// </summary>
		public void Start()
		{
			this.PlantTree();

			_worldTimer = new Timer(this.Heartbeat, null, 1500 - ((DateTime.Now.Ticks) % 1500), 1500);
			_creatureUpdateTimer = new Timer(this.UpdateCreatures, null, 5000, 250);

			_secondTimer = new Timer(_ =>
			{
				EventManager.Instance.TimeEvents.OnRealTimeSecondTick(this, new TimeEventArgs(MabiTime.Now));
			},
			null, 6000, 1000);
		}

		/// <summary>
		/// Initializes quad tree for collision detections.
		/// </summary>
		private void PlantTree()
		{
			foreach (var region in MabiData.RegionDb.Entries.Values)
			{
				foreach (var area in region.Areas.Values)
				{
					foreach (var prop in area.Props.Values)
					{
						foreach (var shape in prop.Shapes)
						{
							if (!this.CollisionTree.ContainsKey(region.Id))
								this.CollisionTree.Add(region.Id, new Quadtree<LinePath>((int)region.X1, (int)region.Y1, (int)region.X2, (int)region.Y2));

							var p1 = new MabiVertex(shape.X1, shape.Y1);
							var p2 = new MabiVertex(shape.X2, shape.Y2);
							var p3 = new MabiVertex(shape.X3, shape.Y3);
							var p4 = new MabiVertex(shape.X4, shape.Y4);

							this.CollisionTree[region.Id].Insert(new LinePath(p1, p2));
							this.CollisionTree[region.Id].Insert(new LinePath(p2, p3));
							this.CollisionTree[region.Id].Insert(new LinePath(p3, p4));
							this.CollisionTree[region.Id].Insert(new LinePath(p4, p1));
						}
					}
				}
			}
		}

		/// <summary>
		/// Sends packet to all clients that match the parameters.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="targets"></param>
		/// <param name="source"></param>
		/// <param name="range"></param>
		public void Broadcast(MabiPacket packet, SendTargets targets, MabiEntity source = null, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			var excludeSender = ((targets & SendTargets.ExcludeSender) != 0);
			WorldClient sourceClient = null;
			if (source != null && source is MabiCreature)
				sourceClient = (source as MabiCreature).Client as WorldClient;

			lock (_clients)
			{
				if ((targets & SendTargets.All) != 0)
				{
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient))
							client.Send(packet);
					}
				}
				else if ((targets & SendTargets.Region) != 0)
				{
					var region = source.Region;
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient))
							if (region == client.Character.Region)
								client.Send(packet);
					}
				}
				else if ((targets & SendTargets.Range) != 0)
				{
					var region = source.Region;
					foreach (var client in _clients)
					{
						if (!(excludeSender && client == sourceClient))
							if (region == client.Character.Region && InRange(client.Character, source, range))
								client.Send(packet);
					}
				}
			}
		}

		/// <summary>
		/// Sends packet to all characters in the given region.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="region"></param>
		public void BroadcastRegion(MabiPacket packet, uint region)
		{
			foreach (var client in _clients.Where(a => a.Character.Region == region))
			{
				client.Send(packet);
			}
		}

		/// <summary>
		/// This is a general method that's run once every 1500ms (1 Erinn minute).
		/// It's used to raise the Erinn and Real time events (once per Erinn/Real minute).
		/// Possibly, it could also be used for other things,
		/// if it's not enough to just subscribe those, to the time events.
		/// </summary>
		/// <param name="state"></param>
		private void Heartbeat(object state)
		{
			var mt = MabiTime.Now;
			var args = new TimeEventArgs(mt);

			// Overload check, basically checking the time from last
			// heartbeat to this one.
			var now = mt.DateTime;
			var serverTicks = now.Ticks / 10000;
			if ((now - _lastHearbeat).TotalMilliseconds > 1700)
			{
				if (++_overloadCounter >= 3)
				{
					Logger.Warning("Server took longer than expected for ErinnTimeTick. (Overloaded?)");
					_overloadCounter = 0;
				}
			}
			_lastHearbeat = mt.DateTime;

			// OnErinnTimeTick, fired every Erinn minute (1.5s)
			EventManager.Instance.TimeEvents.OnErinnTimeTick(this, args);

			// OnErinnDaytimeTick, fired at 6:00am and 6:00pm.
			if (((mt.Hour == 6 || mt.Hour == 18) && mt.Minute == 0) || _firstHeartbeat)
			{
				EventManager.Instance.TimeEvents.OnErinnDaytimeTick(this, args);
				this.DaytimeChange(mt);
			}

			// OnErinnMidnightTick, fired at 0:00am
			if (mt.IsMidnight)
				EventManager.Instance.TimeEvents.OnErinnMidnightTick(this, args);

			// OnRealTimeTick, fired every minute in real time.
			// Some caching is needed here, since this method will be called
			// multiple times during this minute.
			int rlHour = mt.DateTime.Hour, rlMinute = mt.DateTime.Minute;
			if (rlHour != _lastRlHour || rlMinute != _lastRlMinute)
			{
				_lastRlHour = rlHour; _lastRlMinute = rlMinute;
				EventManager.Instance.TimeEvents.OnRealTimeTick(this, new TimeEventArgs(mt));

				ThreadPool.QueueUserWorkItem(CheckAncients);
			}

			_firstHeartbeat = false;
		}

		/// <summary>
		/// Checks all NPCs, to find such that are entitled to become ancient.
		/// </summary>
		/// <param name="state"></param>
		private void CheckAncients(object state)
		{
			if (WorldConf.AncientRate > 0)
			{
				var rnd = RandomProvider.Get();

				for (int i = 0; i < _creatures.Count; i++)
				{
					var c = _creatures[i] as MabiNPC;
					if (c == null || !c.AncientEligible || c.IsDead || c.AncientTime > DateTime.Now)
						continue;

					c.AncientEligible = false;
					if (rnd.NextDouble() <= WorldConf.AncientRate)
						this.Ancientify(c);
				}
			}
		}

		/// <summary>
		/// Called a few times per second to
		/// - remove dead creatures
		/// - respawn mobs
		/// - remove expired dropped items
		/// - update visible entities for all clients
		/// </summary>
		/// <param name="state"></param>
		private void UpdateCreatures(object state)
		{
			// TODO: Not good... >_>
			var entities = new List<MabiEntity>();
			lock (_creatures)
				entities.AddRange(_creatures);
			entities.AddRange(_items);

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
						ScriptManager.Instance.Spawn(npc.SpawnId, 1);
					}
				}
				else if (entity is MabiItem)
				{
					this.RemoveItem(entity as MabiItem);
				}

				entities.Remove(entity);
			}

			var clients = new List<WorldClient>();
			clients.AddRange(_clients);

			foreach (var client in clients)
			{
				var creaturePos = client.Character.GetPosition();

				// XXX: Better/Faster way?
				var toAppear = new List<MabiEntity>();
				var toDisappear = new List<MabiEntity>();

				foreach (var entity in entities)
				{
					if (client.Character.Region != entity.Region)
						continue;
					if (client.Character == entity)
						continue;

					var entityPos = entity.GetPosition();
					var entityCreature = entity as MabiCreature;

					if (InRange(creaturePos, entityPos))
					{
						// Wasn't in range before or was invisible
						if (!InRange(client.Character.PrevPosition, entity.PrevPosition) || (entityCreature != null && (((entityCreature.Conditions.A & CreatureConditionA.Invisible) == 0) && ((entityCreature.PrevConditions.A & CreatureConditionA.Invisible) != 0))))
						{
							toAppear.Add(entity);
							//Send.EntityAppears(client, entity);
						}
						// Invisible now, but not before.
						else if (entityCreature != null && ((entityCreature.Conditions.A & CreatureConditionA.Invisible) != 0) && ((entityCreature.PrevConditions.A & CreatureConditionA.Invisible) == 0))
						{
							toDisappear.Add(entity);
							//Send.EntityDisappears(client, entity);
						}
					}
					else
					{
						// Not in range now
						if (InRange(client.Character.PrevPosition, entity.PrevPosition)) // but was before
						{
							toDisappear.Add(entity);
							//Send.EntityDisappears(client, entity);
						}
					}
				}

				if (toAppear.Count > 0)
					Send.EntitiesAppear(client, toAppear);
				if (toDisappear.Count > 0)
					Send.EntitiesDisappear(client, toDisappear);
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
		/// Broadcasts Eweca notices and updates stat regens.
		/// </summary>
		/// <param name="mt"></param>
		private void DaytimeChange(MabiTime mt)
		{
			var notice = mt.IsNight
				? Localization.Get("world.eweca_night") // Eweca is rising.\nMana is starting to fill the air all around.
				: Localization.Get("world.eweca_day");  // Eweca has disappeared.\nThe surrounding Mana is starting to fade away.
			Send.ChannelNotice(NoticeType.MiddleTop, notice);

			lock (_creatures)
			{
				foreach (var creature in _creatures.Where(a => a.Client is WorldClient))
				{
					if (creature.ManaRegen != null)
					{
						if (mt.IsNight)
							creature.ManaRegen.ChangePerSecond *= 3;
						else
							creature.ManaRegen.ChangePerSecond /= 3;
						this.CreatureStatsUpdate(creature);
					}
				}
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
					WorldDb.Instance.SaveAccount(_clients[i].Account);
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

		// Range and position calculations
		// ==================================================================

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

		/// <summary>
		/// Calculates a position on the line between source and target.
		/// e.g. distance 0 would be the position of target, 100 would be
		/// 100 points farther away from source.
		/// </summary>
		public static MabiVertex CalculatePosOnLine(MabiEntity source, MabiEntity target, int distance)
		{
			return CalculatePosOnLine(source.GetPosition(), target.GetPosition(), distance);
		}

		/// <summary>
		/// Calculates a position on the line between source and target.
		/// e.g. distance 0 would be the position of target, 100 would be
		/// 100 points farther away from source.
		/// </summary>
		public static MabiVertex CalculatePosOnLine(MabiVertex source, MabiVertex target, int distance)
		{
			if (source.Equals(target))
				return new MabiVertex(source.X + 1, source.Y + 1);

			var deltaX = (double)target.X - source.X;
			var deltaY = (double)target.Y - source.Y;

			var deltaXY = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

			var newX = target.X + (distance / deltaXY) * (deltaX);
			var newY = target.Y + (distance / deltaXY) * (deltaY);

			return new MabiVertex((uint)newX, (uint)newY);
		}

		public bool FindCollision(uint region, MabiVertex from, MabiVertex to, out MabiVertex intersection)
		{
			intersection = null;

			double x1 = from.X, y1 = from.Y;
			double x2 = to.X, y2 = to.Y;

			var intersections = new List<MabiVertex>();

			var lines = new List<LinePath>();
			this.CollisionTree[region].GetObjects(new LinePath(from, to).Rect, ref lines);

			foreach (var line in lines)
			{
				MabiVertex inter = null;
				if (FindIntersection(x1, y1, x2, y2, line.P1.X, line.P1.Y, line.P2.X, line.P2.Y, out inter))
					intersections.Add(inter);
			}

			if (intersections.Count < 1)
				return false;

			// Select nearest intersection
			double distance = double.MaxValue;
			foreach (var inter in intersections)
			{
				var interDist = Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2);
				if (interDist < distance)
				{
					intersection = inter;
					distance = interDist;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns whether the lines x1/y1-x2/y2 and x3/y3-x4/y4 intersect.
		/// The intersection point is returned in the corresponding out-variable.
		/// </summary>
		private static bool FindIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out MabiVertex intersection)
		{
			intersection = null;

			double denom = ((x2 - x1) * (y4 - y3)) - ((y2 - y1) * (x4 - x3));
			if (denom == 0) return false; // parallel 

			double numer = ((y1 - y3) * (x4 - x3)) - ((x1 - x3) * (y4 - y3));
			double r = numer / denom;
			double numer2 = ((y1 - y3) * (x2 - x1)) - ((x1 - x3) * (y2 - y1));
			double s = numer2 / denom;
			if ((r < 0 || r > 1) || (s < 0 || s > 1)) return false; // nointersect

			double interX = x1 + (r * (x2 - x1));
			double interY = y1 + (r * (y2 - y1));

			intersection = new MabiVertex((int)interX, (int)interY);

			return true;
		}

		// Entity Management
		// ==================================================================

		/// <summary>
		/// Adds a creature to the world, and raises the EnterRegion event.
		/// </summary>
		/// <param name="creature"></param>
		public void AddCreature(MabiCreature creature)
		{
			lock (_creatures)
			{
				if (!_creatures.Contains(creature))
				{
					_creatures.Add(creature);
					if (creature is MabiPC)
						this.ActivateMobs(creature, creature.GetPosition(), creature.GetPosition());
				}
			}
			if (creature.Client is WorldClient)
			{
				lock (_clients)
				{
					if (!_clients.Contains(creature.Client as WorldClient))
						_clients.Add(creature.Client as WorldClient);
				}
			}

			Send.EntityAppearsOthers(creature);

			EventManager.Instance.EntityEvents.OnEntityEntersRegion(this, new EntityEventArgs(creature));
		}

		/// <summary>
		/// Removes all creatures deriving from NPC and props from this
		/// world manager. Primarily called before reloading scripts.
		/// </summary>
		public void RemoveAllNPCs()
		{
			var toRemove = new List<MabiCreature>();
			foreach (var creature in _creatures)
			{
				if (creature is MabiNPC)
					toRemove.Add(creature);
			}
			foreach (var creature in toRemove)
				this.RemoveCreature(creature);

			var pr = new List<MabiProp>();
			pr.AddRange(_props);
			foreach (var prop in pr)
				this.RemoveProp(prop);
		}

		/// <summary>
		/// Removes a creature from the world, and raises the LeaveRegion event.
		/// </summary>
		/// <param name="creature"></param>
		public void RemoveCreature(MabiCreature creature)
		{
			lock (_creatures)
			{
				_creatures.Remove(creature);
			}
			if (creature.Client is WorldClient)
			{
				lock (_clients)
				{
					_clients.Remove(creature.Client as WorldClient);
				}
			}

			if (creature.Party != null)
				this.CreatureLeaveParty(creature);

			this.CreatureLeaveRegion(creature);
			creature.Dispose();
		}

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
				result.AddRange(_props.FindAll(a => a.Region == region /*&& InRange(a, x, y, range)*/));

			return result;
		}

		public List<MabiEntity> GetEntitiesInRange(MabiEntity entity, uint range = 0)
		{
			var pos = entity.GetPosition();
			return this.GetEntitiesInRange(entity.Region, pos.X, pos.Y, range);
		}

		public List<MabiCreature> GetCreaturesInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			lock (_creatures)
				return _creatures.FindAll(a => a != entity && InRange(a, entity, range));
		}

		public List<MabiCreature> GetAttackableCreaturesInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			lock (_creatures)
				return _creatures.FindAll(a => a != entity && a is MabiNPC && !(a as MabiNPC).Has(CreatureStates.GoodNpc) && !a.IsDead && InRange(a, entity, range));
		}

		public List<MabiCreature> GetPlayersInRange(MabiEntity entity, uint range = 0)
		{
			if (range < 1)
				range = WorldConf.SightRange;

			lock (_creatures)
				return _creatures.FindAll(a => a != entity && a.IsPlayer && a.Region == entity.Region && InRange(a, entity, range));
		}

		public List<MabiItem> GetItemsInRegion(uint region)
		{
			lock (_items)
				return _items.FindAll(a => a.Region == region);
		}

		public List<MabiCreature> GetAllPlayers()
		{
			lock (_creatures)
				return _creatures.Where(c => c is MabiPC).ToList();
		}

		public List<MabiCreature> GetAllPlayersInRegion(uint region)
		{
			lock (_creatures)
				return _creatures.Where(c => c is MabiPC && c.Region == region).ToList();
		}

		/// <summary>
		/// Returns the first character with the given name, or null if it doesn't exist.
		/// If forcePC (force player character) is false this method may return NPCs,
		/// which is required for GMCP functions.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MabiCreature GetCharacterOrNpcByName(string name)
		{
			lock (_creatures)
				return _creatures.FirstOrDefault(a => a.Name.Equals(name) && (a is MabiPC || a is MabiNPC));
		}

		public MabiPC GetCharacterByName(string name, bool forcePC = true)
		{
			lock (_creatures)
				return _creatures.FirstOrDefault(a => a.Name.Equals(name) && a is MabiPC) as MabiPC;
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
			return _creatures.Count;
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
		/// Adds an item to the world and raises the EnterRegion event.
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(MabiItem item)
		{
			lock (_items)
				_items.Add(item);

			item.FirstTimeAppear = true;

			Send.EntityAppears(item);
			//var appears = new MabiPacket(Op.ItemAppears, Id.Broadcast);
			//appears.PutLong(item.Id);
			//appears.PutByte(1);
			//appears.PutBin(item.Info);
			//appears.PutBytes(1, 0, 0, 2);
			//this.Broadcast(appears, SendTargets.Range, item);

			EventManager.Instance.EntityEvents.OnEntityEntersRegion(this, new EntityEventArgs(item));
		}

		/// <summary>
		/// Removes an item from the world and raises the LeaveRegion event.
		/// </summary>
		/// <param name="item"></param>
		public void RemoveItem(MabiItem item)
		{
			lock (_items)
				_items.Remove(item);

			var disappears = new MabiPacket(Op.ItemDisappears, Id.Broadcast);
			disappears.PutLong(item.Id);
			this.Broadcast(disappears, SendTargets.Range, item);

			EventManager.Instance.EntityEvents.OnEntityLeavesRegion(this, new EntityEventArgs(item));

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

		/// <summary>
		/// Adds a prop to the world and raises the EnterRegion event
		/// </summary>
		/// <param name="prop"></param>
		public void AddProp(MabiProp prop)
		{
			lock (_props)
				_props.Add(prop);

			Send.EntityAppears(prop);
			//var appears = new MabiPacket(Op.PropAppears, Id.Broadcast);
			//prop.AddToPacket(appears);
			//this.Broadcast(appears, SendTargets.Region, prop);

			EventManager.Instance.EntityEvents.OnEntityEntersRegion(this, new EntityEventArgs(prop));
		}

		public void RemoveProp(MabiProp prop)
		{
			lock (_props)
				_props.Remove(prop);

			var disappears = new MabiPacket(Op.PropDisappears, Id.Broadcast);
			disappears.PutLong(prop.Id);
			this.Broadcast(disappears, SendTargets.Region, prop);

			EventManager.Instance.EntityEvents.OnEntityLeavesRegion(this, new EntityEventArgs(prop));

			prop.Dispose();
		}

		public void SetPropBehavior(MabiPropBehavior behavior)
		{
			lock (_propBehavior)
				_propBehavior[behavior.Prop.Id] = behavior;
		}

		public MabiPropBehavior GetPropBehavior(ulong propId)
		{
			lock (_propBehavior)
			{
				MabiPropBehavior result;
				_propBehavior.TryGetValue(propId, out result);
				return result;
			}
		}

		public void SpawnCreature(uint race, uint amount, uint region, uint x, uint y, uint radius = 0, bool effect = false)
		{
			this.SpawnCreature(race, amount, region, new MabiVertex(x, y), radius, effect);
		}

		public void SpawnCreature(uint race, uint amount, uint region, MabiVertex pos, uint radius = 0, bool effect = false)
		{
			var spawn = new SpawnInfo();
			spawn.Amount = amount;
			spawn.RaceId = race;
			spawn.Region = region;

			if (radius == 0)
			{
				spawn.SpawnType = SpawnLocationType.Point;
				spawn.SpawnPoint = new Point(pos.X, pos.Y);
			}
			else
			{
				spawn.SpawnType = SpawnLocationType.Polygon;
				spawn.SpawnPolyRegion = new SpawnRegion(new Point[] 
				{
					new Point(pos.X - radius, pos.Y - radius),
					new Point(pos.X - radius, pos.Y + radius),
					new Point(pos.X + radius, pos.Y + radius),
					new Point(pos.X + radius, pos.Y - radius),
				});
				spawn.SpawnPolyBounds = spawn.SpawnPolyRegion.GetBounds();
			}

			ScriptManager.Instance.Spawn(spawn, 0, effect);
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

		// ------------------------------------------------------------------
		// Creature actions and broadcasting
		// ------------------------------------------------------------------

		public void DropItem(MabiItem item, uint region, uint x, uint y)
		{
			item.Info.Region = region;
			item.Info.X = x;
			item.Info.Y = y;
			item.Info.Pocket = (byte)Pocket.None;
			item.DisappearTime = DateTime.Now.AddSeconds((int)Math.Max(60, (item.OptionInfo.Price / 100) * 60));

			this.AddItem(item);
		}

		public void CreatureMove(MabiCreature creature, MabiVertex from, MabiVertex to, bool walking = false)
		{
			var p = new MabiPacket((!walking ? (uint)Op.Running : (uint)Op.Walking), creature.Id);
			p.PutInt(from.X);
			p.PutInt(from.Y);
			p.PutInt(to.X);
			p.PutInt(to.Y);

			this.Broadcast(p, SendTargets.Range, creature);

			if (creature is MabiPC)
				ActivateMobs(creature, from, to);

			switch (creature.ActiveSkillId)
			{
				case SkillConst.RangedCombatMastery:
				case SkillConst.ArrowRevolver:
				case SkillConst.ArrowRevolver2:
				case SkillConst.MagnumShot:
				case SkillConst.SupportShot:
				case SkillConst.ElvenMagicMissile:
				case SkillConst.MirageMissile:
				case SkillConst.CrashShot:
					CombatHelper.ResetCreatureAim(creature);
					break;
			}

			EventManager.Instance.CreatureEvents.OnCreatureMoves(this, new MoveEventArgs(creature, from, to));
		}

		public void CreatureLeaveRegion(MabiCreature creature)
		{
			Send.EntityDisappears(creature);

			EventManager.Instance.EntityEvents.OnEntityLeavesRegion(this, new EntityEventArgs(creature));
		}

		public void CreatureStatsUpdate(MabiCreature creature)
		{
			// Public
			this.Broadcast(
				PacketCreator.StatUpdate(creature, StatUpdateType.Public,
					Stat.Height, Stat.Weight, Stat.Upper, Stat.Lower, Stat.CombatPower,
					Stat.Life, Stat.LifeInjured, Stat.LifeMax, Stat.LifeMaxMod
				)
			, SendTargets.Range, creature);

			// Private
			creature.Client.Send(
				PacketCreator.StatUpdate(creature, StatUpdateType.Private,
					Stat.Life, Stat.LifeInjured, Stat.LifeMax, Stat.LifeMaxMod,
					Stat.Mana, Stat.ManaMax, Stat.ManaMaxMod,
					Stat.Stamina, Stat.Food, Stat.StaminaMax, Stat.StaminaMaxMod,
					Stat.Str, Stat.Dex, Stat.Int, Stat.Luck, Stat.Will,
					Stat.StrMod, Stat.DexMod, Stat.IntMod, Stat.LuckMod, Stat.WillMod,
					Stat.Level, Stat.Experience, Stat.AbilityPoints, Stat.DefenseBaseMod, Stat.DefenseMod, Stat.ProtectBaseMod, Stat.ProtectMod
				)
			);
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
			//WorldManager.Instance.CreatureUseMotion(vehicle, 30, 0, false, false);
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
			if (creature.ActiveSkillId != SkillConst.None)
			{
				MabiSkill skill; SkillHandler handler;
				SkillManager.CheckOutSkill(creature, creature.ActiveSkillId, out skill, out handler);
				if (skill == null || handler == null)
					return;

				var result = handler.Cancel(creature, skill);

				if ((result & SkillResults.Okay) == 0)
					return;

				Send.SkillStackUpdate(creature.Client, creature, skill.Id, 0);
				Send.SkillCancel(creature.Client, creature);
			}

			creature.ActiveSkillId = SkillConst.None;
			creature.ActiveSkillStacks = 0;
		}

		/// <summary>
		/// Revives creates and sends necessary packets.
		/// </summary>
		/// <param name="creature"></param>
		public void ReviveCreature(MabiCreature creature)
		{
			if (!creature.IsDead)
				return;

			creature.Revive();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead1, creature.Id), SendTargets.Range, creature);
			WorldManager.Instance.CreatureStatsUpdate(creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead2, creature.Id), SendTargets.Range, creature);

		}

		public void HandleCombatActionPack(CombatActionPack cap)
		{
			foreach (var action in cap.Actions)
			{
				// Switch to battle stance
				//if (tAction.Creature.BattleState == 0)
				//{
				//    tAction.Creature.BattleState = 1;
				//    Send.ChangesStance(tAction.Creature, 0);
				//}

				// Cancel defense if applicable
				if (action.Is(CombatActionType.Defended))
					WorldManager.Instance.CreatureSkillCancel(action.Creature);

				if (action.Creature.IsDead)
				{
					// Exp, Drops, etc.
					WorldManager.Instance.CreatureDies(action.Creature, cap.Attacker, action.OldPosition, action.SkillId);
				}
			}

			// Start combat action
			WorldManager.Instance.Broadcast(cap.GetPacket(), SendTargets.Range, cap.Attacker);

			// Skill used
			if (cap.SkillId != SkillConst.MeleeCombatMastery)
				cap.Attacker.Client.Send(new MabiPacket(Op.CombatUsedSkill, cap.Attacker.Id).PutShort((ushort)cap.SkillId));

			// End combat action
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatActionEnd, Id.Broadcast).PutInt(cap.CombatActionId), SendTargets.Range, cap.Attacker);

			// Status updates
			foreach (var action in cap.Actions)
				WorldManager.Instance.CreatureStatsUpdate(action.Creature);
		}

		public void CreatureDies(MabiCreature creature, MabiCreature killer, MabiVertex position, SkillConst skillId)
		{
			if (killer != null)
			{
				// Shadow Bunshin soul counter
				if (skillId != SkillConst.ShadowBunshin)
					killer.SoulCount++;

				// Exp
				if (killer.LevelingEnabled)
				{
					// Give exp
					var exp = creature.BattleExp * WorldConf.ExpRate;
					killer.GiveExp((ulong)exp);

					Send.CombatMessage(killer.Client, killer, "+{0} EXP", exp);

					EventManager.Instance.CreatureEvents.OnCreatureKilled(this, new CreatureKilledEventArgs(creature, killer));
					if (killer is MabiPC)
						EventManager.Instance.PlayerEvents.OnKilledByPlayer(this, new CreatureKilledEventArgs(creature, killer));
				}
			}

			var npc = creature as MabiNPC;
			if (npc != null)
			{
				var rnd = RandomProvider.Get();

				// Gold
				if (rnd.NextDouble() < WorldConf.GoldDropRate)
				{
					var amount = rnd.Next(npc.GoldMin, npc.GoldMax + 1);
					if (amount > 0)
					{
						var gold = new MabiItem(2000);
						gold.Info.Amount = (ushort)amount;
						gold.Info.Region = npc.Region;
						gold.Info.X = (uint)(position.X + rnd.Next(-50, 51));
						gold.Info.Y = (uint)(position.Y + rnd.Next(-50, 51));
						gold.DisappearTime = DateTime.Now.AddSeconds(60);

						this.AddItem(gold);
					}
				}

				// Drops
				foreach (var drop in npc.Drops)
				{
					if (rnd.NextDouble() < drop.Chance * WorldConf.DropRate)
					{
						var item = new MabiItem(drop.ItemId);
						item.Info.Amount = 1;
						item.Info.Region = npc.Region;
						item.Info.X = (uint)(position.X + rnd.Next(-50, 51));
						item.Info.Y = (uint)(position.Y + rnd.Next(-50, 51));
						item.DisappearTime = DateTime.Now.AddSeconds(60);

						this.AddItem(item);
					}
				}
			}

			// Set finisher?
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetFinisher, creature.Id).PutLong(killer.Id), SendTargets.Range, creature);

			// Clear target
			Send.CombatTargetSet(killer, null);

			// Finish this finisher part?
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetFinisher2, creature.Id), SendTargets.Range, creature);

			// TODO: There appears to be something missing to let it lay there for finish, if we don't kill it with the following packets.
			// TODO: Check for finishing.

			// Make it dead
			WorldManager.Instance.Broadcast(new MabiPacket(Op.IsNowDead, creature.Id), SendTargets.Range, creature);

			// Remove finisher?
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetFinisher, creature.Id).PutLong(0), SendTargets.Range, creature);

			if (creature.ActiveSkillId != SkillConst.None)
				this.CreatureSkillCancel(creature);

			if (creature.Owner != null)
			{
				Send.DeadFeather(creature, DeadMenuOptions.Here | DeadMenuOptions.FeatherUp);
				// TODO: Unmount.
			}

			creature.CauseOfDeath = DeathCauses.None;

			if (creature.ArenaPvPManager != null && creature.ArenaPvPManager == killer.ArenaPvPManager && creature.ArenaPvPManager.IsAttackableBy(creature, killer))
			{
				creature.ArenaPvPManager.CreatureKilled(creature, killer);
				creature.CauseOfDeath = DeathCauses.Arena;
			}

			// TODO: Trans PvP

			if (creature.CauseOfDeath == DeathCauses.None && creature.EvGEnabled && killer.EvGEnabled)
				if (creature.EvGSupportRace != 0 && killer.EvGSupportRace != 0 && creature.EvGSupportRace != killer.EvGSupportRace)
					creature.CauseOfDeath = DeathCauses.EvG;

			if (creature.CauseOfDeath == DeathCauses.None)
				creature.CauseOfDeath = DeathCauses.Mob;
		}

		public void CreatureCompletesQuest(MabiCreature creature, MabiQuest quest, bool rewards)
		{
			if (rewards)
			{
				// Owl
				WorldManager.Instance.Broadcast(new MabiPacket(Op.QuestOwlComplete, creature.Id).PutLong(quest.Id), SendTargets.Range, creature);

				// Rewards
				foreach (var reward in quest.Info.Rewards)
				{
					switch (reward.Type)
					{
						case RewardType.Exp:
							creature.GiveExp(reward.Amount);
							creature.Client.Send(PacketCreator.AcquireExp(creature, reward.Amount));
							this.CreatureStatsUpdate(creature);
							break;

						case RewardType.Gold:
							creature.GiveItem(2000, reward.Amount);
							creature.Client.Send(PacketCreator.AcquireItem(creature, reward.Id, reward.Amount));
							break;

						case RewardType.Item:
							creature.GiveItem(reward.Id, reward.Amount);
							creature.Client.Send(PacketCreator.AcquireItem(creature, reward.Id, reward.Amount));
							break;

						case RewardType.Skill:
							var id = (SkillConst)reward.Id;
							var rank = (SkillRank)reward.Amount;

							// Only give skill if char doesn't have it or rank is lower.
							var skill = creature.Skills.Get(id);
							if (skill == null || skill.Rank < rank)
							{
								creature.Skills.Give(id, rank);
								WorldManager.Instance.CreatureStatsUpdate(creature);
							}
							break;

						default:
							Logger.Warning("Unsupported reward type '{0}'.", reward.Type);
							break;
					}
				}

				// Only call this if there were rewards, we're using this
				// method to clear quests as well.
				var script = ScriptManager.Instance.GetQuestScript(quest.Info.Id);
				if (script != null)
					script.OnCompleted(creature.Client as WorldClient, quest);
			}

			Send.ItemInfo(creature.Client, creature, quest.QuestItem);

			// Remove from quest log.
			creature.Client.Send(new MabiPacket(Op.QuestClear, creature.Id).PutLong(quest.Id));
		}

		public bool CreateGuild(string name, GuildType type, MabiCreature leader, IEnumerable<MabiCreature> otherMembers)
		{
			if (WorldDb.Instance.GetGuildForChar(leader.Id) != null)
			{
				Send.MsgBox(leader.Client, leader, "You are already a member of a guild");
				return false;
			}
			foreach (var mem in otherMembers)
			{
				if (WorldDb.Instance.GetGuildForChar(mem.Id) != null)
				{
					Send.MsgBox(leader.Client, leader, "{0} is already a member of a guild", mem.Name);
					return false;
				}
			}

			if (!WorldDb.Instance.GuildNameOkay(name))
			{
				Send.MsgBox(leader.Client, leader, "That name is not valid or is already in use.");
				return false;
			}

			// TODO: checks in here...
			MabiGuild g = new MabiGuild();
			g.Gold = g.Gp = 0;
			g.GuildLevel = (byte)GuildLevel.Beginner;
			g.IntroMessage = "Guild stone for the " + name + " guild";
			g.LeavingMessage = "You have left the " + name + " guild";
			g.RejectionMessage = "You have been denied admission to the " + name + " guild.";
			g.WelcomeMessage = "Welcome to the " + name + " guild!";
			g.Name = name;
			g.Region = leader.Region;
			var pos = leader.GetPosition();
			g.X = pos.X;
			g.Y = pos.Y;
			g.Rotation = leader.Direction;
			g.StoneClass = (uint)GuildStoneType.Normal;
			g.Type = (byte)type;

			var gid = g.Save();

			leader.GuildMemberInfo = new MabiGuildMemberInfo() { CharacterId = leader.Id, MemberRank = (byte)GuildMemberRank.Leader };
			WorldDb.Instance.SaveGuildMember(leader.GuildMemberInfo, gid);
			foreach (var m in otherMembers)
			{
				m.GuildMemberInfo = new MabiGuildMemberInfo() { CharacterId = m.Id, MemberRank = (byte)GuildMemberRank.SeniorMember };
				WorldDb.Instance.SaveGuildMember(m.GuildMemberInfo, gid);
			}

			g = WorldDb.Instance.GetGuild(gid); // Reload guild to make sure it gets initialized and gets an id

			leader.Guild = g;

			this.Broadcast(PacketCreator.GuildMembershipChanged(g, leader, (byte)GuildMemberRank.Leader), SendTargets.Range, leader);

			foreach (var m in otherMembers)
			{
				m.Guild = g;
				this.Broadcast(PacketCreator.GuildMembershipChanged(g, m, (byte)GuildMemberRank.SeniorMember), SendTargets.Range, m);
			}

			var p = new MabiProp("", g.Name, string.Format("<xml guildid=\"{0}\"/>", g.Id), g.StoneClass, g.Region, g.X, g.Y, g.Rotation);
			WorldManager.Instance.AddProp(p);
			WorldManager.Instance.SetPropBehavior(new MabiPropBehavior(p, GuildstoneTouch));

			Send.ChannelNotice(NoticeType.Top, 20000, "{0} Guild has been created. Guild leader: {1}", name, leader.Name);

			return true;
		}

		public void LoadGuilds()
		{
			var guilds = WorldDb.Instance.LoadGuilds();

			foreach (var guild in guilds)
			{
				var extra = string.Format("<xml guildid=\"{0}\" {1}/>", guild.Id, guild.HasOption(GuildOptionFlags.Warp) ? "gh_warp=\"true\"" : "");
				var p = new MabiProp("", guild.Name, extra, guild.StoneClass, guild.Region, guild.X, guild.Y, guild.Rotation);

				WorldManager.Instance.AddProp(p);
				WorldManager.Instance.SetPropBehavior(new MabiPropBehavior(p, GuildstoneTouch));
			}

			Logger.ClearLine();
			Logger.Info("Done loading {0} guilds.", guilds.Count);
		}

		public static void GuildstoneTouch(WorldClient client, MabiCreature creature, MabiProp p)
		{
			// TODO: Better way to get this ID... Pake could be used to fake it
			string gid = p.ExtraData.Substring(p.ExtraData.IndexOf("guildid=\""));
			gid = gid.Substring(9);
			gid = gid.Substring(0, gid.IndexOf("\""));
			ulong bid = ulong.Parse(gid);

			var g = WorldDb.Instance.GetGuild(bid);
			if (g != null)
			{
				if (creature.Guild != null)
				{
					if (g.Id == creature.Guild.Id && creature.GuildMemberInfo.MemberRank < (byte)GuildMemberRank.Applied)
						client.Send(new MabiPacket(Op.OpenGuildPanel, creature.Id).PutLong(g.Id).PutBytes(0, 0, 0)); // 3 Unknown bytes...
					else
						client.Send(new MabiPacket(Op.GuildInfo, creature.Id).PutLong(g.Id).PutStrings(g.Name, g.LeaderName)
							.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < (byte)GuildMemberRank.Applied))
							.PutString(g.IntroMessage));
				}
				else
					client.Send(new MabiPacket(Op.GuildInfoNoGuild, creature.Id).PutLong(g.Id).PutStrings(g.Name, g.LeaderName)
						.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < (byte)GuildMemberRank.Applied))
						.PutString(g.IntroMessage));
			}
		}

		public void AddParty(MabiParty party)
		{
			lock (_parties)
				_parties.Add(party);
		}

		public void RemoveParty(MabiParty party)
		{
			lock (_parties)
				_parties.Remove(party);
		}

		public void CreatureLeaveParty(MabiCreature creature)
		{
			if (creature.Client == null || creature.Party == null)
				return;

			var party = creature.Party;

			// Remove creature from party
			party.RemovePartyMember(creature);
			creature.Party = null;
			creature.PartyNumber = 0;

			// Update party
			if (party.Members.Count > 0)
			{
				foreach (var member in party.Members)
					member.Client.Send(new MabiPacket(Op.PartyLeaveUpdate, member.Id).PutLong(creature.Id));

				if (party.IsOpen)
					this.PartyMemberWantedRefresh(party);

				foreach (var member in party.Members)
					member.Client.Send(new MabiPacket(0xA43C, member.Id).PutLong(creature.Id).PutByte(1).PutByte(1).PutShort(0).PutInt(0));

				if (party.Leader == creature)
					this.PartyChangeLeader(party.GetNextLeader(), party);
			}
			// Remove party
			else
			{
				if (party.IsOpen)
					this.PartyMemberWantedHide(party);

				this.RemoveParty(party);
			}
		}

		public void PartyMemberWantedRefresh(MabiParty party)
		{
			var p = new MabiPacket(Op.PartyWantedUpdate, party.Leader.Id).PutByte(party.IsOpen).PutString(party.GetMemberWantedString());
			this.Broadcast(p, SendTargets.Range, party.Leader);
		}

		public void PartyMemberWantedHide(MabiParty party)
		{
			party.IsOpen = false;

			foreach (var member in party.Members)
				member.Client.Send(new MabiPacket(Op.PartyWantedClosed, member.Id));

			PartyMemberWantedRefresh(party);
		}

		public void PartyMemberWantedShow(MabiParty party)
		{
			party.IsOpen = true;

			foreach (var member in party.Members)
				member.Client.Send(new MabiPacket(Op.PartyWantedOpened, member.Id));

			PartyMemberWantedRefresh(party);
		}

		public void PartyChangeLeader(MabiCreature leader, MabiParty party)
		{
			if (party.IsOpen)
				this.PartyMemberWantedHide(party);

			party.SetLeader(leader);

			foreach (var member in party.Members)
				member.Client.Send(new MabiPacket(Op.PartyChangeLeaderUpdate, member.Id).PutLong(leader.Id));
		}

		public void Ancientify(MabiNPC creature)
		{
			creature.Title = 30038;
			Send.TitleUpdate(creature);

			creature.GoldMax *= 20;
			creature.GoldMin *= 20;

			/// XXX: Maybe do this by default for all monsters?
			creature.Drops = new List<DropInfo>(creature.Drops);
			creature.Drops.AddRange(MabiData.AncientDropDb.Entries);

			creature.StatMods.Add(Stat.ProtectMod, 10, StatModSource.Title, 30038);
			creature.StatMods.Add(Stat.DefenseMod, 10, StatModSource.Title, 30038);
			creature.StatMods.Add(Stat.LifeMaxMod, creature.LifeMax * 10 - creature.LifeMaxBaseTotal, StatModSource.Title, 30038);

			creature.FullHeal();

			creature.BattleExp *= 20;

			creature.Height *= 2;

			this.CreatureStatsUpdate(creature);
		}

		public void SharpMind(MabiCreature user, SharpMindStatus state, SkillConst skill)
		{
			var inRange = this.GetPlayersInRange(user, WorldConf.SightRange);
			foreach (var c in inRange)
			{
				c.Client.Send(PacketCreator.SharpMind(user, c, skill, state));
			}
		}

		public void CreatureEnterRegionPVPStuff(MabiCreature creature)
		{
			if (creature.ArenaPvPManager != null && creature.Region != creature.ArenaPvPManager.LobbyRegion && creature.Region != creature.ArenaPvPManager.ArenaRegion)
			{
				creature.ArenaPvPManager.Leave(creature);
				creature.ArenaPvPManager = null;
				Send.PvPInformation(creature);
			}

			foreach (var arena in _arenaPvPs)
			{
				if (creature.Region == arena.LobbyRegion)
				{
					creature.ArenaPvPManager = arena;
					arena.EnterLobby(creature);
				}
				else if (creature.Region == arena.ArenaRegion)
				{
					creature.ArenaPvPManager = arena;
					arena.EnterArena(creature);
				}
			}
		}
	}

	public enum SendTargets : byte { All = 1, Region = 2, Range = 4, Party = 8, Guild = 16, ExcludeSender = 32 }
}
