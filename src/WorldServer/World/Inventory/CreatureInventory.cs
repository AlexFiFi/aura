﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

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
		/// Attempts to store item somewhere in the inventory.
		/// If temp is true, it will fallback to the temp inv, if there's not space.
		/// Returns whether the item was successfully stored somewhere.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="temp"></param>
		/// <returns></returns>
		public bool PutItem(MabiItem item, bool tempFallback)
		{
			// Try inv
			if (_pockets[Pocket.Inventory].PutItem(item))
				return true;

			// Try temp
			if (tempFallback && _pockets[Pocket.Temporary].PutItem(item))
				return true;

			return false;
		}

		public bool FitIn(MabiItem item, bool tempFallback)
		{
			var amount = item.Info.Amount;

			List<MabiItem> changed;
			bool success;

			// Try inv
			success = _pockets[Pocket.Inventory].FitIn(item, out changed);
			this.UpdateChangedItemAmounts(changed);

			// Try temp
			if (!success && tempFallback)
				success = _pockets[Pocket.Temporary].PutItem(item);

			// Inform about new item, if it wasn't added to stacks completely
			if (success && item.Info.Amount > 0)
				Send.ItemInfo(_creature.Client, _creature, item);

			return success;
		}

		private void UpdateChangedItemAmounts(IEnumerable<MabiItem> items)
		{
			if (items == null)
				return;

			foreach (var item in items)
				_creature.Client.Send(PacketCreator.ItemAmount(_creature, item));
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
}
