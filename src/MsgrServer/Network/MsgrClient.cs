// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Network;
using Msgr.Chat;

namespace Msgr.Network
{
	public class MsgrClient : Client
	{
		public Contact Contact { get; set; }

		public override void CheckEncoding(byte[] raw)
		{
		}

		public override void Kill()
		{
			if (this.State == SessionState.LoggedIn)
				Manager.Instance.RemoveContact(this.Contact);

			base.Kill();
		}
	}
}
