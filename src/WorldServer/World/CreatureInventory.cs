// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Util;
using Aura.World.Network;

namespace Aura.World.World
{
	public class CreatureInventory
	{
		private const uint DefaultWidth = 6;
		private const uint DefaultHeight = 10;

		private MabiCreature _creature;
		private Dictionary<Pocket, InventoryPocket> _pockets;

		public IEnumerable<MabiItem> Items
		{
			get
			{
				foreach (var pocket in _pockets.Values)
				{
					foreach (var item in pocket.Items)
					{
						if (item == null)
							continue;

						yield return item;
					}
				}
			}
		}

		public CreatureInventory(MabiCreature creature)
		{
			_creature = creature;

			_pockets = new Dictionary<Pocket, InventoryPocket>();

			var width = (creature.RaceInfo != null ? creature.RaceInfo.InvWidth : DefaultWidth);
			var height = (creature.RaceInfo != null ? creature.RaceInfo.InvHeight : DefaultHeight);
			if (creature.RaceInfo == null)
				Logger.Warning("Race for creature '{0:X016}' not loaded before initializing inventory.", creature.Id);

			// Cursor, Inv, Equipment, Style
			this.Add(new InventoryPocketStack(Pocket.Temporary));
			this.Add(new InventoryPocketSingle(Pocket.Cursor));
			this.Add(new InventoryPocketNormal(Pocket.Inventory, width, height));
			this.Add(new InventoryPocketNormal(Pocket.PersonalInventory, width, height));
			this.Add(new InventoryPocketNormal(Pocket.VIPInventory, width, height));
			for (var i = Pocket.Face; i <= Pocket.Robe; ++i)
				this.Add(new InventoryPocketSingle(i));
			for (var i = Pocket.ArmorStyle; i <= Pocket.RobeStyle; ++i)
				this.Add(new InventoryPocketSingle(i));
		}

		private InventoryPocket this[Pocket pocket]
		{
			get
			{
				InventoryPocket result;
				_pockets.TryGetValue(pocket, out result);
				return result;
			}
		}

		public void Add(InventoryPocket inventoryPocket)
		{
			_pockets[inventoryPocket.Pocket] = inventoryPocket;
		}

		public bool Has(Pocket pocket)
		{
			return _pockets.ContainsKey(pocket);
		}

		public MabiItem GetItem(ulong itemId)
		{
			return this.Items.FirstOrDefault(a => a.Id == itemId);
		}

		public bool MoveItem(MabiItem item, Pocket target, byte targetX, byte targetY)
		{
			if (!this.Has(target))
				return false;

			var source = item.Pocket;
			var amount = item.Info.Amount;

			MabiItem collidingItem = null;
			if (!_pockets[target].TryPutItem(item, targetX, targetY, out collidingItem))
				return false;

			// If amount differs (item was added to stack)
			if (collidingItem != null && item.Info.Amount != amount)
			{
				Send.ItemAmount(_creature, collidingItem);

				// Left overs, update
				if (item.Info.Amount > 0)
				{
					Send.ItemAmount(_creature, item);
				}
				// All in, remove from cursor.
				else
				{
					_pockets[item.Pocket].Remove(item);
					Send.ItemRemove(_creature, item);
				}
			}
			else
			{
				// Remove the item from the source pocket
				_pockets[source].Remove(item);

				// Toss it in, it should be the cursor.
				if (collidingItem != null)
					_pockets[source].ForcePutItem(collidingItem);

				Send.ItemMoveInfo(_creature, item, source, collidingItem);
			}

			return true;
		}

		/// <summary>
		/// Tries to put item into pocket, sends ItemNew on success.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pocket"></param>
		/// <returns></returns>
		public bool PutItem(MabiItem item, Pocket pocket)
		{
			var success = _pockets[pocket].PutItem(item);
			if (success)
				Send.ItemInfo(_creature.Client, _creature, item);

			return success;
		}

