// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Common.Constants
{
	public enum ItemType : ushort
	{
		Armor = 0,
		Headgear = 1,
		Glove = 2,
		Shoe = 3,
		Book = 4,
		Gold = 5,
		ItemBag = 6,
		Weapon = 7,
		Weapons2H = 8, // 2H, bows, tools, etc
		Weapon2 = 9, // Ineffective Weapons? Signs, etc.
		Instrument = 10,
		Shield = 11,
		Robe = 12,
		Accessory = 13,
		Arrow = 14,
		MusicScroll = 15,
		Manual = 16,
		EnchantScroll = 17,
		CollectionBook = 18,
		ShopLicense = 19,
		FaliasTreasure = 20,
		Kiosk = 21,
		Hair = 100,
		Face = 101,
		Usable = 501,
		Quest = 502,
		Usable2 = 503,
		Unknown1 = 504,
		Sac = 1000,
		Misc = 1001, // ?
	}

	public enum BundleType : byte
	{
		None = 0,
		Stackable = 1,
		Sac = 2,
	}

	public enum Pocket : byte
	{
		None = 0,
		Cursor = 1,
		Inventory = 2,
		Face = 3,
		Hair = 4,
		Armor = 5,
		Glove = 6,
		Shoe = 7,
		Head = 8,
		Robe = 9,
		LeftHand1 = 10,
		LeftHand2 = 11,
		RightHand1 = 12,
		RightHand2 = 13,
		Arrow1 = 14,
		Arrow2 = 15,
		Accessory1 = 16,
		Accessory2 = 17,
		Trade = 19,
		Temporary = 20,
		Quest = 23,
		Trash = 24,
		BattleReward = 28,
		EnchantReward = 29,
		ManaCrystalReward = 30,
		Falias1 = 32,
		Falias2 = 33,
		Falias3 = 34,
		Falias4 = 35,
		ComboCard = 41,
		SpecialInventory = 49,
		ExpandedInventory = 72,
		FarmStone = 81,
		Inventory2 = 100,
	}
}
