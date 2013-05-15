// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.World;

namespace Aura.World.Skills
{
	public class ManaShieldHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.Conditions.A |= CreatureConditionA.ManaShield;
			WorldManager.Instance.SendStatusEffectUpdate(creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.ManaShield), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.Conditions.A &= ~CreatureConditionA.ManaShield;
			WorldManager.Instance.SendStatusEffectUpdate(creature);

			return SkillResults.Okay;
		}
	}

	public class HealingHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			WorldManager.Instance.SendSkillInitEffect(creature, "healing");
			creature.Client.SendSkillPrepare(creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			SkillHelper.InitStack(creature, skill);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.Healing).PutString("healing"), SendTargets.Range, creature);

			creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			creature.Client.SendSkillComplete(creature, skill.Id);
			if (creature.ActiveSkillStacks > 0)
				creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.Send(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("healing_stack").PutBytes(0, 0));

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			var targetId = packet.GetLong();
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

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

			SkillHelper.DecStack(creature, skill);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.UseMagic).PutString("healing").PutLong(target.Id), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			SkillHelper.GiveSkillExp(creature, skill, 20);

			creature.Client.SendSkillUse(creature, skill.Id, targetId);

			return SkillResults.Okay;
		}
	}
}
