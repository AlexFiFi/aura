// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Util;
using Aura.World.Player;
using Aura.World.World;

namespace Aura.World.Events
{
	public delegate void EntityEventHandler(MabiEntity entity);
	public delegate void CreatureEventHandler(MabiCreature player);
	public delegate void PlayerEventHandler(MabiPC player);
	public delegate void NpcEventHandler(MabiNPC player);

	public delegate void TimeEventHandler(MabiTime time);

	public delegate void ChatEventHandler(MabiCreature creature, string message);

	public delegate void KillEventHandler(MabiCreature victim, MabiCreature killer);

	public delegate void SkillChangeEventHandler(MabiCreature creature, MabiSkill skill, bool isNew);

	public delegate void ItemEventHandler(MabiCreature creature, MabiItem item);
	public delegate void ItemClassEventHandler(MabiCreature creature, uint cls);

	public delegate void MoveEventHandler(MabiCreature creature, MabiVertex from, MabiVertex to);
}
