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

		// 00XX<word:region><word:area><word:id>
		public const ulong Props = 0x00A1000000000000;
		public const ulong AreaEvents = 0x00B0000000000000;

		public const ulong Parties = 0x0040000000000001;

		// Quests is probably 0x0060000000000001, but we'll leave some space
		// between quests and items (quest items), just in case.
		public const ulong Quests = 0x006000F000000001;
		public const ulong QuestsTmp = 0x0060F00000000001;
		public const ulong QuestItemOffset = 0x0010000000000000;

		public const ulong Instances = 0x0100000000000001;
	}
}
