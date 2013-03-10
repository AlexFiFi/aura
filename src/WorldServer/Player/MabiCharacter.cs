// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Data;
using Aura.Shared.Util;

namespace Aura.World.Player
{
	/// <summary>
	/// Characters, Pets
	/// </summary>
	public class MabiCharacter : MabiPC
	{
		public MabiCharacter()
		{
			this.CreationTime = DateTime.Now;
			this.LevelingEnabled = true;
		}

		public override void CalculateBaseStats()
		{
			base.CalculateBaseStats();

			this.Height = (1.0f / 7.0f * (this.Age - 10.0f));

			var ageInfo = MabiData.StatsBaseDb.Find(this.Race, this.Age);
			if (ageInfo == null)
			{
				Logger.Warning("Unable to find age info for race '" + this.Race.ToString() + "', age '" + this.Age.ToString() + "'.");
				return;
			}

			this.LifeMaxBase = ageInfo.Life;
			this.Life = ageInfo.Life;

			this.ManaMaxBase = ageInfo.Mana;
			this.Mana = ageInfo.Mana;

			this.StaminaMaxBase = ageInfo.Stamina;
			this.Stamina = ageInfo.Stamina;

			this.StrBase = ageInfo.Str;
			this.IntBase = ageInfo.Int;
			this.DexBase = ageInfo.Dex;
			this.WillBase = ageInfo.Will;
			this.LuckBase = ageInfo.Luck;
		}
	}
}
