// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.Shared.Const
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
		/// Logged values: "", "healing", "flashing", "thunder", "icespear"
		/// string:?
		/// </summary>
		public static readonly uint SkillInit = 11;

		/// <summary>
		/// Logged values: "healing", "thunder", "icespear"
		/// string:?
		/// </summary>
		public static readonly uint HoldMagic = 12;

		/// <summary>
		/// Logged values: "healing_stack"
		/// string:?, byte:amount?, byte:0
		/// </summary>
		public static readonly uint StackUpdate = 13;

		/// <summary>
		/// Logged values: "healing_firstaid", "healing", "healing_phoenix", "thunder"
		/// string:?, long:targetCreatureId
		/// Healing Effects?
		/// </summary>
		public static readonly uint UseMagic = 14;

		/// <summary>
		/// b:type, i|s:song, i:?, si:?, i:?, b:quality?, b:instrument, b:?, b:?, b:loops
		/// </summary>
		public static readonly uint PlayMusic = 17;

		/// <summary>
		/// On music complete
		/// </summary>
		public static readonly uint StopMusic = 18;

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
		/// int:region, float:x, float:y, byte:type (0=monster,1=pet,2=pet_despawn,3=monster_despawn,4=golem,5=golem_despawn)
		/// </summary>
		public static readonly uint Spawn = 29;

		/// <summary>
		/// Sent by thunder
		/// </summary>
		public static readonly uint Lightningbolt = 30;

		/// <summary>
		/// Fireball in the air. int:Region, float:fromx, float:fromy, float:tox, float:toy, int:time, byte:0
		/// </summary>
		public static readonly uint FireballFly = 39;

		/// <summary>
		/// The frozen effect of Ice Spear
		/// </summary>
		public static readonly uint IcespearFreeze = 65;

		public static readonly uint IcespearBoom = 66;

		/// <summary>
		/// Chef Owl
		/// </summary>
		public static readonly uint ChefOwl = 121;

		/// <summary>
		/// According to older logs, this should've been 121.
		/// </summary>
		public static readonly uint ManaShield = 122;

		/// <summary>
		/// Parameters: None
		/// </summary>
		public static readonly uint AwakeningOfLight1 = 173;

		/// <summary>
		/// Parameters: None
		/// </summary>
		public static readonly uint AwakeningOfLight2 = 176;

		public static readonly uint SupportShot = 240;

		/// <summary>
		/// ?
		/// </summary>
		public static readonly uint Casting = 247;

		/// <summary>
		/// Shadow Bunshin casting, clones, etc.
		/// </summary>
		public static readonly uint ShadowBunshin = 262;

		/// <summary>
		/// Used in thunder's final stage
		/// </summary>
		public static readonly uint Thunderbolt = 297;

		/// <summary>
		/// Cherry blossoms falling onto the character.
		/// byte:1|0 (on/off)
		/// </summary>
		public static readonly uint CherryBlossoms = 345;
	}

	public enum SpawnEffect : byte
	{
		Monster = 0,
		Pet = 1,
		PetDespawn = 2,
		MonsterDespawn = 3,
		Golem = 4,
		GolemDespawn = 5,
		//GolemDespawn = 6, // ?
		//Demi? = 7, // ?
		//Demi? = 8, // ?
	}
}
