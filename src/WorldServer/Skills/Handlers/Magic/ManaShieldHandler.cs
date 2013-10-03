// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.ManaShield)]
	public class ManaShieldHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.Conditions.A |= CreatureConditionA.ManaShield;
			Send.StatusEffectUpdate(creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.ManaShield), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.Conditions.A &= ~CreatureConditionA.ManaShield;
			Send.StatusEffectUpdate(creature);

			return SkillResults.Okay;
		}
	}
}
