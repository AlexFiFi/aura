// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

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
		protected EntityEvents() { }

		public EventHandler<EntityEventArgs> CreatureLevelsUp;
		public EventHandler<EntityEventArgs> CreatureStatUpdates;
		public EventHandler<EntityEventArgs> CreatureStatusEffectUpdate;
		public EventHandler<ItemUpdateEventArgs> CreatureItemUpdate;
		public EventHandler<ItemEventArgs> CreatureDropItem;
		public EventHandler<SkillUpdateEventArgs> CreatureSkillUpdate;

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

		public void OnCreatureStatusEffectUpdate(MabiCreature c)
		{
			if (CreatureStatusEffectUpdate != null)
				CreatureStatusEffectUpdate(c, new EntityEventArgs(c));
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

		public void OnCreatureSkillUpdate(MabiCreature creature, MabiSkill skill, bool isNew)
		{
			if (CreatureSkillUpdate != null)
				CreatureSkillUpdate(creature, new SkillUpdateEventArgs(skill, isNew));
		}
	}
}
