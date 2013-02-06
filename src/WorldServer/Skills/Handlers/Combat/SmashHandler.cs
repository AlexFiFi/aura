// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.Events;
using Common.Network;
using Common.Tools;
using Common.World;
using World.World;

namespace World.Skills
{
	public class SmashHandler : SkillHandler
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
			creature.Client.Send(new MabiPacket(Op.SkillUse, creature.Id).PutShort(creature.ActiveSkillId).PutInts(600, 1));

			return SkillResults.Okay;
		}

		public override SkillResults UseCombat(MabiCreature creature, MabiCreature target, CombatAction sourceAction, MabiSkill skill)
		{
			if (!WorldManager.InRange(creature, target, (uint)(creature.RaceInfo.AttackRange + 50)))
				return SkillResults.OutOfRange;

			var rnd = RandomProvider.Get();

			creature.StopMove();
			target.StopMove();

			this.SetAggro(creature, target);

			var combatArgs = new CombatEventArgs();
			combatArgs.CombatActionId = MabiCombat.ActionId;

			var targetAction = new CombatAction();
			targetAction.Creature = target;
			targetAction.Target = creature;
			targetAction.ActionType = CombatActionType.TakeDamage;
			targetAction.SkillId = sourceAction.SkillId;

			// TODO: Race

			var damage = creature.GetRndTotalDamage();
			damage *= skill.RankInfo.Var1 / 100;

			var critRate = creature.GetCritical();

			// +20% dmg and +5% crit for 2H
			if (creature.RightHand != null && creature.RightHand.OptionInfo.WeaponType == (byte)ItemType.Weapon2H)
			{
				damage *= 1.2f;
				critRate *= 1.05f;
			}

			// Crit
			if (rnd.NextDouble() < critRate)
			{
				damage *= 1.5f; // R1
				targetAction.Critical = true;
			}

			targetAction.CombatDamage = damage;
			target.TakeDamage(damage);
			targetAction.Finish = sourceAction.Finish = target.IsDead();
			if (target.IsDead())
			{
				targetAction.Finish = sourceAction.Finish = true;
			}

			targetAction.Knockdown = sourceAction.Knockdown = true;
			targetAction.StunTime = sourceAction.StunTime = 3000;
			creature.AddStun(sourceAction.StunTime, true);
			target.AddStun(targetAction.StunTime, true);

			targetAction.OldPosition = target.GetPosition();
			var pos = WorldManager.CalculatePosOnLine(creature, target, 375);
			target.SetPosition(pos.X, pos.Y);

			combatArgs.CombatActions.Add(sourceAction);
			combatArgs.CombatActions.Add(targetAction);

			WorldManager.Instance.CreatureCombatAction(creature, target, combatArgs);
			WorldManager.Instance.CreatureCombatSubmit(creature, combatArgs.CombatActionId);

			WorldManager.Instance.CreatureStatsUpdate(creature);
			WorldManager.Instance.CreatureStatsUpdate(target);

			this.GiveSkillExp(creature, skill, 20);

			return SkillResults.Okay;
		}
	}
}
