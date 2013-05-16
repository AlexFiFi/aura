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
	// These extensions are an experiment.

	public static class ClientSkillsSendingExt
	{
		/// <summary>
		/// Normal skill prepare response, skillId None means fail.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="castTime"></param>
		public static void SendSkillPrepare(this Client client, MabiCreature creature, SkillConst skillId, uint castTime)
		{
			var p = new MabiPacket(Op.SkillPrepare, creature.Id);
			p.PutShort((ushort)skillId);
			if (skillId != SkillConst.None)
				p.PutInt(castTime);

			client.Send(p);
		}

		public static void SendSkillPrepareFail(this Client client, MabiCreature creature)
		{
			SendSkillPrepare(client, creature, SkillConst.None, 0);
		}

		/// <summary>
		/// Default skill ready packet, with or without string parameters.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="parameters"></param>
		public static void SendSkillReady(this Client client, MabiCreature creature, SkillConst skillId, string parameters = "")
		{
			var p = new MabiPacket(Op.SkillReady, creature.Id);
			p.PutShort((ushort)skillId);
			if (!string.IsNullOrWhiteSpace(parameters) && parameters.Length > 0)
				p.PutString(parameters);

			client.Send(p);
		}

		/// <summary>
		/// Skill ready packet with 2 ulong parameters for ids (e.g. dyeing).
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="id1"></param>
		/// <param name="id2"></param>
		public static void SendSkillReady(this Client client, MabiCreature creature, SkillConst skillId, ulong id1, ulong id2)
		{
			var p = new MabiPacket(Op.SkillReady, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutLongs(id1, id2);

			client.Send(p);
		}

		/// <summary>
		/// Default skill complete with only the skill id.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		public static void SendSkillComplete(this Client client, MabiCreature creature, SkillConst skillId)
		{
			var p = new MabiPacket(Op.SkillComplete, creature.Id);
			p.PutShort((ushort)skillId);

			client.Send(p);
		}

		/// <summary>
		/// Skill complete with an additional id parameter (eg item object id).
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="id"></param>
		public static void SendSkillComplete(this Client client, MabiCreature creature, SkillConst skillId, ulong id)
		{
			var p = new MabiPacket(Op.SkillComplete, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutLong(id);

			client.Send(p);
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
		public static void SendSkillComplete(this Client client, MabiCreature creature, SkillConst skillId, ulong id, uint unk1, uint unk2)
		{
			var p = new MabiPacket(Op.SkillComplete, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutLong(id);
			p.PutInts(unk1, unk2);

			client.Send(p);
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
		public static void SendDyeSkillComplete(this Client client, MabiCreature creature, SkillConst skillId, uint part, ushort x, ushort y)
		{
			var p = new MabiPacket(Op.SkillComplete, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutInt(part);
			p.PutShorts(x, y);

			client.Send(p);
		}

		/// <summary>
		/// Fixed dye complete
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		public static void SendDyeSkillComplete(this Client client, MabiCreature creature, SkillConst skillId, uint part)
		{
			var p = new MabiPacket(Op.SkillComplete, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutInt(part);

			client.Send(p);
		}

		public static void SendSkillUse(this Client client, MabiCreature creature, SkillConst skillId, ulong targetId)
		{
			var p = new MabiPacket(Op.SkillUse, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutLong(targetId);

			client.Send(p);
		}

		public static void SendSkillUse(this Client client, MabiCreature creature, SkillConst skillId, ulong targetId, uint unk1, uint unk2)
		{
			var p = new MabiPacket(Op.SkillUse, creature.Id);
			p.PutShort((ushort)skillId);
			if (skillId != SkillConst.None)
			{
				p.PutLong(targetId);
				p.PutInt(unk1);
				p.PutInt(unk2);
			}

			client.Send(p);
		}

		/// <summary>
		/// Skill use with a delay?
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="ms"></param>
		/// <param name="unk"></param>
		public static void SendSkillUse(this Client client, MabiCreature creature, SkillConst skillId, uint ms, uint unk)
		{
			var p = new MabiPacket(Op.SkillUse, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutInts(ms, unk);

			client.Send(p);
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
		public static void SendSkillUseDye(this Client client, MabiCreature creature, SkillConst skillId, uint part, ushort x, ushort y)
		{
			var p = new MabiPacket(Op.SkillUse, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutInt(part);
			p.PutShorts(x, y);

			client.Send(p);
		}

		/// <summary>
		/// Fixed dye use complete.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="part"></param>
		/// <param name="unk"></param>
		public static void SendSkillUseDye(this Client client, MabiCreature creature, SkillConst skillId, uint part, byte unk)
		{
			var p = new MabiPacket(Op.SkillUse, creature.Id);
			p.PutShort((ushort)skillId);
			p.PutInt(part);
			p.PutByte(unk);

			client.Send(p);
		}

		public static void SendSkillStackSet(this Client client, MabiCreature creature, SkillConst skillId, byte stack)
		{
			var p = new MabiPacket(Op.SkillStackSet, creature.Id);
			p.PutBytes(creature.ActiveSkillStacks, 1);
			p.PutShort((ushort)creature.ActiveSkillId);

			client.Send(p);
		}

		/// <summary>
		/// Updates the stack amount (how many uses left).
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="remaining"></param>
		/// <param name="max"></param>
		public static void SendSkillStackUpdate(this Client client, MabiCreature creature, SkillConst skillId, byte remaining)
		{
			var p = new MabiPacket(Op.SkillStackUpdate, creature.Id);
			p.PutBytes(remaining, 1, 0);
			p.PutShort((ushort)skillId);

			client.Send(p);
		}

		/// <summary>
		/// Simple skill cancel.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void SendSkillCancel(this Client client, MabiCreature creature)
		{
			var p = new MabiPacket(Op.SkillCancel, creature.Id);
			p.PutBytes(0, 1);

			client.Send(p);
		}

		/// <summary>
		/// Cancel packet for WM?
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void SendSkillSilentCancel(this Client client, MabiCreature creature)
		{
			var p = new MabiPacket(Op.SkillSilentCancel, creature.Id);

			client.Send(p);
		}
	}
}
