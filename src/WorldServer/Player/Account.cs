// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.World.Player
{
	public class Account
	{
		public bool LoggedIn;
		public string Username;

		public byte Authority;

		public string Userpass;

		public DateTime Creation = DateTime.Now;
		public DateTime LastLogin;
		public string LastIp;

		public string BannedReason = "";
		public DateTime BannedExpiration;

		public List<MabiCharacter> Characters = new List<MabiCharacter>();
		public List<MabiPet> Pets = new List<MabiPet>();
		public List<MabiCard> CharacterCards = new List<MabiCard>();
		public List<MabiCard> PetCards = new List<MabiCard>();

		public Account()
		{
		}
	}
}
