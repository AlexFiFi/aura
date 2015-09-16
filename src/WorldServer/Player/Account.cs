// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.World.Scripting;

namespace Aura.World.Player
{
	public class Account
	{
		public string Name { get; set; }
		public string Password { get; set; }

		public byte Authority { get; set; }

		public DateTime LastLogin { get; set; }
		public string LastIp { get; set; }

		public string BannedReason { get; set; }
		public DateTime BannedExpiration { get; set; }

		public bool LoggedIn { get; set; }

		public List<MabiCharacter> Characters = new List<MabiCharacter>();
		public List<MabiPet> Pets = new List<MabiPet>();

		public ScriptingVariables Vars { get; protected set; }
        
        public AccountBankManager BankManager { get; set; }

		public Account()
		{
			this.Vars = new ScriptingVariables();
		}
	}
}
