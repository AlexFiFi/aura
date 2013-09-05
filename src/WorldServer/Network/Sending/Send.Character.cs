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
		/// <summary>
		/// Sends lock for client character.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="lockType"></param>
		public static void CharacterLock(WorldClient client, uint lockType = 0xEFFFFFFE)
		{
			CharacterLock(client, client.Character, lockType);
		}

		/// <summary>
		/// Sends lock for creature.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="lockType"></param>
		public static void CharacterLock(WorldClient client, MabiCreature creature, uint lockType = 0xEFFFFFFE)
		{
			var p = new MabiPacket(Op.CharacterLock, creature.Id);
			p.PutInt(lockType);
			p.PutInt(0);

			client.Send(p);
		}

		/// <summary>
		/// Sends unlock for client character.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="lockType"></param>
		public static void CharacterUnlock(WorldClient client, uint lockType = 0xEFFFFFFE)
		{
			CharacterUnlock(client, client.Character, lockType);
		}

		/// <summary>
		/// Sends unlock for creature.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="lockType"></param>
		public static void CharacterUnlock(WorldClient client, MabiCreature creature, uint lockType = 0xEFFFFFFE)
		{
			var p = new MabiPacket(Op.CharacterUnlock, creature.Id);
			p.PutInt(lockType);

			client.Send(p);
		}

		/// <summary>
		/// Sends enter region permission, which kinda makes the client warp.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="permission"></param>
		public static void EnterRegionPermission(WorldClient client, MabiCreature creature, bool permission = true)
		{
			var pos = creature.GetPosition();

			var p = new MabiPacket(Op.EnterRegionPermission, Id.World);
			p.PutLong(creature.Id);
			p.PutByte(permission);
			if (permission)
			{
				p.PutInt(creature.Region);
				p.PutInt(pos.X);
				p.PutInt(pos.Y);
			}

			client.Send(p);
		}

		/// <summary>
		/// Enables item shop button.
		/// </summary>
		/// <param name="client"></param>
		public static void ItemShopInfo(WorldClient client)
		{
			var packet = new MabiPacket(Op.ItemShopInfo, client.Character.Id);
			packet.PutByte(0);

			client.Send();
		}

		/// <summary>
		/// Opens GM Control Panel.
		/// </summary>
		/// <param name="client"></param>
		public static void GMCPOpen(WorldClient client)
		{
			var packet = new MabiPacket(Op.GMCPOpen, client.Character.Id);
			client.Send(packet);
		}

		public static void GMCPInvisibilityResponse(WorldClient client, bool success)
		{
			var packet = new MabiPacket(Op.GMCPInvisibilityR, client.Character.Id);
			packet.PutByte(success);

			client.Send(packet);
		}

		public static void GestureResponse(Client client, MabiCreature creature, bool success)
		{
			var p = new MabiPacket(Op.UseGestureR, creature.Id);
			p.PutByte(success);
			client.Send(p);
		}

		public static void UseItemResponse(Client client, MabiCreature creature, bool success, uint itemClass)
		{
			var packet = new MabiPacket(Op.UseItemR, creature.Id);
			packet.PutByte(success);
			if (success)
				packet.PutInt(itemClass);

			client.Send(packet);
		}

		/// <summary>
		/// Broadcasts current conditions of creature.
		/// </summary>
		/// <param name="wm"></param>
		/// <param name="creature"></param>
		public static void StatusEffectUpdate(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.StatusEffectUpdate, creature.Id);
			packet.PutLong((ulong)creature.Conditions.A);
			packet.PutLong((ulong)creature.Conditions.B);
			packet.PutLong((ulong)creature.Conditions.C);
			if (Feature.ConditionD.IsEnabled())
				packet.PutLong((ulong)creature.Conditions.D);
			packet.PutInt(0);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		/// <summary>
		/// Sends revive notice to creature's client.
		/// </summary>
		/// <param name="creature"></param>
		public static void Revived(MabiCreature creature)
		{
			var pos = creature.GetPosition();

			var packet = new MabiPacket(Op.Revived, creature.Id);
			packet.PutInt(1);
			packet.PutInt(creature.Region);
			packet.PutInt(pos.X);
			packet.PutInt(pos.Y);

			creature.Client.Send(packet);
		}

		public static void ReviveFail(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.Revived, creature.Id);
			packet.PutByte(0);

			creature.Client.Send(packet);
		}

		public static void ChangeTitleResponse(Client client, MabiCreature creature, bool titleSuccess, bool optionTitleSuccess)
		{
			var packet = new MabiPacket(Op.ChangeTitleR, creature.Id);
			packet.PutByte(titleSuccess);
			packet.PutByte(optionTitleSuccess);

			client.Send(packet);
		}

		public static void CutsceneStart(WorldClient client, MabiCutscene cutscene)
		{
			var p = new MabiPacket(Op.CutsceneStart, Id.World);
			p.PutLongs(client.Character.Id, cutscene.Leader.Id);
			p.PutString(cutscene.Name);
			p.PutSInt(cutscene.Actors.Count);
			foreach (var a in cutscene.Actors)
			{
				p.PutString(a.Item1);
				p.PutShort((ushort)a.Item2.Length);
				p.PutBin(a.Item2);
			}
			p.PutInt(1);
			p.PutLong(client.Character.Id);

			client.Send(p);
		}

		/// <summary>
		/// Sends character info (5209). Response is negative if character is null.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="character"></param>
		public static void CharacterInfo(Client client, MabiPC character)
		{
			var packet = new MabiPacket(Op.WorldCharInfoRequestR, Id.World);
			if (character != null)
			{
				packet.PutByte(true);
				packet.AddCreatureInfo(character, CreaturePacketType.Private);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends view equipment to client. Response is negative if items is null.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="targetId"></param>
		/// <param name="items"></param>
		public static void ViewEquipmentResponse(WorldClient client, ulong targetId, IEnumerable<MabiItem> items)
		{
			var packet = new MabiPacket(Op.ViewEquipmentR, client.Character.Id);
			if (items != null)
			{
				packet.PutByte(true);
				packet.PutLong(targetId);
				packet.PutInt((ushort)items.Count());
				foreach (var item in items)
					packet.AddItemInfo(item, ItemPacketType.Private);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Broadcasts RankUp animation in range of creature.
		/// Only includes skillId if it is > 0.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		public static void RankUp(MabiCreature creature, ushort skillId = 0)
		{
			var packet = new MabiPacket(Op.RankUp, creature.Id);
			if (skillId > 0)
				packet.PutShort(skillId);
			packet.PutShort(1);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void TalentInfoUpdate(Client client, MabiCreature creature)
		{
			var packet = new MabiPacket(Op.TalentInfoUpdate, creature.Id);
			packet.PutByte(true);
			packet.AddPrivateTalentInfo(creature);

			client.Send(packet);
		}

		private static void AddPrivateTalentInfo(this MabiPacket packet, MabiCreature creature)
		{
			packet.PutShort((ushort)creature.Talents.SelectedTitle);
			packet.PutByte((byte)creature.Talents.Grandmaster);
			packet.PutInt(creature.Talents.GetExp(TalentId.Adventure));
			packet.PutInt(creature.Talents.GetExp(TalentId.Warrior));
			packet.PutInt(creature.Talents.GetExp(TalentId.Mage));
			packet.PutInt(creature.Talents.GetExp(TalentId.Archer));
			packet.PutInt(creature.Talents.GetExp(TalentId.Merchant));
			packet.PutInt(creature.Talents.GetExp(TalentId.BattleAlchemy));
			packet.PutInt(creature.Talents.GetExp(TalentId.Fighter));
			packet.PutInt(creature.Talents.GetExp(TalentId.Bard));
			packet.PutInt(creature.Talents.GetExp(TalentId.Puppeteer));
			packet.PutInt(creature.Talents.GetExp(TalentId.Knight));
			packet.PutInt(creature.Talents.GetExp(TalentId.HolyArts));
			packet.PutInt(creature.Talents.GetExp(TalentId.Transmutaion));
			packet.PutInt(creature.Talents.GetExp(TalentId.Cooking));
			packet.PutInt(creature.Talents.GetExp(TalentId.Blacksmith));
			packet.PutInt(creature.Talents.GetExp(TalentId.Tailoring));
			packet.PutInt(creature.Talents.GetExp(TalentId.Medicine));
			packet.PutInt(creature.Talents.GetExp(TalentId.Carpentry));
			if (Feature.ZeroTalent.IsEnabled())
				packet.PutInt(0);

			// Talent titles
			// ----------------------------------------------------------
			var titles = creature.Talents.GetTitles();

			packet.PutByte((byte)titles.Count);
			foreach (var title in titles)
				packet.PutShort(title);
		}
	}
}
