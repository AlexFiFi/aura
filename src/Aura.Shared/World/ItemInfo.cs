// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Runtime.InteropServices;

namespace Aura.Shared.World
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

		/// <summary>
		/// State of the item? (eg. hoods and helmets)
		/// </summary>
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
}
