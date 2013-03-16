// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Shared.Const;
using Aura.Shared.Network;

namespace Aura.World.World
{
	public class MabiStatMod
	{
		private static uint _statModIndex = 30;

		public uint ModId;
		public Stat Stat;
		public DateTime Start;
		public uint Duration;
		public float ChangePerSecond;
		public float MaxValue;

		public uint TimeLeft
		{
			get
			{
				var now = DateTime.Now;
				var passed = now.Subtract(this.Start);

				if (passed.Milliseconds > this.Duration)
					return 0;
				else
					return this.Duration - (uint)passed.Milliseconds;
			}
		}

		public MabiStatMod(Stat status, uint duration, float changePerSecond, float maxValue)
		{
			this.ModId = _statModIndex++;
			this.Stat = status;
			this.Start = DateTime.Now;
			this.Duration = duration;
			this.ChangePerSecond = changePerSecond;
			this.MaxValue = maxValue;
		}

		public MabiStatMod(Stat status, float changepersecond, float maxValue)
			: this(status, uint.MaxValue, changepersecond, maxValue)
		{
		}

		public void Terminate()
		{
			this.Duration = 0;
		}

		public void AddToPacket(MabiPacket packet)
		{
			packet.PutInt(this.ModId);
			packet.PutFloat(this.ChangePerSecond);
			packet.PutInt(this.TimeLeft);
			packet.PutInt((uint)this.Stat);
			packet.PutByte(0); // ?
			packet.PutFloat(this.MaxValue);
		}
	}
}
