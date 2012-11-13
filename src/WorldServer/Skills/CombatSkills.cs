using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;
using Common.Events;
using World.World;
using Common.Tools;
using Common.Constants;
using Common.Network;
using World.Network;

namespace World.World
{
	public static partial class Skills
	{
		public static SkillResult CombatMasteryUse(MabiCreature source, MabiEntity targetEntity, SkillAction sourceSkillAction, MabiSkill skill, uint var1, uint var2)
		{
			var target = targetEntity as MabiCreature;
			if (target == null)
				return SkillResult.None;

			if (!WorldManager.InRange(source, target, (uint)(source.RaceInfo.AttackRange + 50)))
				return SkillResult.AttackOutOfRange;

			if (source.IsStunned())
				return SkillResult.AttackStunned;

			var sourceAction = sourceSkillAction as CombatAction;

			uint prevCombatActionId = 0;
			var rnd = RandomProvider.Get();

			source.StopMove();
			target.StopMove();

			// Aggro test
			if (target.Target != source)
			{
				target.Target = source;
				target.BattleState = 1;
				WorldManager.Instance.CreatureChangeStance(target, 0);
			}

			// Do this for two weapons, break if there is no second hit.
			for (int i = 1; i <= 2; ++i)
			{
				var combatArgs = new CombatEventArgs();
				combatArgs.CombatActionId = ActionId++;
				combatArgs.PrevCombatActionId = prevCombatActionId;
				combatArgs.Hit = (byte)i;
				combatArgs.HitsMax = (byte)(sourceAction.DualWield ? 2 : 1);

				var targetAction = new CombatAction();
				targetAction.Creature = target;
				targetAction.Target = source;
				targetAction.ActionType = CombatActionType.TakeDamage;
				targetAction.SkillId = sourceAction.SkillId;

				// TODO: We're getting the weapon twice =/ Maybe pass that to get damage.
				var weaponPocket = (i == 1 ? Pocket.LeftHand1 : Pocket.RightHand1);
				var weapon = source.GetItemInPocket(weaponPocket);
				var damage = source.GetDamage(weaponPocket);
				//if (weapon != null && (weapon.Type != ItemType.Weapon && weapon.Type != ItemType.Weapon2))
				//    weapon = null;

				// Crit (temp)
				if (rnd.NextDouble() < source.GetCritical())
				{
					damage *= 1.5f; // R1
					targetAction.Critical = true;
				}

				// Def (temp)
				if (target.ActiveSkillId == (ushort)SkillConst.Defense)
				{
					damage *= 0.1f;
					targetAction.ActionType |= CombatActionType.Defense;
				}

				targetAction.CombatDamage = damage;
				target.TakeDamage(damage);
				targetAction.Finish = target.IsDead();
				sourceAction.Finish = target.IsDead();

				// Stuns
				if (!targetAction.ActionType.HasFlag(CombatActionType.Defense))
				{
					var atkSpeed = (weapon == null ? source.RaceInfo.AttackSpeed : weapon.OptionInfo.AttackSpeed);
					var downHitCount = (weapon == null ? source.RaceInfo.KnockCount : weapon.OptionInfo.KnockCount);
					var targetStunTime = MabiCombat.CalculateStunTarget(atkSpeed, targetAction.IsKnock());

					sourceAction.StunTime = MabiCombat.CalculateStunSource(atkSpeed, targetAction.IsKnock());
					targetAction.StunTime = targetStunTime;

					source.AddStun(sourceAction.StunTime, true);
					target.AddStun(targetAction.StunTime, false);

					// Knockback/down
					if (target.Stun > (downHitCount * targetStunTime))
					{
						targetAction.Knockback = true;
						sourceAction.Knockback = true;
						sourceAction.StunTime = MabiCombat.CalculateStunSource(atkSpeed, true);
						targetAction.StunTime = MabiCombat.CalculateStunTarget(atkSpeed, true);
						source.AddStun(sourceAction.StunTime, true);
						target.AddStun(targetAction.StunTime, true);
					}
				}
				else
				{
					sourceAction.StunTime = 2500;
					targetAction.StunTime = 1000;
					source.AddStun(sourceAction.StunTime, true);
					target.AddStun(targetAction.StunTime, true);
					targetAction.SkillId = SkillConst.Defense;
				}

				if (targetAction.IsKnock())
				{
					targetAction.OldPosition = target.GetPosition().Copy();
					var pos = MabiCombat.CalculateKnockbackPos(source, target, 375);
					target.SetPosition(pos.X, pos.Y);
					targetAction.ActionType &= ~CombatActionType.Defense;
				}

				combatArgs.CombatActions.Add(sourceAction);
				combatArgs.CombatActions.Add(targetAction);

				if (targetAction.IsKnock())
				{
					combatArgs.HitsMax = combatArgs.Hit;
				}

				WorldManager.Instance.CreatureCombatAction(source, target, combatArgs);
				WorldManager.Instance.CreatureCombatSubmit(source, combatArgs.CombatActionId);

				WorldManager.Instance.CreatureStatsUpdate(source);
				WorldManager.Instance.CreatureStatsUpdate(target);

				if (combatArgs.Hit == combatArgs.HitsMax)
					break;

				prevCombatActionId = combatArgs.CombatActionId;
			}

			return SkillResult.Okay;

		}

		public static SkillResult FinalHitReady(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(69).PutBytes(1, 1), SendTargets.Range, creature);
			return SkillResult.Okay;
		}

