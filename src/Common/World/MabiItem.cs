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
		public ushort Bundle;
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
		public ItemDbInfo DataInfo;

		public ItemType Type = ItemType.Misc;
		public BundleType BundleType;
		public ushort BundleMax;
		public uint StackItem;
		public byte Width, Height;

		private static ulong _worldItemIndex = 0x0050F00000000001;

		public MabiItem(uint itemClass, bool worldId = true)
		{
			this.Info.Class = itemClass;
			this.LoadDefault();

			if (worldId)
				this.Id = _worldItemIndex++;
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
			this.BundleType = itemToCopy.BundleType;
			this.BundleMax = itemToCopy.BundleMax;
			this.StackItem = itemToCopy.StackItem;
			this.Width = itemToCopy.Width;
			this.Height = itemToCopy.Height;

			this.Id = _worldItemIndex++;
		}

		public MabiItem(CharCardSetInfo cardItem)
			: this(cardItem.ItemId)
		{
			this.Info.Pocket = cardItem.Pocket;
			this.Info.ColorA = cardItem.Color1;
			this.Info.ColorB = cardItem.Color2;
			this.Info.ColorC = cardItem.Color3;
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

		public int Count { get { return this.Info.Bundle; } }

		public static MabiItem operator +(MabiItem item, int val)
		{
			if (item.Info.Bundle + val < 0)
				item.Info.Bundle = 0;
			else if (item.Info.Bundle + val > ushort.MaxValue)
				item.Info.Bundle = ushort.MaxValue;
			else
				item.Info.Bundle += (ushort)val;
			return item;
		}

		public static MabiItem operator -(MabiItem item, int val)
		{
			return item += -val;
		}

		public void LoadDefault()
		{
			var dbInfo = MabiData.ItemDb.Find(this.Info.Class);
			if (dbInfo != null)
			{
				this.DataInfo = dbInfo;

				this.Info.KnockCount = dbInfo.KnockCount;
				this.OptionInfo.KnockCount = dbInfo.KnockCount;

				this.OptionInfo.Durability = dbInfo.Durability;
				this.OptionInfo.DurabilityMax = dbInfo.Durability;
				this.OptionInfo.DurabilityOriginal = dbInfo.Durability;
				this.OptionInfo.AttackMin = dbInfo.AttackMin;
				this.OptionInfo.AttackMax = dbInfo.AttackMax;
				this.OptionInfo.Balance = dbInfo.Balance;
				this.OptionInfo.Critical = dbInfo.Critical;
				this.OptionInfo.Defense = dbInfo.Defense;
				this.OptionInfo.Protection = dbInfo.Protection;
				this.OptionInfo.Price = dbInfo.Price;
				this.OptionInfo.SellingPrice = dbInfo.SellingPrice;
				this.OptionInfo.WeaponType = dbInfo.WeaponType;
				this.OptionInfo.AttackSpeed = dbInfo.AttackSpeed;

				this.OptionInfo.Flag = 1;

				this.BundleMax = dbInfo.StackMax;
				this.BundleType = (BundleType)dbInfo.StackType;
				this.Type = (ItemType)dbInfo.Type;
				this.StackItem = dbInfo.StackItem;
				this.Width = dbInfo.Width;
				this.Height = dbInfo.Height;

				if (this.Type != ItemType.Sac && this.Info.Bundle < 1)
					this.Info.Bundle = 1;
				if (this.Type != ItemType.Sac && this.BundleMax < 1)
					this.BundleMax = 1;

				this.Info.ColorA = MabiData.ColorMapDb.GetRandom(dbInfo.ColorMap1);
				this.Info.ColorB = MabiData.ColorMapDb.GetRandom(dbInfo.ColorMap2);
				this.Info.ColorC = MabiData.ColorMapDb.GetRandom(dbInfo.ColorMap3);
				//if (dbInfo.ColorMode == 6 || dbInfo.ColorMode == 8)
				//{
				//    this.Info.ColorA = ((this.Info.ColorA & 0xFF) << 16) + ((this.Info.ColorA >> 8 & 0xFF) << 8) + (this.Info.ColorA >> 16 & 0xFF);
				//    this.Info.ColorB = ((this.Info.ColorB & 0xFF) << 16) + ((this.Info.ColorB >> 8 & 0xFF) << 8) + (this.Info.ColorB >> 16 & 0xFF);
				//    this.Info.ColorC = ((this.Info.ColorC & 0xFF) << 16) + ((this.Info.ColorC >> 8 & 0xFF) << 8) + (this.Info.ColorC >> 16 & 0xFF);
				//}
			}
			else
			{
				Logger.Warning("Item '" + this.Info.Class.ToString() + "' couldn't be found in the database.");
			}
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
				case Pocket.LeftHand1:
				case Pocket.LeftHand2:
				case Pocket.RightHand1:
				case Pocket.RightHand2:
				case Pocket.Shoe:
				case Pocket.Glove:
				case Pocket.Robe:
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
			p.PutLong(Id);
			p.PutByte(1);
			p.PutBin(Info);
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
			p.PutString("");
			p.PutString("");
			p.PutByte(0);
			p.PutLong(0);
		}
	}
}