		public void ForcePutItem(MabiItem item, Pocket pocket)
		{
			_pockets[pocket].ForcePutItem(item);
		}
	}

	public abstract class InventoryPocket
	{
		public Pocket Pocket { get; protected set; }
		public abstract IEnumerable<MabiItem> Items { get; }

		public InventoryPocket(Pocket pocket)
		{
			this.Pocket = pocket;
		}

		/// <summary>
		/// Attempts to put item at the coordinates. If another item is
		/// in the new item's space it's returned in colliding.
		/// Returns whether the attempt was successful.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="targetX"></param>
		/// <param name="targetY"></param>
		/// <param name="colliding"></param>
		/// <returns></returns>
		public abstract bool TryPutItem(MabiItem item, byte targetX, byte targetY, out MabiItem colliding);

		/// <summary>
		/// Puts item into the pocket, at the location it has.
		/// </summary>
		/// <param name="item"></param>
		public abstract void ForcePutItem(MabiItem item);

		public abstract bool PutItem(MabiItem item);

		/// <summary>
		/// Removes item from pocket.
		/// </summary>
		/// <param name="item"></param>
		public abstract void Remove(MabiItem item);
	}

	/// <summary>
	/// Normal inventory with a specific size.
	/// </summary>
	public class InventoryPocketNormal : InventoryPocket
	{
		private Dictionary<ulong, MabiItem> _items;
		private MabiItem[,] _map;
		private uint _width, _height;

		public InventoryPocketNormal(Pocket pocket, uint width, uint height)
			: base(pocket)
		{
			_items = new Dictionary<ulong, MabiItem>();
			_map = new MabiItem[width, height];

			_width = width;
			_height = height;
		}

		public override IEnumerable<MabiItem> Items
		{
			get
			{
				foreach (var item in _items.Values)
					yield return item;
			}
		}

		public override bool TryPutItem(MabiItem newItem, byte targetX, byte targetY, out MabiItem collidingItem)
		{
			collidingItem = null;

			if (targetX + newItem.DataInfo.Width > _width || targetY + newItem.DataInfo.Height > _height)
				return false;

			this.TryGetCollidingItem(targetX, targetY, newItem, out collidingItem);

			if (collidingItem != null && ((collidingItem.StackType == BundleType.Sac && (collidingItem.StackItem == newItem.Info.Class || collidingItem.StackItem == newItem.StackItem)) || (newItem.StackType == BundleType.Stackable && newItem.Info.Class == collidingItem.Info.Class)))
			{
				if (collidingItem.Info.Amount < collidingItem.StackMax)
				{
					var diff = (ushort)(collidingItem.StackMax - collidingItem.Info.Amount);

					collidingItem.Info.Amount += Math.Min(diff, newItem.Info.Amount);
					newItem.Info.Amount -= Math.Min(diff, newItem.Info.Amount);

					return true;
				}
			}

			if (collidingItem != null)
			{
				_items.Remove(collidingItem.Id);
				collidingItem.Move(newItem.Pocket, newItem.Info.X, newItem.Info.Y);
				this.ClearFromMap(collidingItem);
			}

			_items.Add(newItem.Id, newItem);
			newItem.Move(this.Pocket, targetX, targetY);
			this.AddToMap(newItem);

			return true;
		}

		protected void AddToMap(MabiItem item)
		{
			for (var x = item.Info.X; x < item.Info.X + item.DataInfo.Width; ++x)
			{
				for (var y = item.Info.Y; y < item.Info.Y + item.DataInfo.Height; ++y)
				{
					_map[x, y] = item;
				}
			}
			//TestMap();
		}

		protected void ClearFromMap(MabiItem item)
		{
			int count = 0;
			int max = item.DataInfo.Width * item.DataInfo.Height;

			for (var x = 0; x < _width; ++x)
			{
				for (var y = 0; y < _height; ++y)
				{
					if (_map[x, y] == item)
					{
						_map[x, y] = null;
						if (++count >= max)
							return;
					}
				}
			}
			//TestMap();
		}

