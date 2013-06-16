// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;

namespace Aura.World.Skills
{
	public class DefenseHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			SkillHelper.SendFlash(creature);
			creature.Client.SendSkillPrepare(creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			creature.Client.SendSkillComplete(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.SendSkillUse(creature, skill.Id, 1000, 1);

			return SkillResults.Okay;
		}
	}
}
