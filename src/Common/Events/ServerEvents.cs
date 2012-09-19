// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;

namespace Common.Events
{
	/// <summary>
	/// The server raises these events. Entities should subscribe in their
	/// "HookUp" method and unsubscribe in their "Dispose" method.
	/// </summary>
	public class ServerEvents
	{
		public readonly static ServerEvents Instance = new ServerEvents();
		protected ServerEvents()
		{

		}

		public EventHandler PlayerLogsIn;
		public EventHandler<TimeEventArgs> ErinnTimeTick, RealTimeTick, Erinn12HourTick;
		public EventHandler<EntityEventArgs> EntityEntersRegion, EntityLeavesRegion;
		public EventHandler<MoveEventArgs> CreatureMoves;
		public EventHandler<ChatEventArgs> CreatureTalks;
		public EventHandler<MotionEventArgs> CreatureUsesMotion;


		public void OnPlayerLogsIn(MabiCreature creature, EventArgs e = null)
		{
			if (PlayerLogsIn != null)
				PlayerLogsIn(creature, null);
		}

		public void OnErinnTimeTick(object sender, TimeEventArgs e)
		{
			if (ErinnTimeTick != null)
				ErinnTimeTick(sender, e);
		}

		public void OnErinnDaytimeTick(object sender, TimeEventArgs e)
		{
			if (Erinn12HourTick != null)
				Erinn12HourTick(sender, e);
		}

		public void OnRealTimeTick(object sender, TimeEventArgs e)
		{
			if (RealTimeTick != null)
				RealTimeTick(sender, e);
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
