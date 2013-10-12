// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Network;

namespace Aura.World.World
{
	public class CreatureInventory
	{
		private const uint DefaultWidth = 6;
		private const uint DefaultHeight = 10;
		private const uint GoldItemId = 2000;
		private const uint GoldStackMax = 1000;

		private MabiCreature _creature;
		private Dictionary<Pocket, InventoryPocket> _pockets;

		/// <summary>
		/// List of all items in this inventory.
		/// </summary>
		public IEnumerable<MabiItem> Items
		{
			get
			{
				foreach (var pocket in _pockets.Values)
					foreach (var item in pocket.Items.Where(a => a != null))
						yield return item;
			}
		}

		/// <summary>
		/// List of all items sitting in equipment pockets in this inventory.
		/// </summary>
		public IEnumerable<MabiItem> Equipment
		{
			get
			{
				foreach (var pocket in _pockets.Values.Where(a => a.Pocket.IsEquip()))
					foreach (var item in pocket.Items.Where(a => a != null))
						yield return item;
			}
		}

		/// <summary>
		/// List of all items in equipment slots, minus hair and face.
		/// </summary>
		public IEnumerable<MabiItem> ActualEquipment
		{
			get
			{
				foreach (var pocket in _pockets.Values.Where(a => a.Pocket.IsEquip() && a.Pocket != Pocket.Hair && a.Pocket != Pocket.Face))
					foreach (var item in pocket.Items.Where(a => a != null))
						yield return item;
			}
		}

		private WeaponSet _weaponSet;
		public WeaponSet WeaponSet
		{
			get { return _weaponSet; }
			set
			{
				_weaponSet = value;
				// params?
				this.UpdateEquipReferences(Pocket.RightHand1, Pocket.RightHand2);
				this.UpdateEquipReferences(Pocket.RightHand2, Pocket.RightHand1);
			}
		}

		public MabiItem RightHand { get; protected set; }
		public MabiItem LeftHand { get; protected set; }
		public MabiItem Magazine { get; protected set; }

		public uint Gold
		{
			get { return this.CountItem(GoldItemId); }
		}

		public CreatureInventory(MabiCreature creature)
		{
			_creature = creature;

			_pockets = new Dictionary<Pocket, InventoryPocket>();

			// Cursor, Temp
			this.Add(new InventoryPocketStack(Pocket.Temporary));
			this.Add(new InventoryPocketSingle(Pocket.Cursor));

			// Equipment
			for (var i = Pocket.Face; i <= Pocket.Accessory2; ++i)
				this.Add(new InventoryPocketSingle(i));

			// Style
			for (var i = Pocket.ArmorStyle; i <= Pocket.RobeStyle; ++i)
				this.Add(new InventoryPocketSingle(i));
		}

		public void Add(InventoryPocket inventoryPocket)
		{
			if (_pockets.ContainsKey(inventoryPocket.Pocket))
				Logger.Warning("Replacing pocket '{0}' in '{1}'s inventory.", inventoryPocket.Pocket, _creature);

			_pockets[inventoryPocket.Pocket] = inventoryPocket;
		}

		/// <summary>
		/// Adds main inventories (Inv, personal, VIP). Call after creature's
		/// defaults have been loaded.
		/// </summary>
		public void AddMainInventory()
		{
			if (_creature.RaceInfo == null)
				Logger.Warning("Race for creature '{0}' ({1:X016}) not loaded before initializing inventory.", _creature.Name, _creature.Id);

			var width = (_creature.RaceInfo != null ? _creature.RaceInfo.InvWidth : DefaultWidth);
			var height = (_creature.RaceInfo != null ? _creature.RaceInfo.InvHeight : DefaultHeight);

			this.Add(new InventoryPocketNormal(Pocket.Inventory, width, height));
			this.Add(new InventoryPocketNormal(Pocket.PersonalInventory, width, height));
			this.Add(new InventoryPocketNormal(Pocket.VIPInventory, width, height));
		}

		public bool Has(Pocket pocket)
		{
			return _pockets.ContainsKey(pocket);
		}

		public MabiItem GetItem(ulong itemId)
		{
			return this.Items.FirstOrDefault(a => a.Id == itemId);
		}

