// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.World;

namespace Aura.World.Network
{
	public static partial class Send
	{
		public static void SystemMessage(Client client, MabiCreature creature, string format, params object[] args)
		{
			SystemMessage(client, creature, "<SYSTEM>", format, args);
		}

		public static void ServerMessage(Client client, MabiCreature creature, string format, params object[] args)
		{
			SystemMessage(client, creature, "<SERVER>", format, args);
		}

		public static void CombatMessage(Client client, MabiCreature creature, string format, params object[] args)
		{
			SystemMessage(client, creature, "<COMBAT>", format, args);
		}

		public static void SystemMessage(Client client, MabiCreature target, string from, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.Chat, target.Id);
			packet.PutByte(0);
			packet.PutString(from);
			packet.PutString(args.Length < 1 ? format : string.Format(format, args));
			packet.PutByte(1);
			packet.PutSInt(-32640);
			packet.PutInt(0);
			packet.PutByte(0);

			client.Send(packet);
		}

		public static void Notice(Client client, string format, params object[] args)
		{
			Notice(client, NoticeType.Middle, 0, format, args);
		}

		public static void Notice(Client client, NoticeType type, string format, params object[] args)
		{
			Notice(client, type, 0, format, args);
		}

		public static void Notice(Client client, NoticeType type, uint duration, string format, params object[] args)
		{
			client.Send(GetNotice(type, duration, format, args));
		}

		/// <summary>
		/// Broadcasts notice in region.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void RegionNotice(uint region, string format, params object[] args)
		{
			WorldManager.Instance.BroadcastRegion(GetNotice(NoticeType.Middle, 0, format, args), region);
		}

		/// <summary>
		/// Broadcasts notice to all players.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void AllNotice(NoticeType type, string format, params object[] args)
		{
			AllNotice(type, 0, format, args);
		}

		/// <summary>
		/// Broadcasts notice to all players.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void AllNotice(NoticeType type, uint duration, string format, params object[] args)
		{
			WorldManager.Instance.Broadcast(GetNotice(type, duration, format, args), SendTargets.All);
		}

		private static MabiPacket GetNotice(NoticeType type, uint duration, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.Notice, Id.Broadcast);
			packet.PutByte((byte)type);
			packet.PutString(string.Format(format, args));
			if (duration > 0)
				packet.PutInt(duration);

			return packet;
		}

		public static void MsgBox(Client client, MabiCreature creature, string format, params object[] args)
		{
			MsgBox(client, creature, MsgBoxTitle.Notice, MsgBoxButtons.Close, MsgBoxAlign.Center, format, args);
		}

		public static void MsgBox(Client client, MabiCreature creature, MsgBoxTitle title, MsgBoxButtons buttons, MsgBoxAlign align, string format, params object[] args)
		{
			MsgBox(client, creature, title.ToString(), MsgBoxButtons.Close, MsgBoxAlign.Center, format, args);
		}

		public static void MsgBox(Client client, MabiCreature creature, string message, string title, MsgBoxButtons buttons, MsgBoxAlign align, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.MsgBox, creature.Id);
			packet.PutString(format, args);

			// Can be send with the title enum as byte as well.
			packet.PutString(title);

			packet.PutByte((byte)buttons);
			packet.PutByte((byte)align);

			client.Send(packet);
		}
	}

	public enum MsgBoxTitle { Notice, Info, Warning, Confirm }
	public enum MsgBoxButtons { None, Close, OkCancel, YesNoCancel }
	public enum MsgBoxAlign { Left, Center }
	public enum NoticeType { Top = 1, TopRed, MiddleTop, Middle, Left, TopGreen, MiddleSystem, System, MiddleLower }
}
