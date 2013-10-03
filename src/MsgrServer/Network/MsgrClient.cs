// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Msgr.Chat;
using Aura.Shared.Network;

namespace Aura.Msgr.Network
{
	public class MsgrClient : Client
	{
		public Contact Contact { get; set; }

		public override void Encode(byte[] raw)
		{ }

		public override void Kill()
		{
			if (this.State == ClientState.LoggedIn)
				Manager.Instance.RemoveContact(this.Contact);

			base.Kill();
		}
	}
}
