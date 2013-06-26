﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Data;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Player;
using Aura.World.World;

namespace Aura.World.Network
{
	public static partial class Send
	{
		private static void AddPublicEntityInfo(this MabiPacket packet, MabiEntity entity)
		{
			if (entity is MabiCreature) packet.AddTest(entity as MabiCreature, CreaturePacketType.Public);
			else if (entity is MabiItem) packet.AddItemInfo(entity as MabiItem, ItemPacketType.Public);
			else if (entity is MabiProp) packet.AddPropInfo(entity as MabiProp);
			else throw new Exception("Unknown entity class '" + entity.GetType() + "'");
		}

		private static void AddItemInfo(this MabiPacket packet, MabiItem item, ItemPacketType type)
		{
			packet.PutLong(item.Id);
			packet.PutByte((byte)type);
			packet.PutBin(item.Info);

			if (type == ItemPacketType.Public)
			{
				packet.PutByte(1);
				packet.PutByte(0);

				//packet.PutByte(0); // Bitmask
				// if & 1
				//     float
				packet.PutByte(1);
				packet.PutFloat(1); // Size multiplicator *hint: Server side giant key mod*

				packet.PutByte(item.FirstTimeAppear); // 0: No bouncing, 1: Bouncing, 2: Delayed bouncing
			}
			else if (type == ItemPacketType.Private)
			{
				packet.PutBin(item.OptionInfo);
				packet.PutString(item.Tags.ToString());
				packet.PutString("");
				packet.PutByte(0);
				packet.PutLong(item.QuestId);
			}
		}

		private static void AddPropInfo(this MabiPacket packet, MabiProp prop)
		{
			// Client side props (A0 range, instead of A1; second case)
			// look a bit different.
			if (prop.Id >= Aura.Shared.Const.Id.Props)
			{
				packet.PutLong(prop.Id);
				packet.PutInt(prop.Info.Class);
				packet.PutString(prop.Name);
				packet.PutString(prop.Title);
				packet.PutBin(prop.Info);
				packet.PutString(prop.State);
				packet.PutLong(0);

				packet.PutByte(true); // Extra data?
				packet.PutString(prop.ExtraData);

				packet.PutInt(0);
				packet.PutShort(0);
			}
			else
			{
				packet.PutLong(prop.Id);
				packet.PutInt(prop.Info.Class);
				packet.PutString(prop.State);
				packet.PutLong(DateTime.Now);
				packet.PutByte(false);
				packet.PutFloat(prop.Info.Direction);
			}
		}

		private static void AddPropUpdateInfo(this MabiPacket packet, MabiProp prop)
		{
			// Client side props (A0 range, instead of A1; second case)
			// look a bit different.
			if (prop.Id >= Aura.Shared.Const.Id.Props)
			{
				packet.PutString(prop.State);
				packet.PutLong(DateTime.Now);
				packet.PutByte(true);
				packet.PutString(prop.ExtraData);
				packet.PutFloat(prop.Info.Direction);
				packet.PutShort(0);
			}
			else
			{
				packet.PutString(prop.State);
				packet.PutLong(DateTime.Now);
				packet.PutByte(false);
				packet.PutFloat(prop.Info.Direction);
				packet.PutShort(0);
			}
		}

		public enum CreaturePacketType { Minimal = 1, Private = 2, Public = 5 }

