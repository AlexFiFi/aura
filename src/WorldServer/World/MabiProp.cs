// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Runtime.InteropServices;
using Common.Network;
using Common.World;

namespace World.World
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MabiPropInfo
	{
		public uint Class;
		public uint Region;
		public float X;
		public float Altitude;
		public float Y;
		public float Direction;
		public float Scale;
		public uint Color1;
		public uint Color2;
		public uint Color3;
		public uint Color4;
		public uint Color5;
		public uint Color6;
		public uint Color7;
		public uint Color8;
		public uint Color9;
		public byte FixedAltitude;
		private byte __unknown65;
		private byte __unknown66;
		private byte __unknown67;
	}

	public class MabiProp : MabiEntity
	{
		public MabiPropInfo Info;
		public string Title;

		private static ulong _propIndex = Common.Constants.Id.Props;

		public MabiProp(uint region = 0, uint area = 0)
		{
			this.Title = "";
			this.Region = region;

			this.Id = ++_propIndex;
			this.Id += (ulong)region << 32;
			this.Id += area << 16;

			this.Info.Scale = 1f;
			this.Info.Color1 =
			this.Info.Color2 =
			this.Info.Color3 =
			this.Info.Color4 =
			this.Info.Color5 =
			this.Info.Color6 =
			this.Info.Color7 =
			this.Info.Color8 =
			this.Info.Color9 = 0xFF808080;
		}

		public override EntityType EntityType
		{
			get { return EntityType.Prop; }
		}

		public override MabiVertex GetPosition()
		{
			return new MabiVertex((uint)this.Info.X, (uint)this.Info.Y);
		}

		public override uint Region
		{
			get { return this.Info.Region; }
			set { this.Info.Region = value; }
		}

		public override ushort DataType
		{
			get { return 160; }
		}

		public override void AddEntityData(MabiPacket packet)
		{
			packet.PutLong(this.Id);
			packet.PutInt(this.Info.Class);
			packet.PutString("");
			packet.PutString(this.Title);
			packet.PutBin(this.Info);
			packet.PutString("single");
			packet.PutLong(0);

			packet.PutByte(1);
			packet.PutString("");

			packet.PutInt(0);
			packet.PutShort(0);
		}
	}
}
