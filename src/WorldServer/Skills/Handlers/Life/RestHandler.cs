// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Rest)]
	public class RestHandler : StartStopSkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			creature.State |= CreatureStates.SitDown;
			Send.SitDown(creature);

			SkillHelper.GiveSkillExp(creature, skill, 20);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			creature.State &= ~CreatureStates.SitDown;
			Send.StandUp(creature);

			return SkillResults.Okay;
		}
	}
}