		/// <summary>
		/// Example. Combination of public and private creature data.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="creature"></param>
		/// <param name="type"></param>
		public static void AddTest(this MabiPacket packet, MabiCreature creature, CreaturePacketType type)
		{
			var character = creature as MabiPC;
			if (type == CreaturePacketType.Private && character == null)
				throw new Exception("MabiPC required for private creature packet.");

			var loc = creature.GetPosition();

			packet.PutLong(creature.Id);
			packet.PutByte((byte)type);

			// Looks/Location
			// --------------------------------------------------------------
			packet.PutString(creature.Name);
			packet.PutString("");				 // Title
			packet.PutString("");				 // Eng Title
			packet.PutInt(creature.Race);
			packet.PutByte(creature.SkinColor);
			packet.PutByte(creature.Eye);
			packet.PutByte(creature.EyeColor);
			packet.PutByte(creature.Lip);
			packet.PutInt((uint)creature.State);
			if (type == CreaturePacketType.Public)
			{
				packet.PutInt((uint)creature.StateEx);
			}
			packet.PutFloat(creature.Height);
			packet.PutFloat(creature.Fat);
			packet.PutFloat(creature.Upper);
			packet.PutFloat(creature.Lower);
			packet.PutInt(creature.Region);
			packet.PutInt(loc.X);
			packet.PutInt(loc.Y);
			packet.PutByte(creature.Direction);
			packet.PutInt(creature.BattleState);
			packet.PutByte(creature.WeaponSet);
			packet.PutInt(creature.Color1);
			packet.PutInt(creature.Color2);
			packet.PutInt(creature.Color3);

			// Stats
			// --------------------------------------------------------------
			packet.PutFloat(creature.CombatPower);
			packet.PutString(creature.Shamala == null ? creature.StandStyle : "");

			if (type == CreaturePacketType.Private)
			{
				packet.PutFloat(creature.Life);
				packet.PutFloat(creature.LifeInjured);
				packet.PutFloat(creature.LifeMaxBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.LifeMaxMod));
				packet.PutFloat(creature.Mana);
				packet.PutFloat(creature.ManaMaxBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.ManaMaxMod));
				packet.PutFloat(creature.Stamina);
				packet.PutFloat(creature.StaminaMaxBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.StaminaMaxMod));
				packet.PutFloat(creature.StaminaHunger);
				packet.PutFloat(0.5f);
				packet.PutShort(creature.Level);
				packet.PutInt(creature.LevelTotal);
				packet.PutShort(0);                  // Max Level
				packet.PutShort(0);					 // Rebirthes
				packet.PutShort(0);
				packet.PutLong(MabiData.ExpDb.CalculateRemaining(creature.Level, creature.Experience) * 1000);
				packet.PutShort(creature.Age);
				packet.PutFloat(creature.StrBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.StrMod));
				packet.PutFloat(creature.DexBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.DexMod));
				packet.PutFloat(creature.IntBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.IntMod));
				packet.PutFloat(creature.WillBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.WillMod));
				packet.PutFloat(creature.LuckBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.LuckMod));
				packet.PutFloat(0);					 // LifeMaxByFood
				packet.PutFloat(0);					 // ManaMaxByFood
				packet.PutFloat(0);					 // StaminaMaxByFood
				packet.PutFloat(0);					 // StrengthByFood
				packet.PutFloat(0);					 // DexterityByFood
				packet.PutFloat(0);					 // IntelligenceByFood
				packet.PutFloat(0);					 // WillByFood
				packet.PutFloat(0);					 // LuckByFood
				packet.PutShort(creature.AbilityPoints);
				packet.PutShort(0);			         // AttackMinBase
				packet.PutShort(0);			         // AttackMinMod
				packet.PutShort(0);			         // AttackMaxBase
				packet.PutShort(0);			         // AttackMaxMod
				packet.PutShort(0);			         // WAttackMinBase
				packet.PutShort(0);			         // WAttackMinMod
				packet.PutShort(0);			         // WAttackMaxBase
				packet.PutShort(0);			         // WAttackMaxMod
				packet.PutShort(0);			         // LeftAttackMinMod
				packet.PutShort(0);			         // LeftAttackMaxMod
				packet.PutShort(0);			         // RightAttackMinMod
				packet.PutShort(0);			         // RightAttackMaxMod
				packet.PutShort(0);			         // LeftWAttackMinMod
				packet.PutShort(0);			         // LeftWAttackMaxMod
				packet.PutShort(0);			         // RightWAttackMinMod
				packet.PutShort(0);			         // RightWAttackMaxMod
				packet.PutFloat(0);			         // LeftCriticalMod
				packet.PutFloat(0);			         // RightCriticalMod
				packet.PutShort(0);			         // LeftRateMod
				packet.PutShort(0);			         // RightRateMod
				packet.PutFloat(0);			         // MagicDefenseMod
				packet.PutFloat(0);			         // MagicAttackMod
				if (Feature.NewUnkCrInfo.IsEnabled())
					packet.PutFloat(0);			     // ?
				packet.PutShort(15);		         // MeleeAttackRateMod
				packet.PutShort(15);		         // RangeAttackRateMod
				packet.PutFloat(0);			         // CriticalBase
				packet.PutFloat(0);			         // CriticalMod
				packet.PutFloat(0);			         // ProtectBase
				packet.PutFloat(creature.StatMods.GetMod(Stat.ProtectMod));			         // ProtectMod
				packet.PutShort(0);			         // DefenseBase
				packet.PutShort((ushort)creature.StatMods.GetMod(Stat.DefenseMod));			         // DefenseMod
				packet.PutShort(0);			         // RateBase
				packet.PutShort(0);			         // RateMod
				packet.PutShort(0);			         // Rank1
				packet.PutShort(0);			         // Rank2
				if (Feature.NewUnkCrInfo.IsEnabled())
					packet.PutShort(0);			     // ?
				packet.PutLong(0);			         // Score
				packet.PutShort(0);			         // AttackMinBaseMod
				packet.PutShort(8);			         // AttackMaxBaseMod
				packet.PutShort(0);			         // WAttackMinBaseMod
				packet.PutShort(0);			         // WAttackMaxBaseMod
				packet.PutFloat(10);		         // CriticalBaseMod
				packet.PutFloat(creature.ProtectionPassive * 100);		             // ProtectBaseMod
				packet.PutShort((ushort)creature.DefensePassive);		             // DefenseBaseMod
				packet.PutShort(30);		         // RateBaseMod
				packet.PutShort(8);			         // MeleeAttackMinBaseMod
				packet.PutShort(18);		         // MeleeAttackMaxBaseMod
				packet.PutShort(0);			         // MeleeWAttackMinBaseMod
				packet.PutShort(0);			         // MeleeWAttackMaxBaseMod
				packet.PutShort(10);		         // RangeAttackMinBaseMod
				packet.PutShort(25);		         // RangeAttackMaxBaseMod
				packet.PutShort(0);			         // RangeWAttackMinBaseMod
				packet.PutShort(0);			         // RangeWAttackMaxBaseMod
				packet.PutShort(0);			         // PoisonBase
				packet.PutShort(0);			         // PoisonMod
				if (Feature.NewPoisonCrInfo.IsEnabled())
				{
					packet.PutShort(0);			     // ?
					packet.PutShort(0);			     // ?
					packet.PutShort(0);			     // ?
					packet.PutShort(0);			     // ?
				}
				packet.PutShort(67);		         // PoisonImmuneBase
				packet.PutShort(0);			         // PoisonImmuneMod
				packet.PutFloat(0.5f);		         // PoisonDamageRatio1
				packet.PutFloat(0);			         // PoisonDamageRatio2
				packet.PutFloat(0);			         // toxicStr
				packet.PutFloat(0);			         // toxicInt
				packet.PutFloat(0);			         // toxicDex
				packet.PutFloat(0);			         // toxicWill
				packet.PutFloat(0);			         // toxicLuck
				packet.PutString("Uladh_main/town_TirChonaill/TirChonaill_Spawn_A"); // Last town
				packet.PutShort(1);					 // ExploLevel
				packet.PutShort(0);					 // ExploMaxKeyLevel
				packet.PutInt(0);					 // ExploCumLevel
				packet.PutLong(0);					 // ExploExp
				packet.PutInt(0);					 // DiscoverCount
				packet.PutFloat(0);					 // conditionStr
				packet.PutFloat(0);					 // conditionInt
				packet.PutFloat(0);					 // conditionDex
				packet.PutFloat(0);					 // conditionWill
				packet.PutFloat(0);					 // conditionLuck
				packet.PutByte(9);					 // ElementPhysical
				packet.PutByte(0);					 // ElementLightning
				packet.PutByte(0);					 // ElementFire
				packet.PutByte(0);					 // ElementIce

				packet.PutInt((uint)creature.StatRegens.Count);
				foreach (var mod in creature.StatRegens)
					mod.AddToPacket(packet);
			}
			else if (type == CreaturePacketType.Public || type == CreaturePacketType.Minimal)
			{
				packet.PutFloat(creature.Life);
				packet.PutFloat(creature.LifeMaxBaseTotal);
				packet.PutFloat(creature.StatMods.GetMod(Stat.LifeMaxMod));
				packet.PutFloat(creature.LifeInjured);

				packet.PutInt((uint)creature.StatRegens.Count);
				foreach (var mod in creature.StatRegens)
					mod.AddToPacket(packet);

				packet.PutInt(0);
			}

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(creature.Title);
			packet.PutLong(creature.TitleApplied);
			if (type == CreaturePacketType.Private)
			{
				// List of available titles
				packet.PutShort((ushort)character.Titles.Count);
				foreach (var title in character.Titles)
				{
					packet.PutShort(title.Key);
					packet.PutByte(title.Value);
					packet.PutInt(0);
				}
			}
			if (type == CreaturePacketType.Private || type == CreaturePacketType.Public)
			{
				packet.PutShort(creature.OptionTitle);
			}

			// Items and expiring? (Last part of minimal)
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Minimal)
			{
				packet.PutString("");
				packet.PutByte(0);

				var items = creature.Items.Where(item => item.IsEquipped());

				packet.PutSInt(items.Count());
				foreach (var item in items)
				{
					packet.PutLong(item.Id);
					packet.PutBin(item.Info);
				}

				packet.PutInt(0);  // PetRemainingTime
				packet.PutLong(0); // PetLastTime
				packet.PutLong(0); // PetExpireTime

				return;
			}

			// Mate
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutLong(0);					 // MateID
				packet.PutString("");				 // MateName
				packet.PutLong(0);					 // MarriageTime
				packet.PutShort(0);					 // MarriageCount
			}
			else if (type == CreaturePacketType.Public)
			{
				packet.PutString("");				 // MateName
			}

			// Destiny
			// --------------------------------------------------------------
			packet.PutByte(0);			         // (0:Venturer, 1:Knight, 2:Wizard, 3:Bard, 4:Merchant, 5:Alchemist)

			// Inventory
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutInt(creature.RaceInfo.InvWidth);
				packet.PutInt(creature.RaceInfo.InvHeight);

				// A little dirty, but better than actually saving and managing
				// the quest items imo... [exec]
				var incompleteQuests = character.Quests.Values.Where(quest => quest.State < MabiQuestState.Complete);

				packet.PutSInt(creature.Items.Count + incompleteQuests.Count());
				foreach (var item in creature.Items)
					packet.AddItemInfo(item, ItemPacketType.Private);
				foreach (var quest in incompleteQuests)
					packet.AddItemInfo(new MabiItem(quest), ItemPacketType.Private);
			}
			else if (type == CreaturePacketType.Public)
			{
				var items = creature.Items.Where(item => item.IsEquipped());

				packet.PutSInt(items.Count());
				foreach (var item in items)
				{
					packet.PutLong(item.Id);
					packet.PutBin(item.Info);
				}
			}

			// Keywords
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutShort((ushort)character.Keywords.Count);
				foreach (var keyword in character.Keywords)
				{
					packet.PutShort(keyword);
				}
			}

			// Skills
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutShort((ushort)creature.Skills.Count);
				foreach (var skill in creature.Skills.Values)
					packet.PutBin(skill.Info);
				packet.PutInt(0);			     // SkillVarBufferList
				// loop						         
				//   packet.PutInt
				//   packet.PutFloat
			}
			else if (type == CreaturePacketType.Public)
			{
				packet.PutShort(0);			     // CurrentSkill
				packet.PutByte(0);			     // SkillStackCount
				packet.PutInt(0);			     // SkillProgress
				packet.PutInt(0);			     // SkillSyncList
				// loop						         
				//   packet.PutShort
				//   packet.PutShort
			}

			if (Feature.UnkAny1.IsEnabled())
				packet.PutByte(0);			     // {PLGCNT}

			// Banner
			// --------------------------------------------------------------
			if (creature.Party != null)
			{
				packet.PutByte(creature.Party.IsOpen && creature.Party.Leader == creature);
				packet.PutString(creature.Party.GetMemberWantedString());
			}
			else
			{
				packet.PutByte(0);
				packet.PutString("");
			}

			// PvP
			// --------------------------------------------------------------
			packet.AddPvPInfo(creature);

			// Statuses
			// --------------------------------------------------------------
			packet.PutLong((ulong)creature.Conditions.A);
			packet.PutLong((ulong)creature.Conditions.B);
			packet.PutLong((ulong)creature.Conditions.C);
			if (Feature.ConditionD.IsEnabled())
				packet.PutLong((ulong)creature.Conditions.D);
			packet.PutInt(0);					 // condition event message list
			// loop
			//   packet.PutInt
			//   packet.PutString

			if (Feature.UnkAny2.IsEnabled())
				packet.PutLong(0);

			// Guild
			// --------------------------------------------------------------
			if (creature.Guild != null)
			{
				packet.PutLong(creature.Guild.Id);
				packet.PutString(creature.Guild.Name);
				packet.PutInt(creature.GuildMemberInfo.MemberRank);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutString(creature.Guild.Title);
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
			if (type == CreaturePacketType.Private)
			{
				packet.PutLong(0);				     // ArbeitID
				packet.PutInt(0);				     // ArbeitRecordList
				// loop
				//   packet.PutShort
				//   packet.PutShort
				//   packet.PutShort
			}

			// Follower
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				if (creature.Owner != null)
				{
					packet.PutLong(creature.Owner.Id);
					packet.PutByte(2);               // Type (1:RPCharacter, 2:Pet, 3:Transport, 4:PartnerVehicle)
					packet.PutByte(0);				 // SubType
				}
				else
				{
					packet.PutLong(0);
					packet.PutByte(0);
					packet.PutByte(0);
				}
			}

			// ?
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private && Feature.UnkAny3.IsEnabled())
			{
				packet.PutFloat(1);
				packet.PutLong(0);
			}

			// Transformation
			// --------------------------------------------------------------
			packet.PutByte(0);				     // Type (1:Paladin, 2:DarkKnight, 3:SubraceTransformed, 4:TransformedElf, 5:TransformedGiant)
			packet.PutShort(0);				     // Level
			packet.PutShort(0);				     // SubType

			// Pet
			// --------------------------------------------------------------
			if (creature.Owner != null)
			{
				packet.PutString(creature.Owner.Name);
				if (type == CreaturePacketType.Private)
				{
					packet.PutInt(2000000000);			// RemainingSummonTime
					packet.PutLong(0);					// LastSummonTime
					packet.PutLong(0);					// PetExpireTime
					packet.PutByte(0);					// Loyalty
					packet.PutByte(0);					// Favor
					packet.PutLong(DateTime.Now);		// SummonTime
					packet.PutByte(0);					// KeepingMode
					packet.PutLong(0);					// KeepingProp
					packet.PutLong(creature.Owner.Id);
					packet.PutByte(0);					// PetSealCount {PSCNT}
				}
				else if (type == CreaturePacketType.Public)
				{
					packet.PutLong(creature.Owner.Id);
					packet.PutByte(0);				 // KeepingMode
					packet.PutLong(0);				 // KeepingProp
				}
			}
			else
			{
				packet.PutString("");
				if (type == CreaturePacketType.Private)
				{
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
				else if (type == CreaturePacketType.Public)
				{
					packet.PutLong(0);
					packet.PutByte(0);
					packet.PutLong(0);
				}
			}

			// House
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
				packet.PutLong(0);				 // HouseID

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
			packet.PutByte(creature.IsFlying);
			if (creature.IsFlying)
			{
				var pos = creature.GetPosition();
				packet.PutFloat(pos.X);
				packet.PutFloat(pos.H);
				packet.PutFloat(pos.Y);
				packet.PutFloat(creature.Destination.X);
				packet.PutFloat(creature.Destination.H);
				packet.PutFloat(creature.Destination.Y);
				packet.PutFloat(creature.Direction);
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
			if (type == CreaturePacketType.Private ||
			   (type == CreaturePacketType.Public && Feature.FarmingPublic.IsEnabled()))
			{
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
			}

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
			if (type == CreaturePacketType.Private)
			{
				packet.PutShort(0);
				packet.PutShort(0);
			}

			// Joust
			// --------------------------------------------------------------
			packet.PutInt(0);					 // JoustId
			if (type == CreaturePacketType.Private)
			{
				packet.PutInt(0);					 // JoustPoint
				packet.PutByte(0);					 // unknown at 0x1D
				packet.PutByte(0);					 // unknown at 0x1C
				packet.PutByte(0);					 // WeekWinCount
				packet.PutShort(0);					 // DailyWinCount
				packet.PutShort(0);					 // DailyLoseCount
				packet.PutShort(0);					 // ServerWinCount
				packet.PutShort(0);					 // ServerLoseCount
			}
			else if (type == CreaturePacketType.Public)
			{
				packet.PutInt(0);			         // JoustId
				packet.PutLong(0);			         // HorseId
				packet.PutFloat(0);	                 // Life
				packet.PutInt(100);		             // LifeMax
				packet.PutByte(9);			         // unknown at 0x6C
				packet.PutByte(0);			         // IsJousting
			}

			// Achievements
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutInt(0);	                 // TotalScore
				packet.PutShort(0);                  // AchievementList
				// loop
				//   packet.PutShort achievementId
			}

			// PrivateFarm
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private)
			{
				packet.PutInt(0);					 // FavoriteFarmList
				// loop
				//   packet.PutLong                  // FarmId
				//   packet.PutInt                   // ZoneId
				//   packet.PutShort                 // PosX
				//   packet.PutShort                 // PoxY
				//   packet.PutString                // FarmName
				//   packet.PutString                // OwnerName
			}

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
			if (type == CreaturePacketType.Private)
			{
				packet.PutInt(0);					 // SupportType (0:None, 1:Neamhain, 2:Morrighan)
			}

			// NPC
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Public && Feature.NPCOptions.IsEnabled())
			{
				if (creature.EntityType == EntityType.NPC)
				{
					packet.PutShort(0);		         // OnlyShowFilter
					packet.PutShort(0);		         // HideFilter
				}
			}

			// Commerce
			// --------------------------------------------------------------
			if (Feature.Commerce.IsEnabled())
			{
				packet.PutByte(1);               // IsInCommerceCombat
				packet.PutLong(0);               // TransportCharacterId
				packet.PutFloat(1);              // ScaleHeight
			}

			// Talents
			// --------------------------------------------------------------
			if (Feature.Talents.IsEnabled())
			{
				if (type == CreaturePacketType.Public)
				{
					packet.PutLong(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutFloat(1);
					packet.PutLong(0);
					packet.PutShort((ushort)creature.SelectedTalentTitle);
					packet.PutByte((byte)creature.Grandmaster);
				}
				else if (type == CreaturePacketType.Private)
				{
					packet.PutShort((ushort)creature.SelectedTalentTitle);
					packet.PutByte((byte)creature.Grandmaster);
					packet.PutInt(creature.GetTalentExp(TalentId.Adventure));
					packet.PutInt(creature.GetTalentExp(TalentId.Warrior));
					packet.PutInt(creature.GetTalentExp(TalentId.Mage));
					packet.PutInt(creature.GetTalentExp(TalentId.Archer));
					packet.PutInt(creature.GetTalentExp(TalentId.Merchant));
					packet.PutInt(creature.GetTalentExp(TalentId.BattleAlchemy));
					packet.PutInt(creature.GetTalentExp(TalentId.Fighter));
					packet.PutInt(creature.GetTalentExp(TalentId.Bard));
					packet.PutInt(creature.GetTalentExp(TalentId.Puppeteer));
					packet.PutInt(creature.GetTalentExp(TalentId.Knight));
					packet.PutInt(creature.GetTalentExp(TalentId.HolyArts));
					packet.PutInt(creature.GetTalentExp(TalentId.Transmutaion));
					packet.PutInt(creature.GetTalentExp(TalentId.Cooking));
					packet.PutInt(creature.GetTalentExp(TalentId.Blacksmith));
					packet.PutInt(creature.GetTalentExp(TalentId.Tailoring));
					packet.PutInt(creature.GetTalentExp(TalentId.Medicine));
					packet.PutInt(creature.GetTalentExp(TalentId.Carpentry));

					if (Feature.ZeroTalent.IsEnabled())
						packet.PutInt(0);

					// Talent titles
					// ----------------------------------------------------------
					var titles = creature.GetTalentTitles();

					packet.PutByte((byte)titles.Count);
					foreach (var title in titles)
						packet.PutShort(title);
				}
			}

			// Shamala
			// --------------------------------------------------------------
			if (Feature.Shamala.IsEnabled())
			{
				if (type == CreaturePacketType.Private)
				{
					// Transformation Diary
					packet.PutSInt(character.Shamalas.Count);
					foreach (var trans in character.Shamalas)
					{
						packet.PutInt(trans.Id);
						packet.PutByte(trans.Counter);
						packet.PutByte((byte)trans.State);
					}
				}
				else if (type == CreaturePacketType.Public)
				{
					// Current transformation info
					if (creature.Shamala != null)
					{
						packet.PutInt(creature.Shamala.Id);
						packet.PutByte(0);
						packet.PutInt(creature.ShamalaRace.Id);
						packet.PutFloat(creature.Shamala.Size);
						packet.PutInt(creature.Shamala.Color1);
						packet.PutInt(creature.Shamala.Color2);
						packet.PutInt(creature.Shamala.Color3);
					}
					else
					{
						packet.PutInt(0);
						packet.PutByte(0);
						packet.PutInt(0);
						packet.PutFloat(1);
						packet.PutInt(0x808080);
						packet.PutInt(0x808080);
						packet.PutInt(0x808080);
					}
					packet.PutByte(0);
					packet.PutByte(0);
				}
			}

			// ?
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Private && Feature.UnkAny6.IsEnabled())
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

			// Character
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Public)
			{
				packet.PutLong(0);			         // AimingTarget
				packet.PutLong(0);			         // Executor
				packet.PutShort(0);			         // ReviveTypeList
				// loop						         
				//   packet.PutInt			         
			}

			packet.PutByte(0);					 // IsGhost
			packet.PutLong(0);					 // SittingProp
			packet.PutSInt(-1);					 // SittedSocialMotionId

			// ? (Last Part of public)
			// --------------------------------------------------------------
			if (type == CreaturePacketType.Public)
			{
				packet.PutLong(0);			         // DoubleGoreTarget
				packet.PutInt(0);			         // DoubleGoreTargetType
				if (!creature.IsMoving)
				{
					packet.PutByte(0);
				}
				else
				{
					packet.PutByte((byte)(!creature.IsWalking ? 2 : 1));
					packet.PutInt(creature.Destination.X);
					packet.PutInt(creature.Destination.Y);
				}

				if (creature.EntityType == EntityType.NPC)
				{
					packet.PutString(creature.StandStyleTalk);
				}

				if (Feature.BombEvent.IsEnabled())
					packet.PutByte(0);			     // BombEventState

				if (Feature.UnkAny5.IsEnabled())
					packet.PutByte(0);

				return;
			}

			// private:

			// This int is needed in the JP client (1704 log),
			// but doesn't appear in the NA 1704 or KR test 1801 log.
			if (Feature.UnkJP1.IsEnabled())
				packet.PutInt(4);

			// Premium stuff
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
			var incompleteQuests2 = character.Quests.Values.Where(a => a.State < MabiQuestState.Complete).ToList();

			packet.PutSInt(incompleteQuests2.Count);
			foreach (var q in incompleteQuests2)
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
