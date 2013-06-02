// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Data;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.Shared.World;
using System;

namespace Aura.World.World
{
	/// <summary>
	/// Info/OptionInfo: These two fields contain structs that contain
	///     values which are send to the client. We simply turn the structs
	///     into bins, that's why those values are in there.
	/// DataInfo: This on the other hand is the data that we read from the
	///     item db file. Some of these values are used for fields in Info/
	///     OptionInfo, but the ones that aren't needed there, might be
	///     used directly. Those values /shouldn't be changed/.
	/// </summary>
	public class MabiItem : MabiEntity
	{
		public MabiItemInfo Info;
		public MabiItemOptionInfo OptionInfo;
		public ItemInfo DataInfo;

		public ItemType Type = ItemType.Misc;
		public BundleType StackType;
		public ushort StackMax;
		public uint StackItem;
		public byte Width, Height;
		public MabiTags Tags = new MabiTags();

		public ulong QuestId;

		private static ulong _worldItemIndex = Aura.Shared.Const.Id.TmpItems;
		public static ulong NewItemId { get { return _worldItemIndex++; } }

		/// <summary>
		/// Returns ".OptionInfo.Balance / 100".
		/// </summary>
		public float Balance
		{
			get { return this.OptionInfo.Balance / 100f; }
		}

		public bool IsWeapon
		{
			get { return (this.IsOneHandWeapon || this.IsTwoHandWeapon); }
		}

		public bool IsOneHandWeapon
		{
			get { return (this.Type == ItemType.Weapon || this.Type == ItemType.Weapon2); }
		}

		public bool IsTwoHandWeapon
		{
			get { return this.Type == ItemType.Weapon2H; }
		}

		public AttackSpeed AttackSpeed
		{
			get { return (AttackSpeed)this.OptionInfo.AttackSpeed; }
		}

		public MabiItem(uint itemClass, bool worldId = true)
		{
			this.Info.Class = itemClass;
			this.LoadDefault();

			if (worldId)
				this.Id = MabiItem.NewItemId;
		}

		public MabiItem(uint itemClass, ulong id)
			: this(itemClass, false)
		{
			this.Id = id;
		}

		public MabiItem(MabiItem itemToCopy)
		{
			this.Info = itemToCopy.Info;
			this.OptionInfo = itemToCopy.OptionInfo;
			this.DataInfo = itemToCopy.DataInfo;

			this.Type = itemToCopy.Type;
			this.StackType = itemToCopy.StackType;
			this.StackMax = itemToCopy.StackMax;
			this.StackItem = itemToCopy.StackItem;
			this.Width = itemToCopy.Width;
			this.Height = itemToCopy.Height;

			this.Id = MabiItem.NewItemId;
		}

		public MabiItem(CharCardSetInfo cardItem)
			: this(cardItem.Class)
		{
			this.Info.Pocket = cardItem.Pocket;
			this.Info.ColorA = cardItem.Color1;
			this.Info.ColorB = cardItem.Color2;
			this.Info.ColorC = cardItem.Color3;
		}

		public MabiItem(MabiQuest quest)
			: this(70024, false)
		{
			this.Id = quest.Id - Aura.Shared.Const.Id.QuestItemOffset;
			this.Info.Pocket = (byte)Pocket.Quest;
			this.OptionInfo.Flag = 1;

			this.QuestId = quest.Id;
			//this.AdditionalInfo = quest.ToolTip;
		}

		public override EntityType EntityType
		{
			get { return EntityType.Item; }
		}

		/// <summary>
		/// Proxy for .Info.Region.
		/// </summary>
		public override uint Region
		{
			get { return this.Info.Region; }
			set { this.Info.Region = value; }
		}

		public override ushort DataType
		{
			get { return 80; }
		}

		/// <summary>
		/// Returns .Info.Amount.
		/// </summary>
		public int Count { get { return this.Info.Amount; } }

