// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Shared.Util;
using Aura.World.Player;

namespace Aura.World.Events
{
	/// <summary>
	/// The server raises these events. Entities should subscribe in their
	/// "HookUp" method and unsubscribe in their "Dispose" method.
	/// </summary>
	public class ServerEvents
	{
		public readonly static ServerEvents Instance = new ServerEvents();
		protected ServerEvents()
		{ }

		public EventHandler PlayerLogsIn, PlayerLoggedIn;

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

		public EventHandler<EntityEventArgs> EntityEntersRegion, EntityLeavesRegion;
		public EventHandler<MoveEventArgs> CreatureMoves;
		public EventHandler<ChatEventArgs> CreatureTalks;
		public EventHandler<MotionEventArgs> CreatureUsesMotion;
		public EventHandler<CreatureKilledEventArgs> CreatureKilled, KilledByPlayer;

		public void OnPlayerLogsIn(MabiPC creature, EventArgs e = null)
		{
			if (PlayerLogsIn != null)
				PlayerLogsIn(creature, null);
		}

		public void OnPlayerLoggedIn(MabiPC creature, EventArgs e = null)
		{
			if (PlayerLoggedIn != null)
				PlayerLoggedIn(creature, null);
		}

		public void OnCreatureKilled(CreatureKilledEventArgs e)
		{
			if (CreatureKilled != null)
				CreatureKilled(null, e);
		}

		public void OnKilledByPlayer(CreatureKilledEventArgs e)
		{
			if (KilledByPlayer != null)
				KilledByPlayer(null, e);
		}

		public void OnErinnTimeTick(object sender, TimeEventArgs e)
		{
			try
			{
				if (ErinnTimeTick != null)
					ErinnTimeTick(sender, e);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "In OnErinnTimeTick: " + ex.Message, false);
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

		public void OnEntityEntersRegion(object sender, EntityEventArgs e = null)
		{
			if (EntityEntersRegion != null)
				EntityEntersRegion(sender, e);
		}

		public void OnEntityLeavesRegion(object sender, EntityEventArgs e = null)
		{
			if (EntityLeavesRegion != null)
				EntityLeavesRegion(sender, e);
		}
		public void OnCreatureMoves(object sender, MoveEventArgs e)
		{
			if (CreatureMoves != null)
				CreatureMoves(sender, null);
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
	}
}
