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
using Aura.World.World.Guilds;

namespace Aura.World.Network
{
	/// <summary>
	/// Packet creator for often used packets
	/// </summary>
	public static class PacketCreator
	{
		/// <summary>
		/// Sends info on guild membership status changed. Pass null for guild to remove.
		/// </summary>
		/// <param name="guild"></param>
		/// <param name="creature"></param>
		/// <returns></returns>
		public static MabiPacket GuildMembershipChanged(MabiGuild guild, MabiCreature creature, GuildMemberRank rank = GuildMemberRank.Member)
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

		// tmp
		public static MabiPacket StatRegenStop(MabiCreature creature, StatUpdateType type, params MabiStatRegen[] stats)
		{
			var packet = new MabiPacket((type & StatUpdateType.Public) != 0 ? Op.StatUpdatePublic : Op.StatUpdatePrivate, creature.Id);

			packet.PutByte((byte)type);
			packet.PutSInt(0); // Stats
			packet.PutInt(0); // Regens

			// Stat mod ids to remove
			packet.PutSInt(stats.Count());
			foreach (var mod in stats)
				packet.PutInt(mod.ModId);

			packet.PutInt(0);

			if (type == StatUpdateType.Public)
			{
				packet.PutInt(0);

				// Stat mod ids to remove
				packet.PutSInt(stats.Count());
				foreach (var mod in stats)
					packet.PutInt(mod.ModId);

				packet.PutInt(0);
			}

			return packet;
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
					case Stat.Weight: packet.PutFloat(creature.Weight); break;
					case Stat.Upper: packet.PutFloat(creature.Upper); break;
					case Stat.Lower: packet.PutFloat(creature.Lower); break;

					case Stat.CombatPower: packet.PutFloat(creature.CombatPower); break;
					case Stat.Level: packet.PutShort(creature.Level); break;
					case Stat.AbilityPoints: packet.PutShort(creature.AbilityPoints); break;
					case Stat.Experience: packet.PutLong(MabiData.ExpDb.CalculateRemaining(creature.Level, creature.Experience) * 1000); break;

					case Stat.Life: packet.PutFloat(creature.Life); break;
					case Stat.LifeMax: packet.PutFloat(creature.LifeMaxBaseTotal); break;
					case Stat.LifeMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.LifeMaxMod)); break;
					case Stat.LifeInjured: packet.PutFloat(creature.LifeInjured); break;
					case Stat.Mana: packet.PutFloat(creature.Mana); break;
					case Stat.ManaMax: packet.PutFloat(creature.ManaMaxBaseTotal); break;
					case Stat.ManaMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.ManaMaxMod)); break;
					case Stat.Stamina: packet.PutFloat(creature.Stamina); break;
					case Stat.Food: packet.PutFloat(creature.StaminaHunger); break;
					case Stat.StaminaMax: packet.PutFloat(creature.StaminaMaxBaseTotal); break;
					case Stat.StaminaMaxMod: packet.PutFloat(creature.StatMods.GetMod(Stat.StaminaMaxMod)); break;

					case Stat.StrMod: packet.PutFloat(creature.StatMods.GetMod(Stat.StrMod)); break;
					case Stat.DexMod: packet.PutFloat(creature.StatMods.GetMod(Stat.DexMod)); break;
					case Stat.IntMod: packet.PutFloat(creature.StatMods.GetMod(Stat.IntMod)); break;
					case Stat.LuckMod: packet.PutFloat(creature.StatMods.GetMod(Stat.LuckMod)); break;
					case Stat.WillMod: packet.PutFloat(creature.StatMods.GetMod(Stat.WillMod)); break;
					case Stat.Str: packet.PutFloat(creature.StrBaseTotal); break;
					case Stat.Int: packet.PutFloat(creature.IntBaseTotal); break;
					case Stat.Dex: packet.PutFloat(creature.DexBaseTotal); break;
					case Stat.Will: packet.PutFloat(creature.WillBaseTotal); break;
					case Stat.Luck: packet.PutFloat(creature.LuckBaseTotal); break;

					case Stat.DefenseBaseMod: packet.PutShort((ushort)creature.DefensePassive); break;
					case Stat.ProtectBaseMod: packet.PutFloat(creature.ProtectionPassive * 100); break;

					case Stat.DefenseMod: packet.PutShort((ushort)creature.StatMods.GetMod(Stat.DefenseMod)); break;
					case Stat.ProtectMod: packet.PutFloat(creature.StatMods.GetMod(Stat.ProtectMod)); break;

					// Client might crash with a mismatching value, 
					// take a chance and put an int by default.
					default: packet.PutInt(1); break;
				}
			}

			// (New?) Stat regens
			packet.PutSInt(creature.StatRegens.Count);
			foreach (var mod in creature.StatRegens)
				mod.AddToPacket(packet);

			// Stat mod ids to remove?
			packet.PutInt(0);

			packet.PutInt(0);					 // ?

			if (type == StatUpdateType.Public)
			{
				packet.PutInt(0);  				 // ?

				// Stat mod ids to remove?
				packet.PutInt(0);

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
