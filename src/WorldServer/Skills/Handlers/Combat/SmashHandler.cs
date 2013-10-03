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
	[SkillAttr(SkillConst.Smash)]
	public class SmashHandler : SkillHandler
	{
		/// <summary>
		/// Stun time for attacker and target.
		/// </summary>
		private const ushort StunTime = 3000;
		private const ushort AfterUseStun = 600;

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
			Send.SkillUse(creature.Client, creature, skill.Id, AfterUseStun, 1);

			return SkillResults.Okay;
		}

		private float calculateSmashDamage(MabiCreature attacker, MabiCreature target)
		{
			var dmg = attacker.GetRndTotalDamage();
			dmg *= attacker.Skills.Get(SkillConst.Smash).RankInfo.Var1 / 100f;

			if (attacker.RightHand != null && attacker.RightHand.IsTwoHandWeapon)
			{
				dmg *= 1.20f;
			}

			return dmg;
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

			attacker.StopMove();
			target.StopMove();

			var factory = new CombatFactory();

			factory.SetAttackerAction(attacker, CombatActionType.HardHit, skill.Id, targetId);
			factory.SetAttackerOptions(AttackerOptions.Result | AttackerOptions.KnockBackHit2);
			factory.SetAttackerStun(AfterUseStun);

			factory.AddTargetAction(target, CombatActionType.TakeHit, skillId: SkillConst.MeleeCombatMastery);
			factory.SetTargetOptions(TargetOptions.Result | TargetOptions.Smash);
			factory.SetTargetStun(StunTime);

			var critChance = attacker.CriticalChanceAgainst(target);
			var critOpts = CombatFactory.CriticalOptions.NoCritical;

			// +5% crit for 2H
			if (attacker.RightHand != null && attacker.RightHand.IsTwoHandWeapon)
				critChance *= 1.05f;

			// Crit
			if (CombatHelper.TryCritical(critChance))
				critOpts = CombatFactory.CriticalOptions.Critical;

			factory.ExecuteDamage(calculateSmashDamage, critOpts);
			factory.ExecuteStun();
			factory.ExecuteKnockback(CombatHelper.MaxKnockBack);

			WorldManager.Instance.HandleCombatActionPack(factory.GetCap());

			Send.SkillUse(attacker.Client, attacker, skill.Id, AfterUseStun, 1);

			CombatHelper.SetAggro(attacker, target);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}
	}
}
