using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;
using Common.Constants;

namespace World.World
{
	public static partial class Skills
	{
		public static SkillResult RestStart(MabiCreature creature, MabiSkill skill, string parameters)
		{
			creature.State |= CreatureStates.SitDown;
			WorldManager.Instance.CreatureSitDown(creature);
			return SkillResult.Okay;
		}

		public static SkillResult RestStop(MabiCreature creature, MabiSkill skill, string parameters)
		{
			creature.State &= ~CreatureStates.SitDown;
			WorldManager.Instance.CreatureStandUp(creature);
			return SkillResult.Okay;
		}
	}
}
