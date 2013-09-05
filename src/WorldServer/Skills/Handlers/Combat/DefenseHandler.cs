// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Defense)]
	public class DefenseHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			Send.Flash(creature);
			Send.SkillPrepare(creature.Client, creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			Send.SkillReady(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			Send.SkillUse(creature.Client, creature, skill.Id, 1000, 1);

			return SkillResults.Okay;
		}
	}
}