		public MabiItem GetItemAt(Pocket pocket, uint x, uint y)
		{
			if (!this.Has(pocket))
				return null;

			return _pockets[pocket].GetItemAt(x, y);
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

			this.UpdateInventory(item, source, target);

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
		/// Attempts to store item somewhere in the inventory.
		/// If temp is true, it will fallback to the temp inv, if there's not space.
		/// Returns whether the item was successfully stored somewhere.
		/// Sends ItemNew on success.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="temp"></param>
		/// <returns></returns>
		public bool PutItem(MabiItem item, bool tempFallback)
		{
			bool success;

			// Try inv
			success = _pockets[Pocket.Inventory].PutItem(item);

			// Try temp
			if (!success && tempFallback)
				success = _pockets[Pocket.Temporary].PutItem(item);

			// Inform about new item
			if (success)
				Send.ItemInfo(_creature.Client, _creature, item);

			return success;
		}

		/// <summary>
		/// Puts item into inventory, if possible. Tries to fill stacks first.
		/// If tempFallback is true, leftovers will be put into temp.
		/// Sends ItemAmount and ItemNew if required/enabled.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="tempFallback"></param>
		/// <returns></returns>
		public bool FitIn(MabiItem item, bool tempFallback, bool sendItemNew)
		{
			var amount = item.Info.Amount;

			List<MabiItem> changed;
			bool success;

			// Try inv
			success = _pockets[Pocket.Inventory].FitIn(item, out changed);
			this.UpdateChangedItems(changed);

			// Try temp
			if (!success && tempFallback)
				success = _pockets[Pocket.Temporary].PutItem(item);

			// Inform about new item, if it wasn't added to stacks completely
			if (success && item.Info.Amount > 0 && sendItemNew)
				Send.ItemInfo(_creature.Client, _creature, item);

			return success;
		}

		/// <summary>
		/// Removes item from inventory. Sends ItemRemove on success,
		/// and possibly others, if equipment is removed.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(MabiItem item)
		{
			foreach (var pocket in _pockets.Values)
			{
				if (pocket.Remove(item))
				{
					this.UpdateInventory(item, item.Pocket, Pocket.None);

					Send.ItemRemove(_creature, item);
					return true;
				}
			}

			return false;
		}

		private void UpdateInventory(MabiItem item, Pocket source, Pocket target)
		{
			this.CheckLeftHand(item, source, target);
			this.UpdateEquipReferences(source, target);
			this.CheckEquipMoved(item, source, target);
		}

		private void UpdateChangedItems(IEnumerable<MabiItem> items)
		{
			if (items == null)
				return;

			foreach (var item in items)
			{
				if (item.Info.Amount > 0 || item.StackType == BundleType.Sac)
					Send.ItemAmount(_creature, item);
				else
					Send.ItemRemove(_creature, item);
			}
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

		private void UpdateEquipReferences(Pocket source, Pocket target)
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

		/// <summary>
		/// Decrements the item by amount, if it exists in this inventory.
		/// Sends ItemAmount/ItemRemove, depending on the resulting amount.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool DecItem(MabiItem item, ushort amount = 1)
		{
			if (!this.Has(item) || item.Info.Amount == 0 || item.Info.Amount < amount)
				return false;

			item.Info.Amount -= amount;

			if (item.Info.Amount > 0 || item.StackType == BundleType.Sac)
			{
				Send.ItemAmount(_creature, item);
			}
			else
			{
				this.Remove(item);
				Send.ItemRemove(_creature, item);
			}

			return true;
		}

		/// <summary>
		/// Returns whether the item exists in this inventory.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Has(MabiItem item)
		{
			foreach (var pocket in _pockets.Values)
				if (pocket.Has(item))
					return true;

			return false;
		}

		/// <summary>
		/// Puts new item(s) of class 'id' into the inventory.
		/// If item is stackable it is "fit in", filling stacks first.
		/// A sack is set to the amount and added as one item.
		/// If it's not a sac/stackable you'll get multiple new items.
		/// Uses temp inv if necessary.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool GiveItem(uint itemId, uint amount = 1)
		{
			var newItem = new MabiItem(itemId);

			if (newItem.StackType == BundleType.Stackable)
			{
				newItem.Info.Amount = (ushort)Math.Min(amount, ushort.MaxValue);
				return this.FitIn(newItem, true, true);
			}
			else if (newItem.StackType == BundleType.Sac)
			{
				newItem.Info.Amount = (ushort)Math.Min(amount, ushort.MaxValue);
				return this.PutItem(newItem, true);
			}
			else
			{
				for (int i = 0; i < amount; ++i)
					this.PutItem(new MabiItem(itemId), true);
				return true;
			}
		}

		public bool GiveGold(uint amount)
		{
			// Add gold, stack for stack
			do
			{
				var stackAmount = Math.Min(GoldStackMax, amount);
				this.GiveItem(GoldItemId, stackAmount);
				amount -= stackAmount;
			}
			while (amount > 0);

			return true;
		}

		public bool RemoveItem(uint itemId, uint amount = 1)
		{
			if (amount < 0)
				amount = 0;

			var changed = new List<MabiItem>();


			foreach (var pocket in _pockets.Values)
			{
				amount -= pocket.Remove(itemId, amount, ref changed);

				if (amount == 0)
					break;
			}

			this.UpdateChangedItems(changed);

			return (amount == 0);
		}

		public bool RemoveGold(uint amount)
		{
			return this.RemoveItem(GoldItemId, amount);
		}

		public uint CountItem(uint itemId)
		{
			uint result = 0;

			foreach (var pocket in _pockets.Values)
				result += pocket.CountItem(itemId);

			return result;
		}

		public bool Has(uint itemId, uint amount = 1)
		{
			return (this.CountItem(itemId) >= amount);
		}

		public bool HasGold(uint amount)
		{
			return this.Has(GoldItemId, amount);
		}
	}

	public enum WeaponSet : byte
	{
		First = 0,
		Second = 1,
	}
}
