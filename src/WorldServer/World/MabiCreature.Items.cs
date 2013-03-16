// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.World.Events;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public List<MabiItem> Items = new List<MabiItem>();

		public MabiItem RightHand { get; set; }
		public MabiItem LeftHand { get; set; }
		public MabiItem Arrows { get; set; }

		/// <summary>
		/// Saves references to the equipment in fields.
		/// </summary>
		/// <param name="pocket">Pocket.None to update all, or the pocket to update.</param>
		public void UpdateItemsFromPockets(Pocket pocket = Pocket.None)
		{
			// Main weapon
			if (pocket == Pocket.None || pocket == Pocket.RightHand1 || pocket == Pocket.RightHand2)
				this.RightHand = this.GetItemInPocket(Pocket.RightHand1);

			// Shield, second hand
			if (pocket == Pocket.None || pocket == Pocket.LeftHand1 || pocket == Pocket.LeftHand2)
				this.LeftHand = this.GetItemInPocket(Pocket.LeftHand1);

			// Arrows
			if (pocket == Pocket.None || pocket == Pocket.Arrow1 || pocket == Pocket.Arrow2)
				this.Arrows = this.GetItemInPocket(Pocket.Arrow1);
		}

		/// <summary>
		/// Returns the item in the given pocket.
		/// </summary>
		/// <param name="slot">Target pocket</param>
		/// <param name="correctForWeaponSet">If true WeaponSet is taken into consideration (eg RightHand1 becomes RightHand2, if second set is selected).</param>
		/// <returns></returns>
		public MabiItem GetItemInPocket(Pocket slot, bool correctForWeaponSet = true)
		{
			if (correctForWeaponSet && (slot == Pocket.RightHand1 || slot == Pocket.LeftHand1 || slot == Pocket.Arrow1))
				slot += this.WeaponSet;

			return this.GetItemInPocket((byte)slot);
		}

		public MabiItem GetItemInPocket(byte slot)
		{
			return this.Items.FirstOrDefault(a => a.Info.Pocket == slot);
		}

		public MabiItem GetItemColliding(Pocket target, uint sourceX, uint sourceY, MabiItem newItem)
		{
			foreach (var item in this.Items)
			{
				if (item == newItem)
					continue;

				if (item.Info.Pocket != (byte)target)
					continue;

				if (!(
					sourceX > item.Info.X + item.Width - 1 || item.Info.X > sourceX + newItem.Width - 1 ||
					sourceY > item.Info.Y + item.Height - 1 || item.Info.Y > sourceY + newItem.Height - 1
				))
				{
					return item;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns null if there is no space for this item.
		/// </summary>
		/// <param name="newItem"></param>
		/// <param name="pocket"></param>
		/// <returns></returns>
		public MabiVertex GetFreeItemSpace(MabiItem newItem, Pocket pocket)
		{
			if (newItem.DataInfo != null)
			{
				// I'm sure there's a way to optimze this, but oh well, good enough for now.
				// Look at every space and see if the new item would fit there.
				for (uint y = 0; y < this.RaceInfo.InvHeight; ++y)
				{
					for (uint x = 0; x < this.RaceInfo.InvWidth; ++x)
					{
						var item = this.GetItemColliding(pocket, x, y, newItem);
						if (item == null && (x + newItem.DataInfo.Width - 1 < this.RaceInfo.InvWidth && y + newItem.DataInfo.Height - 1 < this.RaceInfo.InvHeight))
						{
							return new MabiVertex(x, y);
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Returns item with the given Id from inventory, or null if it's not found.
		/// </summary>
		/// <param name="itemid"></param>
		/// <returns></returns>
		public MabiItem GetItem(ulong itemid)
		{
			return this.Items.FirstOrDefault(a => a.Id == itemid);
		}

		/// <summary>
		/// Adds one or multiple items with the given id to the creature's
		/// inventory. Tries to fill sacs first, inventory afterwards, and
		/// all remaining will be added to the temp inventory.
		/// </summary>
		/// <param name="itemClass"></param>
		/// <param name="amount"></param>
		public MabiItem GiveItem(uint itemClass, uint amount, uint color1 = 0, uint color2 = 0, uint color3 = 0, bool useDBColors = true, bool drop = false)
		{
			MabiItem result = null;

			// Fill stacks and sacs
			foreach (var item in this.Items)
			{
				if ((item.Type == ItemType.Sac && item.StackItem == itemClass) || (item.Info.Class == itemClass && item.StackType == BundleType.Stackable))
				{
					if (item.Info.Amount >= item.StackMax)
						continue;

					var prev = item.Info.Amount;
					var diff = item.StackMax - item.Info.Amount;
					if (diff >= amount)
					{
						item.Info.Amount += (ushort)amount;
						amount = 0;
					}
					else
					{
						item.Info.Amount = item.StackMax;
						amount -= (uint)diff;
					}

					if (prev != item.Info.Amount)
					{
						EntityEvents.Instance.OnCreatureItemUpdate(this, item);
						result = item;
					}
				}
			}

			// Add remaining to inv or temp inv.
			while (amount > 0)
			{
				var item = new MabiItem(itemClass);
				if (!useDBColors)
				{
					item.Info.ColorA = color1;
					item.Info.ColorB = color2;
					item.Info.ColorC = color3;
				}
				var max = Math.Max((ushort)1, item.StackMax); // This way, we can't drag the server into an infinate loop
				if (amount <= max)
				{
					item.Info.Amount = (ushort)amount;
					amount = 0;
				}
				else
				{
					item.Info.Amount = max;
					amount -= max;
				}

				if (drop)
				{
					EntityEvents.Instance.OnCreatureDropItem(this, item);
				}
				else
				{
					var pocket = Pocket.Inventory;
					var space = this.GetFreeItemSpace(item, pocket);
					if (space == null)
					{
						pocket = Pocket.Temporary;
						space = new MabiVertex(0, 0);
					}

					item.Move(pocket, space.X, space.Y);
					this.Items.Add(item);

					EntityEvents.Instance.OnCreatureItemUpdate(this, item, true);
				}

				result = item;
			}

			EntityEvents.Instance.OnCreatureItemAction(this, itemClass);

			return result;
		}

		/// <summary>
		/// Removes the given amount of items with the given id from the
		/// creature's inventory. Tries inventory first, sacs afterwards.
		/// </summary>
		/// <param name="itemClass"></param>
		/// <param name="amount"></param>
		public void RemoveItem(uint itemClass, uint amount)
		{
			var toRemove = new List<MabiItem>();

			// Items first
			foreach (var item in this.Items)
			{
				if (item.Info.Class == itemClass)
				{
					var prev = item.Info.Amount;
					if (amount <= item.Info.Amount)
					{
						item.Info.Amount -= (ushort)amount;
						amount = 0;
					}
					else
					{
						amount = amount - item.Info.Amount;
						item.Info.Amount = 0;
					}

					if (prev != item.Info.Amount)
					{
						if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
							toRemove.Add(item);
						EntityEvents.Instance.OnCreatureItemUpdate(this, item);
					}
				}
			}

			// Sacs afterwards
			if (amount > 0)
			{
				foreach (var item in this.Items)
				{
					if (item.Type == ItemType.Sac && item.StackItem == itemClass)
					{
						var prev = item.Info.Amount;
						if (amount <= item.Info.Amount)
						{
							item.Info.Amount -= (ushort)amount;
							amount = 0;
						}
						else
						{
							amount = amount - item.Info.Amount;
							item.Info.Amount = 0;
						}

						if (prev != item.Info.Amount)
						{
							if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
								toRemove.Add(item);
							EntityEvents.Instance.OnCreatureItemUpdate(this, item);
						}
					}
				}
			}

			foreach (var item in toRemove)
				this.Items.Remove(item);

			EntityEvents.Instance.OnCreatureItemAction(this, itemClass);
		}

		/// <summary>
		/// Adds the given amount of gold to the inventory. See GiveItem.
		/// </summary>
		/// <param name="amount"></param>
		public MabiItem GiveGold(uint amount)
		{
			return this.GiveItem(2000, amount);
		}

		/// <summary>
		/// Removes the given amount of gold from the inventory. See RemoveItem.
		/// </summary>
		/// <param name="amount"></param>
		public void RemoveGold(uint amount)
		{
			this.RemoveItem(2000, amount);
		}

		/// <summary>
		/// Returns wheather the amount of all items with the given id in the
		/// inventory exceeds the given amount. Sacs are counted as well.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool HasItem(uint itemId, uint amount = 1)
		{
			return (this.CountItem(itemId) >= amount);
		}

		public uint CountItem(uint itemId)
		{
			uint total = 0;
			foreach (var item in this.Items)
			{
				if (item.Info.Class == itemId || item.StackItem == itemId)
					total += item.Info.Amount;
			}

			return total;
		}

		/// <summary>
		/// Returns wheather the creature has the given amount of gold in
		/// its inventory. See HasItem.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool HasGold(uint amount)
		{
			return this.HasItem(2000, amount);
		}

		/// <summary>
		/// Decrements item amount by one and sends update packets.
		/// </summary>
		/// <param name="item"></param>
		public void DecItem(MabiItem item)
		{
			if (!this.Items.Contains(item))
				return;

			item.Info.Amount--;

			if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
				this.Items.Remove(item);

			EntityEvents.Instance.OnCreatureItemUpdate(this, item);
			EntityEvents.Instance.OnCreatureItemAction(this, item.Info.Class);
		}
	}
}
