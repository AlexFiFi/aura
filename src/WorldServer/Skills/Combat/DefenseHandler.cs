// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.World;
using Common.Network;

namespace World.Skills
{
	public class DefenseHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill)
		{
			this.SetActive(creature, skill);
			this.Flash(creature);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.Send(new MabiPacket(Op.SkillUse, creature.Id).PutShort(creature.ActiveSkillId).PutInts(1000, 1));

			return SkillResults.Okay;
		}
	}
}