		protected void TestMap()
		{
			var items = Items.ToList();
			for (int i = 0; i < items.Count; ++i)
			{
				Console.WriteLine((i + 1) + ") " + items[i].DataInfo.Name);
				items[i].OptionInfo.Price = (uint)i + 1;
			}
			for (var y = 0; y < _height; ++y)
			{
				for (var x = 0; x < _width; ++x)
				{
					if (_map[x, y] != null)
						Console.Write(_map[x, y].OptionInfo.Price + " ");
					else
						Console.Write(0 + " ");
				}
				Console.WriteLine("|");
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Checks if there's a colliding item in the target space.
		/// Returns true if a colliding item was found.
		/// </summary>
		/// <param name="targetX"></param>
		/// <param name="targetY"></param>
		/// <param name="item"></param>
		/// <param name="collidingItem"></param>
		/// <returns></returns>
		protected bool TryGetCollidingItem(uint targetX, uint targetY, MabiItem item, out MabiItem collidingItem)
		{
			collidingItem = null;

			for (var x = targetX; x < targetX + item.DataInfo.Width; ++x)
			{
				for (var y = targetY; y < targetY + item.DataInfo.Height; ++y)
				{
					if (x > _width - 1 || y > _height - 1)
						continue;

					if (_map[x, y] != null)
					{
						collidingItem = _map[x, y];
						return true;
					}
				}
			}

			return false;
		}

		public override void ForcePutItem(MabiItem item)
		{
			this.AddToMap(item);
			_items.Add(item.Id, item);
			item.Pocket = this.Pocket;
		}

		public override void Remove(MabiItem item)
		{
			if (_items.Remove(item.Id))
				this.ClearFromMap(item);
		}

		public override bool PutItem(MabiItem item)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Pocket only holding a single item (eg Equipment).
	/// </summary>
	public class InventoryPocketSingle : InventoryPocket
	{
		private MabiItem _item;

		public InventoryPocketSingle(Pocket pocket)
			: base(pocket)
		{
		}

		public override IEnumerable<MabiItem> Items
		{
			get
			{
				yield return _item;
			}
		}

		public override bool TryPutItem(MabiItem item, byte targetX, byte targetY, out MabiItem collidingItem)
		{
			collidingItem = null;
			if (_item != null)
			{
				collidingItem = _item;
				collidingItem.Move(item.Pocket, item.Info.X, item.Info.Y);
			}

			_item = item;
			_item.Move(this.Pocket, 0, 0);

			return true;
		}

		public override void ForcePutItem(MabiItem item)
		{
			_item = item;
			_item.Move(this.Pocket, 0, 0);
		}

		public override void Remove(MabiItem item)
		{
			if (_item == item)
				_item = null;
		}

		public override bool PutItem(MabiItem item)
		{
			if (_item != null)
				return false;

			this.ForcePutItem(item);
			return true;
		}
	}

	/// <summary>
	/// Pocket that holds an infinite number of items.
	/// </summary>
	public class InventoryPocketStack : InventoryPocket
	{
		private List<MabiItem> _items;

		public InventoryPocketStack(Pocket pocket)
			: base(pocket)
		{
			_items = new List<MabiItem>();
		}

		public override IEnumerable<MabiItem> Items
		{
			get
			{
				foreach (var item in _items)
					yield return item;
			}
		}

		public override bool TryPutItem(MabiItem item, byte targetX, byte targetY, out MabiItem colliding)
		{
			colliding = null;
			_items.Add(item);
			return true;
		}

		public override void ForcePutItem(MabiItem item)
		{
			_items.Add(item);
			item.Move(this.Pocket, 0, 0);
		}

		public override void Remove(MabiItem item)
		{
			_items.Remove(item);
		}

		public override bool PutItem(MabiItem item)
		{
			this.ForcePutItem(item);
			return true;
		}
	}
}
