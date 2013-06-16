// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;

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

		public abstract void AddToPacket(MabiPacket packet);

		public MabiEntity()
		{

		}

		/// <summary>
		/// Drops the item at the entity's position. If randomize is true, AND there is already another item at those coordinates, a new random location
		/// around the entity will be chosen randomly
		/// Raises CreatureDropItem and CreatureItemAction
		/// </summary>
		/// <param name="item"></param>
		/// <param name="randomize"></param>
		public void DropItem(MabiItem item, bool randomize = true)
		{
			var pos = this.GetPosition();

			uint x = pos.X, y = pos.Y;

			if (randomize)
			{
				var rand = RandomProvider.Get();
				var inrange = WorldManager.Instance.GetEntitiesInRange(this, 200).Where(e => e is MabiItem).Select(e => e.GetPosition()).ToList();

				while (inrange.Exists(e => e.X == x && e.Y == y))
				{
					x = (uint)(pos.X + rand.Next(-100, 101));
					y = (uint)(pos.Y + rand.Next(-100, 101));
				}
			}

			this.DropItem(item, this.Region, x, y);
		}

		/// <summary>
		/// Drops the item at the specified location.
		/// Raises CreatureDropItem and CreatureItemAction
		/// </summary>
		/// <param name="item"></param>
		/// <param name="region"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void DropItem(MabiItem item, uint region, uint x, uint y)
		{
			item.Info.Region = region;
			item.Info.X = x;
			item.Info.Y = y;
			item.Info.Pocket = (byte)Pocket.None;
			item.DisappearTime = DateTime.Now.AddSeconds((int)Math.Max(60, (item.OptionInfo.Price / 100) * 60));

			WorldManager.Instance.AddItem(item);
			EventManager.Instance.CreatureEvents.OnCreatureDropItem(this, new ItemUpdateEventArgs(item));
			EventManager.Instance.CreatureEvents.OnCreatureItemAction(this, new ItemActionEventArgs(item.Info.Class));
		}
	}
}
