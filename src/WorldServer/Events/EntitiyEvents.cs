// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.World.World;

namespace Aura.World.Events
{
	/// <summary>
	/// This class manages events raised by entities. The server subscribes to these.
	/// </summary>
	public class EntityEvents
	{
		public static readonly EntityEvents Instance = new EntityEvents();
		protected EntityEvents() { }

		public EventHandler<EntityEventArgs> CreatureLevelsUp;
		public EventHandler<EntityEventArgs> CreatureStatUpdates;
		public EventHandler<EntityEventArgs> CreatureStatusEffectUpdate;
		public EventHandler<ItemUpdateEventArgs> CreatureItemUpdate;
		public EventHandler<ItemEventArgs> CreatureDropItem;
		public EventHandler<SkillUpdateEventArgs> CreatureSkillUpdate;

		/// <summary>
		/// Fired while a player is changing the region,
		/// </summary>
		public EventHandler<EntityEventArgs> PlayerChangesRegion;

		/// <summary>
		/// Fired if an item is received, dropped, traded, or anything.
		/// Simply passes the involved item class.
		/// Mainly used in quests, to check collect objectives.
		/// </summary>
		public EventHandler<ItemActionEventArgs> CreatureItemAction;

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

		public void OnCreatureItemUpdate(MabiCreature creature, MabiItem item, bool isNew = false)
		{
			if (CreatureItemUpdate != null)
				CreatureItemUpdate(creature, new ItemUpdateEventArgs(item, isNew));
		}

		public void OnCreatureDropItem(MabiCreature creature, MabiItem item)
		{
			if (CreatureDropItem != null)
				CreatureDropItem(creature, new ItemEventArgs(item));
		}

		public void OnCreatureItemAction(MabiCreature creature, uint cls)
		{
			if (CreatureItemAction != null)
				CreatureItemAction(creature, new ItemActionEventArgs(cls));
		}

		public void OnCreatureSkillUpdate(MabiCreature creature, MabiSkill skill, bool isNew)
		{
			if (CreatureSkillUpdate != null)
				CreatureSkillUpdate(creature, new SkillUpdateEventArgs(skill, isNew));
		}

		public void OnPlayerChangesRegion(MabiEntity entity)
		{
			if (PlayerChangesRegion != null)
				PlayerChangesRegion(entity, new EntityEventArgs(entity));
		}
	}
}
