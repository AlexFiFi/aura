using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;
using World.Network;
using Common.Network;
using Common.Events;
using Common.Constants;

namespace World.World
{
	public static partial class Skills
	{
		public static SkillResult HealingPrepare(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			var res = CheckMP(creature, skill);
			if (res == SkillResult.Okay)
			{
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(11).PutString("healing"), SendTargets.Range, creature);
			}
			return res;
		}
		public static SkillResult HealingReady(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(12).PutString("healing"), SendTargets.Range, creature);
			return SkillResult.Okay;
		}
		public static SkillResult HealingUsed(MabiCreature source, MabiEntity targetEntity, SkillAction sourceAction, MabiSkill skill, uint var1, uint var2)
		{
			if (! (targetEntity is MabiCreature))
				return SkillResult.None;

			var target = targetEntity as MabiCreature;

			if (source == target)
				if ((CheckSP(source, Math.Min(skill.RankInfo.Var1, source.LifeInjured - source.Life), true, true) & SkillResult.Okay) == 0)
					return SkillResult.InsufficientStats;

			target.Life += skill.RankInfo.Var1;
			WorldManager.Instance.CreatureStatsUpdate(target);

			source.ActiveSkillStacks--;
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, source.Id).PutInt(14).PutString("healing").PutLong(target.Id), SendTargets.Range, source);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, source.Id).PutInt(13).PutString("healing_stack").PutBytes(source.ActiveSkillStacks, 0), SendTargets.Range, source);
			source.Client.Send(new MabiPacket(Op.SkillStackUpdate, source.Id).PutBytes(source.ActiveSkillStacks, 1, 0).PutShort(skill.Info.Id));

			return SkillResult.Okay;
		}

		public static SkillResult ManaShieldStart(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			if ((CheckMP(creature,skill) & SkillResult.Okay) == 0)
				return SkillResult.InsufficientStats;

			creature.Conditions.A |= CreatureConditionA.ManaShield;
			WorldManager.Instance.CreatureStatusEffectsChange(creature, new EntityEventArgs(creature));
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(121), SendTargets.Range, creature);

			return SkillResult.Okay;
		}
	}
}
