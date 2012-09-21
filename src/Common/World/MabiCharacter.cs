// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Constants;
using Common.Tools;
using Common.Events;
using Common.Data;

namespace Common.World
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

			this.LifeMaxBase = ageInfo.BaseStats["Life"];
			this.Life = ageInfo.BaseStats["Life"];

			this.ManaMaxBase = ageInfo.BaseStats["Mana"];
			this.Mana = ageInfo.BaseStats["Mana"];

			this.StaminaMaxBase = ageInfo.BaseStats["Stamina"];
			this.Stamina = ageInfo.BaseStats["Stamina"];

			this.StrBase = ageInfo.BaseStats["Str"];
			this.IntBase = ageInfo.BaseStats["Int"];
			this.DexBase = ageInfo.BaseStats["Dex"];
			this.WillBase = ageInfo.BaseStats["Will"];
			this.LuckBase = ageInfo.BaseStats["Luck"];
		}
	}
}
