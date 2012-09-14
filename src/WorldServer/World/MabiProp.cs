// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Network;
using Common.World;

namespace World.World
{
	public class MabiProp : MabiEntity
	{
		private UInt64 _baseId;
		public uint PropType;
		public uint LocX;
		public uint LocY;
		public uint[] Colors;

		private static UInt64 _propIdIndex = 0;

		private static UInt64 assignPropId()
		{
			return _propIdIndex++;
		}

		public MabiProp()
		{
			_baseId = assignPropId();
			Colors = new uint[] { 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080, 0xFF808080 };
		}

		public override EntityType EntityType
		{
			get { return EntityType.Prop; }
		}

		public override ulong Id
		{
			get { return _baseId + 0x00A1000100090000; }
		}

		public override MabiVertex GetPosition()
		{
			return new MabiVertex((uint)LocX, (uint)LocY);
		}

		public override ushort DataType
		{
			get { return 160; }
		}

		private byte[] GetBinData()
		{
			byte[] data = new byte[68];

			Array.Copy(BitConverter.GetBytes(this.PropType), data, 4);
			Array.Copy(BitConverter.GetBytes((uint)Region), 0, data, 4, 4);
			Array.Copy(BitConverter.GetBytes((float)LocX), 0, data, 8, 4);
			Array.Copy(BitConverter.GetBytes((uint)0), 0, data, 12, 4); // 00000000
			Array.Copy(BitConverter.GetBytes((float)LocY), 0, data, 16, 4); // ?
			Array.Copy(BitConverter.GetBytes((float)1.39), 0, data, 20, 4); // ?
			Array.Copy(BitConverter.GetBytes((float)1), 0, data, 24, 4); // scale
			Array.Copy(BitConverter.GetBytes(Colors[0]), 0, data, 28, 4);
			Array.Copy(BitConverter.GetBytes(Colors[1]), 0, data, 32, 4);
			Array.Copy(BitConverter.GetBytes(Colors[2]), 0, data, 36, 4);
			Array.Copy(BitConverter.GetBytes(Colors[3]), 0, data, 40, 4);
			Array.Copy(BitConverter.GetBytes(Colors[4]), 0, data, 44, 4);
			Array.Copy(BitConverter.GetBytes(Colors[5]), 0, data, 48, 4);
			Array.Copy(BitConverter.GetBytes(Colors[6]), 0, data, 52, 4);
			Array.Copy(BitConverter.GetBytes(Colors[7]), 0, data, 56, 4);
			Array.Copy(BitConverter.GetBytes(Colors[8]), 0, data, 60, 4);
			Array.Copy(BitConverter.GetBytes(0), 0, data, 64, 4); // 00000000

			return data;
		}


		public override void AddEntityData(MabiPacket packet)
		{
			packet.PutLong(Id);
			packet.PutInt(PropType);
			packet.PutString(""); // ?
			packet.PutString(""); // Guild Name

			/*
			 * 119D0000 TYPE
			 * 01000000 REGION ID
			 * 0008C046
			 * 00000000
			 * 00800447 
			 * 9A99193E
			 * 0000803F
			 * 969696FF
			 * 8CA5AAFF
			 * 808080FF
			 * 808080FF
			 * 808080FF
			 * 808080FF
			 * 808080FF
			 * 808080FF
			 * 808080FF
			 * 00000000
			 */
			packet.PutBin(GetBinData());

			packet.PutString("single"); // ?
			packet.PutLong(0); // ?
			packet.PutByte(1);
			packet.PutString(""); // XML data
			packet.PutInt(0);
			packet.PutShort(0);
		}
	}
}
