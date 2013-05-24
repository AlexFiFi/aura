// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Data;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.World;
using Aura.Shared.Const;

namespace Aura.World.Player
{
	public class MabiPC : MabiCreature
	{
		public string Server;

		public ushort RebirthCount;
		public string SpouseName;
		public ulong SpouseId;
		public uint MarriageTime;
		public ushort MarriageCount;

		public DateTime CreationTime = DateTime.Now;
		public DateTime LastRebirth = DateTime.Now;

		public bool Save = false;

		public List<ushort> Keywords = new List<ushort>();
		public Dictionary<ushort, bool> Titles = new Dictionary<ushort, bool>();
		public List<ShamalaTransformation> Shamalas = new List<ShamalaTransformation>();

		public Dictionary<uint, MabiQuest> Quests = new Dictionary<uint, MabiQuest>();

		public override EntityType EntityType
		{
			get { return EntityType.Character; }
		}

		public override float CombatPower
		{
			// TODO: Cache
			get
			{
				float result = 0;

				result += this.Life;
				result += this.Mana * 0.5f;
				result += this.Stamina * 0.5f;
				result += this.Str;
				result += this.Int * 0.2f;
				result += this.Dex * 0.1f;
				result += this.Will * 0.5f;
				result += this.Luck * 0.1f;

				return result;
			}
		}

		public void GiveTitle(ushort title, bool usable = false)
		{
			if (this.Titles.ContainsKey(title))
				this.Titles[title] = usable;
			else
				this.Titles.Add(title, usable);

			if (usable)
			{
				this.Client.Send(new MabiPacket(Op.AddTitle, this.Id).PutShort(title).PutInt(0));
			}
			else
			{
				this.Client.Send(new MabiPacket(Op.AddTitleKnowledge, this.Id).PutShort(title).PutInt(0));
			}
		}

		public MabiQuest GetQuestOrNull(uint cls)
		{
			MabiQuest result = null;
			this.Quests.TryGetValue(cls, out result);
			return result;
		}

		public MabiQuest GetQuestOrNull(ulong id)
		{
			return this.Quests.Values.FirstOrDefault(a => a.Id == id);
		}

#pragma warning disable 0162
		public void AddPrivateToPacket(MabiPacket packet)
		{
			this.AddToPacket(packet, 2);

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(this.Title);
			packet.PutLong(0);                   // TitleAppliedTime

			packet.PutShort((ushort)this.Titles.Count);
			foreach (var title in this.Titles)
			{
				packet.PutShort(title.Key);
				packet.PutByte(title.Value);
				packet.PutInt(0);
			}
			packet.PutShort(this.OptionTitle);                  // SelectedOptionTitle

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

			// A little dirty, but better than actually saving and managing
			// the quest items imo... [exec]
			var incompleteQuests = this.Quests.Values.Where(a => a.State < MabiQuestState.Complete).ToList();

			packet.PutSInt(this.Items.Count + incompleteQuests.Count);
			foreach (var item in this.Items)
				item.AddToPacket(packet, ItemPacketType.Private);
			foreach (var quest in incompleteQuests)
				new MabiItem(quest).AddToPacket(packet, ItemPacketType.Private);

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
			foreach (var skill in this.Skills.Values)
			{
				packet.PutBin(skill.Info);
			}
			packet.PutInt(0);					 // SkillVarBufferList
			if (Op.Version > 140400)
				packet.PutByte(0);			     // {PLGCNT}

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

			// New PvP data maybe?
			if (Op.Version >= 170300)
			{
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
			}

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

			if (this.Guild != null)
			{
				packet.PutLong(this.Guild.Id);                   // GuildID
				packet.PutString(this.Guild.Name);                // GuildName
				packet.PutInt(this.GuildMemberInfo.MemberRank);	                 // MemberClass
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutString(this.Guild.Title);                // GuildTitle
			}
			else
			{
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
			packet.PutByte(this.IsFlying);
			if (this.IsFlying)
			{
				var pos = this.GetPosition();
				packet.PutFloat(pos.X);
				packet.PutFloat(pos.H);
				packet.PutFloat(pos.Y);
				packet.PutFloat(_destination.X);
				packet.PutFloat(_destination.H);
				packet.PutFloat(_destination.Y);
				packet.PutFloat(Direction);
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

			if (Op.Version >= 170300)
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
				packet.PutShort((ushort)this.SelectedTalentTitle);              // Selected Talent Title
				packet.PutByte((byte)this.Grandmaster); // Grandmaster ID
				packet.PutInt(this.GetTalentExp(TalentId.Adventure)); // Adventure
				packet.PutInt(this.GetTalentExp(TalentId.Warrior)); // Warrior
				packet.PutInt(this.GetTalentExp(TalentId.Mage)); // Mage
				packet.PutInt(this.GetTalentExp(TalentId.Archer)); // Acher
				packet.PutInt(this.GetTalentExp(TalentId.Merchant)); //Merchant
				packet.PutInt(this.GetTalentExp(TalentId.BattleAlchemy)); // Battle Alch
				packet.PutInt(this.GetTalentExp(TalentId.Fighter)); // Fighter
				packet.PutInt(this.GetTalentExp(TalentId.Bard)); // Bard
				packet.PutInt(this.GetTalentExp(TalentId.Puppeteer)); // Puppeteer
				packet.PutInt(this.GetTalentExp(TalentId.Knight)); // Knight
				packet.PutInt(this.GetTalentExp(TalentId.HolyArts)); // Holy Arts
				packet.PutInt(this.GetTalentExp(TalentId.Transmutaion)); // Construct Alch
				packet.PutInt(this.GetTalentExp(TalentId.Cooking)); // Chef
				packet.PutInt(this.GetTalentExp(TalentId.Blacksmith)); // Blacksmith
				packet.PutInt(this.GetTalentExp(TalentId.Tailoring)); // Tailoring
				packet.PutInt(this.GetTalentExp(TalentId.Medicine)); // Apothecary
				packet.PutInt(this.GetTalentExp(TalentId.Carpentry)); // Carpenter

				if (Op.Version >= 180100)
					packet.PutInt(0);

				// Talent titles
				// ----------------------------------------------------------
				var titles = this.GetTalentTitles();

				packet.PutByte((byte)titles.Count);               // Count
				foreach (var title in titles)
					packet.PutShort(title);
			}

			// Transformations Diary (Shamala)
			// --------------------------------------------------------------
			if (Op.Version >= 170300)
			{
				packet.PutSInt(this.Shamalas.Count);
				foreach (var trans in this.Shamalas)
				{
					packet.PutInt(trans.Id);
					packet.PutByte(trans.Counter);
					packet.PutByte((byte)trans.State);
				}
			}

			// ?
			// --------------------------------------------------------------
			if (Op.Version >= 180100)
			{
				packet.PutInt(0);
				packet.PutInt(0);
			}

			// 
			// --------------------------------------------------------------
			packet.PutByte(0);					 // IsGhost
			packet.PutLong(0);					 // SittingProp
			packet.PutSInt(-1);					 // SittedSocialMotionId

			// This int is needed in the JP client (1704 log),
			// but doesn't appear in the NA 1704 or KR test 1801 log.
			//packet.PutInt(4);

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
			if (Op.Version >= 170402 || (Op.Version >= 170300 && Op.Region == MabiRegion.TW))
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
				q.AddToPacket(packet);

			// Char
			// --------------------------------------------------------------
			packet.PutByte(0);					 // NaoDress (0:normal, 12:??, 13:??)
			packet.PutLong(this.CreationTime);
			packet.PutLong(this.LastRebirth);
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
