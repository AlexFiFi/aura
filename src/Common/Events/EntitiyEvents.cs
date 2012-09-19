using System;
using Common.World;

namespace Common.Events
{
	/// <summary>
	/// This class manages events raised by entities. The server subscribes to these.
	/// </summary>
    public class EntityEvents
    {
		public static readonly EntityEvents Instance = new EntityEvents();

		protected EntityEvents()
		{

		}

        public EventHandler<EntityEventArgs> CreatureLevelsUp;
		public EventHandler<EntityEventArgs> CreatureStatUpdates;

		public void OnCreatureLevelsUp(MabiCreature c)
		{
			if (CreatureLevelsUp != null)
				CreatureLevelsUp(c, new EntityEventArgs(c));
		}
		public void OnCreatureStatUpdates(MabiCreature c)
		{
			if (CreatureStatUpdates != null)
				CreatureStatUpdates(c, new EntityEventArgs(c));
		}
	}
}
