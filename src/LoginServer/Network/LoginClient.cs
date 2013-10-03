// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Login.Database;
using Aura.Shared.Network;

namespace Aura.Login.Network
{
	public class LoginClient : Client
	{
		public Account Account { get; set; }
	}
}
