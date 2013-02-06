// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Common.World
{
	public class MabiCard
	{
		// Not actually saved in the database, just used on the login
		// server temporarily.
		private static ulong _index = Constants.Id.Cards;

		public ulong Id;
		public uint Race;
		public bool InDatabase;

		public MabiCard(uint type)
		{
			this.InDatabase = false;
			this.Id = ++_index;
			this.Race = type;
		}

		public MabiCard(ulong id, uint type)
		{
			this.InDatabase = true;
			this.Id = id;
			this.Race = type;
		}
	}
}
