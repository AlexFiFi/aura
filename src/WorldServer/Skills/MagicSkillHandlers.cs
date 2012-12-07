// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.World;
using Common.Events;
using Common.Network;
using System;

namespace World.Skills
{
	public class ManaShieldHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			// XXX: Add setter in MabiCreature?
			creature.Conditions.A |= CreatureConditionA.ManaShield;
			WorldManager.Instance.CreatureStatusEffectsChange(creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.ManaShield), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.Conditions.A &= ~CreatureConditionA.ManaShield;
			WorldManager.Instance.CreatureStatusEffectsChange(creature);

			return SkillResults.Okay;
		}
	}

	public class HealingHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill)
		{
			this.SetActive(creature, skill);
			this.SkillInit(creature, "healing");

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			this.InitStack(creature, skill);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.Healing).PutString("healing"), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			if (creature != target && !WorldManager.InRange(creature, target, 1000))
				return SkillResults.OutOfRange;

			// Reduce Stamina equal to healing amount if a player
			// is using the skill on himself.
			if (creature == target && creature is MabiPC)
			{
				var amount = Math.Min(skill.RankInfo.Var1, creature.LifeInjured - creature.Life);
				if (creature.Stamina < amount)
					return SkillResults.InsufficientStamina;

				creature.Stamina -= amount;
			}

			target.Life += skill.RankInfo.Var1;
			WorldManager.Instance.CreatureStatsUpdate(target);

			this.DecStack(creature, skill);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.HealingMotion).PutString("healing").PutLong(target.Id), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.Send(new MabiPacket(Op.Effect, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(0, 0));

			return SkillResults.Okay;
		}
	}
}
