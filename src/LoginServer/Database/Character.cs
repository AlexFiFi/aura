﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Aura.Login.Database
{
	public class Character
	{
		public ulong Id { get; set; }
		public string Name { get; set; }
		public string Server { get; set; }

		public uint Race { get; set; }
		public byte SkinColor { get; set; }
		public byte Eye { get; set; }
		public byte EyeColor { get; set; }
		public byte Mouth { get; set; }
		public float Height { get; set; }
		public float Weight { get; set; }
		public float Upper { get; set; }
		public float Lower { get; set; }
		public uint Color1 { get; set; }
		public uint Color2 { get; set; }
		public uint Color3 { get; set; }
		public byte Age { get; set; }

		public uint Region { get; set; }
		public uint X { get; set; }
		public uint Y { get; set; }

		public ushort AP { get; set; }
		public float Life { get; set; }
		public float Mana { get; set; }
		public float Stamina { get; set; }
		public float Str { get; set; }
		public float Int { get; set; }
		public float Dex { get; set; }
		public float Will { get; set; }
		public float Luck { get; set; }
		public ushort Defense { get; set; }
		public float Protection { get; set; }

		public CharacterType Type { get; set; }

		/// <summary>
		/// Time at which the character may be deleted for good.
		/// </summary>
		public DateTime DeletionTime { get; set; }

		/// <summary>
		/// Deletion state of the character, based on DeletionTime.
		/// 0: Normal, 1: Recoverable, 2: DeleteReady, 3: ToBeDeleted
		/// </summary>
		public DeletionFlag DeletionFlag
		{
			get
			{
				if (this.DeletionTime == DateTime.MaxValue)
					return DeletionFlag.Delete;
				else if (this.DeletionTime <= DateTime.MinValue)
					return DeletionFlag.Normal;
				else if (this.DeletionTime >= DateTime.Now)
					return DeletionFlag.Recover;
				else
					return DeletionFlag.Ready;
			}
		}

		public Character(CharacterType type)
		{
			this.Type = type;
			this.Weight = 1;
			this.Upper = 1;
			this.Lower = 1;

			this.Life = 10;
			this.Mana = 10;
			this.Stamina = 100;
			this.Str = 10;
			this.Int = 10;
			this.Dex = 10;
			this.Will = 10;
			this.Luck = 10;
		}
	}

	public enum CharacterType { Character, Pet, Partner }
	public enum DeletionFlag { Normal, Recover, Ready, Delete }
}
