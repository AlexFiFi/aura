// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Network;

namespace Common.World
{
	public enum EntityType
	{
		Undefined, Character, Pet, Item, NPC, Prop
	}

	public abstract class MabiEntity
	{
		public virtual ulong Id { get; set; }

		public virtual uint Region { get; set; }
		public abstract MabiVertex GetPosition();
		public readonly MabiVertex PrevPosition = new MabiVertex(0, 0);

		public DateTime DisappearTime = DateTime.MinValue;

		public abstract EntityType EntityType { get; }
		public abstract ushort DataType { get; }

		public abstract void AddEntityData(MabiPacket packet);
	}
}
