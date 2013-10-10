// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.World;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Player;

namespace Aura.World.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Sends item box packet for regular dyed item to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="itemId"></param>
		/// <param name="selected"></param>
		public static void AcquireDyedItem(Client client, MabiCreature creature, ulong itemId, byte selected)
		{
			var packet = new MabiPacket(Op.AcquireInfo2, creature.Id);
			packet.PutString("<xml type='dyeing' objectid='{0}' selected='{1}'/>", itemId, selected);
			packet.PutInt(3000);

			client.Send(packet);
		}

		/// <summary>
		/// Sends item box packet for fixed dyed item to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="itemId"></param>
		public static void AcquireDyedItem(Client client, MabiCreature creature, ulong itemId)
		{
			var packet = new MabiPacket(Op.AcquireInfo2, creature.Id);
			packet.PutString("<xml type='fixed_color_dyeing' objectid='{0}'/>", itemId);
			packet.PutInt(3000);

			client.Send(packet);
		}

		public static void ItemInfo(Client client, MabiCreature creature, MabiItem item)
		{
			var packet = new MabiPacket(Op.ItemNew, creature.Id);
			packet.AddItemInfo(item, ItemPacketType.Private);

			client.Send(packet);
		}

		public static void ItemUpdate(Client client, MabiCreature creature, MabiItem item)
		{
			var packet = new MabiPacket(Op.ItemUpdate, creature.Id);
			packet.AddItemInfo(item, ItemPacketType.Private);

			client.Send(packet);
		}

		/// <summary>
		/// Sends ItemMoveR to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="success"></param>
		public static void ItemMoveR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.ItemMoveR, creature.Id);
			packet.PutByte(success);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ItemMoveInfo or ItemSwitchInfo to creature's client,
		/// depending on whether collidingItem is null.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="item"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="collidingItem"></param>
		public static void ItemMoveInfo(MabiCreature creature, MabiItem item, Pocket source, MabiItem collidingItem)
		{
			var packet = new MabiPacket((uint)(collidingItem == null ? Op.ItemMoveInfo : Op.ItemSwitchInfo), creature.Id);
			packet.PutLong(item.Id);
			packet.PutByte((byte)source);
			packet.PutByte(item.Info.Pocket);
			packet.PutByte(2);
			packet.PutByte((byte)item.Info.X);
			packet.PutByte((byte)item.Info.Y);
			if (collidingItem != null)
			{
				packet.PutLong(collidingItem.Id);
				packet.PutByte(item.Info.Pocket);
				packet.PutByte(collidingItem.Info.Pocket);
				packet.PutByte(2);
				packet.PutByte((byte)collidingItem.Info.X);
				packet.PutByte((byte)collidingItem.Info.Y);
			}

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ItemAmount to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="item"></param>
		public static void ItemAmount(MabiCreature creature, MabiItem item)
		{
			var packet = new MabiPacket(Op.ItemAmount, creature.Id);
			packet.PutLong(item.Id);
			packet.PutShort(item.Info.Amount);
			packet.PutByte(2);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ItemRemove to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="item"></param>
		public static void ItemRemove(MabiCreature creature, MabiItem item)
		{
			var packet = new MabiPacket(Op.ItemRemove, creature.Id);
			packet.PutLong(item.Id);
			packet.PutByte(item.Info.Pocket);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ItemPickUpR to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="success"></param>
		public static void ItemPickUpR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.ItemPickUpR, creature.Id);
			packet.PutByte(success ? (byte)1 : (byte)2);

			creature.Client.Send(packet);
		}

		/// <summary>
		/// Sends ItemSplitR to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="success"></param>
		public static void ItemSplitR(MabiCreature creature, bool success)
		{
			var packet = new MabiPacket(Op.ItemSplitR, creature.Id);
			packet.PutByte(success);

			creature.Client.Send(packet);
		}
	}
}
