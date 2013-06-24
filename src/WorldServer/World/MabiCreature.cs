// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Data;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Skills;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public const float HandBalance = 0.3f;

		public Client Client = new DummyClient();

		public string Name;

		public MabiGuild Guild;
		public MabiGuildMemberInfo GuildMemberInfo;

		public uint Race;
		public RaceInfo RaceInfo = null;

		public uint BattleExp = 0;

		private Dictionary<Stat, object> _stats = new Dictionary<Stat, object>();

		private List<MabiStatRegen> _statRegens = new List<MabiStatRegen>();
		public readonly MabiStatMods StatMods = new MabiStatMods();
		public MabiStatRegen LifeRegen, ManaRegen, StaminaRegen;

		public byte SkinColor, Eye, EyeColor, Lip;
		public string StandStyle = "";
		public string StandStyleTalk = "";

		public uint Color1 = 0x808080;
		public uint Color2 = 0x808080;
		public uint Color3 = 0x808080;

		public byte WeaponSet;
		public byte BattleState;
		public bool IsFlying;

		private ushort _title;
		/// <summary>
		/// Changes/Returns Title and updates TitleApplied.
		/// </summary>
		public ushort Title { get { return _title; } set { _title = value; this.TitleApplied = DateTime.Now; } }
		public ushort OptionTitle;
		public DateTime TitleApplied { get; protected set; }

		public Dictionary<ushort, MabiSkill> Skills = new Dictionary<ushort, MabiSkill>();
		//public MabiSkill ActiveSkill;
		public SkillConst ActiveSkillId;
		public byte ActiveSkillStacks;
		public DateTime ActiveSkillPrepareEnd;
		public uint SoulCount;

		public Dictionary<TalentId, Dictionary<SkillConst, uint>> TalentExps = new Dictionary<TalentId, Dictionary<SkillConst, uint>>();
		public TalentId Grandmaster;
		public TalentTitle SelectedTalentTitle;

		public CreatureTemp Temp = new CreatureTemp();

		public List<MabiCooldown> Cooldowns = new List<MabiCooldown>();

		public MabiCutscene CurrentCutscene;

		public ArenaPvPManager ArenaPvPManager;
		public ulong PvPWins, PvPLosses;
		public bool EvGEnabled, TransPvPEnabled;

		private byte _evgSupportRace;
		public byte EvGSupportRace { get { return (byte)(this.IsHuman ? _evgSupportRace : (this.IsElf ? 1 : 2)); } set { _evgSupportRace = value; } }

		public byte Direction;
		private readonly MabiVertex _position = new MabiVertex(0, 0);
		public MabiVertex Destination { get; protected set; }
		private DateTime _moveStartTime;
		private double _movementX, _movementY, _movementH;
		private double _moveDuration;
		public bool IsWalking { get; protected set; }

		public MabiCreature Target = null;
		public float _stun;
		private DateTime _stunChange;
		public bool WaitingForRes = false;
		public float _knockBack;
		private DateTime _knockBackChange;

		public MabiCreature Owner, Pet, Vehicle;

		public readonly List<MabiCreature> Riders = new List<MabiCreature>();

		public CreatureStates State;
		public CreatureStatesEx StateEx;
		public CreatureCondition Conditions;
		public CreatureCondition PrevConditions;

		public ulong LastEventTriggered = 0;
		public DungeonAltar OnAltar = DungeonAltar.None;

		public MabiParty Party = null;
		public uint PartyNumber = 0;

		public ShamalaInfo Shamala = null;
		public RaceInfo ShamalaRace = null;

		public byte RestPose
		{
			get
			{
				var skill = this.GetSkill(SkillConst.Rest);
				if (skill == null)
					return 0;

				byte pose = 0;
				if (skill.Rank >= SkillRank.R9)
					pose = 4;
				if (skill.Rank >= SkillRank.R1)
					pose = 5;

				return pose;
			}
		}

		public bool LevelingEnabled = false;

		public override ushort DataType
		{
			get { return 16; }
		}

		/// <summary>
		/// No actions supposed to be possible, while being stunned.
		/// </summary>
		public ushort Stun
		{
			get
			{
				if (_stun <= 0)
					return 0;

				var result = _stun - (DateTime.Now - _stunChange).TotalMilliseconds;
				if (result <= 0)
					result = _stun = 0;

				return (ushort)result;
			}

			set
			{
				if (value <= 0)
					_stun = 0;

				_stunChange = DateTime.Now;
				_stun = value;
			}
		}

		public virtual float CombatPower { get { return (this.RaceInfo != null ? this.RaceInfo.CombatPower : 1); } }

		public float KnockBack
		{
			get
			{
				if (_knockBack <= 0)
					return 0;

				var result = _knockBack - ((DateTime.Now - _knockBackChange).TotalMilliseconds / 60f);
				if (result <= 0)
					result = _knockBack = 0;

				return (float)result;
			}
			set
			{
				_knockBack = Math.Min(CombatHelper.MaxKnockBack, value);
				_knockBackChange = DateTime.Now;
			}
		}

		public bool IsStunned { get { return (this.Stun > 0); } }
		public bool IsDead { get { return this.Has(CreatureStates.Dead); } }

		public bool IsMoving { get { return (!_position.Equals(Destination)); } }

		public float Height { get; set; }
		public float Fat { get; set; }
		public float Upper { get; set; }
		public float Lower { get; set; }

		public float StrBaseSkill { get; set; }

		public float WillBaseSkill { get; set; }

		public float IntBaseSkill { get; set; }

		public float LuckBaseSkill { get; set; }

		public float DexBaseSkill { get; set; }

		public float ManaMaxBaseSkill { get; set; }

		public float LifeMaxBaseSkill { get; set; }

		public float StaminaMaxBaseSkill { get; set; }

		private float _life, _injuries;
		public float Life
		{
			get { return _life; }
			set
			{
				if (value > this.LifeInjured)
					_life = this.LifeInjured;
				else if (value < -this.LifeMax)
					_life = -this.LifeMax;
				else
					_life = value;

				/* Bad server lagger here
				if (_life < 0) //Todo: cache?
				{
					if ((this.Conditions.A & CreatureConditionA.Deadly) != CreatureConditionA.Deadly)
					{
						this.Conditions.A |= CreatureConditionA.Deadly; //Todo: What about prevCondition?
						EntityEvents.Instance.OnCreatureStatUpdates(this);
					}
				}
				else
				{
					if ((this.Conditions.A & CreatureConditionA.Deadly) == CreatureConditionA.Deadly)
					{
						this.Conditions.A &= ~CreatureConditionA.Deadly;
						EntityEvents.Instance.OnCreatureStatUpdates(this);
					}
				}*/
			}
		}
		public float Injuries
		{
			get { return _injuries; }
			set
			{
				if (value < 0)
					_injuries = 0;
				else
					_injuries = value;
			}
		}
		public float LifeMaxBase { get; set; }
		public float LifeMaxBaseTotal { get { return this.LifeMaxBase + this.LifeMaxBaseSkill; } }
		public float LifeMax { get { return this.LifeMaxBaseTotal + this.StatMods.GetMod(Stat.LifeMaxMod); } }
		public float LifeInjured { get { return this.LifeMax - _injuries; } }

		private float _mana;
		public float Mana
		{
			get { return _mana; }
			set
			{
				if (value > this.ManaMax)
					_mana = this.ManaMax;
				else if (value < 0)
					_mana = 0;
				else
					_mana = value;
			}
		}

		public float ManaMaxBase { get; set; }
		public float ManaMaxBaseTotal { get { return this.ManaMaxBase + this.ManaMaxBaseSkill; } }
		public float ManaMax { get { return ManaMaxBaseTotal + this.StatMods.GetMod(Stat.ManaMaxMod); } }

		private float _stamina, _hunger;
		public float Stamina
		{
			get { return _stamina; }
			set
			{
				if (value > this.StaminaHunger)
					_stamina = this.StaminaHunger;
				else if (value < 0)
					_stamina = 0;
				else
					_stamina = value;
			}
		}
		public float Hunger
		{
			get { return _hunger; }
			set
			{
				if (value < 0)
					_hunger = 0;
				else
					_hunger = value;
			}
		}
		public float StaminaMaxBase { get; set; }
		public float StaminaMaxBaseTotal { get { return this.StaminaMaxBase + this.StaminaMaxBaseSkill; } }
		public float StaminaMax { get { return this.StaminaMaxBaseTotal + this.StatMods.GetMod(Stat.StaminaMaxMod); } }
		public float StaminaHunger { get { return this.StaminaMax - _hunger; } }

		public List<MabiStatRegen> StatRegens { get { return _statRegens; } }

		public float StrBase { get; set; }
		public float DexBase { get; set; }
		public float IntBase { get; set; }
		public float WillBase { get; set; }
		public float LuckBase { get; set; }
		public float StrBaseTotal { get { return this.StrBase + this.StrBaseSkill; } }
		public float DexBaseTotal { get { return this.DexBase + this.DexBaseSkill; } }
		public float IntBaseTotal { get { return this.IntBase + this.IntBaseSkill; } }
		public float WillBaseTotal { get { return this.WillBase + this.WillBaseSkill; } }
		public float LuckBaseTotal { get { return this.LuckBase + this.LuckBaseSkill; } }
		public float Str { get { return this.StrBaseTotal + this.StatMods.GetMod(Stat.StrMod); } }
		public float Dex { get { return this.DexBaseTotal + this.StatMods.GetMod(Stat.DexMod); } }
		public float Int { get { return this.IntBaseTotal + this.StatMods.GetMod(Stat.IntMod); } }
		public float Will { get { return this.WillBaseTotal + this.StatMods.GetMod(Stat.WillMod); } }
		public float Luck { get { return this.LuckBaseTotal + this.StatMods.GetMod(Stat.LuckMod); } }

		public byte Age { get; set; }

		public ushort AbilityPoints { get; set; }

		public ushort Level { get; set; }
		public uint LevelTotal { get; set; }

		private ulong _exp;
		public ulong Experience { get { return _exp; } set { _exp = Math.Min(value, ulong.MaxValue); } }

		public int DefenseBase { get; set; }
		public int ProtectionBase { get; set; }

		public int DefenseBaseSkill { get; set; }
		public int ProtectionBaseSkill { get; set; }

		public DateTime AimStart;

		public DeadMenuOptions RevivalOptions;
		public DeathCauses CauseOfDeath;

		/// <summary>
		/// Returns base defense and passive defenses
		/// </summary>
		public int DefensePassive
		{
			get
			{
				float result = this.DefenseBase + this.DefenseBaseSkill + this.StatMods.GetMod(Stat.DefenseBaseMod);

				if (this.RaceInfo != null)
					result += this.RaceInfo.Defense;

				// Add equips
				result += this.Items.Where(i => i.IsEquipped(false, this)).Sum(i => i.OptionInfo.Defense);

				return (int)result;

			}
		}

		/// <summary>
		/// Returns total defense, including passive and active Defense bonus.
		/// </summary>
		public int DefenseTotal
		{
			get
			{
				// Don't use DefensePassive, because float truncation

				float result = this.DefenseBase + this.DefenseBaseSkill + this.StatMods.GetMod(Stat.DefenseBaseMod);

				if (this.RaceInfo != null)
					result += this.RaceInfo.Defense;

				// Add base def and def bonus if active
				var defenseSkill = this.GetSkill(SkillConst.Defense);
				if (defenseSkill != null)
				{
					result += defenseSkill.RankInfo.Var1;
					if (this.ActiveSkillId == SkillConst.Defense)
						result += defenseSkill.RankInfo.Var3;
				}

				// Add equips
				result += this.Items.Where(i => i.IsEquipped(false, this)).Sum(i => i.OptionInfo.Defense);

				return (int)result;
			}
		}

		/// <summary>
		/// Returns total protection as a float between 0 and 1
		/// </summary>
		public float ProtectionPassive
		{
			get
			{
				float result = this.ProtectionBase + this.ProtectionBaseSkill + this.StatMods.GetMod(Stat.ProtectMod);

				// Add equips
				result += this.Items.Where(i => i.IsEquipped(false, this)).Sum(i => i.OptionInfo.Protection);

				return (result / 100);
			}
		}

		/// <summary>
		/// Returns total protection as a float between 0 and 1, including active Defense bonus.
		/// </summary>
		public float Protection
		{
			get
			{
				float result = this.ProtectionBase + this.ProtectionBaseSkill + this.StatMods.GetMod(Stat.ProtectMod);

				// Add def bonus if active
				var defenseSkill = this.GetSkill(SkillConst.Defense);
				if (defenseSkill != null && this.ActiveSkillId == SkillConst.Defense)
					result += defenseSkill.RankInfo.Var4;

				// Add equips
				result += this.Items.Where(i => i.IsEquipped(false, this)).Sum(i => i.OptionInfo.Protection);

				return (result / 100);
			}
		}

		public float CriticalChance
		{
			get
			{
				float result = 0;

				result += (this.Will - 10) / 10f;
				result += (this.Luck - 10) / 5f;
				result += this.StatMods.GetMod(Stat.CriticalMod);

				return result / 100f;
			}
		}

		public float CriticalChanceAgainst(MabiCreature other)
		{
			return (this.CriticalChance - (other.Protection * 2));
		}

		public float CriticalMultiplier
		{
			get
			{
				return 1.5f; // R1 Critical Hit
			}
		}

		public float MagicAttack
		{
			get
			{
				return this.Int / 20f;
			}
		}

		public bool IsPlayer { get { return (this.EntityType == EntityType.Character || this.EntityType == EntityType.Pet); } }

		public bool IsHuman { get { return (this.Race == 10001 || this.Race == 10002); } }
		public bool IsElf { get { return (this.Race == 9001 || this.Race == 9002); } }
		public bool IsGiant { get { return (this.Race == 8001 || this.Race == 8002); } }

		public bool IsMale { get { return (this.RaceInfo != null && this.RaceInfo.Gender == Gender.Male); } }
		public bool IsFemale { get { return (this.RaceInfo != null && this.RaceInfo.Gender == Gender.Female); } }

		public MabiCreature()
		{
			this.Destination = new MabiVertex(0, 0);
		}

		public bool Has(CreatureConditionA condition) { return ((this.Conditions.A & condition) != 0); }
		public bool Has(CreatureConditionB condition) { return ((this.Conditions.B & condition) != 0); }
		public bool Has(CreatureConditionC condition) { return ((this.Conditions.C & condition) != 0); }
		public bool Has(CreatureConditionD condition) { return ((this.Conditions.D & condition) != 0); }
		public bool Has(CreatureStates state) { return ((this.State & state) != 0); }

		public bool HasSkillLoaded(SkillConst skill) { return this.ActiveSkillId == skill; }

		public static ushort GetTalentTitle(TalentTitle talent, TalentLevel level)
		{
			if (level == 0)
				return 0;

			return (ushort)((ushort)talent + (byte)level);
		}

		public List<ushort> GetTalentTitles()
		{
			var titles = new List<ushort>();

			var adventureLevel = this.GetTalentLevel(TalentId.Adventure);
			var warriorLevel = this.GetTalentLevel(TalentId.Warrior);
			var mageLevel = this.GetTalentLevel(TalentId.Mage);
			var archerLevel = this.GetTalentLevel(TalentId.Archer);
			var merchantLevel = this.GetTalentLevel(TalentId.Merchant);
			var battleAlchemyLevel = this.GetTalentLevel(TalentId.BattleAlchemy);
			var fighterLevel = this.GetTalentLevel(TalentId.Fighter);
			var bardLevel = this.GetTalentLevel(TalentId.Bard);
			var puppeteerLevel = this.GetTalentLevel(TalentId.Puppeteer);
			var knightLevel = this.GetTalentLevel(TalentId.Knight);
			var holyArtsLevel = this.GetTalentLevel(TalentId.HolyArts);
			var transmutaionLevel = this.GetTalentLevel(TalentId.Transmutaion);
			var cookingLevel = this.GetTalentLevel(TalentId.Cooking);
			var blacksmithLevel = this.GetTalentLevel(TalentId.Blacksmith);
			var tailoringLevel = this.GetTalentLevel(TalentId.Tailoring);
			var medicineLevel = this.GetTalentLevel(TalentId.Medicine);
			var carpentryLevel = this.GetTalentLevel(TalentId.Carpentry);

			titles.Add(GetTalentTitle(TalentTitle.Adventure, adventureLevel));
			titles.Add(GetTalentTitle(TalentTitle.Warrior, warriorLevel));
			titles.Add(GetTalentTitle(TalentTitle.Mage, mageLevel));
			titles.Add(GetTalentTitle(TalentTitle.Archer, archerLevel));
			titles.Add(GetTalentTitle(TalentTitle.Merchant, merchantLevel));
			titles.Add(GetTalentTitle(TalentTitle.BattleAlchemy, battleAlchemyLevel));
			titles.Add(GetTalentTitle(TalentTitle.Fighter, fighterLevel));
			titles.Add(GetTalentTitle(TalentTitle.Bard, bardLevel));
			titles.Add(GetTalentTitle(TalentTitle.Puppeteer, puppeteerLevel));
			titles.Add(GetTalentTitle(TalentTitle.Knight, knightLevel));
			titles.Add(GetTalentTitle(TalentTitle.HolyArts, holyArtsLevel));
			titles.Add(GetTalentTitle(TalentTitle.Transmutaion, transmutaionLevel));
			titles.Add(GetTalentTitle(TalentTitle.Cooking, cookingLevel));
			titles.Add(GetTalentTitle(TalentTitle.Blacksmith, blacksmithLevel));
			titles.Add(GetTalentTitle(TalentTitle.Tailoring, tailoringLevel));
			titles.Add(GetTalentTitle(TalentTitle.Medicine, medicineLevel));
			titles.Add(GetTalentTitle(TalentTitle.Carpentry, carpentryLevel));

			if ((byte)blacksmithLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Artisan, GetHybridTalentLevel(blacksmithLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Artiste, GetHybridTalentLevel(bardLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.BattleBard, GetHybridTalentLevel(bardLevel, warriorLevel)));

			if ((byte)warriorLevel >= 6 && (byte)mageLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.BattleMage, GetHybridTalentLevel(warriorLevel, mageLevel, fighterLevel)));

			if ((byte)archerLevel >= 6 && (byte)battleAlchemyLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Bombardier, GetHybridTalentLevel(archerLevel, battleAlchemyLevel)));

			if ((byte)archerLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Bowyer, GetHybridTalentLevel(archerLevel, carpentryLevel)));

			if ((byte)warriorLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Brawler, GetHybridTalentLevel(warriorLevel, fighterLevel)));

			if ((byte)fighterLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Challenger, GetHybridTalentLevel(fighterLevel, adventureLevel)));

			if ((byte)bardLevel >= 12 && (byte)holyArtsLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Cheerleader, GetHybridTalentLevel(bardLevel, holyArtsLevel, adventureLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)bardLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Choirmaster, GetHybridTalentLevel(holyArtsLevel, bardLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)tailoringLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.CostumeDesigner, GetHybridTalentLevel(puppeteerLevel, tailoringLevel)));

			if ((byte)medicineLevel >= 12 && (byte)merchantLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Doctor, GetHybridTalentLevel(medicineLevel, merchantLevel)));

			if ((byte)mageLevel >= 6 && (byte)battleAlchemyLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Elementalist, GetHybridTalentLevel(mageLevel, battleAlchemyLevel)));

			if ((byte)warriorLevel >= 6 && (byte)carpentryLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Executioner, GetHybridTalentLevel(warriorLevel, carpentryLevel, knightLevel)));

			if ((byte)fighterLevel >= 12 && (byte)cookingLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.FoodFighter, GetHybridTalentLevel(fighterLevel, cookingLevel)));

			if ((byte)battleAlchemyLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.FullAlchemist, GetHybridTalentLevel(battleAlchemyLevel, transmutaionLevel)));

			if ((byte)cookingLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Gourmet, GetHybridTalentLevel(cookingLevel, adventureLevel)));

			if ((byte)bardLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Gypsy, GetHybridTalentLevel(bardLevel, adventureLevel)));

			if ((byte)merchantLevel >= 12 && (byte)blacksmithLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Hawker, GetHybridTalentLevel(merchantLevel, blacksmithLevel, tailoringLevel, carpentryLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)archerLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyArcher, GetHybridTalentLevel(holyArtsLevel, archerLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyKnight, GetHybridTalentLevel(holyArtsLevel, knightLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyWarrior, GetHybridTalentLevel(warriorLevel, holyArtsLevel)));

			if ((byte)cookingLevel >= 12 && (byte)tailoringLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.HomeEconomist, GetHybridTalentLevel(cookingLevel, tailoringLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)bardLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Idol, GetHybridTalentLevel(puppeteerLevel, bardLevel)));

			if ((byte)fighterLevel >= 12 && (byte)blacksmithLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.IronFist, GetHybridTalentLevel(fighterLevel, blacksmithLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)holyArtsLevel >= 6 && (byte)adventureLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.JoySpreader, GetHybridTalentLevel(puppeteerLevel, holyArtsLevel, adventureLevel)));

			if ((byte)adventureLevel >= 12 && (byte)knightLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.KnightErrant, GetHybridTalentLevel(adventureLevel, warriorLevel)));

			if ((byte)carpentryLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Lumberjack, GetHybridTalentLevel(carpentryLevel, adventureLevel)));

			if ((byte)knightLevel >= 6 && (byte)mageLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.MagicKnight, GetHybridTalentLevel(knightLevel, mageLevel)));

			if ((byte)blacksmithLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Miner, GetHybridTalentLevel(blacksmithLevel, adventureLevel)));

			if ((byte)adventureLevel >= 12 && (byte)holyArtsLevel >= 12 && (byte)medicineLevel >= 12 && (byte)merchantLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Missionary, GetHybridTalentLevel(adventureLevel, holyArtsLevel, medicineLevel, medicineLevel, carpentryLevel)));

			if ((byte)fighterLevel >= 12 && (byte)holyArtsLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Monk, GetHybridTalentLevel(fighterLevel, holyArtsLevel)));

			if ((byte)cookingLevel >= 12 && (byte)medicineLevel >= 12 && (byte)transmutaionLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Nutritionist, GetHybridTalentLevel(cookingLevel, medicineLevel)));

			if ((byte)adventureLevel >= 12 && (byte)merchantLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Peddler, GetHybridTalentLevel(adventureLevel, merchantLevel)));

			if ((byte)medicineLevel >= 12 && (byte)transmutaionLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Philosopher, GetHybridTalentLevel(medicineLevel, transmutaionLevel)));

			if ((byte)archerLevel >= 12 && (byte)bardLevel >= 12 && (byte)fighterLevel >= 12 && (byte)battleAlchemyLevel >= 12 && (byte)mageLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Polymath, GetHybridTalentLevel(archerLevel, bardLevel, fighterLevel, battleAlchemyLevel, mageLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Puppetmancer, GetHybridTalentLevel(puppeteerLevel, transmutaionLevel)));

			if ((byte)archerLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Ranger, GetHybridTalentLevel(warriorLevel, archerLevel)));

			if ((byte)mageLevel >= 6 && (byte)transmutaionLevel >= 6 && (byte)medicineLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Researcher, GetHybridTalentLevel(mageLevel, transmutaionLevel, medicineLevel)));

			if ((byte)mageLevel >= 6 && (byte)holyArtsLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Sage, GetHybridTalentLevel(mageLevel, holyArtsLevel)));

			if ((byte)mageLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Scholar, GetHybridTalentLevel(mageLevel, transmutaionLevel)));

			if ((byte)warriorLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Slayer, GetHybridTalentLevel(warriorLevel, knightLevel)));

			if ((byte)warriorLevel >= 6 && (byte)mageLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Spellsword, GetHybridTalentLevel(warriorLevel, mageLevel)));

			if ((byte)warriorLevel >= 12 && (byte)knightLevel >= 12 && (byte)fighterLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Striker, GetHybridTalentLevel(warriorLevel, knightLevel, fighterLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)carpentryLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.ToyMaker, GetHybridTalentLevel(puppeteerLevel, carpentryLevel)));

			if ((byte)archerLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Tracker, GetHybridTalentLevel(archerLevel, fighterLevel)));

			if ((byte)archerLevel >= 6 && (byte)tailoringLevel >= 6 && (byte)carpentryLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Trapper, GetHybridTalentLevel(archerLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 12 && (byte)cookingLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Troubadour, GetHybridTalentLevel(bardLevel, cookingLevel, adventureLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)bardLevel >= 6 && (byte)adventureLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Trouper, GetHybridTalentLevel(puppeteerLevel, bardLevel)));

			if ((byte)adventureLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Vagabond, GetHybridTalentLevel(warriorLevel, adventureLevel)));

			if ((byte)fighterLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Vanguard, GetHybridTalentLevel(fighterLevel, knightLevel)));


			return titles.Where(a => a != 0).ToList();
		}

		public TalentLevel GetTalentLevel(TalentId talent)
		{
			var exp = this.GetTalentExp(talent) / 1000;

			if (exp < 1)
				return TalentLevel.None;
			if (exp < 11)
				return TalentLevel.Fledgling;
			if (exp < 36)
				return TalentLevel.Novice;
			if (exp < 76)
				return TalentLevel.Amateur;
			if (exp < 136)
				return TalentLevel.Green;
			if (exp < 216)
				return TalentLevel.Naive;
			if (exp < 316)
				return TalentLevel.Apprentice;
			if (exp < 436)
				return TalentLevel.Senior;
			if (exp < 576)
				return TalentLevel.Advanced;
			if (exp < 736)
				return TalentLevel.Seasoned;
			if (exp < 926)
				return TalentLevel.Skilled;
			if (exp < 1146)
				return TalentLevel.Expert;
			if (exp < 1396)
				return TalentLevel.Great;
			if (exp < 1696)
				return TalentLevel.Champion;
			if (exp < 2046)
				return TalentLevel.Wise;

			return (this.Grandmaster == talent ? TalentLevel.Grandmaster : TalentLevel.Master);
		}

		public static TalentLevel GetHybridTalentLevel(params TalentLevel[] levels)
		{
			return (TalentLevel)levels.Min(a => (byte)a);
		}

		public void UpdateTalentExp(SkillConst skill, SkillRank rank, bool notifyRankUp = false)
		{
			var mask = (byte)this.GetRaceMask();

			var exps = MabiData.TalentExpDb.Entries.Where(a => a.SkillId == (ushort)skill && a.SkillRank <= (byte)rank && (a.Race & mask) != 0);
			if (exps == null)
				return;
			var info = exps.FirstOrDefault(a => a.SkillRank == (ushort)rank);
			if (info == null)
				return;

			foreach (var talent in info.Exps.Keys)
			{
				uint exp = (uint)exps.Sum(a => a.Exps[talent]);

				if (!this.TalentExps.ContainsKey((TalentId)talent))
					this.TalentExps.Add((TalentId)talent, new Dictionary<SkillConst, uint>());

				var expInfo = this.TalentExps[(TalentId)talent];

				if (!expInfo.ContainsKey(skill))
					expInfo.Add(skill, 0);

				expInfo[skill] = exp;

				if (notifyRankUp)
				{
					UpdateTalentInfo();
				}
			}
		}

		public void UpdateTalentInfo()
		{
			var p = new MabiPacket(Op.TalentInfoUpdate, this.Id);
			p.PutByte(1);

			p.PutShort((ushort)this.SelectedTalentTitle);
			p.PutByte((byte)this.Grandmaster);
			p.PutInt(this.GetTalentExp(TalentId.Adventure));
			p.PutInt(this.GetTalentExp(TalentId.Warrior));
			p.PutInt(this.GetTalentExp(TalentId.Mage));
			p.PutInt(this.GetTalentExp(TalentId.Archer));
			p.PutInt(this.GetTalentExp(TalentId.Merchant));
			p.PutInt(this.GetTalentExp(TalentId.BattleAlchemy));
			p.PutInt(this.GetTalentExp(TalentId.Fighter));
			p.PutInt(this.GetTalentExp(TalentId.Bard));
			p.PutInt(this.GetTalentExp(TalentId.Puppeteer));
			p.PutInt(this.GetTalentExp(TalentId.Knight));
			p.PutInt(this.GetTalentExp(TalentId.HolyArts));
			p.PutInt(this.GetTalentExp(TalentId.Transmutaion));
			p.PutInt(this.GetTalentExp(TalentId.Cooking));
			p.PutInt(this.GetTalentExp(TalentId.Blacksmith));
			p.PutInt(this.GetTalentExp(TalentId.Tailoring));
			p.PutInt(this.GetTalentExp(TalentId.Medicine));
			p.PutInt(this.GetTalentExp(TalentId.Carpentry));

			if (Feature.ZeroTalent.IsEnabled())
				p.PutInt(0);

			// Talent titles
			// ----------------------------------------------------------
			var titles = this.GetTalentTitles();

			p.PutByte((byte)titles.Count);               // Count
			foreach (var title in titles)
				p.PutShort(title);

			this.Client.Send(p);

			UpdateTalentStats();
			WorldManager.Instance.CreatureStatsUpdate(this);
		}

		public void UpdateTalentStats()
		{
			foreach (var talentObj in Enum.GetValues(typeof(TalentId)))
			{
				var talentId = (TalentId)talentObj;

				var info = MabiData.TalentRankDb.Find((byte)talentId, (byte)this.GetTalentLevel(talentId), (byte)this.GetRaceMask());

				if (info != null)
				{
					foreach (var kvp in info.Bonuses)
					{
						this.StatMods.Remove((Stat)kvp.Key, StatModSource.Talent, (ulong)talentId);
						this.StatMods.Add((Stat)kvp.Key, kvp.Value, StatModSource.Talent, (ulong)talentId);
						switch ((Stat)kvp.Key)
						{
							case Stat.LifeMaxMod: this.Life += kvp.Value; break;
							case Stat.StaminaMaxMod: this.Stamina += kvp.Value; break;
							case Stat.ManaMaxMod: this.Mana += kvp.Value; break;
						}
					}
				}
			}
		}

		public TalentRace GetRaceMask()
		{
			if (this.IsHuman)
				return TalentRace.Human;
			if (this.IsElf)
				return TalentRace.Elf;
			if (this.IsGiant)
				return TalentRace.Giant;

			return TalentRace.None;
		}

		public uint GetTalentExp(TalentId talent)
		{
			if (!this.TalentExps.ContainsKey(talent))
				return 0;

			uint exp = 0;

			foreach (var skill in this.TalentExps[talent])
				exp += skill.Value;

			return exp;
		}

		/// <summary>
		/// Calculates the damage of left-and-right slots together
		/// </summary>
		/// <returns></returns>
		public float GetRndTotalDamage()
		{
			var balance = this.GetRndAverageBalance();

			var dmg = this.GetRndDamage(this.RightHand, balance);
			if (this.LeftHand != null)
				dmg += this.GetRndDamage(this.LeftHand, balance);

			return dmg;
		}

		/// <summary>
		/// Calculates random damage using the given item.
		/// </summary>
		/// <param name="weapon">null for hands</param>
		/// <param name="balance">NaN for individual balance calculation</param>
		/// <returns></returns>
		public float GetRndDamage(MabiItem weapon, float balance = float.NaN)
		{
			float min = 0, max = 0;

			if (weapon != null)
			{
				min += weapon.OptionInfo.AttackMin;
				max += weapon.OptionInfo.AttackMax;
			}
			else
			{
				min = this.RaceInfo.AttackMin;
				max = this.RaceInfo.AttackMax;
			}

			min += this.Str / 3.0f;
			max += this.Str / 2.5f;

			if (min > max)
				min = max;

			if (float.IsNaN(balance))
				balance = this.GetRndBalance(weapon);

			return min + ((max - min) * balance);
		}

		public float GetRndRangeDamage()
		{
			float min = 0, max = 0;

			min += this.RightHand.OptionInfo.AttackMin;
			max += this.RightHand.OptionInfo.AttackMax;

			min += this.Magazine.OptionInfo.AttackMin;
			max += this.Magazine.OptionInfo.AttackMax;

			min += this.Dex / 3.5f;
			max += this.Dex / 2.5f;

			var balance = this.GetRndBalance(this.RightHand);

			return min + ((max - min) * balance);
		}

		public void LoadDefault()
		{
			if (this.Race == uint.MaxValue)
				throw new Exception("Set race before calling LoadDefault.");

			var dbInfo = MabiData.RaceDb.Find(this.Race);
			if (dbInfo == null)
			{
				// Try to default to Human
				dbInfo = MabiData.RaceDb.Find(10000);
				if (dbInfo == null)
				{
					throw new Exception("Unable to load race defaults, race '" + this.Race.ToString() + "' not found.");
				}
				Logger.Warning("Race '" + this.Race.ToString() + "' not found, using human instead.");
			}

			DefenseBase = dbInfo.Defense;
			ProtectionBase = dbInfo.Protection;

			this.RaceInfo = dbInfo;

			_statRegens.Add(this.LifeRegen = new MabiStatRegen(Stat.Life, 0.12f, this.LifeMax));
			_statRegens.Add(this.ManaRegen = new MabiStatRegen(Stat.Mana, 0.05f, this.ManaMax));
			_statRegens.Add(this.StaminaRegen = new MabiStatRegen(Stat.Stamina, 0.4f, this.StaminaMax));
			//_statMods.Add(new MabiStatMod(Stat.Food, -0.01f, this.StaminaMax / 2));

			if (MabiTime.Now.IsNight)
				this.ManaRegen.ChangePerSecond *= 3;

			this.HookUp();
		}

		protected override void HookUp()
		{
			EventManager.Instance.TimeEvents.RealTimeSecondTick += this.RestoreStats;
			base.HookUp();
		}

		public override void Dispose()
		{
			EventManager.Instance.TimeEvents.RealTimeSecondTick -= this.RestoreStats;
			base.Dispose();
		}

		protected virtual void RestoreStats(object sender, TimeEventArgs e)
		{
			if (this.IsDead)
				return;

			lock (_statRegens)
			{
				var toRemove = new List<MabiStatRegen>();
				foreach (var mod in _statRegens)
				{
					if (mod.TimeLeft < 1)
					{
						toRemove.Add(mod);
					}
					else
					{
						switch (mod.Stat)
						{
							case Stat.Life: this.Life += mod.ChangePerSecond; break;
							case Stat.Mana: this.Mana += mod.ChangePerSecond; break;
							case Stat.Stamina: this.Stamina += mod.ChangePerSecond; break;
						}
					}
				}
				foreach (var mod in toRemove)
					_statRegens.Remove(mod);
			}
		}

		/// <summary>
		/// Adds a cooldown to the creature. If one already exists, the Expiry and Error messages are updated.
		/// If the expiry has already passed, the cooldown is not added and removed if needed.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <param name="expires"></param>
		/// <param name="error">The error message to be shown. Use {0} to insert the time left.</param>
		/// <returns></returns>
		public void AddCooldown(CooldownType type, uint id, DateTime expires, string error)
		{
			var cooldown = Cooldowns.FirstOrDefault(c => c.Type == type && c.Id == id);
			if (cooldown != null)
			{
				cooldown.Expires = expires;
				cooldown.ErrorMessage = error;
				if (cooldown.Expires < DateTime.Now)
					Cooldowns.Remove(cooldown);
				return;
			}

			if (expires < DateTime.Now)
				return;

			cooldown = new MabiCooldown(type, id, expires, error);
			Cooldowns.Add(cooldown);
		}

		/// <summary>
		/// Checks for a valid cooldown of the specified type and id. Optionally sends the error message to the client.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <param name="sendError"></param>
		/// <returns>True if a valid cooldown is found, false otherwise</returns>
		public bool CheckForCooldown(CooldownType type, uint id, bool sendError = true)
		{
			var cooldown = Cooldowns.FirstOrDefault(c => c.Type == type && c.Id == id);
			if (cooldown == null)
				return false;

			if (cooldown.Expires < DateTime.Now)
			{
				Cooldowns.Remove(cooldown);
				return false;
			}

			if (sendError)
				cooldown.SendErrorMessage(this);

			return true;
		}

		public virtual void CalculateBaseStats()
		{
			this.Height = 1.0f;
			this.Fat = 1.0f;
			this.Upper = 1.0f;
			this.Lower = 1.0f;

			this.LifeMaxBase = 10;
			this.ManaMaxBase = 10;
			this.StaminaMaxBase = 10;
			this.Life = 10;
			this.Mana = 10;
			this.Stamina = 10;

			this.StrBase = 10;
			this.IntBase = 10;
			this.DexBase = 10;
			this.WillBase = 10;
			this.LuckBase = 10;
		}

		public void FullHeal()
		{
			this.Injuries = 0;
			this.Hunger = 0;
			this.Life = this.LifeMax;
			this.Mana = this.ManaMax;
			this.Stamina = this.StaminaMax;

			WorldManager.Instance.CreatureStatsUpdate(this);
		}

		public void FullHealLife()
		{
			this.Injuries = 0;
			this.Life = this.LifeMax;

			WorldManager.Instance.CreatureStatsUpdate(this);
		}

		/// <summary>
		/// Returns true if the current creature can be attacked by other.
		/// </summary>
		public abstract bool IsAttackableBy(MabiCreature other);

		/// <summary>
		/// Returns randomized average balance, taking both weapons into consideration.
		/// </summary>
		/// <returns></returns>
		public float GetRndAverageBalance()
		{
			var baseBalance = HandBalance;
			if (this.RightHand != null)
			{
				baseBalance = this.RightHand.Balance;
				if (this.LeftHand != null)
				{
					baseBalance += this.LeftHand.Balance;
					baseBalance /= 2f; // average
				}
			}

			return this.GetRndBalance(baseBalance);
		}

		/// <summary>
		/// Calculates random balance for the given weapon.
		/// </summary>
		/// <param name="weapon">null for hands</param>
		/// <returns></returns>
		public float GetRndBalance(MabiItem weapon)
		{
			return this.GetRndBalance(weapon != null ? weapon.Balance : 0.3f);
		}

		/// <summary>
		/// Calculates random balance using the given base balance (eg 0.3 for hands).
		/// </summary>
		/// <param name="baseBalance"></param>
		/// <returns></returns>
		protected float GetRndBalance(float baseBalance)
		{
			var rnd = RandomProvider.Get();
			var balance = baseBalance;

			// Dex
			balance += (Math.Max(0, this.Dex - 10) / 4) / 100f;

			// Randomization, balance+-(100-balance), eg 80 = 60~100
			var diff = 1.0f - balance;
			balance += ((diff - (diff * 2 * (float)rnd.NextDouble())) * (float)rnd.NextDouble());
			balance = (float)Math.Max(0f, Math.Round(balance, 2));

			return balance;
		}

		public float GetMagicDamage(MabiItem item, float spellDamage)
		{
			double damage = spellDamage;

			damage *= (1 + this.MagicAttack / 100f);

			return (float)damage;
		}

		public bool HasSkill(ushort id)
		{
			return this.Skills.ContainsKey(id);
		}

		public bool HasSkill(SkillConst id)
		{
			return this.HasSkill((ushort)id);
		}

		public void GiveSkill(ushort skillId, byte rank, bool showFlashIfNew = false)
		{
			this.GiveSkill((SkillConst)skillId, (SkillRank)rank, showFlashIfNew);
		}

		public void GiveSkill(SkillConst skillId, SkillRank rank, bool showFlashIfNew = false)
		{
			var skill = this.GetSkill(skillId);
			if (skill == null)
			{
				skill = new MabiSkill(skillId, rank, this.Race);
				this.AddSkill(skill);
				WorldManager.Instance.CreatureSkillUpdate(this, skill, true);
				EventManager.Instance.CreatureEvents.OnCreatureSkillUpdate(this, new SkillUpdateEventArgs(this, skill, true));
				if (showFlashIfNew)
					WorldManager.Instance.Broadcast(new MabiPacket(Op.RankUp, this.Id).PutShort(1), SendTargets.Range, this);
			}
			else
			{
				RemoveSkillBonuses(skill);

				skill.Info.Experience = 0;

				skill.Info.Rank = (byte)rank;
				skill.LoadRankInfo();
				WorldManager.Instance.CreatureSkillUpdate(this, skill, false);
				EventManager.Instance.CreatureEvents.OnCreatureSkillUpdate(this, new SkillUpdateEventArgs(this, skill, false));
			}

			AddSkillBonuses(skill);

			this.UpdateTalentExp(skillId, rank, true);
		}

		public void AddSkillBonuses(MabiSkill skill)
		{
			this.StrBaseSkill += skill.RankInfo.StrTotal;
			this.WillBaseSkill += skill.RankInfo.WillTotal;
			this.IntBaseSkill += skill.RankInfo.IntTotal;
			this.LuckBaseSkill += skill.RankInfo.LuckTotal;
			this.DexBaseSkill += skill.RankInfo.DexTotal;
			var mana = skill.RankInfo.ManaTotal;
			this.ManaMaxBaseSkill += mana;
			this.Mana += mana;
			var life = skill.RankInfo.LifeTotal;
			this.LifeMaxBaseSkill += life;
			this.Life += life;
			var stamina = skill.RankInfo.StaminaTotal;
			this.StaminaMaxBaseSkill += stamina;
			this.Stamina += stamina;

			if (skill.Id == SkillConst.MeleeCombatMastery)
			{
				this.StatMods.Add(Stat.LifeMaxMod, skill.RankInfo.Var3, StatModSource.SkillRank, (ulong)skill.Id);
				this.Life += skill.RankInfo.Var3;
			}

			if (skill.Id == SkillConst.MagicMastery)
			{
				this.StatMods.Add(Stat.ManaMaxMod, skill.RankInfo.Var1, StatModSource.SkillRank, (ulong)skill.Id);
				this.Mana += skill.RankInfo.Var1;
			}

			if (skill.Id == SkillConst.Defense)
			{
				this.DefenseBaseSkill += (int)skill.RankInfo.Var1;
			}
		}

		public void RemoveSkillBonuses(MabiSkill skill)
		{
			this.StrBaseSkill -= skill.RankInfo.StrTotal;
			this.WillBaseSkill -= skill.RankInfo.WillTotal;
			this.IntBaseSkill -= skill.RankInfo.IntTotal;
			this.LuckBaseSkill -= skill.RankInfo.LuckTotal;
			this.DexBaseSkill -= skill.RankInfo.DexTotal;
			var mana = skill.RankInfo.ManaTotal;
			this.Mana -= mana;
			this.ManaMaxBaseSkill -= mana;
			var life = skill.RankInfo.LifeTotal;
			this.Life -= life;
			this.LifeMaxBaseSkill -= life;
			var stamina = skill.RankInfo.StaminaTotal;
			this.Stamina -= stamina;
			this.StaminaMaxBaseSkill -= stamina;

			if (skill.Id == SkillConst.MeleeCombatMastery)
			{
				this.Life -= skill.RankInfo.Var3;
				this.StatMods.Remove(Stat.LifeMaxMod, StatModSource.SkillRank, (ulong)skill.Id);
			}

			if (skill.Id == SkillConst.MagicMastery)
			{
				this.Mana -= skill.RankInfo.Var1;
				this.StatMods.Remove(Stat.ManaMaxMod, StatModSource.SkillRank, (ulong)skill.Id);
			}

			if (skill.Id == SkillConst.Defense)
			{
				this.DefenseBaseSkill -= (int)skill.RankInfo.Var1;
			}
		}

		public MabiSkill GetSkill(SkillConst skillId)
		{
			return this.GetSkill((ushort)skillId);
		}

		public MabiSkill GetSkill(ushort skillId)
		{
			MabiSkill skill;
			this.Skills.TryGetValue(skillId, out skill);
			return skill;
		}

		/// <summary>
		/// Shortcut for .Skills.Add(skill.Info.Id, skill)
		/// </summary>
		/// <param name="skill"></param>
		public void AddSkill(MabiSkill skill)
		{
			this.Skills.Add(skill.Info.Id, skill);
		}

		public float GetSpeed()
		{
			RaceInfo ri = null;

			if (this.Vehicle == null)
			{
				if (this.Shamala == null)
				{
					// Normal
					ri = this.RaceInfo;
				}
				else
				{
					// Transformed
					ri = this.ShamalaRace;
				}
			}
			else
			{
				// Mounted
				ri = this.Vehicle.RaceInfo;
				if (this.Vehicle.IsFlying && ri.FlightInfo != null)
					return ri.FlightInfo.FlightSpeed;

			}

			return (!IsWalking ? ri.SpeedRun : ri.SpeedWalk);
		}

		public void SetLocation(uint region, uint x, uint y)
		{
			this.Region = region;
			this.SetPosition(x, y);
		}

		public MabiVertex SetPosition(uint x, uint y, uint h = 0)
		{
			_position.X = Destination.X = x;
			_position.Y = Destination.Y = y;
			_position.H = Destination.H = h;

			return _position.Copy();
		}

		/// <summary>
		/// Returns a new MabiVertex with the current position.
		/// </summary>
		public override MabiVertex GetPosition()
		{
			if (!this.IsMoving)
				return _position.Copy();

			var passed = (DateTime.Now - _moveStartTime).TotalSeconds;
			if (passed >= _moveDuration)
				return this.SetPosition(Destination.X, Destination.Y, Destination.H);

			var xt = _position.X + (_movementX * passed);
			var yt = _position.Y + (_movementY * passed);
			var ht = 0.0;
			if (this.IsFlying)
				ht = _position.H + (_movementH < 0 ? Math.Max(_movementH * passed, Destination.H) : Math.Min(_movementH * passed, Destination.H));

			return new MabiVertex((uint)xt, (uint)yt, (uint)ht);
		}

		public MabiVertex StartMove(MabiVertex dest, bool walk = false)
		{
			var pos = this.GetPosition();

			_position.X = pos.X;
			_position.Y = pos.Y;
			_position.H = pos.H;

			Destination.X = dest.X;
			Destination.Y = dest.Y;
			Destination.H = dest.H;

			_moveStartTime = DateTime.Now;
			IsWalking = walk;

			var diffX = (int)dest.X - (int)pos.X;
			var diffY = (int)dest.Y - (int)pos.Y;
			_moveDuration = Math.Sqrt(diffX * diffX + diffY * diffY) / this.GetSpeed();
			_movementX = diffX / _moveDuration;
			_movementY = diffY / _moveDuration;
			_movementH = 0;

			if (this.IsFlying)
			{
				_movementH = (pos.H < dest.H ? this.RaceInfo.FlightInfo.DescentSpeed : this.RaceInfo.FlightInfo.AscentSpeed);
				_moveDuration = Math.Max(_moveDuration, Math.Abs((int)dest.H - (int)pos.H) / _movementH);
			}

			this.Direction = (byte)(Math.Floor(Math.Atan2(_movementY, _movementX) / 0.02454369260617026));

			return pos;
		}

		/// <summary>
		/// Stops movement, returning new current position.
		/// Sends: RunTo/WalkTo
		/// </summary>
		public MabiVertex StopMove()
		{
			if (!this.IsMoving)
				return _position.Copy();

			var pos = this.GetPosition();
			this.SetPosition(pos.X, pos.Y, pos.H);

			if (this.IsWalking)
				Send.WalkTo(this, pos);
			else
				Send.RunTo(this, pos);

			return pos;
		}

		/// <summary>
		/// Returns true if the creature is currently moving
		/// to the given position.
		/// </summary>
		/// <param name="dest"></param>
		/// <returns></returns>
		public bool IsDestination(MabiVertex dest)
		{
			return (Destination.Equals(dest));
		}

		public void TakeDamage(float damage)
		{
			var hpBefore = this.Life;
			if (hpBefore < 1)
			{
				this.Die();
				return;
			}

			var hp = Math.Max(-this.LifeMaxBaseTotal, hpBefore - damage);
			this.Life = hp;

			if (hp > 0 || this.ShouldSurvive() || (this is MabiPC && hpBefore >= this.LifeMax / 2))
				return;

			this.Die();
		}

		/// <summary>
		/// Calculates a chance to survive, based on Will.
		/// </summary>
		/// <returns></returns>
		protected virtual bool ShouldSurvive()
		{
			// TODO: Actual, proper calculation.
			return ((this.Will * 10) + RandomProvider.Get().Next(1001)) > 999;
		}

		/// <summary>
		/// Adds creature state "Dead".
		/// </summary>
		public virtual void Die()
		{
			this.State |= CreatureStates.Dead;
		}

		/// <summary>
		/// Removes dead state
		/// </summary>
		public void Revive()
		{
			this.State &= ~CreatureStates.Dead;

			if (((this is MabiNPC) && (Util.WorldConf.ChalkOnDeath & (int)Util.WorldConf.ChalkDeathFlags.Mob) != 0) ||
				((this is MabiPC) && (Util.WorldConf.ChalkOnDeath & (int)Util.WorldConf.ChalkDeathFlags.Player) != 0))
			{
				var pos = this.GetPosition();
				var p = new MabiProp(50, this.Region, pos.X, pos.Y, this.Direction + 90);
				p.DisappearTime = ((Util.WorldConf.ChalkOnDeath & (int)Util.WorldConf.ChalkDeathFlags.Permanent) != 0 ? DateTime.MaxValue : DateTime.Now.AddMinutes(2));

				WorldManager.Instance.AddProp(p);
			}

			this.CauseOfDeath = DeathCauses.None;
			this.WaitingForRes = false;
		}

		/// <summary>
		/// Adds the given amount of exp and handles the level up.
		/// </summary>
		/// <param name="val"></param>
		public void GiveExp(ulong val)
		{
			this.Experience += val;
			var prevLevel = this.Level;

			var levelStats = MabiData.StatsLevelUpDb.Find(this.Race, this.Age);
			if (levelStats == null)
				Logger.Unimplemented("Level up stats missing for race '" + this.Race.ToString() + "'.");

			while (this.Level < MabiData.ExpDb.MaxLevel && this.Experience >= MabiData.ExpDb.GetTotalForNextLevel(this.Level))
			{
				this.Level++;

				if (levelStats == null)
					continue;

				this.AbilityPoints += levelStats.AP;
				this.LifeMaxBase += levelStats.Life;
				this.ManaMaxBase += levelStats.Mana;
				this.StaminaMaxBase += levelStats.Stamina;
				this.StrBase += levelStats.Str;
				this.IntBase += levelStats.Int;
				this.DexBase += levelStats.Dex;
				this.WillBase += levelStats.Will;
				this.LuckBase += levelStats.Luck;
			}

			if (prevLevel < this.Level)
			{
				WorldManager.Instance.Broadcast(new MabiPacket(Op.LevelUp, this.Id).PutShort(this.Level), SendTargets.Range, this);
				this.FullHeal();
				EventManager.Instance.CreatureEvents.OnCreatureLevelsUp(this, new CreatureEventArgs(this));
				WorldManager.Instance.CreatureStatsUpdate(this);
			}
		}

		public double GetAimPercent(double aimBoost, double aimInitial = 0)
		{
			if (!WorldManager.InRange(this, this.Target, (uint)(this.RightHand.OptionInfo.EffectiveRange)))
			{
				// TODO: SoG?
				return 0;
			}

			var aPos = this.GetPosition();
			var tPos = this.Target.GetPosition();

			var distance = Math.Sqrt((Math.Pow(((float)aPos.X - (float)tPos.X), 2) + Math.Pow(((float)aPos.Y - (float)tPos.Y), 2)));

			var aimTime = (DateTime.Now - this.AimStart).TotalMilliseconds;
			var aimRate = Math.Min(320 / distance, 1);
			var aimPercent = Math.Min(aimTime * aimRate * aimBoost / 10 + aimInitial, 99);
			var overTime = aimTime - (990 / (aimRate * aimBoost)); // Time after 99%

			if (aimPercent == 99 && overTime > 1000)
				aimPercent = 100;

			if (this.Target.IsMoving)
			{
				if (this.Target.IsWalking && aimPercent > 95)
					aimPercent = 95;
				else if (!this.Target.IsWalking && aimPercent > 90)
					aimPercent = 90;
			}

			return aimPercent / 100;
		}
	}
}
