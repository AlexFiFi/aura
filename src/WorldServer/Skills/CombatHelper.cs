// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.World.World;
using System;
using Aura.Shared.Util;
namespace Aura.World.Skills
{
	public static class CombatHelper
	{
		private enum CombatStunNormalSelf { VeryFast = 450, Fast = 520, Normal = 600, Slow = 800, VerySlow = 1000 }
		private enum CombatStunNormalTarget { VeryFast = 1200, Fast = 1700, Normal = 2000, Slow = 2800, VerySlow = 3000 }

		private enum CombatStunKnockbackSelf { VeryFast = 1200, Fast = 2500, Normal = 2500, Slow = 2500, VerySlow = 2500 }
		private enum CombatStunKnockbackTarget { VeryFast = 1500, Fast = 2000, Normal = 2000, Slow = 2800, VerySlow = 3000 }

		/// <summary>
		/// When to perform the knock back.
		/// </summary>
		public const float LimitKnockBack = 100;
		/// <summary>
		/// Max knock back value possible.
		/// </summary>
		public const float MaxKnockBack = 120;

		private static uint _actionId = 1;

		/// <summary>
		/// Returns a new usable action id.
		/// </summary>
		public static uint GetNewActionId() { return _actionId++; }

		public static ushort GetStunSource(int weaponSpeed, bool knockback)
		{
			if (knockback)
			{
				switch (weaponSpeed)
				{
					case 0x00: return (ushort)CombatStunKnockbackSelf.VeryFast;
					case 0x01: return (ushort)CombatStunKnockbackSelf.Fast;
					case 0x02: return (ushort)CombatStunKnockbackSelf.Normal;
					case 0x03: return (ushort)CombatStunKnockbackSelf.Slow;
					default: return (ushort)CombatStunKnockbackSelf.VerySlow;
				}
			}
			else
			{
				switch (weaponSpeed)
				{
					case 0x00: return (ushort)CombatStunNormalSelf.VeryFast;
					case 0x01: return (ushort)CombatStunNormalSelf.Fast;
					case 0x02: return (ushort)CombatStunNormalSelf.Normal;
					case 0x03: return (ushort)CombatStunNormalSelf.Slow;
					default: return (ushort)CombatStunNormalSelf.VerySlow;
				}
			}
		}

		public static ushort GetStunTarget(int weaponSpeed, bool knockback)
		{
			if (knockback)
			{
				switch (weaponSpeed)
				{
					case 0x00: return (ushort)CombatStunKnockbackTarget.VeryFast;
					case 0x01: return (ushort)CombatStunKnockbackTarget.Fast;
					case 0x02: return (ushort)CombatStunKnockbackTarget.Normal;
					case 0x03: return (ushort)CombatStunKnockbackTarget.Slow;
					default: return (ushort)CombatStunKnockbackTarget.VerySlow;
				}
			}
			else
			{
				switch (weaponSpeed)
				{
					case 0x00: return (ushort)CombatStunNormalTarget.VeryFast;
					case 0x01: return (ushort)CombatStunNormalTarget.Fast;
					case 0x02: return (ushort)CombatStunNormalTarget.Normal;
					case 0x03: return (ushort)CombatStunNormalTarget.Slow;
					default: return (ushort)CombatStunNormalTarget.VerySlow;
				}
			}
		}

		public static float GetKnockDown(MabiItem weapon)
		{
			var count = weapon != null ? weapon.Info.KnockCount + 1 : 3;
			var speed = weapon != null ? weapon.AttackSpeed : AttackSpeed.Normal;

			switch (count)
			{
				default:
				case 1:
					return 100;
				case 2:
					switch (speed)
					{
						default:
						case AttackSpeed.VerySlow: return 70;
						case AttackSpeed.Slow: return 68;
						case AttackSpeed.Normal: return 68; // ?
						case AttackSpeed.Fast: return 68; // ?
					}
				case 3:
					switch (speed)
					{
						default:
						case AttackSpeed.VerySlow: return 60;
						case AttackSpeed.Slow: return 56; // ?
						case AttackSpeed.Normal: return 53;
						case AttackSpeed.Fast: return 50;
					}
				case 5:
					switch (speed)
					{
						default:
						case AttackSpeed.Fast: return 40; // ?
						case AttackSpeed.VeryFast: return 35; // ?
					}
			}
		}

