// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.World.World;

namespace Aura.World.Network
{
	public static partial class Send
	{
		public static void SkillInfo(Client client, MabiCreature creature, MabiSkill skill)
		{
			var packet = new MabiPacket(Op.SkillInfo, creature.Id);
			packet.PutBin(skill.Info);

			client.Send(packet);
		}

		public static void SkillRankUp(Client client, MabiCreature creature, MabiSkill skill)
		{
			var packet = new MabiPacket(Op.SkillRankUp, creature.Id);
			packet.PutByte(1);
			packet.PutBin(skill.Info);
			packet.PutFloat(0);

			client.Send(packet);
		}

		/// <summary>
		/// Broadcasts skill init flash effect to all players in range of creature.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="str"></param>
		public static void Flash(MabiCreature creature)
		{
			SkillInitEffect(creature, "flashing");
		}

		/// <summary>
		/// Broadcasts skill init effect to all players in range of creature.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="str"></param>
		public static void SkillInitEffect(MabiCreature creature, string str = "")
		{
			var packet = new MabiPacket(Op.Effect, creature.Id);
			packet.PutInt(Aura.Shared.Const.Effect.SkillInit);
			packet.PutString(str);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		/// <summary>
		/// Normal skill prepare response, skillId None means fail.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="castTime"></param>
		public static void SkillPrepare(Client client, MabiCreature creature, SkillConst skillId, uint castTime)
		{
			var packet = new MabiPacket(Op.SkillPrepare, creature.Id);
			packet.PutShort((ushort)skillId);
			if (skillId != SkillConst.None)
				packet.PutInt(castTime);

			client.Send(packet);
		}

		public static void SendSkillPrepareFail(Client client, MabiCreature creature)
		{
			SkillPrepare(client, creature, SkillConst.None, 0);
		}

		/// <summary>
		/// Default skill ready packet, with or without string parameters.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="parameters"></param>
		public static void SkillReady(Client client, MabiCreature creature, SkillConst skillId, string parameters = "")
		{
			var packet = new MabiPacket(Op.SkillReady, creature.Id);
			packet.PutShort((ushort)skillId);
			if (!string.IsNullOrWhiteSpace(parameters) && parameters.Length > 0)
				packet.PutString(parameters);

			client.Send(packet);
		}

		/// <summary>
		/// Skill ready packet with 2 ulong parameters for ids (e.g. dyeing).
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="id1"></param>
		/// <param name="id2"></param>
		public static void SkillReady(Client client, MabiCreature creature, SkillConst skillId, ulong id1, ulong id2)
		{
			var packet = new MabiPacket(Op.SkillReady, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutLongs(id1, id2);

			client.Send(packet);
		}

		/// <summary>
		/// Default skill complete with only the skill id.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		public static void SkillComplete(Client client, MabiCreature creature, SkillConst skillId)
		{
			var packet = new MabiPacket(Op.SkillComplete, creature.Id);
			packet.PutShort((ushort)skillId);

			client.Send(packet);
		}

		/// <summary>
		/// Skill complete with an additional id parameter (eg item object id).
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="id"></param>
		public static void SkillComplete(Client client, MabiCreature creature, SkillConst skillId, ulong id)
		{
			var packet = new MabiPacket(Op.SkillComplete, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutLong(id);

			client.Send(packet);
		}

		/// <summary>
		/// Skill complete with an additional id parameter, and 2 unknown ints.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="id"></param>
		/// <param name="unk1"></param>
		/// <param name="unk2"></param>
		public static void SkillComplete(Client client, MabiCreature creature, SkillConst skillId, ulong id, uint unk1, uint unk2)
		{
			var packet = new MabiPacket(Op.SkillComplete, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutLong(id);
			packet.PutInts(unk1, unk2);

			client.Send(packet);
		}

		/// <summary>
		/// Regular dye complete
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void DyeSkillComplete(Client client, MabiCreature creature, SkillConst skillId, uint part, ushort x, ushort y)
		{
			var packet = new MabiPacket(Op.SkillComplete, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutInt(part);
			packet.PutShorts(x, y);

			client.Send(packet);
		}

		/// <summary>
		/// Fixed dye complete
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		public static void DyeSkillComplete(Client client, MabiCreature creature, SkillConst skillId, uint part)
		{
			var packet = new MabiPacket(Op.SkillComplete, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutInt(part);

			client.Send(packet);
		}

		public static void SkillUse(Client client, MabiCreature creature, SkillConst skillId, ulong targetId)
		{
			var packet = new MabiPacket(Op.SkillUse, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutLong(targetId);

			client.Send(packet);
		}

		public static void SkillUse(Client client, MabiCreature creature, SkillConst skillId, ulong targetId, uint unk1, uint unk2)
		{
			var packet = new MabiPacket(Op.SkillUse, creature.Id);
			packet.PutShort((ushort)skillId);
			if (skillId != SkillConst.None)
			{
				packet.PutLong(targetId);
				packet.PutInt(unk1);
				packet.PutInt(unk2);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Skill use with a delay?
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="ms"></param>
		/// <param name="unk"></param>
		public static void SkillUse(Client client, MabiCreature creature, SkillConst skillId, uint ms, uint unk)
		{
			var packet = new MabiPacket(Op.SkillUse, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutInts(ms, unk);

			client.Send(packet);
		}

		/// <summary>
		/// Regular dye use packet.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SkillUseDye(Client client, MabiCreature creature, SkillConst skillId, uint part, ushort x, ushort y)
		{
			var packet = new MabiPacket(Op.SkillUse, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutInt(part);
			packet.PutShorts(x, y);

			client.Send(packet);
		}

		/// <summary>
		/// Fixed dye use complete.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		/// <param name="unk"></param>
		public static void SkillUseDye(Client client, MabiCreature creature, SkillConst skillId, uint part, byte unk)
		{
			var packet = new MabiPacket(Op.SkillUse, creature.Id);
			packet.PutShort((ushort)skillId);
			packet.PutInt(part);
			packet.PutByte(unk);

			client.Send(packet);
		}

		public static void SkillStackSet(Client client, MabiCreature creature, SkillConst skillId, byte stack)
		{
			var packet = new MabiPacket(Op.SkillStackSet, creature.Id);
			packet.PutBytes(creature.ActiveSkillStacks, 1);
			packet.PutShort((ushort)creature.ActiveSkillId);

			client.Send(packet);
		}

		/// <summary>
		/// Updates the stack amount (how many uses left).
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="remaining"></param>
		/// <param name="max"></param>
		public static void SkillStackUpdate(Client client, MabiCreature creature, SkillConst skillId, byte remaining)
		{
			var packet = new MabiPacket(Op.SkillStackUpdate, creature.Id);
			packet.PutBytes(remaining, 1, 0);
			packet.PutShort((ushort)skillId);

			client.Send(packet);
		}

		/// <summary>
		/// Simple skill cancel.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void SkillCancel(Client client, MabiCreature creature)
		{
			var packet = new MabiPacket(Op.SkillCancel, creature.Id);
			packet.PutBytes(0, 1);

			client.Send(packet);
		}

		/// <summary>
		/// Cancel packet for WM?
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void SkillSilentCancel(Client client, MabiCreature creature)
		{
			var packet = new MabiPacket(Op.SkillSilentCancel, creature.Id);

			client.Send(packet);
		}

		/// <summary>
		/// Broadcasts SpreadWingsOn or SpreadWingsOff in range.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="onOff"></param>
		public static void SpreadWings(MabiCreature creature, bool onOff)
		{
			var packet = new MabiPacket(onOff ? Op.SpreadWingsOn : Op.SpreadWingsOff, creature.Id);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}
	}
}
