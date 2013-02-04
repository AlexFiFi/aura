// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace World.World
{
	public enum AttackResult { None, Stunned, OutOfRange, Okay }

	public static class MabiCombat
	{
		private enum CombatStunNormalSelf { VeryFast = 450, Fast = 520, Normal = 600, Slow = 800, VerySlow = 1000 }
		private enum CombatStunNormalTarget { VeryFast = 1200, Fast = 1700, Normal = 2000, Slow = 2800, VerySlow = 3000 }

		private enum CombatStunKnockbackSelf { VeryFast = 1200, Fast = 2500, Normal = 2500, Slow = 2500, VerySlow = 2500 }
		private enum CombatStunKnockbackTarget { VeryFast = 1500, Fast = 2000, Normal = 2000, Slow = 2800, VerySlow = 3000 }

		private static uint _actionId = 1;

		/// <summary>
		/// Returns a new usable action id.
		/// </summary>
		public static uint ActionId { get { return _actionId++; } }

		public static ushort CalculateStunSource(int weaponSpeed, bool knockback)
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

		public static ushort CalculateStunTarget(int weaponSpeed, bool knockback)
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
	}
}
