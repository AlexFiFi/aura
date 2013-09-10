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
using Aura.World.Scripting;
using Aura.World.Skills;
using Aura.World.World.Guilds;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public const float HandBalance = 0.3f;

		public Client Client = new DummyClient();

		public string Name;

		public MabiGuild Guild;
		public MabiGuildMember GuildMember;

		public uint Race;
		public RaceInfo RaceInfo = null;

		public uint BattleExp = 0;

		private Dictionary<Stat, object> _stats = new Dictionary<Stat, object>();

		public List<MabiStatRegen> StatRegens = new List<MabiStatRegen>();
		public readonly MabiStatMods StatMods = new MabiStatMods();
		public MabiStatRegen LifeRegen, ManaRegen, StaminaRegen;

		public byte SkinColor, Eye, EyeColor, Lip;

		private string _standStyle = "", _standStyleTalk = "";
		public string StandStyle { get { return (this.Shamala == null ? _standStyle : ""); } set { _standStyle = value; } }
		public string StandStyleTalk { get { return (this.Shamala == null ? _standStyleTalk : ""); } set { _standStyleTalk = value; } }

		public uint Color1 = 0x808080;
		public uint Color2 = 0x808080;
		public uint Color3 = 0x808080;

		public byte WeaponSet;
		public byte BattleState;
		public bool IsFlying;

		public ScriptingVariables Vars = new ScriptingVariables();

		private ushort _title;
		/// <summary>
		/// Changes/Returns Title and updates TitleApplied.
		/// </summary>
		public ushort Title { get { return _title; } set { _title = value; this.TitleApplied = DateTime.Now; } }
		public ushort OptionTitle;
		public DateTime TitleApplied { get; protected set; }

		public CreatureSkillManager Skills { get; protected set; }
		//public Dictionary<ushort, MabiSkill> Skills = new Dictionary<ushort, MabiSkill>();
		//public MabiSkill ActiveSkill;
		public SkillConst ActiveSkillId;
		public byte ActiveSkillStacks;
		public DateTime ActiveSkillPrepareEnd;
		public uint SoulCount;

		public CreatureTalentManager Talents { get; protected set; }

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
				var skill = this.Skills.Get(SkillConst.Rest);
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
						EntityEvents.OnCreatureStatUpdates(this);
					}
				}
				else
				{
					if ((this.Conditions.A & CreatureConditionA.Deadly) == CreatureConditionA.Deadly)
					{
						this.Conditions.A &= ~CreatureConditionA.Deadly;
						EntityEvents.OnCreatureStatUpdates(this);
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
				var defenseSkill = this.Skills.Get(SkillConst.Defense);
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
				var defenseSkill = this.Skills.Get(SkillConst.Defense);
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
			this.Skills = new CreatureSkillManager(this);
			this.Talents = new CreatureTalentManager(this);
		}

		public bool Has(CreatureConditionA condition) { return ((this.Conditions.A & condition) != 0); }
		public bool Has(CreatureConditionB condition) { return ((this.Conditions.B & condition) != 0); }
		public bool Has(CreatureConditionC condition) { return ((this.Conditions.C & condition) != 0); }
		public bool Has(CreatureConditionD condition) { return ((this.Conditions.D & condition) != 0); }
		public bool Has(CreatureStates state) { return ((this.State & state) != 0); }

		/// <summary>
		/// Acivates condition and sends update.
		/// </summary>
		public void Activate(CreatureConditionA condition) { this.Conditions.A |= condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Acivates condition and sends update.
		/// </summary>
		public void Activate(CreatureConditionB condition) { this.Conditions.B |= condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Acivates condition and sends update.
		/// </summary>
		public void Activate(CreatureConditionC condition) { this.Conditions.C |= condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Acivates condition and sends update.
		/// </summary>
		public void Activate(CreatureConditionD condition) { this.Conditions.D |= condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Deacivates condition and sends update.
		/// </summary>
		public void Deactivate(CreatureConditionA condition) { this.Conditions.A &= ~condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Deacivates condition and sends update.
		/// </summary>
		public void Deactivate(CreatureConditionB condition) { this.Conditions.B &= ~condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Deacivates condition and sends update.
		/// </summary>
		public void Deactivate(CreatureConditionC condition) { this.Conditions.C &= ~condition; Send.StatusEffectUpdate(this); }
		/// <summary>
		/// Deacivates condition and sends update.
		/// </summary>
		public void Deactivate(CreatureConditionD condition) { this.Conditions.D &= ~condition; Send.StatusEffectUpdate(this); }

		public bool HasSkillLoaded(SkillConst skill) { return this.ActiveSkillId == skill; }

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

			this.StatRegens.Add(this.LifeRegen = new MabiStatRegen(Stat.Life, 0.12f, this.LifeMax));
			this.StatRegens.Add(this.ManaRegen = new MabiStatRegen(Stat.Mana, 0.05f, this.ManaMax));
			this.StatRegens.Add(this.StaminaRegen = new MabiStatRegen(Stat.Stamina, 0.4f, this.StaminaMax));
			//_statMods.Add(new MabiStatMod(Stat.Food, -0.01f, this.StaminaMax / 2));

			if (MabiTime.Now.IsNight)
				this.ManaRegen.ChangePerSecond *= 3;

			this.HookUp();
		}

		protected override void HookUp()
		{
			EventManager.TimeEvents.RealTimeSecondTick += this.RestoreStats;
			base.HookUp();
		}

		public override void Dispose()
		{
			EventManager.TimeEvents.RealTimeSecondTick -= this.RestoreStats;
			base.Dispose();
		}

		protected virtual void RestoreStats(MabiTime time)
		{
			if (this.IsDead)
				return;

			lock (this.StatRegens)
			{
				var toRemove = new List<MabiStatRegen>();
				foreach (var mod in this.StatRegens)
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
					this.StatRegens.Remove(mod);
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

		public float GetSpeed()
		{
			RaceInfo ri = null;
			float multiply = 1f;

			if (this.Vehicle == null)
			{
				if (this.Shamala == null)
				{
					// Normal
					ri = this.RaceInfo;

					if (this.Has(CreatureConditionB.Demigod))
						multiply = 1.5f;
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

			return (!IsWalking ? ri.SpeedRun : ri.SpeedWalk) * multiply;
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

		/// <summary>
		/// Starts movement towards to. Also moves Vehicle.
		/// Sends: Walking/Running
		/// </summary>
		public MabiVertex Move(MabiVertex to, bool walk = false)
		{
			var from = this.GetPosition();

			// Server calculation
			{
				_position.X = from.X;
				_position.Y = from.Y;
				_position.H = from.H;

				this.Destination.X = to.X;
				this.Destination.Y = to.Y;
				this.Destination.H = to.H;

				_moveStartTime = DateTime.Now;
				IsWalking = walk;

				var diffX = (int)to.X - (int)from.X;
				var diffY = (int)to.Y - (int)from.Y;
				_moveDuration = Math.Sqrt(diffX * diffX + diffY * diffY) / this.GetSpeed();
				_movementX = diffX / _moveDuration;
				_movementY = diffY / _moveDuration;
				_movementH = 0;

				if (this.IsFlying)
				{
					_movementH = (from.H < to.H ? this.RaceInfo.FlightInfo.DescentSpeed : this.RaceInfo.FlightInfo.AscentSpeed);
					_moveDuration = Math.Max(_moveDuration, Math.Abs((int)to.H - (int)from.H) / _movementH);
				}

				this.Direction = (byte)(Math.Floor(Math.Atan2(_movementY, _movementX) / 0.02454369260617026));
			}

			// Client Update
			{
				if (!this.IsFlying)
				{
					var p = new MabiPacket(!walk ? Op.Running : Op.Walking, this.Id);
					p.PutInt(from.X);
					p.PutInt(from.Y);
					p.PutInt(to.X);
					p.PutInt(to.Y);
					WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

					if (this.Vehicle != null)
					{
						this.Vehicle.Move(to, walk);
					}
				}
			}

			// Server Updates
			{
				switch (this.ActiveSkillId)
				{
					case SkillConst.RangedCombatMastery:
					case SkillConst.ArrowRevolver:
					case SkillConst.ArrowRevolver2:
					case SkillConst.MagnumShot:
					case SkillConst.SupportShot:
					case SkillConst.ElvenMagicMissile:
					case SkillConst.MirageMissile:
					case SkillConst.CrashShot:
						this.ResetAim();
						break;
				}

				EventManager.CreatureEvents.OnCreatureMoves(this, from, to);
			}

			return from;
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
		/// Revives creature and sends necessary packets (BackFromDead + Stat Update).
		/// </summary>
		public void Revive()
		{
			if (!this.IsDead)
				return;

			this.State &= ~CreatureStates.Dead;
			this.CauseOfDeath = DeathCauses.None;
			this.WaitingForRes = false;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead1, this.Id), SendTargets.Range, this);
			WorldManager.Instance.CreatureStatsUpdate(this);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.BackFromTheDead2, this.Id), SendTargets.Range, this);
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
				WorldManager.Instance.CreatureStatsUpdate(this);

				EventManager.CreatureEvents.OnCreatureLevelsUp(this);
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

		public void ResetAim()
		{
			if (this.Target != null)
				this.Client.Send(new MabiPacket(Op.CombatSetAimR, this.Id).PutByte(0));

			this.AimStart = DateTime.MaxValue;
		}

		/// <summary>
		/// Cancels active skill.
		/// Sends: SkillStackUpdate, SkillCancel
		/// </summary>
		public void CancelSkill()
		{
			if (this.ActiveSkillId != SkillConst.None)
			{
				MabiSkill skill; SkillHandler handler;
				SkillManager.CheckOutSkill(this, this.ActiveSkillId, out skill, out handler);
				if (skill == null || handler == null)
					return;

				var result = handler.Cancel(this, skill);

				if ((result & SkillResults.Okay) == 0)
					return;

				Send.SkillStackUpdate(this.Client, this, skill.Id, 0);
				Send.SkillCancel(this.Client, this);
			}

			this.ActiveSkillId = SkillConst.None;
			this.ActiveSkillStacks = 0;
		}

	}
}
