// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.World;
using Common.Network;
using System.Collections.Generic;
using Common.Constants;
using System;

namespace World.Network
{
	public enum MsgBoxTitle { Notice, Info, Warning, Confirm }
	public enum MsgBoxButtons { None, Close, OkCancel, YesNoCancel }
	public enum MsgBoxAlign { Left, Center }
	public enum NoticeType { Top = 1, TopRed, MiddleTop, Middle, Left, TopGreen, MiddleSystem, System, MiddleLower }

	/// <summary>
	/// Packet creator for often used packets
	/// </summary>
	public static class PacketCreator
	{
		public static MabiPacket SystemMessage(MabiCreature target, string from, string message, params object[] args)
		{
			var p = new MabiPacket(Op.Chat, target.Id);

			p.PutByte(0);
			p.PutString(from);
			p.PutString(args.Length < 1 ? message : string.Format(message, args));
			p.PutByte(1);
			p.PutSInt(-32640);
			p.PutInt(0);
			p.PutByte(0);

			return p;
		}

		public static MabiPacket SystemMessage(MabiCreature target, string message, params object[] args)
		{
			return PacketCreator.SystemMessage(target, "<SYSTEM>", message, args);
		}

		public static MabiPacket ServerMessage(MabiCreature target, string message, params object[] args)
		{
			return PacketCreator.SystemMessage(target, "<SERVER>", message, args);
		}

		public static MabiPacket CombatMessage(MabiCreature target, string message, params object[] args)
		{
			return PacketCreator.SystemMessage(target, "<COMBAT>", message);
		}

		public static MabiPacket MsgBox(MabiCreature target, string message, MsgBoxTitle title = MsgBoxTitle.Notice, MsgBoxButtons buttons = MsgBoxButtons.Close, MsgBoxAlign align = MsgBoxAlign.Center)
		{
			var p = new MabiPacket(Op.MsgBox, target.Id);

			p.PutString(message);
			p.PutByte((byte)title);
			p.PutByte((byte)buttons);
			p.PutByte((byte)align);

			return p;
		}

		public static MabiPacket MsgBox(MabiCreature target, string message, string title, MsgBoxButtons buttons = MsgBoxButtons.Close, MsgBoxAlign align = MsgBoxAlign.Center)
		{
			var p = new MabiPacket(Op.MsgBox, target.Id);

			p.PutString(message);
			p.PutString(title);
			p.PutByte((byte)buttons);
			p.PutByte((byte)align);

			return p;
		}

		public static MabiPacket Notice(ulong Id, string message, NoticeType type = NoticeType.Middle, uint duration = 0)
		{
			var p = new MabiPacket(Op.Notice, Id);

			p.PutByte((byte)type);
			p.PutString(message);
			if (duration > 0)
			{
				p.PutInt(duration);
			}

			return p;
		}

		public static MabiPacket Notice(MabiCreature target, string message, NoticeType type = NoticeType.Middle, uint duration = 0)
		{
			return Notice(target.Id, message, type, duration);
		}

		/// <summary>
		/// Sends a notice to all clients.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static MabiPacket Notice(string message, NoticeType type = NoticeType.Middle, uint duration = 0)
		{
			return Notice(Id.Broadcast, message, type, duration);
		}

		public static MabiPacket Whisper(MabiCreature target, string sender, string msg)
		{
			var p = new MabiPacket(Op.WhisperChat, target.Id);
			p.PutStrings(sender, msg);
			return p;
		}

		public static MabiPacket EntitiesAppear(List<MabiEntity> entities)
		{
			var p = new MabiPacket(Op.EntitiesSpawn, Id.Broadcast);

			p.PutShort((ushort)entities.Count);
			foreach (var entity in entities)
			{
				var data = new MabiPacket(0, 0);
				entity.AddEntityData(data);
				var dataBytes = data.Build(false);

				p.PutShort(entity.DataType);
				p.PutInt((uint)dataBytes.Length);
				p.PutBin(dataBytes);
			}

			return p;
		}

		public static MabiPacket EntityAppears(MabiEntity entity)
		{
			var op = Op.EntityAppears;
			if (entity is MabiItem)
				op = Op.ItemAppears;

			var p = new MabiPacket(op, Id.Broadcast);
			entity.AddEntityData(p);
			return p;
		}

		public static MabiPacket EntitiesLeave(List<MabiEntity> entities)
		{
			var p = new MabiPacket(Op.EntitiesDisappear, Id.Broadcast);

			p.PutShort((ushort)entities.Count);
			foreach (var entity in entities)
			{
				p.PutShort(entity.DataType);
				p.PutLong(entity.Id);
			}

			return p;
		}

		public static MabiPacket EntityLeaves(MabiEntity entity)
		{
			uint op = Op.EntityDisappears;
			if (entity is MabiItem)
				op = Op.ItemDisappears;

			var p = new MabiPacket(op, Id.Broadcast);
			p.PutLong(entity.Id);
			p.PutByte(0);

			return p;
		}

		public static MabiPacket EnterRegionPermission(MabiEntity entity, bool permission = true)
		{
			var p = new MabiPacket(Op.EnterRegionPermission, Id.World);
			var pos = entity.GetPosition();

			p.PutLong(entity.Id);
			if (permission)
			{
				p.PutByte(1);
				p.PutInt(entity.Region);
				p.PutInt(pos.X);
				p.PutInt(pos.Y);
			}
			else
			{
				p.PutByte(0);
			}

			return p;
		}

		public static MabiPacket ItemInfo(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.ItemNew, creature.Id);
			item.AddPrivateEntityData(p);
			return p;
		}

		public static MabiPacket ItemRemove(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.ItemRemove, creature.Id);
			p.PutLong(item.Id);
			p.PutByte(item.Info.Pocket);
			return p;
		}

		public static MabiPacket ItemAmount(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.ItemAmount, creature.Id);
			p.PutLong(item.Id);
			p.PutShort(item.Info.Amount);
			p.PutByte(2);
			return p;
		}

		public static MabiPacket Lock(MabiCreature creature)
		{
			var p = new MabiPacket(Op.CharacterLock, creature.Id);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			return p;
		}

		public static MabiPacket Unlock(MabiCreature creature)
		{
			var p = new MabiPacket(Op.CharacterUnlock, creature.Id);
			p.PutInt(0xEFFFFFFE);
			return p;
		}

		//public static MabiPacket Effect(MabiCreature creature, uint id, params object[] args)
		//{
		//    var p = new MabiPacket(Op.Effect, creature.Id);
		//    p.PutInt(id);

		//    foreach (var arg in args)
		//        p.Put(arg);

		//    return p;
		//}

		public static MabiPacket TurnTo(MabiEntity creature, MabiEntity target)
		{
			var cpos = creature.GetPosition();
			var tpos = target.GetPosition();

			var p = new MabiPacket(Op.TurnTo, creature.Id);
			p.PutFloat((float)tpos.X - (float)cpos.X);
			p.PutFloat((float)tpos.Y - (float)cpos.Y);

			return p;
		}
	}
}
