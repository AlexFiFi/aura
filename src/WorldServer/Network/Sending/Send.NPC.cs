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
		public static void NPCTalkStartResponse(WorldClient client, bool success, ulong npcId)
		{
			var packet = new MabiPacket(Op.NPCTalkStartR, client.Character.Id);
			packet.PutByte(success);
			if (success)
				packet.PutLong(npcId);

			client.Send(packet);
		}

		public static void NPCTalkPartnerStartResponse(WorldClient client, bool success, ulong id, string partnerName)
		{
			var packet = new MabiPacket(Op.NPCTalkPartnerR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutLong(id);
				packet.PutString(client.Character.Name + "'s " + partnerName);
				packet.PutString(client.Character.Name + "'s " + partnerName);
				client.Send(packet);
			}

			client.Send(packet);
		}

		public static void NPCTalkKeywordResponse(WorldClient client, bool success, string keyword)
		{
			var packet = new MabiPacket(Op.NPCTalkKeywordR, client.Character.Id);
			packet.PutByte(success);
			if (success)
				packet.PutString(keyword);

			client.Send(packet);
		}

		public static void NPCTalkSelectEnd(WorldClient client)
		{
			var packet = new MabiPacket(Op.NPCTalkSelectEnd, client.Character.Id);
			client.Send(packet);
		}

		public static void OpenNPCShop(WorldClient client, MabiShop shop)
		{
			var packet = new MabiPacket(Op.OpenNPCShop, client.Character.Id);
			packet.PutString("shopname");
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte((byte)shop.Tabs.Count);
			for (var i = 0; i < shop.Tabs.Count; ++i)
			{
				packet.PutString("[{0}]{1}", i, shop.Tabs[i].Name);

				// [160200] ?
				{
					packet.PutByte(0);
				}

				packet.PutShort((ushort)shop.Tabs[i].Items.Count);
				foreach (var item in shop.Tabs[i].Items)
					packet.AddItemInfo(item, ItemPacketType.Private);
			}
			client.Send(packet);
		}
	}
}