		/// <summary>
		/// Tries to load default data from item db, and saves reference
		/// to data in DataInfo.
		/// </summary>
		public void LoadDefault()
		{
			this.DataInfo = MabiData.ItemDb.Find(this.Info.Class);
			if (this.DataInfo != null)
			{
				this.Info.KnockCount = this.DataInfo.KnockCount;
				this.OptionInfo.KnockCount = this.DataInfo.KnockCount;

				this.OptionInfo.Durability = this.DataInfo.Durability;
				this.OptionInfo.DurabilityMax = this.DataInfo.Durability;
				this.OptionInfo.DurabilityOriginal = this.DataInfo.Durability;
				this.OptionInfo.AttackMin = this.DataInfo.AttackMin;
				this.OptionInfo.AttackMax = this.DataInfo.AttackMax;
				this.OptionInfo.Balance = this.DataInfo.Balance;
				this.OptionInfo.Critical = this.DataInfo.Critical;
				this.OptionInfo.Defense = this.DataInfo.Defense;
				this.OptionInfo.Protection = this.DataInfo.Protection;
				this.OptionInfo.Price = this.DataInfo.Price;
				this.OptionInfo.SellingPrice = this.DataInfo.SellingPrice;
				this.OptionInfo.WeaponType = this.DataInfo.WeaponType;
				this.OptionInfo.AttackSpeed = this.DataInfo.AttackSpeed;
				this.OptionInfo.EffectiveRange = this.DataInfo.Range;

				this.OptionInfo.Flag = 1;

				this.Type = (ItemType)this.DataInfo.Type;
				this.StackType = (BundleType)this.DataInfo.StackType;
				this.StackMax = this.DataInfo.StackMax;
				this.StackItem = this.DataInfo.StackItem;
				this.Width = this.DataInfo.Width;
				this.Height = this.DataInfo.Height;

				var rand = RandomProvider.Get();
				this.Info.ColorA = MabiData.ColorMapDb.GetRandom(this.DataInfo.ColorMap1, rand);
				this.Info.ColorB = MabiData.ColorMapDb.GetRandom(this.DataInfo.ColorMap2, rand);
				this.Info.ColorC = MabiData.ColorMapDb.GetRandom(this.DataInfo.ColorMap3, rand);
			}
			else
			{
				Logger.Warning("Item '{0}' couldn't be found in the database.", this.Info.Class);
			}

			if (this.StackType != BundleType.Sac && this.Info.Amount < 1)
				this.Info.Amount = 1;
			if (this.StackMax < 1)
				this.StackMax = 1;
		}

		public bool IsEquipped(bool secondaryIsEquipped = true, MabiCreature creature = null)
		{
			if (!secondaryIsEquipped && creature == null)
				throw new ArgumentException("Creature cannot be null if you wish to eliminate secondary equips");

			switch ((Pocket)this.Info.Pocket)
			{
				case Pocket.Face:
				case Pocket.Hair:
				case Pocket.Accessory1:
				case Pocket.Accessory2:
				case Pocket.Head:
				case Pocket.Armor:
				case Pocket.Shoe:
				case Pocket.Glove:
				case Pocket.Robe:
				case Pocket.HeadStyle:
				case Pocket.ArmorStyle:
				case Pocket.ShoeStyle:
				case Pocket.GloveStyle:
				case Pocket.RobeStyle:
					return true;
				case Pocket.RightHand1:
				case Pocket.RightHand2:
					if (secondaryIsEquipped)
							return true;
					return creature.RightHand == this;

				case Pocket.LeftHand1:
				case Pocket.LeftHand2:
					if (secondaryIsEquipped)
							return true;
					return creature.LeftHand == this;

				default:
					return false;
			}
		}

		public override MabiVertex GetPosition()
		{
			return new MabiVertex(Info.X, Info.Y);
		}

