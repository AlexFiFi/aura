// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Linq;
using Common.Network;
using Common.Tools;
using Common.World;
using MabiNatives;

namespace World.World
{
	public class MabiShop
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

		public void AddTabs(params string[] names)
		{
			foreach (var name in names)
			{
				if (this.Tabs.Exists(a => a.Name == name))
					return;

				this.Tabs.Add(new MabiShopTab(name));
			}
		}

		public void AddItems(string tab, params MabiItem[] items)
		{
			this.AddTabs(tab);
			this.Tabs.First(a => a.Name == tab).Items.AddRange(items);
		}

		public void AddItem(string tab, string itemName, int price = -1)
		{
			uint itemClass = 50003; // Apple

			var dbInfo = MabiData.ItemDb.Find(itemName);
			if (dbInfo == null)
				Logger.Warning("Unknown item '" + itemName + "'.");
			else
				itemClass = dbInfo.Id;

			this.AddItem(tab, itemClass, price);
		}

		public void AddItem(string tab, string itemName, uint color1, uint color2, uint color3, int price = -1)
		{
			uint itemClass = 50003; // Apple

			var dbInfo = MabiData.ItemDb.Find(itemName);
			if (dbInfo == null)
				Logger.Warning("Unknown item '" + itemName + "'.");
			else
				itemClass = dbInfo.Id;

			this.AddItem(tab, itemClass, color1, color2, color3, price);
		}

		public void AddItem(string tab, uint itemClass, int price = -1)
		{
			var item = new MabiItem(itemClass);
			this.AddItem(tab, item, price);
		}

		public void AddItem(string tab, uint itemClass, uint color1, uint color2, uint color3, int price = -1)
		{
            var item = new MabiItem(itemClass);
			item.Info.ColorA = color1;
			item.Info.ColorB = color2;
			item.Info.ColorC = color3;
			this.AddItem(tab, item, price);
		}

		public void AddItem(string tab, MabiItem item, int price = -1)
		{
			if (price >= 0)
			{
				item.OptionInfo.Price = (uint)price;
				if (item.OptionInfo.Price <= item.OptionInfo.SellingPrice)
				{
					Logger.Warning(string.Format("Price for '{0}' is lower than the selling price. ({1} < {2})", item.Info.Class.ToString(), item.OptionInfo.Price, item.OptionInfo.SellingPrice));
				}
			}
			this.AddItems(tab, item);
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
