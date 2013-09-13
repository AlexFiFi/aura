// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.World;

namespace Aura.World.Skills.Handlers.Actions
{
	[SkillAttr(SkillConst.SpreadWings)]
	public class SpreadWingsHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.Activate(CreatureConditionD.SpreadWings);

			Send.SpreadWings(creature, true);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.Deactivate(CreatureConditionD.SpreadWings);

			Send.SpreadWings(creature, false);

			return SkillResults.Okay;
		}
	}
}
