// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aura.World.World
{
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

		public abstract bool FitIn(MabiItem item, out List<MabiItem> changed);

		/// <summary>
		/// Removes item from pocket.
		/// </summary>
		/// <param name="item"></param>
		public abstract bool Remove(MabiItem item);

		public abstract MabiItem GetItemAt(uint x, uint y);
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

		public override bool Remove(MabiItem item)
		{
			if (_items.Remove(item.Id))
			{
				this.ClearFromMap(item);
				return true;
			}

			return false;
		}

		public override bool PutItem(MabiItem item)
		{
			MabiItem collidingItem;

			for (byte y = 0; y <= _height - item.DataInfo.Height; ++y)
			{
				for (byte x = 0; x <= _width - item.DataInfo.Width; ++x)
				{
					if (_map[x, y] != null)
						continue;

					if (!this.TryGetCollidingItem(x, y, item, out collidingItem))
					{
						this.TryPutItem(item, x, y, out collidingItem);
						return true;
					}
				}
			}

			return false;
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			if (x > _width - 1 || y > _height - 1)
				return null;
			return _map[x, y];
		}

		public override bool FitIn(MabiItem item, out List<MabiItem> changed)
		{
			changed = new List<MabiItem>();

			// Try to fit item into stacks first.
			if (item.StackType == BundleType.Stackable)
			{
				foreach (var invItem in _items.Values)
				{
					// If same class or item is stack item of inventory item
					if (item.Info.Class == invItem.Info.Class || invItem.StackItem == item.Info.Class)
					{
						// If item fits into stack 100%
						if (invItem.Info.Amount + item.Info.Amount <= invItem.StackMax)
						{
							invItem.Info.Amount += item.Info.Amount;
							item.Info.Amount = 0;

							changed.Add(invItem);

							return true;
						}

						// If stack is not full
						if (invItem.Info.Amount < invItem.StackMax)
						{
							item.Info.Amount -= (ushort)(invItem.StackMax - invItem.Info.Amount);
							invItem.Info.Amount = invItem.StackMax;

							changed.Add(invItem);
						}
					}
				}
			}

			// Try to put it into an empty space.
			return this.PutItem(item);
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

		public override bool Remove(MabiItem item)
		{
			if (_item == item)
			{
				_item = null;
				return true;
			}

			return false;
		}

		public override bool PutItem(MabiItem item)
		{
			if (_item != null)
				return false;

			this.ForcePutItem(item);
			return true;
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _item;
		}

		public override bool FitIn(MabiItem item, out List<MabiItem> changed)
		{
			changed = null;
			return this.PutItem(item);
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

		public override bool Remove(MabiItem item)
		{
			return _items.Remove(item);
		}

		public override bool PutItem(MabiItem item)
		{
			this.ForcePutItem(item);
			return true;
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _items.FirstOrDefault();
		}

		public override bool FitIn(MabiItem item, out List<MabiItem> changed)
		{
			changed = null;
			return this.PutItem(item);
		}
	}
}
