// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Util;
using Aura.World.Events;

namespace Aura.World.World
{
	public class MabiShop : IDisposable
	{
		public class MabiShopTab
		{
			public string Name;
			public List<MabiItem> Items = new List<MabiItem>();

			public MabiShopTab(string name)
			{
				this.Name = name;
			}
		}

		public List<MabiShopTab> Tabs = new List<MabiShopTab>();
		public bool ColorChangeEnabled = true;

		public bool Disposed { get; private set; }

		public MabiShop()
		{
			this.Disposed = false;
			EventManager.TimeEvents.ErinnMidnightTick += this.ChangeItemColors;
		}

		public virtual void Dispose()
		{
			if (this.Disposed)
				return;

			EventManager.TimeEvents.ErinnMidnightTick -= this.ChangeItemColors;
			this.Disposed = true;
		}

		public void DisableRandomColors()
		{
			this.ColorChangeEnabled = false;
		}

		private void ChangeItemColors(MabiTime time)
		{
			if (!WorldConf.ColorChange || !this.ColorChangeEnabled)
				return;

			var rand = RandomProvider.Get();

			foreach (var tab in Tabs)
			{
				foreach (var item in tab.Items)
				{
					item.Info.ColorA = MabiData.ColorMapDb.GetRandom(item.DataInfo.ColorMap1, rand);
					item.Info.ColorB = MabiData.ColorMapDb.GetRandom(item.DataInfo.ColorMap2, rand);
					item.Info.ColorC = MabiData.ColorMapDb.GetRandom(item.DataInfo.ColorMap3, rand);
				}
			}
		}

		public void AddTabs(params string[] names)
		{
			foreach (var name in names)
			{
				if (this.Tabs.Exists(a => a.Name == name))
					return;

				this.Tabs.Add(new MabiShopTab(name));
			}
		}

		public void AddItem(string tab, string itemName, ushort amount = 1, int price = -1)
		{
			uint itemClass = 50003; // Apple

			var dbInfo = MabiData.ItemDb.Find(itemName);
			if (dbInfo == null)
				Logger.Warning("Unknown item '" + itemName + "'.");
			else
				itemClass = dbInfo.Id;

			this.AddItem(tab, itemClass, amount, price);
		}

		public void AddItem(string tab, string itemName, uint color1, uint color2, uint color3, ushort amount = 1, int price = -1)
		{
			uint itemClass = 50003; // Apple

			var dbInfo = MabiData.ItemDb.Find(itemName);
			if (dbInfo == null)
				Logger.Warning("Unknown item '" + itemName + "'.");
			else
				itemClass = dbInfo.Id;

			this.AddItem(tab, itemClass, color1, color2, color3, amount, price);
		}

		public void AddItem(string tab, uint itemClass, ushort amount = 1, int price = -1)
		{
			var item = new MabiItem(itemClass);
			item.Info.Amount = Math.Min(amount, item.StackMax);
			this.AddItem(tab, item, amount, price);
		}

		public void AddItem(string tab, uint itemClass, uint color1, uint color2, uint color3, ushort amount = 1, int price = -1)
		{
			var item = new MabiItem(itemClass);
			item.Info.Amount = Math.Min(amount, item.StackMax);
			item.Info.ColorA = color1;
			item.Info.ColorB = color2;
			item.Info.ColorC = color3;
			this.AddItem(tab, item, amount, price);
		}

		public void AddItem(string tab, MabiItem item, ushort amount = 1, int price = -1)
		{
			if (price >= 0)
			{
				item.OptionInfo.Price = (uint)price * item.StackMax;
				item.OptionInfo.SellingPrice = (uint)(item.OptionInfo.Price * 0.1f);

				if (item.OptionInfo.Price < item.OptionInfo.SellingPrice)
					Logger.Warning(string.Format("Price for '{0}' is lower than the selling price. ({1} < {2})", item.Info.Class.ToString(), item.OptionInfo.Price, item.OptionInfo.SellingPrice));
			}
			this.AddItems(tab, item);
		}

		public void AddItems(string tab, params MabiItem[] items)
		{
			this.AddTabs(tab);
			this.Tabs.First(a => a.Name == tab).Items.AddRange(items);
		}

		public MabiItem GetItem(ulong itemId)
		{
			MabiItem item = null;
			foreach (var tab in this.Tabs)
			{
				item = tab.Items.FirstOrDefault(a => a.Id == itemId);
				if (item != null)
					break;
			}

			var newItem = (item != null ? new MabiItem(item) : null);
			return newItem;
		}
	}
}
