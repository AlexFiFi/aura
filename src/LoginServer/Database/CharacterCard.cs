// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Login.Database
{
	public class Card
	{
		public ulong Id { get; set; }
		public uint Type { get; set; }

		public Card()
		{ }

		public Card(uint type)
		{
			this.Type = type;
		}

		public Card(ulong id, uint type)
		{
			this.Id = id;
			this.Type = type;
		}
	}
}
