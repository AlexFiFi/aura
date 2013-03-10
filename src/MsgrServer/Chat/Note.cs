// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Msgr.Chat
{
	public class Note
	{
		public ulong Id { get; set; }
		public uint ContactId { get; set; }
		public string Sender { get; set; }
		public string Message { get; set; }
		public DateTime Time { get; set; }
		public bool Read { get; set; }
	}
}