		public void Move(Pocket pocket, uint x, uint y)
		{
			this.Move((byte)pocket, x, y);
		}

		public void Move(byte pocket, uint x, uint y)
		{
			this.Info.Pocket = pocket;
			this.Info.X = x;
			this.Info.Y = y;
		}

		public uint ReduceDurability(int val)
		{
			if (this.OptionInfo.Durability - val < 0)
				return (this.OptionInfo.Durability = 0);
			else
				return (this.OptionInfo.Durability -= (uint)val);
		}

		public override void AddToPacket(MabiPacket p)
		{
			this.AddToPacket(p, ItemPacketType.Public);
		}

		public void AddToPacket(MabiPacket p, ItemPacketType type)
		{
			p.PutLong(this.Id);
			p.PutByte((byte)type);
			p.PutBin(this.Info);

			if (type == ItemPacketType.Public)
			{
				p.PutByte(1);
				p.PutByte(0);

				p.PutByte(0); // Bitmask
				// if & 1
				//     float

				p.PutByte(1);
			}
			else if (type == ItemPacketType.Private)
			{
				p.PutBin(this.OptionInfo);
				p.PutString(this.Tags.ToString());
				p.PutString("");
				p.PutByte(0);
				p.PutLong(this.QuestId);
			}
		}
	}

	public enum ItemPacketType : byte { Public = 1, Private = 2 }

	public enum ItemType : ushort
	{
		Armor = 0,
		Headgear = 1,
		Glove = 2,
		Shoe = 3,
		Book = 4,
		Currency = 5,
		ItemBag = 6,
		Weapon = 7,
		Weapon2H = 8, // 2H, bows, tools, etc
		Weapon2 = 9, // Ineffective Weapons? Signs, etc.
		Instrument = 10,
		Shield = 11,
		Robe = 12,
		Accessory = 13,
		SecondaryWeapon = 14,
		MusicScroll = 15,
		Manual = 16,
		EnchantScroll = 17,
		CollectionBook = 18,
		ShopLicense = 19,
		FaliasTreasure = 20,
		Kiosk = 21,
		StyleArmor = 22,
		StyleHeadgear = 23,
		StyleGlove = 24,
		StyleShoe = 25,
		ComboCard = 27,
		Unknown2 = 28,
		Hair = 100,
		Face = 101,
		Usable = 501,
		Quest = 502,
		Usable2 = 503,
		Unknown1 = 504,
		Sac = 1000,
		Misc = 1001,
	}

	public enum BundleType : byte
	{
		None = 0,
		Stackable = 1,
		Sac = 2,
	}

	public enum UsableType : byte
	{
		Food = 0,
		Life = 1,
		Mana = 2,
		Stamina = 3,
		Injury = 4,
		LifeMana = 5,
		LifeStamina = 6,
		Antidote = 11,
		Recovery = 12,
		Others = 13,
		Elixir = 14,
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

		// Actual RIGHT hand (left side in inv).
		RightHand1 = 10,
		RightHand2 = 11,

		// Actual LEFT hand (right side in inv).
		LeftHand1 = 12,
		LeftHand2 = 13,

		// Arrows go here, not in the left hand.
		Magazine1 = 14,
		Magazine2 = 15,

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
		ArmorStyle = 43,
		GloveStyle = 44,
		ShoeStyle = 45,
		HeadStyle = 46,
		RobeStyle = 47,
		PersonalInventory = 49,
		VIPInventory = 50,
		FarmStone = 81,
		Inventory2 = 100,

		Max,
	}

	public enum AttackSpeed { VeryFast, Fast, Normal, Slow, VerySlow }

	public static class PocketExtensions
	{
		public static bool IsEquip(this Pocket pocket)
		{
			if ((pocket >= Pocket.Face && pocket <= Pocket.Accessory2) || (pocket >= Pocket.ArmorStyle && pocket <= Pocket.RobeStyle))
				return true;
			return false;
		}
	}
}
