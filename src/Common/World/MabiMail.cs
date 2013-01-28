using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Database;
using Common.Network;

namespace Common.World
{
	public enum MailTypes : byte
	{
		Normal = 1,
		Item = 2,
		Return = 3,
		Payment = 4
	}

	public class MabiMail
	{
		public ulong MessageId, SenderId, RecipientId, ItemId;
		public string SenderName, RecipientName, Text;
		public uint COD;
		public DateTime Sent;
		/// <summary>
		/// 1 = unread, 2 = read
		/// </summary>
		public byte Read;
		public byte Type;

		public MabiMail()
		{
			MessageId = SenderId = RecipientId = ItemId = COD = 0;
			Read = 1;
			Type = (byte)MailTypes.Normal;
		}

		public void Save(bool setSent)
		{
			if (setSent)
				this.Sent = DateTime.Today;

			MabiDb.Instance.SaveMail(this);
		}

		public void Delete()
		{
			MabiDb.Instance.DeleteMail(this.MessageId);
		}

		public void Return(string message)
		{
			MabiMail m = new MabiMail();
			m.COD = 0;
			m.ItemId = this.ItemId;
			m.SenderId = this.RecipientId;
			m.SenderName = this.RecipientName;
			m.RecipientId = this.SenderId;
			m.RecipientName = this.SenderName;
			m.Text = message;
			m.Type = (byte)MailTypes.Return;
			this.Delete();
			m.Save(true);
		}

		public void AddEntityData(MabiPacket packet, MabiEntity forEntity)
		{
			packet.PutLong(this.MessageId);
			packet.PutByte(this.Type);
			packet.PutByte(this.Read);
			packet.PutLong((ulong)this.Sent.Ticks / 10000);
			packet.PutString(this.SenderName);
			packet.PutString(this.RecipientName);
			packet.PutString(this.Text);
			packet.PutLong(this.ItemId);

			if (this.ItemId != 0)
			{
				packet.PutInt(this.COD);

				// TODO: No, just... no.
				var item = MabiDb.Instance.GetItem(this.ItemId);

				item.AddPrivateEntityData(packet);
			}
		}

		public static List<MabiMail> FindAllSent(MabiEntity e)
		{
			return FindAllSent(e.Id);
		}

		public static List<MabiMail> FindAllSent(ulong senderId)
		{
			return MabiDb.Instance.GetSentMail(senderId);
		}

		public static List<MabiMail> FindAllRecieved(MabiEntity e)
		{
			return FindAllRecieved(e.Id);
		}

		public static List<MabiMail> FindAllRecieved(ulong recipientId)
		{
			return MabiDb.Instance.GetRecievedMail(recipientId);
		}

		public static int GetUnreadCount(MabiEntity e)
		{
			return GetUnreadCount(e.Id);
		}

		public static int GetUnreadCount(ulong Id)
		{
			return FindAllRecieved(Id).Count(m => m.Read == 1);
		}

		public static MabiMail GetMail(ulong mailId)
		{
			return MabiDb.Instance.GetMail(mailId);
		}
	}
}
