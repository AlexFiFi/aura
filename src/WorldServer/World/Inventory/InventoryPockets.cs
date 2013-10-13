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
		public abstract bool TryAdd(MabiItem item, byte targetX, byte targetY, out MabiItem colliding);

		/// <summary>
		/// Puts item into the pocket, at the location it has.
		/// </summary>
		/// <param name="item"></param>
		public abstract void ForceAdd(MabiItem item);

		public abstract bool Add(MabiItem item);

		/// <summary>
		/// Fills stacks that take this item. Returns true if item has been
		/// was completely added to stacks/sacs.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="changed"></param>
		/// <returns></returns>
		public abstract bool FillStacks(MabiItem item, out List<MabiItem> changed);

		/// <summary>
		/// Removes item from pocket.
		/// </summary>
		/// <param name="item"></param>
		public abstract bool Remove(MabiItem item);

		/// <summary>
		/// Returns whether the item exists in this pocket.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public abstract bool Has(MabiItem item);

		/// <summary>
		/// Returns the item at the location, or null.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public abstract MabiItem GetItemAt(uint x, uint y);

		public abstract uint Remove(uint itemId, uint amount, ref List<MabiItem> changed);

		public abstract uint Count(uint itemId);
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

		public override bool TryAdd(MabiItem newItem, byte targetX, byte targetY, out MabiItem collidingItem)
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

		public override void ForceAdd(MabiItem item)
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

		public override bool Add(MabiItem item)
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
						this.TryAdd(item, x, y, out collidingItem);
						return true;
					}
				}
			}

			return false;
		}

		public void TestMap()
		{
			var items = Items.ToList();
			for (int i = 0; i < items.Count; ++i)
			{
				Console.WriteLine((i + 1) + ") " + items[i].DataInfo.Name);
				items[i].OptionInfo.DucatPrice = (uint)i + 1;
			}
			for (var y = 0; y < _height; ++y)
			{
				for (var x = 0; x < _width; ++x)
				{
					if (_map[x, y] != null)
						Console.Write(_map[x, y].OptionInfo.DucatPrice.ToString().PadLeft(2) + " ");
					else
						Console.Write(" 0" + " ");
				}
				Console.WriteLine("|");
				Console.WriteLine();
			}
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			if (x > _width - 1 || y > _height - 1)
				return null;
			return _map[x, y];
		}

		public override bool FillStacks(MabiItem item, out List<MabiItem> changed)
		{
			changed = new List<MabiItem>();

			if (item.StackType != BundleType.Stackable)
				return false;

			for (var y = 0; y < _height; ++y)
			{
				for (var x = 0; x < _width; ++x)
				{
					var invItem = _map[x, y];
					if (invItem == null || changed.Contains(invItem))
						continue;

					// If same class or item is stack item of inventory item
					if (item.Info.Class == invItem.Info.Class || invItem.StackItem == item.Info.Class)
					{
						// If item fits into stack 100%
						if ((uint)invItem.Info.Amount + (uint)item.Info.Amount <= (uint)invItem.StackMax)
						{
							invItem.Info.Amount += item.Info.Amount;
							item.Info.Amount = 0;

							changed.Add(invItem);

							return true;
						}

						// If stack is not full
						if (invItem.Info.Amount < invItem.StackMax)
						{
							var diff = Math.Min(item.Info.Amount, (ushort)(invItem.StackMax - invItem.Info.Amount));
							item.Info.Amount -= diff;
							invItem.Info.Amount += diff;

							changed.Add(invItem);
						}
					}
				}
			}

			return false;
		}

		public override bool Has(MabiItem item)
		{
			return _items.ContainsValue(item);
		}

		public override uint Remove(uint itemId, uint amount, ref List<MabiItem> changed)
		{
			uint result = 0;

			for (int y = (int)_height - 1; y >= 0; --y)
			{
				for (int x = (int)_width - 1; x >= 0; --x)
				{
					var item = _map[x, y];
					if (item == null || changed.Contains(item))
						continue;

					// Normal
					if (item.Info.Class == itemId && item.StackType == BundleType.None)
					{
						result++;
						amount--;
						item.Info.Amount = 0;
						changed.Add(item);
						_items.Remove(item.Id);
					}

					// Sacs/Stackables
					if (item.StackItem == itemId || (item.Info.Class == itemId && item.StackType == BundleType.Stackable))
					{
						if (amount >= item.Info.Amount)
						{
							result += item.Info.Amount;
							amount -= item.Info.Amount;
							item.Info.Amount = 0;
							changed.Add(item);
							if (item.StackType != BundleType.Sac)
							{
								_items.Remove(item.Id);
								this.ClearFromMap(item);
							}
						}
						else
						{
							result += amount;
							item.Info.Amount -= (ushort)amount;
							amount = 0;
							changed.Add(item);
						}
					}

					if (amount == 0)
						goto L_Result;
				}
			}

		L_Result:
			return result;
		}

		public override uint Count(uint itemId)
		{
			uint result = 0;

			foreach (var item in _items.Values)
				if (item.Info.Class == itemId || item.StackItem == itemId)
					result += item.Info.Amount;

			return result;
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

		public override bool TryAdd(MabiItem item, byte targetX, byte targetY, out MabiItem collidingItem)
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

		public override void ForceAdd(MabiItem item)
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

		public override bool Add(MabiItem item)
		{
			if (_item != null)
				return false;

			this.ForceAdd(item);
			return true;
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _item;
		}

		public override bool FillStacks(MabiItem item, out List<MabiItem> changed)
		{
			changed = null;
			return false;
		}

		public override bool Has(MabiItem item)
		{
			return _item == item;
		}

		public override uint Remove(uint itemId, uint amount, ref List<MabiItem> changed)
		{
			return 0;
		}

		public override uint Count(uint itemId)
		{
			if (_item != null && _item.Info.Class == itemId)
				return _item.Info.Amount;
			return 0;
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

		public override bool TryAdd(MabiItem item, byte targetX, byte targetY, out MabiItem colliding)
		{
			colliding = null;
			_items.Add(item);
			return true;
		}

		public override void ForceAdd(MabiItem item)
		{
			_items.Add(item);
			item.Move(this.Pocket, 0, 0);
		}

		public override bool Remove(MabiItem item)
		{
			return _items.Remove(item);
		}

		public override bool Add(MabiItem item)
		{
			this.ForceAdd(item);
			return true;
		}

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _items.FirstOrDefault();
		}

		public override bool FillStacks(MabiItem item, out List<MabiItem> changed)
		{
			changed = null;
			return false;
		}

		public override bool Has(MabiItem item)
		{
			return _items.Contains(item);
		}

		public override uint Remove(uint itemId, uint amount, ref List<MabiItem> changed)
		{
			return 0;
		}

		public override uint Count(uint itemId)
		{
			uint result = 0;

			foreach (var item in _items)
				if (item.Info.Class == itemId || item.StackItem == itemId)
					result += item.Info.Amount;

			return result;
		}
	}
}
