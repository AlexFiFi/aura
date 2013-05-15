// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using Aura.World.Events;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	public class SmashHandler : SkillHandler
	{
		/// <summary>
		/// Stun time for attacker and target.
		/// </summary>
		private const ushort StunTime = 3000;
		private const ushort AfterUseStun = 600;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			WorldManager.Instance.SendFlash(creature);
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
			creature.Client.SendSkillUse(creature, skill.Id, AfterUseStun, 1);

			return SkillResults.Okay;
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			// if rank > x then
			//     for splash targets

			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, (uint)(attacker.RaceInfo.AttackRange + 50)))
				return SkillResults.OutOfRange;

			var rnd = RandomProvider.Get();

			attacker.StopMove();
			target.StopMove();

			var sAction = new SourceAction(CombatActionType.HardHit, attacker, skill.Id, targetId);
			sAction.Options |= SourceOptions.Result | SourceOptions.KnockBackHit2;

			var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, SkillConst.MeleeCombatMastery);
			tAction.Options = TargetOptions.Result | TargetOptions.Smash;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction, tAction);

			var damage = attacker.GetRndTotalDamage();
			damage *= skill.RankInfo.Var1 / 100f;

			var critChance = (attacker.CriticalChance - target.Protection);

			// +20% dmg and +5% crit for 2H
			if (attacker.RightHand != null && attacker.RightHand.IsTwoHandWeapon)
			{
				damage *= 1.20f;
				critChance *= 1.05f;
			}

			// Crit
			if (CombatHelper.TryAddCritical(attacker, ref damage, critChance))
				tAction.Options |= TargetOptions.Critical;

			// Def/Prot
			CombatHelper.ReduceDamage(ref damage, target.Defense, target.Protection);

			// Mana Shield
			tAction.ManaDamage = CombatHelper.DealManaDamage(target, ref damage);

			// Deal Life Damage
			if (damage > 0)
				target.TakeDamage(tAction.Damage = damage);

			if (target.IsDead)
				tAction.Options |= TargetOptions.FinishingHit | TargetOptions.Finished;

			attacker.Stun = sAction.StunTime = StunTime;
			target.Stun = tAction.StunTime = StunTime;
			target.KnockBack = CombatHelper.MaxKnockBack;

			tAction.OldPosition = CombatHelper.KnockBack(target, attacker);

			WorldManager.Instance.HandleCombatActionPack(cap);

			attacker.Client.SendSkillUse(attacker, skill.Id, AfterUseStun, 1);

			CombatHelper.SetAggro(attacker, target);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}
	}
}
