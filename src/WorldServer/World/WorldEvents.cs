// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.World;

namespace World.World
{
	public class WorldEvents
	{
		public readonly static WorldEvents Instance = new WorldEvents();
		static WorldEvents() { }
		private WorldEvents() { }

		public EventHandler PlayerLogsIn;
		public EventHandler<TimeEventArgs> ErinnTimeTick, RealTimeTick;
		public EventHandler EntityEntersRegion, EntityLeavesRegion;
		public EventHandler<MoveEventArgs> CreatureMoves;
		public EventHandler<ChatEventArgs> CreatureTalks;
		public EventHandler<MotionEventArgs> CreatureUsesMotion;
		public EventHandler CreatureLevelsUp;

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

		public void OnRealTimeTick(object sender, TimeEventArgs e)
		{
			if (RealTimeTick != null)
				RealTimeTick(sender, e);
		}

		public void OnEntityEntersRegion(object sender, EventArgs e = null)
		{
			if (EntityEntersRegion != null)
				EntityEntersRegion(sender, e);
		}

		public void OnEntityLeavesRegion(object sender, EventArgs e = null)
		{
			if (EntityLeavesRegion != null)
				EntityLeavesRegion(sender, e);
		}

		public void OnCreatureLevelsUp(object sender, EventArgs e = null)
		{
			if (CreatureLevelsUp != null)
				CreatureLevelsUp(sender, e);
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
