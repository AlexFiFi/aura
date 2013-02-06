// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Runtime.InteropServices;
using Common.Constants;
using Common.Network;
using Common.Data;
using System.Net;
using System;
using Common.Tools;

namespace Common.World
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MabiItemInfo
	{
		public byte Pocket;
		private byte __unknown2;
		private byte __unknown3;
		private byte __unknown4;
		public uint Class;
		public uint ColorA;
		public uint ColorB;
		public uint ColorC;
		public ushort Amount;
		private ushort __unknown7;
		public uint Region;
		public uint X;
		public uint Y;
		public byte FigureA;
		public byte uFigureB;
		public byte uFigureC;
		public byte uFigureD;
		public byte KnockCount;
		private byte __unknown12;
		private byte __unknown13;
		private byte __unknown14;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MabiItemOptionInfo
	{
		public byte Flag;
		private byte __unknown15;
		private byte __unknown16;
		private byte __unknown17;
		public uint Price;
		public uint SellingPrice;
		public uint LinkedPocketId;
		public uint Durability;
		public uint DurabilityMax;
		public uint DurabilityOriginal;
		public ushort AttackMin;
		public ushort AttackMax;
		public ushort WAttackMin;
		public ushort WAttackMax;
		public byte Balance;
		public byte Critical;
		private byte __unknown24;
		private byte __unknown25;
		public uint Defense;
		public short Protection;
		public ushort EffectiveRange;
		public byte AttackSpeed;
		public byte KnockCount;
		public ushort Experience;
		public byte EP;
		public byte Upgraded;
		public byte UpgradeMax;
		public byte WeaponType;
		public uint Grade;
		public ushort Prefix;
		public ushort Suffix;
		public ushort Elemental;
		private ushort __unknown31;
		public uint ExpireTime;
		public uint StackRemainingTime;
		public uint JoustPointPrice;
		public uint DucatPrice;
	}

	/// <summary>
	/// Info/OptionInfo: These two fields containt structs that contain
	///     values which are send to the client. We simply turn the structs
	///     into bins, that's why those values are in there.
	/// DataInfo: This on the other hand is the data that we read from the
	///     item db file. Some of these values are use for fields in Info/
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
		public string AdditionalInfo = "";

		public ulong QuestId;

		private static ulong _worldItemIndex = Common.Constants.Id.TmpItems;
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

			this.Type = itemToCopy.Type;
			this.StackType = itemToCopy.StackType;
			this.StackMax = itemToCopy.StackMax;
			this.StackItem = itemToCopy.StackItem;
			this.Width = itemToCopy.Width;
			this.Height = itemToCopy.Height;

			this.Id = MabiItem.NewItemId;
		}

		public MabiItem(CharCardSetInfo cardItem)
			: this(cardItem.ItemId)
		{
			this.Info.Pocket = cardItem.Pocket;
			this.Info.ColorA = cardItem.Color1;
			this.Info.ColorB = cardItem.Color2;
			this.Info.ColorC = cardItem.Color3;
		}

		public MabiItem(MabiQuest quest)
			: this(70024, false)
		{
			this.Id = quest.Id - Constants.Id.QuestItemOffset;
			this.Info.Pocket = (byte)Pocket.Quest;
			this.OptionInfo.Flag = 1;

			this.QuestId = quest.Id;
			//this.AdditionalInfo = quest.ToolTip;
		}

		public override EntityType EntityType
		{
			get { return EntityType.Item; }
		}

		// Has to get overriden for MabiEntity ~_~
		public override uint Region
		{
			get { return this.Info.Region; }
			set { this.Info.Region = value; }
		}

		public override ushort DataType
		{
			get { return 80; }
		}

		public int Count { get { return this.Info.Amount; } }

		//public static MabiItem operator +(MabiItem item, int val)
		//{
		//    if (item.Info.Amount + val < 0)
		//        item.Info.Amount = 0;
		//    else if (item.Info.Amount + val > ushort.MaxValue)
		//        item.Info.Amount = ushort.MaxValue;
		//    else if (item.Info.Amount + val > item.StackMax)
		//        item.Info.Amount = item.StackMax;
		//    else
		//        item.Info.Amount += (ushort)val;
		//    return item;
		//}

		//public static MabiItem operator -(MabiItem item, int val)
		//{
		//    return item += -val;
		//}

		//public static MabiItem operator ++(MabiItem item)
		//{
		//    return item += 1;
		//}

		//public static MabiItem operator --(MabiItem item)
		//{
		//    return item += -1;
		//}

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
				Logger.Warning("Item '" + this.Info.Class.ToString() + "' couldn't be found in the database.");
			}

			if (this.StackType != BundleType.Sac && this.Info.Amount < 1)
				this.Info.Amount = 1;
			if (this.StackMax < 1)
				this.StackMax = 1;
		}

		public bool IsEquipped()
		{
			switch ((Pocket)this.Info.Pocket)
			{
				case Pocket.Face:
				case Pocket.Hair:
				case Pocket.Accessory1:
				case Pocket.Accessory2:
				case Pocket.Head:
				case Pocket.Armor:
				case Pocket.RightHand1:
				case Pocket.RightHand2:
				case Pocket.LeftHand1:
				case Pocket.LeftHand2:
				case Pocket.Shoe:
				case Pocket.Glove:
				case Pocket.Robe:
				case Pocket.HeadStyle:
				case Pocket.ArmorStyle:
				case Pocket.ShoeStyle:
				case Pocket.GloveStyle:
				case Pocket.RobeStyle:
					return true;
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

		public override void AddEntityData(MabiPacket p)
		{
			p.PutLong(this.Id);
			p.PutByte(1);
			p.PutBin(this.Info);
			p.PutByte(1);
			p.PutByte(0);
			p.PutByte(0);
			p.PutByte(1);
		}

		public void AddPrivateEntityData(MabiPacket p)
		{
			p.PutLong(this.Id);
			p.PutByte(2);
			p.PutBin(this.Info);
			p.PutBin(this.OptionInfo);
			p.PutString(this.AdditionalInfo); // Additional information, like for quests.
			p.PutString("");
			p.PutByte(0);
			p.PutLong(this.QuestId);
		}
	}
}
