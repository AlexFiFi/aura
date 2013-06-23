// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Shared.Util;
using Aura.World.Player;

namespace Aura.World.Events
{
	/// <summary>
	/// The server raises these events.
	/// </summary>
	public class EventManager
	{
		public readonly static EventManager Instance = new EventManager();
		protected EventManager()
		{
		}

		public readonly CreatureEvents CreatureEvents = new CreatureEvents();
		public readonly EntityEvents EntityEvents = new EntityEvents();
		public readonly PlayerEvents PlayerEvents = new PlayerEvents();
		public readonly TimeEvents TimeEvents = new TimeEvents();
	}

	/// <summary>
	/// Contains all events dealing with time
	/// </summary>
	public class TimeEvents
	{
		/// <summary>
		/// Raised every minute (erinn time) (1.5s real time).
		/// </summary>
		public EventHandler<TimeEventArgs> ErinnTimeTick;
		/// <summary>
		/// Raised every minute (real time).
		/// </summary>
		public EventHandler<TimeEventArgs> RealTimeTick;
		/// <summary>
		/// Raised every second (real time).
		/// </summary>
		public EventHandler<TimeEventArgs> RealTimeSecondTick;
		/// <summary>
		/// Raised at at 6:00am and 6:00pm (erinn time) (every 18 minutes real time).
		/// </summary>
		public EventHandler<TimeEventArgs> ErinnDaytimeTick;
		/// <summary>
		/// Raised at 0:00am (erinn time).
		/// </summary>
		public EventHandler<TimeEventArgs> ErinnMidnightTick;

		public void OnErinnTimeTick(object sender, TimeEventArgs e)
		{
			try
			{
				if (ErinnTimeTick != null)
					ErinnTimeTick(sender, e);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "In OnErinnTimeTick: " + ex.Message, true);
			}
		}

		public void OnErinnDaytimeTick(object sender, TimeEventArgs e)
		{
			if (ErinnDaytimeTick == null)
				return;

			// Iterate through the handlers to be able to tell which one errored,
			// and let the ones after that on be called.
			foreach (var handler in ErinnDaytimeTick.GetInvocationList().Cast<EventHandler<TimeEventArgs>>())
			{
				try { handler(sender, e); }
				catch (Exception ex) { Logger.Error("Source: {0}.{1}, Error: {2}", handler.Target, handler.Method.Name, ex.Message); }
			}
		}

		public void OnErinnMidnightTick(object sender, TimeEventArgs e)
		{
			if (ErinnMidnightTick != null)
				ErinnMidnightTick(sender, e);
		}

		public void OnRealTimeTick(object sender, TimeEventArgs e)
		{
			if (RealTimeTick != null)
				RealTimeTick(sender, e);
		}

		public void OnRealTimeSecondTick(object sender, TimeEventArgs e)
		{
			if (RealTimeSecondTick != null)
				RealTimeSecondTick(sender, e);
		}
	}

	/// <summary>
	/// Contains player-only events
	/// </summary>
	public class PlayerEvents
	{
		public EventHandler<PlayerEventArgs> PlayerLogsIn, PlayerLoggedIn, PlayerChangesRegion;

		public EventHandler<CreatureKilledEventArgs> KilledByPlayer;

		public void OnPlayerLogsIn(object sender, PlayerEventArgs e)
		{
			if (PlayerLogsIn != null)
				PlayerLogsIn(sender, e);
		}

		public void OnPlayerLoggedIn(object sender, PlayerEventArgs e)
		{
			if (PlayerLoggedIn != null)
				PlayerLoggedIn(sender, e);
		}

		public void OnKilledByPlayer(object sender, CreatureKilledEventArgs e)
		{
			if (KilledByPlayer != null)
				KilledByPlayer(sender, e);
		}

		public void OnPlayerChangesRegion(object sender, PlayerEventArgs e)
		{
			if (PlayerChangesRegion != null)
				PlayerChangesRegion(sender, e);
		}
	}

	/// <summary>
	/// Contains all events dealing with entities
	/// </summary>
	public class EntityEvents
	{
		public EventHandler<EntityEventArgs> EntityEntersRegion, EntityLeavesRegion;

		public void OnEntityEntersRegion(object sender, EntityEventArgs e)
		{
			if (EntityEntersRegion != null)
				EntityEntersRegion(sender, e);
		}

		public void OnEntityLeavesRegion(object sender, EntityEventArgs e)
		{
			if (EntityLeavesRegion != null)
				EntityLeavesRegion(sender, e);
		}
	}

	/// <summary>
	/// Contains all events dealing with creatures
	/// </summary>
	public class CreatureEvents
	{
		public EventHandler<MoveEventArgs> CreatureMoves;
		public EventHandler<ChatEventArgs> CreatureTalks;
		public EventHandler<MotionEventArgs> CreatureUsesMotion;
		public EventHandler<CreatureKilledEventArgs> CreatureKilled;
		public EventHandler<CreatureEventArgs> CreatureStatUpdates, CreatureLevelsUp;
		public EventHandler<SkillUpdateEventArgs> CreatureSkillUpdate;
		public EventHandler<ItemUpdateEventArgs> CreatureItemUpdate;
		public EventHandler<ItemEventArgs> CreatureDropItem;
		public EventHandler<ItemActionEventArgs> CreatureItemAction;

		public void OnCreatureKilled(object sender, CreatureKilledEventArgs e)
		{
			if (CreatureKilled != null)
				CreatureKilled(sender, e);
		}

		public void OnCreatureMoves(object sender, MoveEventArgs e)
		{
			if (CreatureMoves != null)
				CreatureMoves(sender, e);
		}

		public void OnCreatureTalks(object sender, ChatEventArgs e)
		{
			if (CreatureTalks != null)
				CreatureTalks(sender, e);
		}

		public void OnCreatureUsesMotion(object sender, MotionEventArgs e)
		{
			if (CreatureUsesMotion != null)
				CreatureUsesMotion(sender, e);
		}

		public void OnCreatureLevelsUp(object sender, CreatureEventArgs e)
		{
			if (CreatureLevelsUp != null)
				CreatureLevelsUp(sender, e);
		}

		public void OnCreatureStatUpdates(object sender, CreatureEventArgs e)
		{
			if (CreatureStatUpdates != null)
				CreatureStatUpdates(sender, e);
		}

		public void OnCreatureSkillUpdate(object sender, SkillUpdateEventArgs e)
		{
			if (CreatureSkillUpdate != null)
				CreatureSkillUpdate(sender, e);
		}

		public void OnCreatureItemUpdate(object sender, ItemUpdateEventArgs e)
		{
			if (CreatureItemUpdate != null)
				CreatureItemUpdate(sender, e);
		}

		public void OnCreatureDropItem(object sender, ItemEventArgs e)
		{
			if (CreatureDropItem != null)
				CreatureDropItem(sender, e);
		}

		public void OnCreatureItemAction(object sender, ItemActionEventArgs e)
		{
			if (CreatureItemAction != null)
				CreatureItemAction(sender, e);
		}
	}
}
