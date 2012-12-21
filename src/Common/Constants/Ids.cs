// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Common.Constants
{
	public static class Id
	{
		// These ids are used as packet ids if there's no specific
		// target for the packet, or in situations where you're technically not
		// "identifiable" by a character id.
		public const ulong Login = 0x1000000000000010;
		public const ulong World = 0x1000000000000001;
		public const ulong Broadcast = 0x3000000000000000;

		// Id range starts
		public const ulong Characters = 0x0010000000000001;
		public const ulong Pets = 0x0010010000000001;
		public const ulong Partners = 0x0010030000000001;
		public const ulong Items = 0x0050000000000001;
		public const ulong TmpItems = 0x0050F00000000001;
		public const ulong Props = 0x00A1000100000000;
	}
}
