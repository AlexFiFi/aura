// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Network;
using Common.Constants;

namespace Common.World
{
	public class MabiStatMod
	{
		public UInt32 ModId;
		public Stat StatusAttribute;
		public DateTime TimeStarted;
		public UInt32 Milliseconds;
		public float ChangePerSecond;
		public bool NeverEnding;
		public float MaxValue;

		private static UInt32 _statModIndex = 30;

		public MabiStatMod(Stat status, UInt32 milliseconds, float changepersecond, float maxValue)
		{
			ModId = assignStatModId();
			StatusAttribute = status;
			TimeStarted = DateTime.Now;
			Milliseconds = milliseconds;
			ChangePerSecond = changepersecond;
			NeverEnding = false;
			MaxValue = maxValue;
		}

		public MabiStatMod(Stat status, float changepersecond, float maxValue)
		{
			ModId = assignStatModId();
			StatusAttribute = status;
			TimeStarted = DateTime.Now;
			Milliseconds = 0xFFFFFFFF;
			ChangePerSecond = changepersecond;
			NeverEnding = true;
			MaxValue = maxValue;
		}

		public float GetCurrentChange()
		{
			DateTime nowTime = DateTime.Now;
			TimeSpan timePassed = nowTime.Subtract(TimeStarted);

			if ((uint)timePassed.TotalMilliseconds > Milliseconds)
				return ChangePerSecond * (float)(Milliseconds / 1000);
			else
				return ChangePerSecond * (float)(timePassed.TotalMilliseconds / 1000);
		}

		public void Terminate()
		{
			Milliseconds = (UInt32)DateTime.Now.Subtract(TimeStarted).TotalMilliseconds;
			NeverEnding = false;
		}

		public UInt32 GetMillisecondsLeft()
		{
			DateTime nowTime = DateTime.Now;
			TimeSpan timePassed = nowTime.Subtract(TimeStarted);

			if (timePassed.Milliseconds > Milliseconds)
				return 0;
			else
				return Milliseconds - (uint)timePassed.Milliseconds;
		}

		public void AddData(MabiPacket packet)
		{
			packet.PutInt(ModId);
			packet.PutFloat(ChangePerSecond);
			packet.PutInt(GetMillisecondsLeft());
			packet.PutInt((UInt32)StatusAttribute);
			packet.PutByte(0); // ?
			packet.PutFloat(MaxValue); // Max Value
		}

		private UInt32 assignStatModId()
		{
			return _statModIndex++;
		}
	}
}
