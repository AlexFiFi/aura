// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.World.Database;

namespace Aura.World.World
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

			WorldDb.Instance.SaveMail(this);
		}

		public void Delete()
		{
			WorldDb.Instance.DeleteMail(this.MessageId);
		}

		public void Return(string format, params object[] args)
		{
			MabiMail m = new MabiMail();
			m.COD = 0;
			m.ItemId = this.ItemId;
			m.SenderId = this.RecipientId;
			m.SenderName = this.RecipientName;
			m.RecipientId = this.SenderId;
			m.RecipientName = this.SenderName;
			m.Text = string.Format(format, args);
			m.Type = (byte)MailTypes.Return;
			this.Delete();
			m.Save(true);
		}

		public static List<MabiMail> FindAllSent(MabiEntity e)
		{
			return FindAllSent(e.Id);
		}

		public static List<MabiMail> FindAllSent(ulong senderId)
		{
			return WorldDb.Instance.GetSentMail(senderId);
		}

		public static List<MabiMail> FindAllRecieved(MabiEntity e)
		{
			return FindAllRecieved(e.Id);
		}

		public static List<MabiMail> FindAllRecieved(ulong recipientId)
		{
			return WorldDb.Instance.GetRecievedMail(recipientId);
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
			return WorldDb.Instance.GetMail(mailId);
		}
	}
}
