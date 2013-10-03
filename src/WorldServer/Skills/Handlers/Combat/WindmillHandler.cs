// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using System.Collections.Generic;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Windmill)]
	public class WindmillHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			Send.SkillInitEffect(creature);

			creature.StopMove();

			Send.SkillPrepare(creature.Client, creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			SkillHelper.FillStack(creature, skill);
			Send.SkillReady(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature attacker, MabiSkill skill, MabiPacket packet)
		{
			//Logger.Debug(packet);

			var targetId = packet.GetLong();
			var unk1 = packet.GetInt();
			var unk2 = packet.GetInt();

			// Determine range, doesn't seem to be included in rank info.
			var range = this.GetRange(skill);

			// Add attack range from race, range must increase depending on "size".
			range += (uint)attacker.RaceInfo.AttackRange;

			var enemies = WorldManager.Instance.GetAttackableCreaturesInRange(attacker, range);
			if (enemies.Count < 1)
			{
				Send.Notice(attacker.Client, Localization.Get("skills.wm_no_target")); // Unable to use when there is no target.
				Send.SkillSilentCancel(attacker.Client, attacker);

				return SkillResults.OutOfRange | SkillResults.Failure;
			}

			var rnd = RandomProvider.Get();

			attacker.StopMove();

			// Spin motion
			Send.UseMotion(attacker, 8, 4);

			var cap = new CombatActionPack(attacker, skill.Id);

			// One source action, target actions are added for every target
			// and then we send the pack on its way.
			var sAction = new AttackerAction(CombatActionType.Hit, attacker, skill.Id, targetId);
			sAction.Options |= AttackerOptions.Result | AttackerOptions.KnockBackHit1;

			cap.Add(sAction);

			attacker.Stun = sAction.StunTime = 2500;

			// For aggro selection, only one enemy gets it.
			MabiCreature aggroTarget = null;
			var survived = new List<MabiCreature>();

			foreach (var target in enemies)
			{
				target.StopMove();

				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
				cap.Add(tAction);

				var damage = attacker.GetRndTotalDamage();
				damage *= skill.RankInfo.Var1 / 100;

				if (CombatHelper.TryAddCritical(attacker, ref damage, attacker.CriticalChance))
					tAction.Options |= TargetOptions.Critical;

				target.TakeDamage(tAction.Damage = damage);
				if (target.IsDead)
					tAction.Options |= TargetOptions.FinishingKnockDown;

				tAction.Options |= TargetOptions.KnockDown;
				target.Stun = tAction.StunTime = CombatHelper.GetStunTarget(CombatHelper.GetAverageAttackSpeed(attacker), true);

				tAction.OldPosition = CombatHelper.KnockBack(target, attacker, 375);

				tAction.Delay = (uint)rnd.Next(300, 351);

				if (target.Target == attacker)
					aggroTarget = target;

				if (!target.IsDead)
					survived.Add(target);
			}

			// No aggro yet, random target.
			if (aggroTarget == null && survived.Count > 0)
				aggroTarget = survived[rnd.Next(0, survived.Count)];

			if (aggroTarget != null)
				CombatHelper.SetAggro(attacker, aggroTarget);

			WorldManager.Instance.HandleCombatActionPack(cap);

			Send.SkillUse(attacker.Client, attacker, skill.Id, targetId, unk1, unk2);
			//attacker.Client.SendSkillStackUpdate(attacker, skill.Id, 0);

			SkillHelper.DecStack(attacker, skill);
			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}

		protected virtual uint GetRange(MabiSkill skill)
		{
			uint range = 300;
			if (skill.Rank >= SkillRank.R5)
				range += 100;
			if (skill.Rank >= SkillRank.R1)
				range += 100;
			return range;
		}
	}

	/// <summary>
	/// The GM skill has 2 vars, 1000 and 1500, we'll asume it never cost HP,
	/// var 1 is still the damage, and var 2 is here the range.
	/// </summary>
	[SkillAttr(SkillConst.SuperWindmillGMSkill)]
	public class SuperWindmillHandler : WindmillHandler
	{
		protected override uint GetRange(MabiSkill skill)
		{
			return (uint)skill.RankInfo.Var2;
		}
	}
}
