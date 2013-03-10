// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using Aura.Msgr.Network;

namespace Aura.Msgr.Chat
{
	public class Contact
	{
		public MsgrClient Client { get; set; }

		public uint Id { get; set; }
		public string Name { get; set; }
		public string Server { get; set; }
		public string FullName { get; set; }

		public List<Note> Notes = new List<Note>();
	}
}
