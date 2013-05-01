// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Data;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Player;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public const float HandBalance = 0.3f;

		public Client Client = null;

		public string Name;

		public uint GuildPosition;
		public string GuildName;

		public uint Race;
		public RaceInfo RaceInfo = null;

		public uint BattleExp = 0;

		private Dictionary<Stat, object> _stats = new Dictionary<Stat, object>();

		private List<MabiStatMod> _statMods = new List<MabiStatMod>();
		public MabiStatMod LifeRegen, ManaRegen, StaminaRegen;

		public byte SkinColor, Eye, EyeColor, Lip;
		public string StandStyle = "";
		public string StandStyleTalk = "";

		public uint Color1 = 0x808080;
		public uint Color2 = 0x808080;
		public uint Color3 = 0x808080;

		public byte WeaponSet;
		public byte BattleState;
		public bool IsFlying;

		public ushort Title /*, TmpTitle ?*/;

		public Dictionary<ushort, MabiSkill> Skills = new Dictionary<ushort, MabiSkill>();
		//public MabiSkill ActiveSkill;
		public ushort ActiveSkillId;
		public MabiItem ActiveSkillItem;
		public MabiCreature ActiveSkillTarget;
		public byte ActiveSkillStacks;
		public DateTime ActiveSkillPrepareEnd;
		public uint SoulCount;

		public CreatureTemp Temp = new CreatureTemp();

		public byte Direction;
		private readonly MabiVertex _position = new MabiVertex(0, 0);
		public readonly MabiVertex _destination = new MabiVertex(0, 0);
		private DateTime _moveStartTime;
		private double _movementX, _movementY, _movementH;
		private double _moveDuration;
		private bool _moveIsWalk;

		public MabiCreature Target = null;
		public float _stun;
		private DateTime _stunStart;
		public bool WaitingForRes = false;

		public MabiCreature Owner, Pet, Vehicle;

		public readonly List<MabiCreature> Riders = new List<MabiCreature>();

		public CreatureStates State;
		public CreatureStatesEx StateEx;
		public CreatureCondition Conditions;
		public CreatureCondition PrevConditions;

		public ulong LastEventTriggered = 0;
		public DungeonAltar OnAltar = DungeonAltar.None;

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

		public float Stun
		{
			get
			{
				if (_stun <= 0)
					return 0;

				var result = _stun - (float)(DateTime.Now - _stunStart).TotalMilliseconds;
				if (result <= 0)
					result = _stun = 0;

				return result;
			}
		}

		public virtual float CombatPower { get { return (this.RaceInfo != null ? this.RaceInfo.CombatPower : 1); } }

		public float CriticalChance { get { return 0.3f; } }

		public bool IsStunned { get { return (this.Stun > 0); } }
		public bool IsDead { get { return ((this.State & CreatureStates.Dead) != 0); } }

		public bool IsMoving { get { return (!_position.Equals(_destination)); } }

		private float _height, _fat, _upper, _lower;
		public float Height { get { return _height; } set { _height = value; } }
		public float Fat { get { return _fat; } set { _fat = value; } }
		public float Upper { get { return _upper; } set { _upper = value; } }
		public float Lower { get { return _lower; } set { _lower = value; } }

		private float _life, _lifeMaxBase, _lifeMaxMod, _injuries;
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
		public float LifeMaxBase { get { return _lifeMaxBase; } set { _lifeMaxBase = value; } }
		public float LifeMaxMod { get { return _lifeMaxMod; } set { _lifeMaxMod = value; } }
		public float LifeMax { get { return _lifeMaxBase + _lifeMaxMod; } }
		public float LifeInjured { get { return this.LifeMax - _injuries; } }

		private float _mana, _manaMaxBase, _manaMaxMod;
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
		public float ManaMaxBase { get { return _manaMaxBase; } set { _manaMaxBase = value; } }
		public float ManaMaxMod { get { return _manaMaxMod; } set { _manaMaxMod = value; } }
		public float ManaMax { get { return _manaMaxBase + _manaMaxMod; } }

		private float _stamina, _staminaMaxBase, _staminaMaxMod, _hunger;
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
		public float StaminaMaxBase { get { return _staminaMaxBase; } set { _staminaMaxBase = value; } }
		public float StaminaMaxMod { get { return _staminaMaxMod; } set { _staminaMaxMod = value; } }
		public float StaminaMax { get { return _staminaMaxBase + _staminaMaxMod; } }
		public float StaminaHunger { get { return this.StaminaMax - _hunger; } }

		public List<MabiStatMod> StatMods { get { return _statMods; } }

		private float _strBase, _dexBase, _intBase, _willBase, _luckBase;
		private float _strMod, _dexMod, _intMod, _willMod, _luckMod;
		public float StrBase { get { return _strBase; } set { _strBase = value; } }
		public float DexBase { get { return _dexBase; } set { _dexBase = value; } }
		public float IntBase { get { return _intBase; } set { _intBase = value; } }
		public float WillBase { get { return _willBase; } set { _willBase = value; } }
		public float LuckBase { get { return _luckBase; } set { _luckBase = value; } }
		public float StrMod { get { return _strMod; } set { _strMod = value; } }
		public float DexMod { get { return _dexMod; } set { _dexMod = value; } }
		public float IntMod { get { return _intMod; } set { _intMod = value; } }
		public float WillMod { get { return _willMod; } set { _willMod = value; } }
		public float LuckMod { get { return _luckMod; } set { _luckMod = value; } }
		public float Str { get { return _strBase + _strMod; } }
		public float Dex { get { return _dexBase + _dexMod; } }
		public float Int { get { return _intBase + _intMod; } }
		public float Will { get { return _willBase + _willMod; } }
		public float Luck { get { return _luckBase + _luckMod; } }

		private byte _age;
		public byte Age { get { return _age; } set { _age = value; } }

		private ushort _ap;
		public ushort AbilityPoints { get { return _ap; } set { _ap = value; } }

		private ushort _lvl;
		private uint _lvlTotal;
		public ushort Level { get { return _lvl; } set { _lvl = value; } }
		public uint LevelTotal { get { return _lvlTotal; } set { _lvlTotal = value; } }

		private ulong _exp;
		public ulong Experience { get { return _exp; } set { _exp = Math.Min(value, ulong.MaxValue); } }

		public uint Defense { get { return (this.RaceInfo != null ? this.RaceInfo.Defense : 0); } }
		public float Protection { get { return (this.RaceInfo != null ? this.RaceInfo.Protection : 0); } }

		public bool IsPlayer { get { return (this.EntityType == EntityType.Character || this.EntityType == EntityType.Pet); } }

		public bool IsHuman { get { return (this.Race == 10001 || this.Race == 10002); } }
		public bool IsElf { get { return (this.Race == 9001 || this.Race == 9002); } }
		public bool IsGiant { get { return (this.Race == 8001 || this.Race == 8002); } }

		public MabiCreature()
		{
		}

		/// <summary>
		/// Returns whether the creature has the given state, short for
		/// (x.State & state) != 0
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public bool HasState(CreatureStates state)
		{
			return (this.State & state) != 0;
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

			this.RaceInfo = dbInfo;

			_statMods.Add(this.LifeRegen = new MabiStatMod(Stat.Life, 0.12f, this.LifeMax));
			_statMods.Add(this.ManaRegen = new MabiStatMod(Stat.Mana, 0.05f, this.ManaMax));
			_statMods.Add(this.StaminaRegen = new MabiStatMod(Stat.Stamina, 0.4f, this.StaminaMax));
			//_statMods.Add(new MabiStatMod(Stat.Food, -0.01f, this.StaminaMax / 2));

			if (MabiTime.Now.IsNight)
				this.ManaRegen.ChangePerSecond *= 3;

			this.HookUp();
		}

		protected override void HookUp()
		{
			ServerEvents.Instance.RealTimeSecondTick += this.RestoreStats;
			base.HookUp();
		}

		public override void Dispose()
		{
			ServerEvents.Instance.RealTimeSecondTick -= this.RestoreStats;
			base.Dispose();
		}

		protected virtual void RestoreStats(object sender, TimeEventArgs e)
		{
			if (this.IsDead)
				return;

			lock (_statMods)
			{
				var toRemove = new List<MabiStatMod>();
				foreach (var mod in _statMods)
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
					_statMods.Remove(mod);
			}
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

			EntityEvents.Instance.OnCreatureStatUpdates(this);
		}

		public void FullHealLife()
		{
			this.Injuries = 0;
			this.Life = this.LifeMax;

			EntityEvents.Instance.OnCreatureStatUpdates(this);
		}

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

		public void AddStun(ushort ms, bool total)
		{
			if (ms == 0)
			{
				_stun = 0;
				return;
			}

			var stun = this.Stun;

			_stunStart = DateTime.Now;

			if (total)
				_stun = ms;
			else
				_stun = stun + ms;
		}

		public bool HasSkill(ushort id)
		{
			return this.Skills.ContainsKey(id);
		}

		public bool HasSkill(SkillConst id)
		{
			return this.HasSkill((ushort)id);
		}

		public void GiveSkill(ushort skillId, byte rank)
		{
			this.GiveSkill((SkillConst)skillId, (SkillRank)rank);
		}

		public void GiveSkill(SkillConst skillId, SkillRank rank)
		{
			var skill = this.GetSkill(skillId);
			if (skill == null)
			{
				skill = new MabiSkill(skillId, rank, this.Race);
				this.AddSkill(skill);
				EntityEvents.Instance.OnCreatureSkillUpdate(this, skill, true);
			}
			else
			{
				skill.Info.Experience = 0;

				skill.Info.Rank = (byte)rank;
				skill.LoadRankInfo();
				EntityEvents.Instance.OnCreatureSkillUpdate(this, skill, false);
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

			return (!_moveIsWalk ? ri.SpeedRun : ri.SpeedWalk);
		}

		public void SetLocation(uint region, uint x, uint y)
		{
			this.Region = region;
			this.SetPosition(x, y);
		}

		public MabiVertex SetPosition(uint x, uint y, uint h = 0)
		{
			_position.X = _destination.X = x;
			_position.Y = _destination.Y = y;
			_position.H = _destination.H = h;

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
				return this.SetPosition(_destination.X, _destination.Y, _destination.H);

			var xt = _position.X + (_movementX * passed);
			var yt = _position.Y + (_movementY * passed);
			var ht = 0.0;
			if (this.IsFlying)
				ht = _position.H + (_movementH < 0 ? Math.Max(_movementH * passed, _destination.H) : Math.Min(_movementH * passed, _destination.H));

			return new MabiVertex((uint)xt, (uint)yt, (uint)ht);
		}

		public MabiVertex StartMove(MabiVertex dest, bool walk = false)
		{
			var pos = this.GetPosition();

			_position.X = pos.X;
			_position.Y = pos.Y;
			_position.H = pos.H;

			_destination.X = dest.X;
			_destination.Y = dest.Y;
			_destination.H = dest.H;

			_moveStartTime = DateTime.Now;
			_moveIsWalk = walk;

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
		/// Stops calculation of movement, doesn't actually stop movement
		/// on the client side.
		/// </summary>
		public void StopMove()
		{
			var pos = this.GetPosition();
			this.SetPosition(pos.X, pos.Y, pos.H);
		}

		/// <summary>
		/// Returns true if the creature is currently moving
		/// to the given position.
		/// </summary>
		/// <param name="dest"></param>
		/// <returns></returns>
		public bool IsDestination(MabiVertex dest)
		{
			return (_destination.Equals(dest));
		}

		public void TakeDamage(float damage)
		{
			var hpBefore = this.Life;
			if (hpBefore < 1)
			{
				this.Die();
				return;
			}

			var hp = Math.Max(-this.LifeMaxBase, hpBefore - damage);
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
		/// Removes dead state and recovers some injuries and life.
		/// </summary>
		public void Revive()
		{
			this.Injuries = 0;
			this.Life = this.LifeInjured / 2;
			this.State &= ~CreatureStates.Dead;
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
				this.FullHeal();
				EntityEvents.Instance.OnCreatureLevelsUp(this);
				// TODO: stats update
			}
		}
	}
}
