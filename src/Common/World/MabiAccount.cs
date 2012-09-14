// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Common.World
{
	public class MabiAccount
	{
		public bool LoggedIn;
		public string Username;

		public byte Authority;

		public string Userpass;

		public DateTime Creation = DateTime.MinValue;
		public DateTime LastLogin = DateTime.MinValue;
		public string LastIp;

		public byte Banned;
		public string BannedReason;
		public DateTime BannedTime = DateTime.MinValue;
		public DateTime BannedExpiration = DateTime.MinValue;

		public List<MabiCharacter> Characters = new List<MabiCharacter>();
		public List<MabiPet> Pets = new List<MabiPet>();
		public List<MabiCard> CharacterCards = new List<MabiCard>();
		public List<MabiCard> PetCards = new List<MabiCard>();

		public MabiAccount()
		{
		}
	}
}
