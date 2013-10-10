// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Util;
using Aura.Data;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.DualGunMastery)]
	public class DualGunMasteryHandler : SkillHandler
	{
		private const string BulletCountTag = "GVBC";
		private const ushort AfterKillStun = 850;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			var targetId = packet.GetLong();

			if (creature.RightHand == null)
			{
				Send.SkillPrepareFail(creature.Client, creature);
				return SkillResults.Failure;
			}

			if (!creature.RightHand.Tags.Has(BulletCountTag))
				creature.RightHand.Tags.SetShort(BulletCountTag, 5);

			ushort bulletCount = creature.RightHand.Tags[BulletCountTag];
			if (bulletCount < 2)
			{
				Send.SkillPrepareFail(creature.Client, creature);
				return SkillResults.Failure;
			}

			Send.SkillUse(creature.Client, creature, SkillConst.DualGunMastery, targetId);

			return SkillResults.Okay;
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, 1000))
				return SkillResults.OutOfRange;

			var atkSpeed = CombatHelper.GetAverageAttackSpeed(attacker);

			var aAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, targetId);
			aAction.Options |= AttackerOptions.Result;

			var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, SkillConst.MeleeCombatMastery);
			tAction.Options |= TargetOptions.Result;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(aAction, tAction);

			// Damage
			{
				var rnd = RandomProvider.Get();
				var damage = (float)rnd.Next(attacker.RightHand.OptionInfo.AttackMin, attacker.RightHand.OptionInfo.AttackMax);

				target.TakeDamage(tAction.Damage = damage);
			}

			// Killed?
			if (target.IsDead)
			{
				tAction.Options |= TargetOptions.FinishingKnockDown;
				attacker.Stun = aAction.StunTime = AfterKillStun;
			}
			else
			{
				// Defense
				if (target.HasSkillLoaded(SkillConst.Defense))
				{
					tAction.Type = CombatActionType.Defended;
					tAction.SkillId = SkillConst.Defense;
					target.Stun = tAction.StunTime = 1000;
					attacker.Stun = aAction.StunTime = 2500;
				}
				// Normal hit
				else
				{
					// Knock back
					//if (target.RaceInfo.Is(RaceStands.KnockBackable))
					//{
					//    target.KnockBack += 30;
					//    if (target.KnockBack >= CombatHelper.LimitKnockBack)
					//    {
					//        // Knock down if critical
					//        tAction.Options |= (tAction.Has(TargetOptions.Critical) ? TargetOptions.KnockDown : TargetOptions.KnockBack);
					//    }
					//}

					attacker.Stun = aAction.StunTime = CombatHelper.GetStunSource(atkSpeed, tAction.IsKnock);
					target.Stun = tAction.StunTime = CombatHelper.GetStunTarget(atkSpeed, tAction.IsKnock);
				}
			}

			// Stop on knock back
			if (tAction.IsKnock && target.RaceInfo.Is(RaceStands.KnockBackable))
			{
				tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
				aAction.Options |= AttackerOptions.KnockBackHit2;
			}

			// Aggro
			CombatHelper.SetAggro(attacker, target);

			// Item update
			{
				ushort bulletCount = attacker.RightHand.Tags[BulletCountTag];
				bulletCount -= 2;
				attacker.RightHand.Tags.SetShort(BulletCountTag, bulletCount);
				Send.ItemUpdate(attacker.Client, attacker, attacker.RightHand);
			}

			// Submit
			WorldManager.Instance.HandleCombatActionPack(cap);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}
	}
}
