// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.World;
using Aura.World.Events;
using System;
using Aura.Shared.Network;
using Aura.Data;

namespace Aura.World.Skills
{
	public class CombatMasteryHandler : SkillHandler
	{
		public override SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			var targetId = packet.GetLong();
			return this.Use(creature, targetId);
		}

		public SkillResults Use(MabiCreature creature, ulong targetId)
		{
			if (creature.IsStunned)
				return SkillResults.AttackStunned;

			MabiSkill skill; SkillHandler handler;
			if (creature.ActiveSkillId != SkillConst.None)
			{
				skill = creature.GetSkill(creature.ActiveSkillId);
				handler = SkillManager.GetHandler(skill.Id);
			}
			else
			{
				skill = creature.GetSkill(SkillConst.MeleeCombatMastery);
				handler = this;
			}

			if (handler == null)
				return SkillResults.Unimplemented;

			return handler.UseCombat(creature, targetId, skill);
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, (uint)(attacker.RaceInfo.AttackRange + 50)))
				return SkillResults.OutOfRange;

			if (attacker.IsStunned)
				return SkillResults.AttackStunned;

			attacker.StopMove();
			target.StopMove();

			// Check counter
			var counter = CombatHelper.TryCounter(target, attacker);
			if (counter != SkillResults.None)
				return SkillResults.Okay;

			uint prevCombatActionId = 0;
			var rnd = RandomProvider.Get();

			var rightHand = attacker.RightHand;
			var leftHand = attacker.LeftHand;
			if (leftHand != null && !leftHand.IsOneHandWeapon)
				leftHand = null;

			var sAction = new SourceAction(CombatActionType.Hit, attacker, skill.Id, targetId);
			sAction.Options |= SourceOptions.Result;
			if (rightHand != null && leftHand != null)
				sAction.Options |= SourceOptions.DualWield;

			// Do this for two weapons, break if there is no second hit.
			for (byte i = 1; i <= 2; ++i)
			{
				var weapon = (i == 1 ? rightHand : leftHand);
				var atkSpeed = CombatHelper.GetAverageAttackSpeed(attacker);

				var cap = new CombatActionPack(attacker, skill.Id);
				cap.PrevCombatActionId = prevCombatActionId;
				cap.Hit = i;
				cap.HitsMax = (byte)(sAction.Has(SourceOptions.DualWield) ? 2 : 1);

				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);

				cap.Add(sAction, tAction);

				// Damage
				{
					var damage = attacker.GetRndDamage(weapon);
					var protection = target.Protection;

					// Crit
					if (CombatHelper.TryAddCritical(attacker, ref damage, (attacker.CriticalChance - protection)))
						tAction.Options |= TargetOptions.Critical;

					// Def/Prot
					CombatHelper.ReduceDamage(ref damage, target.Defense, protection);

					// Mana Shield
					tAction.ManaDamage = CombatHelper.DealManaDamage(target, ref damage);

					// Deal Life Damage
					if (damage > 0)
						target.TakeDamage(tAction.Damage = damage);
				}

				// Killed?
				if (target.IsDead)
				{
					tAction.Options |= TargetOptions.FinishingKnockDown;
					attacker.Stun = sAction.StunTime = CombatHelper.GetStunSource(atkSpeed, tAction.IsKnock);
				}
				else
				{
					// Defense
					if (target.HasSkillLoaded(SkillConst.Defense))
					{
						tAction.Type = CombatActionType.Defended;
						tAction.SkillId = SkillConst.Defense;
						target.Stun = tAction.StunTime = 1000;
						attacker.Stun = sAction.StunTime = 2500;
					}
					// Normal hit
					else
					{
						// Knock back
						if (target.RaceInfo.Is(RaceStands.KnockBackable))
						{
							target.KnockBack += CombatHelper.GetKnockDown(weapon) / cap.HitsMax;
							if (target.KnockBack >= CombatHelper.LimitKnockBack)
							{
								// Knock down if critical
								tAction.Options |= (tAction.Has(TargetOptions.Critical) ? TargetOptions.KnockDown : TargetOptions.KnockBack);
							}
						}

						attacker.Stun = sAction.StunTime = CombatHelper.GetStunSource(atkSpeed, tAction.IsKnock);
						target.Stun = tAction.StunTime = CombatHelper.GetStunTarget(atkSpeed, tAction.IsKnock);
					}
				}

				// Stop on knock back
				if (tAction.IsKnock && target.RaceInfo.Is(RaceStands.KnockBackable))
				{
					tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
					sAction.Options |= SourceOptions.KnockBackHit2;
					cap.HitsMax = cap.Hit;
				}

				// Weapon Exp
				if (weapon != null)
					SkillHelper.GiveItemExp(attacker, weapon);

				// Aggro
				CombatHelper.SetAggro(attacker, target);

				// Submit
				WorldManager.Instance.HandleCombatActionPack(cap);

				// Stop when all hits are done
				if (cap.Hit >= cap.HitsMax)
					break;

				prevCombatActionId = cap.CombatActionId;
			}

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}
	}
}
