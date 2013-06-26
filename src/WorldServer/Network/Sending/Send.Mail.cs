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
using Aura.World.Database;

namespace Aura.World.Network
{
	public static partial class Send
	{
		public static void UnreadMailCount(Client client, MabiCreature creature, uint count)
		{
			var packet = new MabiPacket(Op.UnreadMailCount, creature.Id);
			packet.PutInt(count);

			client.Send(packet);
		}

		public static void GetMailsResponse(WorldClient client, IEnumerable<MabiMail> mails)
		{
			var p = new MabiPacket(Op.GetMailsR, client.Character.Id);
			foreach (var mail in mails)
				p.Add(mail);
			p.PutLong(0);

			client.Send(p);
		}

		public static void ConfirmMailRecipentResponse(WorldClient client, bool success, ulong recipientId)
		{
			var packet = new MabiPacket(Op.ConfirmMailRecipentR, client.Character.Id);
			packet.PutByte(success);
			if (success)
				packet.PutLong(recipientId);

			client.Send(packet);
		}

		public static void SendMailFail(WorldClient client)
		{
			SendMailResponse(client, null);
		}

		public static void SendMailResponse(WorldClient client, MabiMail mail)
		{
			var packet = new MabiPacket(Op.SendMailR, client.Character.Id);
			if (mail != null)
			{
				packet.PutByte(true);
				packet.Add(mail);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		public static void MarkMailReadResponse(WorldClient client, bool success, ulong mailId)
		{
			var packet = new MabiPacket(Op.MarkMailReadR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutByte(success);
				packet.PutLong(mailId);
			}

			client.Send(packet);
		}

		public static void ReturnMailResponse(WorldClient client, bool success, ulong mailId)
		{
			var packet = new MabiPacket(Op.ReturnMailR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutByte(success);
				packet.PutLong(mailId);
			}

			client.Send(packet);
		}

		public static void RecallMailResponse(WorldClient client, bool success, ulong mailId)
		{
			var packet = new MabiPacket(Op.RecallMailR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutByte(success);
				packet.PutLong(mailId);
			}

			client.Send(packet);
		}

		public static void ReceiveMailItemResponse(WorldClient client, bool success, ulong mailId)
		{
			var packet = new MabiPacket(Op.ReceiveMailItemR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutByte(success);
				packet.PutLong(mailId);
			}

			client.Send(packet);
		}

		public static void DeleteMailResponse(WorldClient client, bool success, ulong mailId)
		{
			var packet = new MabiPacket(Op.DeleteMailR, client.Character.Id);
			packet.PutByte(success);
			if (success)
			{
				packet.PutByte(success);
				packet.PutLong(mailId);
			}

			client.Send(packet);
		}

		private static void Add(this MabiPacket packet, MabiMail mail)
		{
			packet.PutLong(mail.MessageId);
			packet.PutByte(mail.Type);
			packet.PutByte(mail.Read);
			packet.PutLong(mail.Sent);
			packet.PutString(mail.SenderName);
			packet.PutString(mail.RecipientName);
			packet.PutString(mail.Text);
			packet.PutLong(mail.ItemId);

			if (mail.ItemId != 0)
			{
				packet.PutInt(mail.COD);

				var item = WorldDb.Instance.GetItem(mail.ItemId);
				packet.AddItemInfo(item, ItemPacketType.Private);
			}
		}
	}
}
