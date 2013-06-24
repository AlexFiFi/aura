// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Data;
using Aura.Shared.Network;
using Aura.Shared.Const;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public override void AddToPacket(MabiPacket packet)
		{
			this.AddToPacket(packet, 5);

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(this.Title);		 
			packet.PutLong(this.TitleApplied);
			packet.PutShort(this.OptionTitle);

			// Mate
			// --------------------------------------------------------------
			packet.PutString("");		         // MateName

			// Destiny
			// --------------------------------------------------------------
			packet.PutByte(0);			         // (0:Venturer, 1:Knight, 2:Wizard, 3:Bard, 4:Merchant, 5:Alchemist)

			// Inventory
			// --------------------------------------------------------------
			var items = this.Items.FindAll(a => a.IsEquipped());

			packet.PutInt((ushort)items.Count);
			foreach (var item in items)
			{
				packet.PutLong(item.Id);
				packet.PutBin(item.Info);
			}

			// Skills
			// --------------------------------------------------------------
			packet.PutShort(0);			         // CurrentSkill
			packet.PutByte(0);			         // SkillStackCount
			packet.PutInt(0);			         // SkillProgress
			packet.PutInt(0);			         // SkillSyncList
			// loop						         
			//   packet.PutShort
			//   packet.PutShort
			if (Feature.UnkAny1.IsEnabled())
				packet.PutByte(0);			     // {PLGCNT}

			// Party Banner
			// --------------------------------------------------------------
			if (this.Party != null)
			{
				packet.PutByte(this.Party.IsOpen && this.Party.Leader == this); 					 // IsActivate
				packet.PutString(this.Party.GetMemberWantedString());				 // Content
			}
			else
			{
				packet.PutByte(0);
				packet.PutString("");
			}

			// PvP
			// --------------------------------------------------------------
			this.AddPvPInfoToPacket(packet);

			// Statuses
			// --------------------------------------------------------------
			packet.PutLong((ulong)this.Conditions.A);
			packet.PutLong((ulong)this.Conditions.B);
			packet.PutLong((ulong)this.Conditions.C);
			if (Feature.ConditionD.IsEnabled())
				packet.PutLong((ulong)this.Conditions.D);
			packet.PutInt(0);					 // condition event message list
			// loop
			//   packet.PutInt
			//   packet.PutString

			if (Feature.UnkAny2.IsEnabled())
				packet.PutLong(0);

			// Guild
			// --------------------------------------------------------------
			if (this.Guild != null)
			{
				packet.PutLong(this.Guild.Id);
				packet.PutString(this.Guild.Name);
				packet.PutInt(this.GuildMemberInfo.MemberRank);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutString(this.Guild.Title);
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

			// Transformation
			// --------------------------------------------------------------
			packet.PutByte(0);				     // Type (1:Paladin, 2:DarkKnight, 3:SubraceTransformed, 4:TransformedElf, 5:TransformedGiant)
			packet.PutShort(0);				     // Level
			packet.PutShort(0);				     // SubType

			// Follower (Pets)
			// --------------------------------------------------------------
			if (this.Owner != null)
			{
				packet.PutString(this.Owner.Name);
				packet.PutLong(this.Owner.Id);
				packet.PutByte(0);				 // KeepingMode
				packet.PutLong(0);				 // KeepingProp
			}
			else
			{
				packet.PutString("");
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutLong(0);
			}

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
			packet.PutLong(0);			         // VehicleId
			packet.PutInt(0);                    // SeatIndex
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
				packet.PutFloat(Destination.X);
				packet.PutFloat(Destination.H);
				packet.PutFloat(Destination.Y);
				packet.PutFloat(this.Direction);
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
			if (Feature.FarmingPublic.IsEnabled())
			{
				// This seems to be missing in 170400

				packet.PutLong(0);			     // FarmId
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

			// Joust
			// --------------------------------------------------------------
			packet.PutInt(0);			         // JoustId
			packet.PutLong(0);			         // HorseId
			packet.PutFloat(0.0f);	             // Life
			packet.PutInt(100);		             // LifeMax
			packet.PutByte(9);			         // unknown at 0x6C
			packet.PutByte(0);			         // IsJousting

			// Family
			// --------------------------------------------------------------
			packet.PutLong(0);					 // FamilyId
			// loop
			//   packet.WriteString				 // FamilyName
			//   packet.PutShort
			//   packet.PutShort
			//   packet.PutShort
			//   packet.WriteString				 // FamilyTitle

			// NPC
			// --------------------------------------------------------------
			if (Feature.NPCOptions.IsEnabled())
			{
				if (this.EntityType == EntityType.NPC)
				{
					packet.PutShort(0);		         // OnlyShowFilter
					packet.PutShort(0);		         // HideFilter
				}
			}

			// Commerce
			// --------------------------------------------------------------
			if (Feature.Commerce.IsEnabled())
			{
				packet.PutByte(1);					 // IsInCommerceCombat
				packet.PutLong(0);					 // TransportCharacterId
				packet.PutFloat(1);					 // ScaleHeight
			}

			// Talents
			// --------------------------------------------------------------
			if (Feature.Talents.IsEnabled())
			{
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutFloat(1);
				packet.PutLong(0);
				packet.PutShort((ushort)this.SelectedTalentTitle);
				packet.PutByte((byte)this.Grandmaster);
			}

			// Shamala Transformation
			// --------------------------------------------------------------
			if (Feature.Shamala.IsEnabled())
			{
				if (this.Shamala == null)
				{
					packet.PutInt(0);
					packet.PutByte(0);
					packet.PutInt(0);
					packet.PutFloat(1);
					packet.PutInt(0x808080);
					packet.PutInt(0x808080);
					packet.PutInt(0x808080);
				}
				else
				{
					packet.PutInt(this.Shamala.Id);
					packet.PutByte(0);
					packet.PutInt(this.ShamalaRace.Id);
					packet.PutFloat(this.Shamala.Size);
					packet.PutInt(this.Shamala.Color1);
					packet.PutInt(this.Shamala.Color2);
					packet.PutInt(this.Shamala.Color3);
				}
				packet.PutByte(0);
				packet.PutByte(0);
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
			packet.PutLong(0);			         // AimingTarget
			packet.PutLong(0);			         // Executor
			packet.PutShort(0);			         // ReviveTypeList
			// loop						         
			//   packet.PutInt			         

			packet.PutByte(0);	                 // IsGhost
			packet.PutLong(0);			         // SittingProp
			packet.PutSInt(-1);		             // SittedSocialMotionId

			packet.PutLong(0);			         // DoubleGoreTarget
			packet.PutInt(0);			         // DoubleGoreTargetType
			if (!this.IsMoving)
			{
				packet.PutByte(0);
			}
			else
			{
				packet.PutByte((byte)(!IsWalking ? 2 : 1));
				packet.PutInt(Destination.X);
				packet.PutInt(Destination.Y);
			}

			if (this.EntityType == EntityType.NPC)
			{
				packet.PutString(this.StandStyleTalk);
			}

			if (Feature.BombEvent.IsEnabled())
				packet.PutByte(0);			     // BombEventState

			if (Feature.UnkAny5.IsEnabled())
				packet.PutByte(0);
		}

		public void AddToPacket(MabiPacket packet, byte type)
		{
			packet.PutLong(Id);
			packet.PutByte(type);

			// Looks/Location
			// --------------------------------------------------------------
			var loc = this.GetPosition();
			packet.PutString(Name);
			packet.PutString("");				 // Title
			packet.PutString("");				 // Eng Title
			packet.PutInt(this.Race);
			packet.PutByte(this.SkinColor);
			packet.PutByte(this.Eye);
			packet.PutByte(this.EyeColor);
			packet.PutByte(this.Lip);
			packet.PutInt((uint)this.State);
			if (type == 5)
			{
				packet.PutInt((uint)this.StateEx);
			}
			packet.PutFloat(this.Height);
			packet.PutFloat(this.Fat);
			packet.PutFloat(this.Upper);
			packet.PutFloat(this.Lower);
			packet.PutInt(this.Region);
			packet.PutInt(loc.X);
			packet.PutInt(loc.Y);
			packet.PutByte(this.Direction);
			packet.PutInt(this.BattleState);
			packet.PutByte(this.WeaponSet);
			packet.PutInt(this.Color1);
			packet.PutInt(this.Color2);
			packet.PutInt(this.Color3);

			// Stats
			// --------------------------------------------------------------
			packet.PutFloat(this.CombatPower);
			packet.PutString(this.Shamala == null ? this.StandStyle : "");

			if (type == 2)
			{
				packet.PutFloat(this.Life);
				packet.PutFloat(this.LifeInjured);
				packet.PutFloat(this.LifeMaxBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.LifeMaxMod));
				packet.PutFloat(this.Mana);
				packet.PutFloat(this.ManaMaxBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.ManaMaxMod));
				packet.PutFloat(this.Stamina);
				packet.PutFloat(this.StaminaMaxBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.StaminaMaxMod));
				packet.PutFloat(this.StaminaHunger);
				packet.PutFloat(0.5f);
				packet.PutShort(this.Level);
				packet.PutInt(this.LevelTotal);
				packet.PutShort(0);                  // Max Level
				packet.PutShort(0);					 // Rebirthes
				packet.PutShort(0);
				packet.PutLong(MabiData.ExpDb.CalculateRemaining(this.Level, this.Experience) * 1000);
				packet.PutShort(Age);
				packet.PutFloat(this.StrBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.StrMod));
				packet.PutFloat(this.DexBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.DexMod));
				packet.PutFloat(this.IntBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.IntMod));
				packet.PutFloat(this.WillBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.WillMod));
				packet.PutFloat(this.LuckBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.LuckMod));
				packet.PutFloat(0);					 // LifeMaxByFood
				packet.PutFloat(0);					 // ManaMaxByFood
				packet.PutFloat(0);					 // StaminaMaxByFood
				packet.PutFloat(0);					 // StrengthByFood
				packet.PutFloat(0);					 // DexterityByFood
				packet.PutFloat(0);					 // IntelligenceByFood
				packet.PutFloat(0);					 // WillByFood
				packet.PutFloat(0);					 // LuckByFood
				packet.PutShort(this.AbilityPoints);
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
				packet.PutFloat(this.StatMods.GetMod(Stat.ProtectMod));			         // ProtectMod
				packet.PutShort(0);			         // DefenseBase
				packet.PutShort((ushort)this.StatMods.GetMod(Stat.DefenseMod));			         // DefenseMod
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
				packet.PutFloat(this.ProtectionPassive * 100);		             // ProtectBaseMod
				packet.PutShort((ushort)this.DefensePassive);		             // DefenseBaseMod
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

				packet.PutInt((uint)_statRegens.Count);
				foreach (var mod in _statRegens)
					mod.AddToPacket(packet);
			}
			else
			{
				packet.PutFloat(this.Life);
				packet.PutFloat(this.LifeMaxBaseTotal);
				packet.PutFloat(this.StatMods.GetMod(Stat.LifeMaxMod));
				packet.PutFloat(this.LifeInjured);

				packet.PutInt((uint)_statRegens.Count);
				foreach (var mod in _statRegens)
					mod.AddToPacket(packet);

				packet.PutInt(0);
			}
		}

		public void AddPvPInfoToPacket(MabiPacket p)
		{
			var arena = this.ArenaPvPManager != null;
			p
				.PutByte(arena) // ArenaPvP
				.PutInt(arena ? this.ArenaPvPManager.GetTeam(this) : (uint)0)
				.PutByte(this.TransPvPEnabled)
				.PutInt(arena ? this.ArenaPvPManager.GetStars(this) : 0)
				.PutByte(this.EvGEnabled)
				.PutByte(this.EvGSupportRace)
				.PutByte(1) // IsPvPMode
				.PutLong(this.PvPWins)
				.PutLong(this.PvPLosses)
				.PutInt(0) // PenaltyPoints
				.PutByte(1); // unk

			if (Feature.NewPVPInfo.IsEnabled())
			{
				p.PutByte(0);
				p.PutInt(0);
				p.PutInt(0);
				p.PutInt(0);
				p.PutInt(0);
			}
		}
	}
}
