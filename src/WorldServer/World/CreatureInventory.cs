// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Aura.World.Network;

namespace Aura.World.World
{
	public class CreatureInventory
	{
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
						yield return item;
					}
				}
			}
		}

		public CreatureInventory(MabiCreature creature)
		{
			_creature = creature;

			_pockets = new Dictionary<Pocket, InventoryPocket>();

			// Cursor, Inv, Equipment, Style
			this.Add(new InventoryPocketSingle(Pocket.Cursor));
			this.Add(new InventoryPocketNormal(Pocket.Inventory, creature.RaceInfo.InvWidth, creature.RaceInfo.InvHeight));
			this.Add(new InventoryPocketNormal(Pocket.PersonalInventory, creature.RaceInfo.InvWidth, creature.RaceInfo.InvHeight));
			this.Add(new InventoryPocketNormal(Pocket.VIPInventory, creature.RaceInfo.InvWidth, creature.RaceInfo.InvHeight));
			for (var i = Pocket.Face; i <= Pocket.Robe; ++i)
				this.Add(new InventoryPocketSingle(i));
			for (var i = Pocket.ArmorStyle; i <= Pocket.RobeStyle; ++i)
				this.Add(new InventoryPocketSingle(i));
		}

		public InventoryPocket this[Pocket pocket]
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

		public bool MoveItem(MabiItem item, Pocket pocket, byte targetX, byte targetY)
		{
			if (!this.Has(pocket))
				return false;

			var source = item.Pocket;
			var amount = item.Info.Amount;

			MabiItem collidingItem = null;
			if (!_pockets[pocket].TryPutItem(item, targetX, targetY, out collidingItem))
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
				Send.ItemMoveInfo(_creature, item, source, collidingItem);
			}

			return true;
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

		public abstract bool TryPutItem(MabiItem item, byte targetX, byte targetY, out MabiItem colliding);
		public abstract void ForcePutItem(MabiItem item);
		public abstract void Remove(MabiItem item);
	}

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

			if (_map[targetX, targetY] != null)
				collidingItem = _map[targetX, targetY];

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
				collidingItem.Move(newItem.Pocket, newItem.Info.X, newItem.Info.Y);
				this.ClearFromMap(collidingItem);
			}

			newItem.Move(this.Pocket, targetX, targetY);
			this.AddToMap(newItem);

			return true;
		}

		protected void AddToMap(MabiItem item)
		{
			for (var x = item.Info.X; x < item.Info.X + item.DataInfo.Width; ++x)
				for (var y = item.Info.Y; y < item.Info.Y + item.DataInfo.Height; ++y)
					_map[x, y] = item;
		}

		protected void ClearFromMap(MabiItem item)
		{
			for (var x = item.Info.X; x < item.Info.X + item.DataInfo.Width; ++x)
				for (var y = item.Info.Y; y < item.Info.Y + item.DataInfo.Height; ++y)
					_map[x, y] = null;
		}

		public override void ForcePutItem(MabiItem item)
		{
			this.AddToMap(item);
			_items.Add(item.Id, item);
		}

		public override void Remove(MabiItem item)
		{
			if (_items.Remove(item.Id))
				this.ClearFromMap(item);
		}
	}

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
		}

		public override void Remove(MabiItem item)
		{
			if (_item == item)
				_item = null;
		}
	}
}
