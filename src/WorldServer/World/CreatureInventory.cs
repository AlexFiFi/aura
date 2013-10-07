// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.Shared.Const;

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

		public MabiItem RightHand { get; protected set; }
		public MabiItem LeftHand { get; protected set; }
		public MabiItem Magazine { get; protected set; }

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

			this.CheckLeftHand(item, source, target);
			this.UpdateEquipReferences(item, source, target);
			this.CheckEquipMoved(item, source, target);

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

		/// <summary>
		/// Unequips item in left hand/magazine, if item in right hand is moved.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		private void CheckLeftHand(MabiItem item, Pocket source, Pocket target)
		{
			var pocketOfInterest = Pocket.None;

			if (source == Pocket.RightHand1 || source == Pocket.RightHand2)
				pocketOfInterest = source;
			if (target == Pocket.RightHand1 || target == Pocket.RightHand2)
				pocketOfInterest = target;

			if (pocketOfInterest != Pocket.None)
			{
				var leftPocket = pocketOfInterest + 2; // Left Hand 1/2
				var leftItem = _pockets[leftPocket].GetItemAt(0, 0);
				if (leftItem == null)
				{
					leftPocket += 2; // Magazine 1/2
					leftItem = _pockets[leftPocket].GetItemAt(0, 0);
				}
				if (leftItem != null)
				{
					// Try inventory first.
					// TODO: List of pockets stuff can be auto-moved to.
					if (!_pockets[Pocket.Inventory].PutItem(leftItem))
					{
						// Fallback, temp inv
						_pockets[Pocket.Temporary].PutItem(leftItem);
					}

					Send.ItemMoveInfo(_creature, leftItem, leftPocket, null);
					Send.EquipmentMoved(_creature, leftPocket);
				}
			}
		}

		private void UpdateEquipReferences(MabiItem item, Pocket source, Pocket target)
		{
			// Right Hand
			if (source == Pocket.RightHand1 || target == Pocket.RightHand1)
				this.RightHand = _pockets[Pocket.RightHand1].GetItemAt(0, 0);
			if (source == Pocket.RightHand2 || target == Pocket.RightHand2)
				this.RightHand = _pockets[Pocket.RightHand2].GetItemAt(0, 0);

			// Left Hand
			if (source == Pocket.LeftHand1 || target == Pocket.LeftHand1)
				this.RightHand = _pockets[Pocket.LeftHand1].GetItemAt(0, 0);
			if (source == Pocket.LeftHand2 || target == Pocket.LeftHand2)
				this.RightHand = _pockets[Pocket.LeftHand2].GetItemAt(0, 0);

			// Magazine
			if (source == Pocket.Magazine1 || target == Pocket.Magazine1)
				this.RightHand = _pockets[Pocket.Magazine1].GetItemAt(0, 0);
			if (source == Pocket.Magazine2 || target == Pocket.Magazine2)
				this.RightHand = _pockets[Pocket.Magazine2].GetItemAt(0, 0);
		}

		private void CheckEquipMoved(MabiItem item, Pocket source, Pocket target)
		{
			if (source.IsEquip())
				Send.EquipmentMoved(_creature, source);

			if (target.IsEquip())
			{
				Send.EquipmentChanged(_creature, item);

				// TODO: Equip/Unequip item scripts
				switch (item.Info.Class)
				{
					// Umbrella Skill
					case 41021:
					case 41022:
					case 41023:
					case 41025:
					case 41026:
					case 41027:
					case 41061:
					case 41062:
					case 41063:
						if (!_creature.Skills.Has(SkillConst.Umbrella))
							_creature.Skills.Give(SkillConst.Umbrella, SkillRank.Novice);
						break;

					// Spread Wings
					case 19138:
					case 19139:
					case 19140:
					case 19141:
					case 19142:
					case 19143:
					case 19157:
					case 19158:
					case 19159:
						if (!_creature.Skills.Has(SkillConst.SpreadWings))
							_creature.Skills.Give(SkillConst.SpreadWings, SkillRank.Novice);
						break;
				}
			}
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

		public override MabiItem GetItemAt(uint x, uint y)
		{
			if (x > _width - 1 || y > _height - 1)
				return null;
			return _map[x, y];
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

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _item;
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

		public override MabiItem GetItemAt(uint x, uint y)
		{
			return _items.FirstOrDefault();
		}
	}
}
