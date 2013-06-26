// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Shared.Network;

namespace Aura.World.World
{
	public enum EntityType { Undefined, Character, Pet, Item, NPC, Prop }

	public abstract class MabiEntity : IDisposable
	{
		private bool _disposed = false;

		/// <summary>
		/// Derived classes should override this method to hook their events into the HookableEvents instance.
		/// It will be automatically called. Derived classes should ALWAYS call their base method.
		/// </summary>
		protected virtual void HookUp()
		{
		}

		/// <summary>
		/// UNHOOK YOUR EVENTS HERE!
		/// </summary>
		public virtual void Dispose()
		{
			if (!_disposed)
				_disposed = true;
		}

		public virtual ulong Id { get; set; }

		public virtual uint Region { get; set; }
		public abstract MabiVertex GetPosition();
		public readonly MabiVertex PrevPosition = new MabiVertex(0, 0);

		public DateTime DisappearTime = DateTime.MinValue;

		public abstract EntityType EntityType { get; }
		public abstract ushort DataType { get; }

		public MabiEntity()
		{
		}
	}
}
