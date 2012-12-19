// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Common.Network;
using Common.Constants;

namespace Common.World
{
	public class MabiPC : MabiCreature
	{
		public string Server;

		public ushort RebirthCount;
		public string SpouseName;
		public ulong SpouseId;
		public uint MarriageTime;
		public ushort MarriageCount;

		public DateTime DeletionTime;

		public bool Save = false;

		public List<ushort> Keywords = new List<ushort>();
		public Dictionary<ushort, bool> Titles = new Dictionary<ushort, bool>();

		public override EntityType EntityType
		{
			get { return EntityType.Character; }
		}

		/// <summary>
		/// Used in login check handler. Modes: 0 = Normal, 1 = Recover, 2 = Ready for deletion
		/// </summary>
		/// <returns></returns>
		public byte GetDeletionFlag()
		{
			return (byte)((this.DeletionTime <= DateTime.MinValue) ? 0 : ((this.DeletionTime >= DateTime.Now) ? 1 : 2));
		}

#pragma warning disable 0162
		public void AddPrivateEntityData(MabiPacket packet)
		{
			this.AddEntityData(packet, 2);

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(this.Title);
			packet.PutLong(0);                   // TitleAppliedTime

			packet.PutShort((ushort)this.Titles.Count);
			foreach (var title in this.Titles)
			{
				packet.PutShort(title.Key);
				packet.PutByte((byte)(title.Value ? 1 : 0));
				packet.PutInt(0);
			}
			packet.PutShort(0);                  // SelectedOptionTitle

			// Mate
			// --------------------------------------------------------------
			packet.PutLong(0);					 // MateID
			packet.PutString("");				 // MateName
			packet.PutLong(0);					 // MarriageTime
			packet.PutShort(0);					 // MarriageCount

			packet.PutByte(0);					 // JobId

			// Inventory
			// --------------------------------------------------------------
			packet.PutInt(this.RaceInfo.InvWidth);
			packet.PutInt(this.RaceInfo.InvHeight);

			packet.PutInt((ushort)this.Items.Count);
			foreach (var item in this.Items)
			{
				packet.PutLong(item.Id);
				packet.PutByte(2);
				packet.PutBin(item.Info);
				packet.PutBin(item.OptionInfo);
				packet.PutString("");
				packet.PutString("");
				packet.PutByte(0);
				packet.PutLong(0);
			}

			// Keywords
			// --------------------------------------------------------------
			packet.PutShort((ushort)this.Keywords.Count);
			foreach (var keyword in this.Keywords)
			{
				packet.PutShort(keyword);
			}

			// Skills
			// --------------------------------------------------------------
			packet.PutShort((ushort)this.Skills.Count);
			foreach (var skill in this.Skills)
			{
				packet.PutBin(skill.Info);
			}
			packet.PutInt(0);					 // SkillVarBufferList
			if (Op.Version > 140400)
				packet.PutByte(0);					 // {PLGCNT}

			// Banner
			// --------------------------------------------------------------
			packet.PutByte(0); 					 // IsActivate
			packet.PutString("");				 // Content

			// PvP
			// --------------------------------------------------------------
			packet.PutByte(0);                   // IsAttackFree
			packet.PutInt(0);                    // ArenaTeam
			packet.PutByte(0);                   // IsTransformPVP
			packet.PutInt(0);                    // Point
			packet.PutByte(0);                   // IsGiantElfPVP
			packet.PutByte(0);                   // SupportRaceType
			packet.PutByte(0);                   // IsPVPMode
			packet.PutLong(0);                   // WinCount
			packet.PutLong(0);                   // LoseCount
			packet.PutInt(0);                    // PenaltyPoint
			packet.PutByte(1);					 // IsCommonPVP

			// Statuses
			// --------------------------------------------------------------
			packet.PutLong((ulong)Conditions.A);
			packet.PutLong((ulong)Conditions.B);
			packet.PutLong((ulong)Conditions.C);
			if (Op.Version > 140400)
				packet.PutLong((ulong)Conditions.D);
			packet.PutInt(0);					 // condition event message list
			// loop
			//   packet.PutInt
			//   packet.PutString

			if (Op.Version >= 170100)
				packet.PutLong(0);

			// Guild
			// --------------------------------------------------------------
			packet.PutLong(0);                   // GuildID
			packet.PutString("");                // GuildName
			packet.PutInt(0);	                 // MemberClass
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutString("");                // GuildTitle

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
			if (this.Owner == null)
			{
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutByte(0);
			}
			else
			{
				packet.PutLong(this.Owner.Id);	 // MasterID
				packet.PutByte(2);               // Type (1:RPCharacter, 2:Pet, 3:Transport, 4:PartnerVehicle)
				packet.PutByte(0);				 // SubType
			}

			// ?
			// --------------------------------------------------------------
			if (Op.Version >= 170100)
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
			if (this.Owner == null)
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
				packet.PutString(this.Owner.Name);
				packet.PutInt(2000000000);			                         // RemainingSummonTime
				packet.PutLong(0);								             // LastSummonTime
				packet.PutLong(0);								             // PetExpireTime
				packet.PutByte(0);											 // Loyalty
				packet.PutByte(0);											 // Favor
				packet.PutLong(DateTime.Now);								 // SummonTime
				packet.PutByte(0);											 // KeepingMode
				packet.PutLong(0);											 // KeepingProp
				packet.PutLong(this.Owner.Id);	                             // OwnerID
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
			packet.PutByte(0);//packet.PutByte(this.Flying);					 // IsAviating
			// loop
			//   packet.PutFloat				 // FromX
			//   packet.PutFloat				 // FromHeight
			//   packet.PutFloat				 // FromY
			//   packet.PutFloat				 // ToX
			//   packet.PutFloat				 // ToHeight
			//   packet.PutFloat				 // ToY
			//   packet.PutFloat				 // Direction

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
			if (Op.Version > 140400)
			{
				packet.PutByte(1);               // IsInCommerceCombat
				packet.PutLong(0);               // TransportCharacterId
				packet.PutFloat(1);              // ScaleHeight
			}

			// Talents?
			// --------------------------------------------------------------
			if (Op.Version >= 170100)
			{
				packet.PutShort(0);              // Selected Talent Title
				packet.PutByte(255);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);

				// Talent titles
				// ----------------------------------------------------------
				packet.PutByte(0);               // Count
				//packet.PutShort(id);
			}

			// ?
			// --------------------------------------------------------------
			if (Op.Version >= 170300)
			{
				packet.PutInt(0);               // Count
				//    packet.PutInt             // 1, 2, 3, ...
				//    packet.PutByte			// 1 | 2 | 3
				//    packet.PutByte			// 1 | 2 | 3
			}

			// ?
			// --------------------------------------------------------------
			if (Op.Version >= 170300)
			{
				packet.PutInt(0);
			}

			// 
			// --------------------------------------------------------------
			packet.PutByte(0);					 // IsGhost
			packet.PutLong(0);					 // SittingProp
			packet.PutSInt(-1);					 // SittedSocialMotionId

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
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);

			// Quests
			// --------------------------------------------------------------
			packet.PutInt(0);

			// Char
			// --------------------------------------------------------------
			packet.PutByte(0);					 // NaoDress (0:normal, 12:??, 13:??)
			packet.PutLong(0x39A0CE2EC180);      // Char creation?
			packet.PutLong(0);                   // Last rebirth?
			packet.PutString("");
			packet.PutByte(0);
			packet.PutByte(2);

			// Pocket ExpireTime List
			// --------------------------------------------------------------
			if (Op.Version > 140400)
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
#pragma warning restore 0162
}
