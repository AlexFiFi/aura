// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Constants;
using Common.Tools;
using MabiNatives;
using Common.Events;

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

			var ageInfo = MabiData.AgeDb.Find((byte)(this.IsHuman() ? 0 : (this.IsElf() ? 1 : 2)), this.Age);
			if (ageInfo == null)
			{
				Logger.Warning("Unable to find age info for " + this.Race.ToString() + " " + this.Age.ToString() + ".");
				return;
			}

			this.LifeMaxBase = ageInfo.BaseStats["life"];
			this.Life = ageInfo.BaseStats["life"];

			this.ManaMaxBase = ageInfo.BaseStats["mana"];
			this.Mana = ageInfo.BaseStats["mana"];

			this.StaminaMaxBase = ageInfo.BaseStats["stamina"];
			this.Stamina = ageInfo.BaseStats["stamina"];

			this.StrBase = ageInfo.BaseStats["str"];
			this.IntBase = ageInfo.BaseStats["int"];
			this.DexBase = ageInfo.BaseStats["dex"];
			this.WillBase = ageInfo.BaseStats["will"];
			this.LuckBase = ageInfo.BaseStats["luck"];
		}
	}
}
