// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Common.World
{
	public static class Authority
	{
		public static byte Player = 0;
		public static byte VIP = 1;
		public static byte GameMaster = 50;
		public static byte Admin = 99;
	}

	public class MabiAccount
	{
		public bool LoggedIn;
		public string Username;

		public byte Authority;

		public string Userpass;

		public DateTime Creation = DateTime.Now;
		public DateTime LastLogin;
		public string LastIp;

		public string BannedReason;
		public DateTime BannedExpiration;

		public List<MabiCharacter> Characters = new List<MabiCharacter>();
		public List<MabiPet> Pets = new List<MabiPet>();
		public List<MabiCard> CharacterCards = new List<MabiCard>();
		public List<MabiCard> PetCards = new List<MabiCard>();

		public MabiAccount()
		{
		}
	}
}
