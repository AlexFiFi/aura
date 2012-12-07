// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.World;

namespace World.Skills
{
	public class RestHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.State |= CreatureStates.SitDown;
			WorldManager.Instance.CreatureSitDown(creature);
			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.State &= ~CreatureStates.SitDown;
			WorldManager.Instance.CreatureStandUp(creature);
			return SkillResults.Okay;
		}
	}
}
