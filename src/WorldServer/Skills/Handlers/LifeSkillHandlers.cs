// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.World.World;

namespace Aura.World.Skills
{
	public class RestHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.State |= CreatureStates.SitDown;
			WorldManager.Instance.CreatureSitDown(creature);

			this.GiveSkillExp(creature, skill, 20);

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
