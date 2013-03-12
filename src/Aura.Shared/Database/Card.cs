// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Shared.Database
{
	public class Card
	{
		public ulong Id { get; set; }
		public uint Type { get; set; }
		public uint Race { get; set; }

		public Card()
		{ }

		public Card(uint type, uint race)
		{
			this.Type = type;
			this.Race = race;
		}

		public Card(ulong id, uint type, uint race)
			: this(type, race)
		{
			this.Id = id;
		}
	}
}
