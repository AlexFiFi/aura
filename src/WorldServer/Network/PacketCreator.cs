// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.World;
using Aura.Data;
using Aura.World.Player;
using Aura.Shared.Util;

namespace Aura.World.Network
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
			return PacketCreator.SystemMessage(target, "<COMBAT>", message, args);
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

		public static MabiPacket MsgBoxFormat(MabiCreature target, string message, params object[] args)
		{
			return MsgBox(target, string.Format(message, args));
		}

		public static MabiPacket Notice(ulong id, string message, NoticeType type = NoticeType.Middle, uint duration = 0)
		{
			var p = new MabiPacket(Op.Notice, id);

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

		public static MabiPacket EntitiesAppear(IEnumerable<MabiEntity> entities)
		{
			var p = new MabiPacket(Op.EntitiesSpawn, Id.Broadcast);

			p.PutShort((ushort)entities.Count());
			foreach (var entity in entities)
			{
				var data = new MabiPacket(0, 0);
				entity.AddToPacket(data);
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
			entity.AddToPacket(p);
			return p;
		}

		public static MabiPacket EntitiesLeave(IEnumerable<MabiEntity> entities)
		{
			var p = new MabiPacket(Op.EntitiesDisappear, Id.Broadcast);

			p.PutShort((ushort)entities.Count());
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

		public static MabiPacket GuildMessage(MabiGuild guild, MabiCreature target, string message)
		{
			var character = target as MabiPC;
			return new MabiPacket(Op.GuildMessage, target.Id)
				.PutLong(guild.Id)
				.PutString(character == null ? "Aura" : character.Server)
				.PutLong(target.Id)
				.PutString(guild.Name)
				.PutString(message)
				.PutByte(1)
				.PutByte(1);
		}

		/// <summary>
		/// Sends info on guild membership status changed. Pass null for guild to remove.
		/// </summary>
		/// <param name="guild"></param>
		/// <param name="creature"></param>
		/// <returns></returns>
		public static MabiPacket GuildMembershipChanged(MabiGuild guild, MabiCreature creature, byte rank = (byte)GuildMemberRank.Member)
		{
			var p = new MabiPacket(Op.GuildMembershipChanged, creature.Id);
			if (guild == null)
			{
				p.PutInt(0);
			}
			else
			{
				p.PutInt(1);
				p.PutString(guild.Name);
				p.PutLong(guild.Id);
				p.PutInt((uint)rank); // (5) Member Rank?
				p.PutByte(0);
			}
			return p;
		}

		public static MabiPacket ItemInfo(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.ItemNew, creature.Id);
			item.AddToPacket(p, ItemPacketType.Private);
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

		//public static MabiPacket Effect(MabiCreature creature, uint id, params object[] args)
		//{
		//    var p = new MabiPacket(Op.Effect, creature.Id);
		//    p.PutInt(id);

		//    foreach (var arg in args)
		//        p.Put(arg);

		//    return p;
		//}

		/// <summary>
		/// Lets the creature face the target.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static MabiPacket TurnTo(MabiEntity creature, MabiEntity target)
		{
			var cpos = creature.GetPosition();
			var tpos = target.GetPosition();

			var p = new MabiPacket(Op.TurnTo, creature.Id);
			p.PutFloat((float)tpos.X - (float)cpos.X);
			p.PutFloat((float)tpos.Y - (float)cpos.Y);

			return p;
		}

		public static MabiPacket AcquireItem(MabiCreature creature, uint cls, uint amount)
		{
			var p = new MabiPacket(Op.AcquireInfo, creature.Id);
			p.PutString("<xml type='item' classid='{0}' value='{1}'/>", cls, amount);
			p.PutInt(3000);
			return p;
		}

		public static MabiPacket AcquireItem(MabiCreature creature, ulong itemId)
		{
			var p = new MabiPacket(Op.AcquireInfo, creature.Id);
			p.PutString("<xml type='item' objectid='{0}'/>", itemId);
			p.PutInt(3000);

			// 001 [................] String : <xml type='stamina' value='17' simple='true' onlyLog='false' />
			// 002 [........00000BB8] Int    : 3000

			return p;
		}

		public static MabiPacket AcquireExp(MabiCreature creature, uint amount)
		{
			var p = new MabiPacket(Op.AcquireInfo, creature.Id);
			p.PutString("<xml type='exp' value='{0}'/>", amount);
			p.PutInt(3000);
			return p;
		}

		public static MabiPacket AcquireAp(MabiCreature creature, uint amount)
		{
			var p = new MabiPacket(Op.AcquireInfo, creature.Id);
			p.PutString("<xml type='ap' value='{0}' simple='true' onlyLog='false' />", amount);
			p.PutInt(3000);
			return p;
		}

		public static MabiPacket SpawnEffect(MabiEntity entity, SpawnEffect type)
		{
			return SpawnEffect(entity, type, entity.GetPosition());
		}


		public static MabiPacket SpawnEffect(MabiEntity entity, SpawnEffect type, MabiVertex pos)
		{
			return
				new MabiPacket(Op.Effect, entity.Id)
				.PutInt(Effect.Spawn)
				.PutInt(entity.Region)
				.PutFloats(pos.X, pos.Y)
				.PutByte((byte)type);
		}

		public static MabiPacket ItemUpdate(MabiCreature creature, MabiItem item)
		{
			var p = new MabiPacket(Op.ItemUpdate, creature.Id);
			item.AddToPacket(p, ItemPacketType.Private);
			return p;
		}

		public static MabiPacket StatUpdate(MabiCreature creature, StatUpdateType type, params Stat[] stats)
		{
			var packet = new MabiPacket((type & StatUpdateType.Public) != 0 ? Op.StatUpdatePublic : Op.StatUpdatePrivate, creature.Id);

			packet.PutByte((byte)type);

			// Stats
			packet.PutSInt(stats.Length);
			foreach (var stat in stats)
			{
				packet.PutInt((uint)stat);
				switch (stat)
				{
					case Stat.Height: packet.PutFloat(creature.Height); break;
					case Stat.Weight: packet.PutFloat(creature.Fat); break;
					case Stat.Upper: packet.PutFloat(creature.Upper); break;
					case Stat.Lower: packet.PutFloat(creature.Lower); break;

					case Stat.CombatPower: packet.PutFloat(creature.CombatPower); break;
					case Stat.Level: packet.PutShort(creature.Level); break;
					case Stat.AbilityPoints: packet.PutShort(creature.AbilityPoints); break;
					case Stat.Experience: packet.PutLong(MabiData.ExpDb.CalculateRemaining(creature.Level, creature.Experience) * 1000); break;

					case Stat.Life: packet.PutFloat(creature.Life); break;
					case Stat.LifeMax: packet.PutFloat(creature.LifeMaxBase); break;
					case Stat.LifeMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.LifeMaxMod)); break;
					case Stat.LifeInjured: packet.PutFloat(creature.LifeInjured); break;
					case Stat.Mana: packet.PutFloat(creature.Mana); break;
					case Stat.ManaMax: packet.PutFloat(creature.ManaMaxBase); break;
					case Stat.ManaMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.ManaMaxMod)); break;
					case Stat.Stamina: packet.PutFloat(creature.Stamina); break;
					case Stat.Food: packet.PutFloat(creature.StaminaHunger); break;
					case Stat.StaminaMax: packet.PutFloat(creature.StaminaMaxBase); break;
					case Stat.StaminaMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.StaminaMaxMod)); break;

					case Stat.StrMod: packet.PutFloat(creature.StatMods.GetMod(Stat.StrMod)); break;
					case Stat.DexMod: packet.PutFloat(creature.StatMods.GetMod(Stat.DexMod)); break;
					case Stat.IntMod: packet.PutFloat(creature.StatMods.GetMod(Stat.IntMod)); break;
					case Stat.LuckMod: packet.PutFloat(creature.StatMods.GetMod(Stat.LuckMod)); break;
					case Stat.WillMod: packet.PutFloat(creature.StatMods.GetMod(Stat.WillMod)); break;
					case Stat.Str: packet.PutFloat(creature.StrBase); break;
					case Stat.Int: packet.PutFloat(creature.IntBase); break;
					case Stat.Dex: packet.PutFloat(creature.DexBase); break;
					case Stat.Will: packet.PutFloat(creature.WillBase); break;
					case Stat.Luck: packet.PutFloat(creature.LuckBase); break;

					// Client might crash with a mismatching value, 
					// take a chance and put an int by default.
					default: packet.PutInt(1); break;
				}
			}

			// Stat mods
			if (type == StatUpdateType.Public)
			{
				packet.PutSInt(creature.StatRegens.Count);
				foreach (var mod in creature.StatRegens)
					mod.AddToPacket(packet);
			}
			else
				packet.PutInt(0);				 // probably mods as well

			packet.PutInt(0);					 // ?
			packet.PutInt(0);					 // ?

			if (type == StatUpdateType.Public)
			{
				packet.PutInt(0);  				 // ?
				packet.PutInt(0);  				 // ?
				packet.PutInt(0);				 // ?
			}

			return packet;
		}

		/// <summary>
		/// Playing instrument effect (sound and motion) for creature,
		/// based on the given MML code.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="instrument"></param>
		/// <param name="quality"></param>
		/// <param name="compressedMML"></param>
		/// <returns></returns>
		public static MabiPacket PlayEffect(MabiCreature creature, InstrumentType instrument, PlayingQuality quality, string compressedMML)
		{
			var p = new MabiPacket(Op.Effect, creature.Id);
			p.PutInt(Effect.PlayMusic);
			p.PutByte(true); // has scroll
			p.PutString(compressedMML);
			p.PutInt(0);
			p.PutShort(0);
			p.PutInt(14113); // ?
			p.PutByte((byte)quality);
			p.PutByte((byte)instrument);
			p.PutByte(0);
			p.PutByte(0);
			p.PutByte(1); // loops
			return p;
		}

		/// <summary>
		/// Playing instrument effect (sound and motion) for creature,
		/// based on the given score id (client:score.xml).
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="instrument"></param>
		/// <param name="quality"></param>
		/// <param name="score"></param>
		/// <returns></returns>
		public static MabiPacket PlayEffect(MabiCreature creature, InstrumentType instrument, PlayingQuality quality, uint score)
		{
			var p = new MabiPacket(Op.Effect, creature.Id);
			p.PutInt(Effect.PlayMusic);
			p.PutByte(false);
			p.PutInt(score);
			p.PutInt(0);
			p.PutShort(0);
			p.PutInt(14113);
			p.PutByte((byte)quality);
			p.PutByte((byte)instrument);
			p.PutByte(0);
			p.PutByte(0);
			p.PutByte(1);
			return p;
		}

		public static MabiPacket SharpMind(MabiCreature user, MabiCreature target, SkillConst skillId, SharpMindStatus state)
		{
			return new MabiPacket(Op.SharpMind, target.Id)
				.PutLong(user.Id)
				.PutByte(1)
				.PutByte(1)
				.PutShort((ushort)skillId)
				.PutInt((uint)state);
		}
	}

	public enum StatUpdateType : byte { Private = 3, Public = 4 }
}