		public static SkillResult SmashUse(MabiCreature source, MabiEntity targetEntity, SkillAction sourceSkillAction, MabiSkill skill, uint var1, uint var2)
		{
			var target = targetEntity as MabiCreature;
			var sourceAction = sourceSkillAction as CombatAction;

			if (target == null)
				return SkillResult.None;

			if (!WorldManager.InRange(source, target, (uint)(source.RaceInfo.AttackRange + 50)))
				return SkillResult.AttackOutOfRange;

			var rnd = RandomProvider.Get();

			source.StopMove();
			target.StopMove();

			if (target.Target != source)
			{
				target.Target = source;
				target.BattleState = 1;
				WorldManager.Instance.CreatureChangeStance(target, 0);
			}

			var combatArgs = new CombatEventArgs();
			combatArgs.CombatActionId = Skills.ActionId++;

			var targetAction = new CombatAction();
			targetAction.Creature = target;
			targetAction.Target = source;
			targetAction.ActionType = CombatActionType.TakeDamage;
			targetAction.SkillId = sourceAction.SkillId;

			// TODO: Race

			float damage = source.GetSmashDamage();

			damage *= skill.RankInfo.Var1;

			MabiItem weapon = source.GetItemInPocket(Pocket.LeftHand1);
			if (weapon != null && weapon.OptionInfo.WeaponType == (byte)ItemType.Weapon2H)
				damage *= 1.2f;

			damage /= 100;

			// Crit
			if (rnd.NextDouble() < source.GetCritical())
			{
				damage *= 1.5f; // R1
				targetAction.Critical = true;
			}

			targetAction.CombatDamage = damage;
			target.TakeDamage(damage);
			if (target.IsDead())
			{
				targetAction.Finish = sourceAction.Finish = true;
			}

			targetAction.Knockdown = sourceAction.Knockdown = true;
			targetAction.StunTime = sourceAction.StunTime = 3000;
			source.AddStun(sourceAction.StunTime, true);
			target.AddStun(targetAction.StunTime, true);

			targetAction.OldPosition = target.GetPosition().Copy();
			var pos = MabiCombat.CalculateKnockbackPos(source, target, 375);
			target.SetPosition(pos.X, pos.Y);

			combatArgs.CombatActions.Add(sourceAction);
			combatArgs.CombatActions.Add(targetAction);

			WorldManager.Instance.CreatureCombatAction(source, target, combatArgs);
			WorldManager.Instance.CreatureCombatSubmit(source, combatArgs.CombatActionId);

			WorldManager.Instance.CreatureStatsUpdate(source);
			WorldManager.Instance.CreatureStatsUpdate(target);

			return SkillResult.Okay;

		}

		public static SkillResult WindmillUse(MabiCreature source, MabiEntity targetEntity, SkillAction skillSourceAction, MabiSkill skill, uint var1, uint var2)
		{
			var creaturesInRange = WorldManager.Instance.GetCreaturesInRange(source, 600); //Rank 1

			// TODO: Filter out non-attackables

			if (creaturesInRange.Count == 0)
			{
				source.Client.Send(PacketCreator.Notice(source, "Unable to use when there is no target.", NoticeType.MIDDLE));
				source.Client.Send(new MabiPacket(Op.SkillUnkown1, source.Id));

				return SkillResult.AttackOutOfRange;
			}

			WorldManager.Instance.CreatureUseMotion(source, 8, 4);

			CombatAction sourceAction = skillSourceAction as CombatAction;

			bool crit = RandomProvider.Get().NextDouble() < source.GetCritical();

			foreach (var target in creaturesInRange)
			{
				if (target == source)
					continue;
				var combatArgs = new CombatEventArgs();
				combatArgs.CombatActionId = ActionId++;
				combatArgs.PrevCombatActionId = 0;
				combatArgs.Hit = 1;
				combatArgs.HitsMax = 1;

				sourceAction = new CombatAction();
				sourceAction.Creature = source;
				sourceAction.Target = target;
				sourceAction.ActionType = CombatActionType.Hit;
				sourceAction.SkillId = SkillConst.Windmill;

				var targetAction = new CombatAction();
				targetAction.Creature = target;
				targetAction.Target = source;
				targetAction.ActionType = CombatActionType.TakeDamage;
				targetAction.SkillId = SkillConst.Windmill;

				// Damage Calculation
				float damage = source.GetSmashDamage();

				//Todo: Prot/def
				damage *= skill.RankInfo.Var1 / 100;

				if (crit)
				{
					damage *= 1.5f; // R1
					targetAction.Critical = true;
				}

				//Damage Infliction
				targetAction.CombatDamage = damage;
				target.TakeDamage(damage);
				targetAction.Finish = sourceAction.Finish = target.IsDead();

				sourceAction.StunTime = 2500;
				targetAction.StunTime = 1500;

				source.AddStun(sourceAction.StunTime, true);
				target.AddStun(targetAction.StunTime, false);

				// TODO: surviving enemies

				targetAction.Knockdown = sourceAction.Knockdown = true;

				targetAction.OldPosition = target.GetPosition().Copy();
				var pos = MabiCombat.CalculateKnockbackPos(source, target, 375);
				target.SetPosition(pos.X, pos.Y);

				combatArgs.CombatActions.Add(sourceAction);
				combatArgs.CombatActions.Add(targetAction);

				WorldManager.Instance.CreatureCombatAction(source, target, combatArgs);
				WorldManager.Instance.CreatureCombatSubmit(source, combatArgs.CombatActionId);

				WorldManager.Instance.CreatureStatsUpdate(target);
				WorldManager.Instance.CreatureStatsUpdate(source);
			}

			source.Client.Send(new MabiPacket(Op.SkillStackUpdate, source.Id).PutBytes(0, 1, 0).PutShort(skill.Info.Id));
			return SkillResult.Okay;
		}
	}
}