		/// <summary>
		/// Applies critical multiplier if crit occures.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="chance"></param>
		/// <param name="damage"></param>
		/// <returns>Returns whether a crit happened.</returns>
		public static bool TryAddCritical(MabiCreature creature, ref float damage, float chance)
		{
			if (RandomProvider.Get().NextDouble() < chance)
			{
				damage *= creature.CriticalMultiplicator;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Reduces damage by a set amount of points (defense) and a
		/// percentage (protection) afterwards.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="defense"></param>
		/// <param name="protection"></param>
		public static void ReduceDamage(ref float damage, float defense, float protection)
		{
			damage = Math.Max(1, damage - defense);
			if (damage > 1)
				damage = Math.Max(1, damage - (damage * protection));
		}

		/// <summary>
		/// Returns total amount of hits for the creature, depending on both
		/// weapons. Added together if dual wielding.
		/// </summary>
		/// <param name="creature"></param>
		/// <returns></returns>
		public static int GetTotalDownHitCount(MabiCreature creature)
		{
			var result = creature.RaceInfo.KnockCount;
			if (creature.RightHand != null && creature.RightHand.IsWeapon)
			{
				result = creature.RightHand.Info.KnockCount;
				if (creature.LeftHand != null && creature.LeftHand.IsWeapon)
					result += creature.LeftHand.Info.KnockCount;
			}

			return result;
		}

		/// <summary>
		/// Returns average rounded down attack speed for creature,
		/// depending on both weapons.
		/// </summary>
		/// <param name="creature"></param>
		/// <returns></returns>
		public static int GetAverageAttackSpeed(MabiCreature creature)
		{
			var result = creature.RaceInfo.AttackSpeed;
			if (creature.RightHand != null && creature.RightHand.IsWeapon)
			{
				result = creature.RightHand.OptionInfo.AttackSpeed;
				if (creature.LeftHand != null && creature.LeftHand.IsWeapon)
				{
					result += creature.LeftHand.Info.KnockCount;
					result = (int)Math.Floor(result / 2f);
				}
			}

			return result;
		}

		/// <summary>
		/// Sets the attacking creature for target. Should be called after
		/// stunning, or the target might attack instantly.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		public static void SetAggro(MabiCreature attacker, MabiCreature target)
		{
			if (!target.IsDead && target.Target != attacker)
			{
				target.Target = attacker;
				target.BattleState = 1;
				WorldManager.Instance.CreatureChangeStance(target, 0);
			}
		}

		/// <summary>
		/// Returns strength rating of the two creatures, based on their CP.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static StrengthRating GetStrengthRating(MabiCreature attacker, MabiCreature target)
		{
			float ratio = target.CombatPower / attacker.CombatPower;

			if (ratio >= 3f)
				return StrengthRating.Boss;
			if (ratio >= 2f)
				return StrengthRating.Awful;
			if (ratio >= 1.4f)
				return StrengthRating.Strong;
			if (ratio >= 1f)
				return StrengthRating.Normal;
			if (ratio >= 0.8f)
				return StrengthRating.Weak;

			return StrengthRating.Weakest;
		}

		/// <summary>
		/// Reduces given damage if Mana Shield is active and returns the total
		/// mana damage. Also cancels Mana Shield if Mana is insufficent and
		/// reduces mana on target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <returns></returns>
		public static float DealManaDamage(MabiCreature target, ref float damage)
		{
			if (!target.Has(CreatureConditionA.ManaShield))
				return 0;

			var skill = target.GetSkill(SkillConst.ManaShield);
			if (skill == null)
				return 0;

			// Var 1 = Efficiency
			var manaDamage = damage / skill.RankInfo.Var1;
			if (target.Mana >= manaDamage)
				damage = 0;
			else
			{
				damage -= (manaDamage - target.Mana) * skill.RankInfo.Var1;
				manaDamage = target.Mana;
			}

			target.Mana -= manaDamage;

			if (target.Mana <= 0)
				SkillManager.GetHandler(SkillConst.ManaShield).Stop(target, skill);

			return manaDamage;
		}

		/// <summary>
		/// Calculates and sets new position for creature, and returns a copy
		/// of the current coordinates, for later use.
		/// </summary>
		/// <param name="target">Creature to knock back</param>
		/// <param name="attacker">Creature that attacked</param>
		/// <param name="distance">Knock back distance</param>
		/// <returns>Position of creature, before the knock back</returns>
		public static MabiVertex KnockBack(MabiCreature target, MabiCreature attacker, int distance = 375)
		{
			var oldPos = target.GetPosition();
			var pos = WorldManager.CalculatePosOnLine(attacker, target, distance);
			target.SetPosition(pos.X, pos.Y);
			return oldPos;
		}
	}

	public enum StrengthRating
	{
		Weakest, Weak, Normal, Strong, Awful, Boss
	}
}
