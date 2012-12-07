// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Common.Constants
{
	// No enum because we don't need type safety and string conversion,
	// but something that can be passed to a function. (No casts ftw.)
	public static class Effect
	{
		public static readonly uint Revive = 4;

		/// <summary>
		/// Used when picking up a dungeon key.
		/// int:itemId, int:?, int:?, int:?
		/// </summary>
		public static readonly uint PickUpKey = 8;

		/// <summary>
		/// Logged values: "", "healing", "flashing"
		/// string:?
		/// </summary>
		public static readonly uint SkillInit = 11;

		/// <summary>
		/// Logged values: "healing"
		/// string:?
		/// </summary>
		public static readonly uint Healing = 12;

		/// <summary>
		/// Logged values: "healing_stack"
		/// string:?, byte:amount?, byte:0
		/// </summary>
		public static readonly uint StackUpdate = 13;

		/// <summary>
		/// Logged values: "healing_firstaid", "healing", "healing_phoenix"
		/// string:?, long:targetCreatureId
		/// Healing Effects?
		/// </summary>
		public static readonly uint HealingMotion = 14;

		/// <summary>
		/// Used with special pets like Ice Dragon on spawn, and when reviving.
		/// long:creatureId, byte:0, byte:0
		/// </summary>
		public static readonly uint SomePetEffect = 19;

		/// <summary>
		/// White flash.
		/// int:duration, int:0
		/// </summary>
		public static readonly uint ScreenFlash = 27;

		/// <summary>
		/// int:region, float:x, float:y, byte:1
		/// </summary>
		public static readonly uint PetSpawn = 29;

		/// <summary>
		/// Chef Owl
		/// </summary>
		public static readonly uint ChefOwl = 121;

		/// <summary>
		/// According to older logs, this should've been 121.
		/// TODO: Check if it has changed at some point.
		/// </summary>
		public static readonly uint ManaShield = 122;
	}
}
