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

		public static void Revived(Client client, MabiCreature creature)
		{
			var pos = creature.GetPosition();

			var packet = new MabiPacket(Op.Revived, creature.Id);
			packet.PutInt(1);
			packet.PutInt(creature.Region);
			packet.PutInt(pos.X);
			packet.PutInt(pos.Y);

			client.Send(packet);
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
				packet.Add(character);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		private static void Add(this MabiPacket packet, MabiPC character)
		{
			character.AddToPacket(packet, 2);

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(character.Title);
			packet.PutLong(character.TitleApplied);

			packet.PutShort((ushort)character.Titles.Count);
			foreach (var title in character.Titles)
			{
				packet.PutShort(title.Key);
				packet.PutByte(title.Value);
				packet.PutInt(0);
			}
			packet.PutShort(character.OptionTitle);

			// Mate
			// --------------------------------------------------------------
			packet.PutLong(0);					 // MateID
			packet.PutString("");				 // MateName
			packet.PutLong(0);					 // MarriageTime
			packet.PutShort(0);					 // MarriageCount

			packet.PutByte(0);					 // JobId

			// Inventory
			// --------------------------------------------------------------
			packet.PutInt(character.RaceInfo.InvWidth);
			packet.PutInt(character.RaceInfo.InvHeight);

			// A little dirty, but better than actually saving and managing
			// the quest items imo... [exec]
			var incompleteQuests = character.Quests.Values.Where(a => a.State < MabiQuestState.Complete).ToList();

			packet.PutSInt(character.Items.Count + incompleteQuests.Count);
			foreach (var item in character.Items)
				item.AddToPacket(packet, ItemPacketType.Private);
			foreach (var quest in incompleteQuests)
				new MabiItem(quest).AddToPacket(packet, ItemPacketType.Private);

			// Keywords
			// --------------------------------------------------------------
			packet.PutShort((ushort)character.Keywords.Count);
			foreach (var keyword in character.Keywords)
			{
				packet.PutShort(keyword);
			}

			// Skills
			// --------------------------------------------------------------
			packet.PutShort((ushort)character.Skills.Count);
			foreach (var skill in character.Skills.Values)
			{
				packet.PutBin(skill.Info);
			}
			packet.PutInt(0);					 // SkillVarBufferList
			if (Feature.UnkAny1.IsEnabled())
				packet.PutByte(0);			     // {PLGCNT}

			// Banner
			// --------------------------------------------------------------
			packet.PutByte(0); 					 // IsActivate
			packet.PutString("");				 // Content

			// PvP
			// --------------------------------------------------------------
			character.AddPvPInfoToPacket(packet);

			// Statuses
			// --------------------------------------------------------------
			packet.PutLong((ulong)character.Conditions.A);
			packet.PutLong((ulong)character.Conditions.B);
			packet.PutLong((ulong)character.Conditions.C);
			if (Feature.ConditionD.IsEnabled())
				packet.PutLong((ulong)character.Conditions.D);
			packet.PutInt(0);					 // condition event message list
			// loop
			//   packet.PutInt
			//   packet.PutString

			if (Feature.UnkAny2.IsEnabled())
				packet.PutLong(0);

			if (character.Guild != null)
			{
				packet.PutLong(character.Guild.Id);
				packet.PutString(character.Guild.Name);
				packet.PutInt(character.GuildMemberInfo.MemberRank);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutString(character.Guild.Title);
			}
			else
			{
				packet.PutLong(0);
				packet.PutString("");
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutString("");
			}

			// PTJ
			// --------------------------------------------------------------
			packet.PutLong(0);				     // ArbeitID
			packet.PutInt(0);				     // ArbeitRecordList
			// loop
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort

			// Follower (Pets)
			// --------------------------------------------------------------
			if (character.Owner == null)
			{
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutByte(0);
			}
			else
			{
				packet.PutLong(character.Owner.Id);
				packet.PutByte(2);               // Type (1:RPCharacter, 2:Pet, 3:Transport, 4:PartnerVehicle)
				packet.PutByte(0);				 // SubType
			}

			// ?
			// --------------------------------------------------------------
			if (Feature.UnkAny3.IsEnabled())
			{
				packet.PutFloat(1);
				packet.PutLong(0);
			}

			// Transformation
			// --------------------------------------------------------------
			packet.PutByte(0);				     // Type (1:Paladin, 2:DarkKnight, 3:SubraceTransformed, 4:TransformedElf, 5:TransformedGiant)
			packet.PutShort(0);				     // Level
			packet.PutShort(0);				     // SubType

			// Follower (Pets)
			// --------------------------------------------------------------
			if (character.Owner == null)
			{
				packet.PutString("");
				packet.PutInt(0);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutByte(0);
			}
			else
			{
				packet.PutString(character.Owner.Name);
				packet.PutInt(2000000000);			                         // RemainingSummonTime
				packet.PutLong(0);								             // LastSummonTime
				packet.PutLong(0);								             // PetExpireTime
				packet.PutByte(0);											 // Loyalty
				packet.PutByte(0);											 // Favor
				packet.PutLong(DateTime.Now);								 // SummonTime
				packet.PutByte(0);											 // KeepingMode
				packet.PutLong(0);											 // KeepingProp
				packet.PutLong(character.Owner.Id);
				packet.PutByte(0);											 // PetSealCount {PSCNT}
			}

			// House
			// --------------------------------------------------------------
			packet.PutLong(0);					 // HouseID

			// Taming
			// --------------------------------------------------------------
			packet.PutLong(0);					 // MasterID
			packet.PutByte(0);					 // IsTamed
			packet.PutByte(0);					 // TamedType (1:DarkKnightTamed, 2:InstrumentTamed, 3:AnimalTraining, 4:MercenaryTamed, 5:Recalled, 6:SoulStoneTamed, 7:TamedFriend)
			packet.PutByte(1);					 // IsMasterMode
			packet.PutInt(0);					 // LimitTime

			// Vehicle
			// --------------------------------------------------------------
			packet.PutInt(0);					 // Type
			packet.PutInt(0);					 // TypeFlag (0x1:Driver, 0x4:Owner)
			packet.PutLong(0);					 // VehicleId
			packet.PutInt(0);					 // SeatIndex
			packet.PutByte(0);					 // PassengerList
			// loop
			//   packet.PutLong

			// Showdown
			// --------------------------------------------------------------
			packet.PutInt(0);	                 // unknown at 0x18
			packet.PutLong(0);                   // unknown at 0x08
			packet.PutLong(0);	                 // unknown at 0x10
			packet.PutByte(1);	                 // IsPartyPvpDropout

			// Transport
			// --------------------------------------------------------------
			packet.PutLong(0);					 // TransportID
			packet.PutInt(0);					 // HuntPoint

			// Aviation
			// --------------------------------------------------------------
			packet.PutByte(character.IsFlying);
			if (character.IsFlying)
			{
				var pos = character.GetPosition();
				packet.PutFloat(pos.X);
				packet.PutFloat(pos.H);
				packet.PutFloat(pos.Y);
				packet.PutFloat(character.Destination.X);
				packet.PutFloat(character.Destination.H);
				packet.PutFloat(character.Destination.Y);
				packet.PutFloat(character.Direction);
			}

			// Skiing
			// --------------------------------------------------------------
			packet.PutByte(0);					 // IsSkiing
			// loop
			//   packet.PutFloat
			//   packet.PutFloat
			//   packet.PutFloat
			//   packet.PutFloat
			//   packet.PutInt
			//   packet.PutInt
			//   packet.PutByte
			//   packet.PutByte

			// Farming
			// --------------------------------------------------------------
			packet.PutLong(0);					 // FarmId
			//   packet.PutLong
			//   packet.PutLong
			//   packet.PutLong
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutByte
			//   packet.PutLong
			//   packet.PutByte
			//   packet.PutLong

			// Event (CaptureTheFlag, WaterBalloonBattle)
			// --------------------------------------------------------------
			packet.PutByte(0);				     // EventFullSuitIndex
			packet.PutByte(0);				     // TeamId
			// packet.PutInt					 // HitPoint
			// packet.PutInt					 // MaxHitPoint

			if (Feature.UnkAny4.IsEnabled())
			{
				packet.PutString("");
				packet.PutByte(0);
			}

			// Heartstickers
			// --------------------------------------------------------------
			packet.PutShort(0);
			packet.PutShort(0);

			// 
			// --------------------------------------------------------------
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutShort(0);
			packet.PutShort(0);
			packet.PutShort(0);
			packet.PutShort(0);

			// Achievements
			// --------------------------------------------------------------
			packet.PutInt(0);	                 // TotalScore
			packet.PutShort(0);                  // AchievementList
			// loop
			//   packet.PutShort achievementId

			// PrivateFarm
			// --------------------------------------------------------------
			packet.PutInt(0);					 // FavoriteFarmList
			// loop
			//   packet.PutLong                  // FarmId
			//   packet.PutInt                   // ZoneId
			//   packet.PutShort                 // PosX
			//   packet.PutShort                 // PoxY
			//   packet.PutString                // FarmName
			//   packet.PutString                // OwnerName

			// Family
			// --------------------------------------------------------------
			packet.PutLong(0);					 // FamilyId
			// loop
			//   packet.WriteString				 // FamilyName
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.WriteString				 // FamilyTitle

			// Demigod
			// --------------------------------------------------------------
			packet.PutInt(0);					 // SupportType (0:None, 1:Neamhain, 2:Morrighan)

			// Commerce
			// --------------------------------------------------------------
			if (Feature.Commerce.IsEnabled())
			{
				packet.PutByte(1);               // IsInCommerceCombat
				packet.PutLong(0);               // TransportCharacterId
				packet.PutFloat(1);              // ScaleHeight
			}

			// Talents?
			// --------------------------------------------------------------
			if (Feature.Talents.IsEnabled())
			{
				packet.PutShort((ushort)character.SelectedTalentTitle);
				packet.PutByte((byte)character.Grandmaster);
				packet.PutInt(character.GetTalentExp(TalentId.Adventure));
				packet.PutInt(character.GetTalentExp(TalentId.Warrior));
				packet.PutInt(character.GetTalentExp(TalentId.Mage));
				packet.PutInt(character.GetTalentExp(TalentId.Archer));
				packet.PutInt(character.GetTalentExp(TalentId.Merchant));
				packet.PutInt(character.GetTalentExp(TalentId.BattleAlchemy));
				packet.PutInt(character.GetTalentExp(TalentId.Fighter));
				packet.PutInt(character.GetTalentExp(TalentId.Bard));
				packet.PutInt(character.GetTalentExp(TalentId.Puppeteer));
				packet.PutInt(character.GetTalentExp(TalentId.Knight));
				packet.PutInt(character.GetTalentExp(TalentId.HolyArts));
				packet.PutInt(character.GetTalentExp(TalentId.Transmutaion));
				packet.PutInt(character.GetTalentExp(TalentId.Cooking));
				packet.PutInt(character.GetTalentExp(TalentId.Blacksmith));
				packet.PutInt(character.GetTalentExp(TalentId.Tailoring));
				packet.PutInt(character.GetTalentExp(TalentId.Medicine));
				packet.PutInt(character.GetTalentExp(TalentId.Carpentry));

				if (Feature.ZeroTalent.IsEnabled())
					packet.PutInt(0);

				// Talent titles
				// ----------------------------------------------------------
				var titles = character.GetTalentTitles();

				packet.PutByte((byte)titles.Count);
				foreach (var title in titles)
					packet.PutShort(title);
			}

			// Transformations Diary (Shamala)
			// --------------------------------------------------------------
			if (Feature.Shamala.IsEnabled())
			{
				packet.PutSInt(character.Shamalas.Count);
				foreach (var trans in character.Shamalas)
				{
					packet.PutInt(trans.Id);
					packet.PutByte(trans.Counter);
					packet.PutByte((byte)trans.State);
				}
			}

			// ?
			// --------------------------------------------------------------
			if (Feature.UnkAny6.IsEnabled())
			{
				packet.PutInt(0);
				packet.PutInt(0);
			}

			// ?
			// --------------------------------------------------------------
			if (Feature.UnkNATW1.IsEnabled())
			{
				packet.PutInt(0);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutString("");
				packet.PutByte(0);
			}

			// 
			// --------------------------------------------------------------
			packet.PutByte(0);					 // IsGhost
			packet.PutLong(0);					 // SittingProp
			packet.PutSInt(-1);					 // SittedSocialMotionId

			// This int is needed in the JP client (1704 log),
			// but doesn't appear in the NA 1704 or KR test 1801 log.
			if (Feature.UnkJP1.IsEnabled())
				packet.PutInt(4);

			// Premium stuff?
			// --------------------------------------------------------------
			packet.PutByte(0);                   // IsUsingExtraStorage (old service)
			packet.PutByte(0);                   // IsUsingNaosSupport (old service)
			packet.PutByte(0);                   // IsUsingAdvancedPlay (old service)
			packet.PutByte(0);                   // PremiumService 0
			packet.PutByte(0);                   // PremiumService 1
			packet.PutByte(1);                   // Premium Gestures
			packet.PutByte(0);
			packet.PutByte(0);
			if (Feature.NewPremiumThing.IsEnabled())
				packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);

			// Quests
			// --------------------------------------------------------------
			packet.PutSInt(incompleteQuests.Count);
			foreach (var q in incompleteQuests)
				packet.AddQuest(q);

			// Char
			// --------------------------------------------------------------
			packet.PutByte(0);					 // NaoDress (0:normal, 12:??, 13:??)
			packet.PutLong(character.CreationTime);
			packet.PutLong(character.LastRebirth);
			packet.PutString("");
			packet.PutByte(0);
			packet.PutByte(2);

			// Pocket ExpireTime List
			// --------------------------------------------------------------
			if (Feature.ExpiringPockets.IsEnabled())
			{
				// Style
				packet.PutLong(DateTime.Now.AddMonths(1));
				packet.PutShort(72);

				// ?
				packet.PutLong(0);
				packet.PutShort(73);
			}
		}
	}
}
