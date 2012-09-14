// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Constants;

namespace Common.World
{
	public class MabiPet : MabiPC
	{
		public MabiPet()
		{
			this.CreationTime = DateTime.Now;
			this.LevelingEnabled = true;
		}

		public override EntityType EntityType
		{
			get { return World.EntityType.Pet; }
		}

		public override void CalculateBaseStats()
		{
			base.CalculateBaseStats();

			this.Height = 0.7f;
		}
	}
}
