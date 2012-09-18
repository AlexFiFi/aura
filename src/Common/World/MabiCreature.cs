// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using Common.Network;
using MabiNatives;

namespace Common.World
{
	public abstract class MabiCreature : MabiEntity
	{
		public Client Client = null;

		public string Name;

		public uint GuildPosition;
		public string GuildName;

		public uint Race;
		public RaceInfo RaceInfo = null;

		public uint BattleExp = 0;

		private Dictionary<Stat, object> _stats = new Dictionary<Stat, object>();

		private List<Stat> _statChanges = new List<Stat>();
		private List<MabiStatMod> _statMods = new List<MabiStatMod>();

		public byte SkinColor, Eye, EyeColor, Lip;
		public string StandStyle = "";

		public uint ColorA = 0x808080;
		public uint ColorB = 0x808080;
		public uint ColorC = 0x808080;

		public byte WeaponSet;
		public byte BattleState;

		public List<MabiItem> Items = new List<MabiItem>();

		public ushort Title;

		public List<MabiSkill> Skills = new List<MabiSkill>();
		public ushort ActiveSkillId;
		public MabiItem ActiveSkillItem;
		public MabiCreature ActiveSkillTarget;
		public byte ActiveSkillStacks;
		public DateTime ActiveSkillLoadTime;

		public byte Direction;
		private readonly MabiVertex _position = new MabiVertex(0, 0);
		public readonly MabiVertex _destination = new MabiVertex(0, 0);
		private DateTime _moveStartTime;
		private double _movementX, _movementY;
		private double _moveDuration;
		private bool _moveIsWalk;

		public MabiCreature Target = null;
		public float _stun;
		private DateTime _stunStart;
		public bool WaitingForRes = false;

		public DateTime CreationTime;

		public MabiCreature Owner, Vehicle;

		public CreatureStates Status;
		public CreatureStatesEx StatusEx;
		public CreatureCondition StatusEffects;

		public byte RestPose = 4; // 0, 2, 4

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

		public float CombatPower
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
			}
		}
		public float LifeMaxBase { get { return _lifeMaxBase; } set { _lifeMaxBase = value; } }
		public float LifeMaxMod { get { return _lifeMaxMod; } set { _lifeMaxMod = value; } }
		public float Injuries { get { return _injuries; } set { _injuries = value; } }
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

		private float _stamina, _staminaMaxBase, _staminaMaxMod, _food;
		public float Stamina
		{
			get { return _stamina; }
			set
			{
				if (value > this.StaminaFood)
					_stamina = this.StaminaFood;
				else if (value < -this.StaminaMax)
					_stamina = -this.StaminaMax;
				else
					_stamina = value;
			}
		}
		public float StaminaMaxBase { get { return _staminaMaxBase; } set { _staminaMaxBase = value; } }
		public float StaminaMaxMod { get { return _staminaMaxMod; } set { _staminaMaxMod = value; } }
		public float Food { get { return _food; } set { _food = value; } }
		public float StaminaMax { get { return _staminaMaxBase + _staminaMaxMod; } }
		public float StaminaFood { get { return this.StaminaMax - _food; } }

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

		private uint _lvl, _lvlTotal;
		public uint Level { get { return _lvl; } set { _lvl = value; } }
		public uint LevelTotal { get { return _lvlTotal; } set { _lvlTotal = value; } }

		private ulong _exp;
		public ulong Experience { get { return _exp; } set { _exp = value; } }

		public bool IsPlayer()
		{
			return (this.EntityType == EntityType.Character || this.EntityType == EntityType.Pet);
		}

		public bool IsHuman()
		{
			return (this.Race == 10001 || this.Race == 10002);
		}

		public bool IsElf()
		{
			return (this.Race == 9001 || this.Race == 9002);
		}

		public bool IsGiant()
		{
			return (this.Race == 8001 || this.Race == 8002);
		}

		public void LoadDefault()
		{
			if (this.Race == 0)
				throw new Exception("Set race before calling LoadDefault.");

			var dbInfo = MabiData.RaceDb.Find(this.Race);
			if (dbInfo == null)
			{
				// Try to default to Human
				dbInfo = MabiData.RaceDb.Find(10000);
				if (dbInfo == null)
				{
					throw new Exception("Unable to load race defaults, race not found.");
				}
			}

			this.RaceInfo = dbInfo;
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
			this.Food = 0;
			this.Life = this.LifeMax;
			this.Mana = this.ManaMax;
			this.Stamina = this.StaminaMax;
		}

		public float GetBalance()
		{
			return 0.8f;
		}

		public float GetCritical()
		{
			return 0.3f;
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

		public bool IsStunned()
		{
			return (this.Stun > 0);
		}

		/// <summary>
		/// Returns the remaining stun time. See IsStunned for more info.
		/// </summary>
		/// <param name="half"></param>
		/// <returns></returns>
		//public ushort GetStunTime(bool half = false)
		//{
		//    if (this._stunLength == 0 || this._stunStart == null)
		//        return 0;

		//    var now = DateTime.Now;
		//    var passedTime = now.Subtract(this._stunStart);

		//    if (half)
		//        passedTime = TimeSpan.FromTicks((long)(passedTime.Ticks * 1.5f));

		//    if (passedTime.TotalMilliseconds > _stunLength)
		//        return (this._stunLength = 0);

		//    return (ushort)(_stunLength - passedTime.TotalMilliseconds);
		//}

		public MabiSkill GetSkill(ushort id)
		{
			return this.Skills.FirstOrDefault(a => a.Info.Id == id);
		}

		public MabiSkill GetSkill(SkillConst id)
		{
			return this.GetSkill((ushort)id);
		}

		public bool HasSkill(ushort id)
		{
			return this.Skills.Exists(a => a.Info.Id == id);
		}

		public bool HasSkill(SkillConst id)
		{
			return this.HasSkill((ushort)id);
		}

		public MabiItem GetItem(Pocket slot)
		{
			// Maybe it would make more sense to really switch the pocket on toggling the set.
			if (slot == Pocket.LeftHand1 || slot == Pocket.RightHand1 || slot == Pocket.Arrow1)
				slot += this.WeaponSet;

			return this.GetItemInSlot((byte)slot);
		}

		public MabiItem GetItemInSlot(byte slot)
		{
			return this.Items.FirstOrDefault(a => a.Info.Pocket == slot);
		}

		public MabiItem GetItemColliding(Pocket target, uint sourceX, uint sourceY, MabiItem newItem)
		{
			foreach (var item in this.Items)
			{
				if (item == newItem)
					continue;

				if (item.Info.Pocket != (byte)target)
					continue;

				if (!(
					sourceX > item.Info.X + item.Width - 1 || item.Info.X > sourceX + newItem.Width - 1 ||
					sourceY > item.Info.Y + item.Height - 1 || item.Info.Y > sourceY + newItem.Height - 1
				))
				{
					return item;
				}
			}

			return null;
		}

		public MabiVertex GetFreeItemSpace(MabiItem newItem, Pocket pocket)
		{
			var info = MabiData.ItemDb.Find(newItem.Info.Class);

			// I'm sure there's a way to optimze this, but oh well, good enough for now.
			// Look at every space and see if the new item would fit there.
			for (uint y = 0; y < this.RaceInfo.InvHeight; ++y)
			{
				for (uint x = 0; x < this.RaceInfo.InvWidth; ++x)
				{
					var item = this.GetItemColliding(pocket, x, y, newItem);
					if (item == null && (x + info.Width - 1 < 6 && y + info.Height - 1 < 10))
					{
						return new MabiVertex(x, y);
					}
				}
			}

			return null;
		}

		public float GetSpeed()
		{
			if (this.Vehicle == null)
				return (!_moveIsWalk ? this.RaceInfo.SpeedRun : this.RaceInfo.SpeedWalk);
			else
				return (!_moveIsWalk ? this.Vehicle.RaceInfo.SpeedRun : this.Vehicle.RaceInfo.SpeedWalk);
		}

		public void SetLocation(uint region, uint x, uint y)
		{
			this.Region = region;
			this.SetPosition(x, y);
		}

		public MabiVertex SetPosition(uint x, uint y)
		{
			_position.X = _destination.X = x;
			_position.Y = _destination.Y = y;

			return _position.Copy();
		}

		public override MabiVertex GetPosition()
		{
			if (!this.IsMoving())
				return _position.Copy();

			var passed = (DateTime.Now - _moveStartTime).TotalSeconds;
			if (passed >= _moveDuration)
				//return this.SetPosition(_destination.X, _destination.Y);
				return new MabiVertex(_destination.X, _destination.Y);

			var xt = _position.X + (_movementX * passed);
			var yt = _position.Y + (_movementY * passed);

			return new MabiVertex((uint)xt, (uint)yt);
		}

		public MabiVertex StartMove(MabiVertex dest, bool walk = false)
		{
			var pos = this.GetPosition();

			_position.X = pos.X;
			_position.Y = pos.Y;

			_destination.X = dest.X;
			_destination.Y = dest.Y;

			_moveStartTime = DateTime.Now;
			_moveIsWalk = walk;

			var diffX = (int)dest.X - (int)pos.X;
			var diffY = (int)dest.Y - (int)pos.Y;
			_moveDuration = Math.Sqrt(diffX * diffX + diffY * diffY) / this.GetSpeed();

			_movementX = diffX / _moveDuration;
			_movementY = diffY / _moveDuration;

			this.Direction = (byte)(Math.Floor(Math.Atan2(_movementY, _movementX) / 0.02454369260617026));

			return pos;
		}

		public void StopMove()
		{
			var pos = this.GetPosition();
			this.SetPosition(pos.X, pos.Y);
		}

		public bool IsMoving()
		{
			return (!_position.Equals(_destination));
		}

		public bool IsDestination(MabiVertex dest)
		{
			return (_destination.Equals(dest));
		}

		/// <summary>
		/// Returns item with the given Id from inventory, or null if it's not found.
		/// </summary>
		/// <param name="itemid"></param>
		/// <returns></returns>
		public MabiItem GetItem(ulong itemid)
		{
			return this.Items.FirstOrDefault(a => a.Id == itemid);
		}

		//public void AddStatModifier(Stat attribute, uint time, float change)
		//{
		//    var max = this.GetStatMax(attribute);
		//    var mod = new MabiStatMod(attribute, time, change, max);

		//    _statMods.Add(mod);
		//}

		//public float GetStatModifier(Stat stat)
		//{
		//    float result = 0;

		//    foreach (var mod in _statMods)
		//    {
		//        if (mod.StatusAttribute == stat)
		//        {
		//            result += mod.GetCurrentChange();
		//        }
		//    }

		//    return result;
		//}

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

			if (hp <= 0 && (hpBefore < this.LifeMax / 2))
			{
				this.Die();
			}
		}

		public virtual void Die()
		{
			this.Status |= CreatureStates.Dead;
		}

		public void Revive()
		{
			this.Injuries = 0;
			this.Life = this.LifeInjured / 2;
			this.Status &= ~CreatureStates.Dead;
		}

		public void GiveExp(ulong val)
		{
			this.Experience += val;

			if (this.Experience < ExpTable.GetTotalForNextLevel(this.Level))
				return;

			Dictionary<string, ushort> addedStats = new Dictionary<string,ushort>()
			{
				{ "Level", 0 },
				{ "AP", 0 }
			};

			while (this.Experience >= ExpTable.GetTotalForNextLevel(this.Level) && this.Level < ExpTable.GetMaxLevel())
			{
				addedStats["Level"]++;
				addedStats["AP"]++;
			}
			LevelUp(addedStats);
		}

		public void LevelUp(Dictionary<string, ushort> addedStats)
		{
			this.AbilityPoints += addedStats["AP"];
			this.Level += addedStats["Level"];

			//TODO: Other Stats

			FullHeal();
		}

		public bool IsDead()
		{
			return this.Status.HasFlag(CreatureStates.Dead);
		}

		public void AddPublicStatData(MabiPacket packet)
		{
			packet.PutInt(9); // Number of stats

			packet.PutInt((uint)Stat.Height);
			packet.PutFloat(this.Height);

			packet.PutInt((uint)Stat.Fat);
			packet.PutFloat(this.Fat);

			packet.PutInt((uint)Stat.Upper);
			packet.PutFloat(this.Upper);

			packet.PutInt((uint)Stat.Lower);
			packet.PutFloat(this.Lower);

			packet.PutInt((uint)Stat.CombatPower);
			packet.PutFloat(this.CombatPower);

			packet.PutInt((uint)Stat.Life);
			packet.PutFloat(this.Life);

			packet.PutInt((uint)Stat.LifeMax);
			packet.PutFloat(this.LifeMaxBase);

			packet.PutInt((uint)Stat.LifeMaxMod);
			packet.PutFloat(this.LifeMaxMod);

			packet.PutInt((uint)Stat.LifeInjured);
			packet.PutFloat(this.LifeInjured);

			packet.PutInt((uint)_statMods.Count);

			foreach (var mod in _statMods)
			{
				mod.AddData(packet);
			}

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
		}

		public void AddPrivateStatData(MabiPacket packet)
		{
			// Number of stats get inserted later

			packet.PutInt((uint)Stat.Life);
			packet.PutFloat(this.Life);

			packet.PutInt((uint)Stat.LifeInjured);
			packet.PutFloat(this.LifeInjured);

			packet.PutInt((uint)Stat.LifeMax);
			packet.PutFloat(this.LifeMaxBase);

			packet.PutInt((uint)Stat.LifeMaxMod);
			packet.PutFloat(this.LifeMaxMod);

			packet.PutInt((uint)Stat.Mana);
			packet.PutFloat(this.Mana);

			packet.PutInt((uint)Stat.ManaMax);
			packet.PutFloat(this.ManaMaxBase);

			packet.PutInt((uint)Stat.ManaMaxMod);
			packet.PutFloat(this.ManaMaxMod);

			packet.PutInt((uint)Stat.Stamina);
			packet.PutFloat(this.Stamina);

			packet.PutInt((uint)Stat.Food);
			packet.PutFloat(this.StaminaFood);

			packet.PutInt((uint)Stat.StaminaMax);
			packet.PutFloat(this.StaminaMaxBase);

			packet.PutInt((uint)Stat.StaminaMaxFood);
			packet.PutFloat(this.Food);

			// TODO: Only update required stats.

			packet.PutInt((uint)Stat.Level);
			packet.PutInt(this.Level);

			//packet.PutInt((uint)Stat.LevelTotal);
			//packet.PutInt(this.LevelTotal);

			packet.PutInt((uint)Stat.Experience);
			packet.PutLong(ExpTable.CalculateRemaining(this.Level, this.Experience) * 1000);

			packet.Put<uint>(11 + 2, 1);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
		}

		// Playable characters overwrite this, applies to monsters and NPCs
		public override void AddEntityData(MabiPacket packet)
		{
			this.AddEntityData(packet, 5);

			// Titles
			// --------------------------------------------------------------
			packet.PutShort(this.Title);		 // SelectedTitle
			packet.PutLong(0);                   // TitleAppliedTime
			packet.PutShort(0);					 // SelectedOptionTitle

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
			packet.PutByte(0);			         // {PLGCNT}

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
			packet.PutLong((ulong)StatusEffects.A);
			packet.PutLong((ulong)StatusEffects.B);
			packet.PutLong((ulong)StatusEffects.C);
			packet.PutLong((ulong)StatusEffects.D);
			packet.PutInt(0);					 // condition event message list
			// loop
			//   packet.PutInt
			//   packet.PutString

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

			// Transformation
			// --------------------------------------------------------------
			packet.PutByte(0);				     // Type (1:Paladin, 2:DarkKnight, 3:SubraceTransformed, 4:TransformedElf, 5:TransformedGiant)
			packet.PutShort(0);				     // Level
			packet.PutShort(0);				     // SubType

			// Follower (Pets)
			// --------------------------------------------------------------
			packet.PutString(Owner != null ? Owner.Name : "");
			packet.PutLong(Owner != null ? Owner.Id : 0);
			packet.PutByte(0);									// KeepingMode
			packet.PutLong(0);									// KeepingProp

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
			packet.PutByte(0);					 // IsAviating
			// loop
			//   packet.PutFloat				 // FromX
			//   packet.PutFloat
			//   packet.PutFloat				 // FromY
			//   packet.PutFloat				 // ToX
			//   packet.PutFloat
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
			if (this.EntityType == EntityType.NPC)
			{
				packet.PutShort(0);		         // OnlyShowFilter
				packet.PutShort(0);		         // HideFilter
			}

			// Commerce
			// --------------------------------------------------------------
			packet.PutByte(1);					 // IsInCommerceCombat
			packet.PutLong(0);					 // TransportCharacterId
			packet.PutFloat(1);					 // ScaleHeight

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
			if (!this.IsMoving())
			{
				packet.PutByte(0);
			}
			else
			{
				packet.PutByte((byte)(!_moveIsWalk ? 2 : 1));
				packet.PutInt(_destination.X);
				packet.PutInt(_destination.Y);
			}

			if (this.EntityType == EntityType.NPC)
			{
				packet.PutString("");	         // NPC_TALKING_MOTION
			}
			packet.PutByte(0);			         // BombEventState
		}

		public void AddEntityData(MabiPacket packet, byte attr)
		{
			packet.PutLong(Id);
			packet.PutByte(attr);

			// Looks/Location
			// --------------------------------------------------------------
			var loc = this.GetPosition();
			packet.PutString(Name);
			packet.PutString("");				 // Title
			packet.PutString("");				 // Eng Title
			packet.PutInt(Race);
			packet.PutByte(SkinColor);
			packet.PutByte(Eye);
			packet.PutByte(EyeColor);
			packet.PutByte(Lip);
			packet.PutInt((uint)Status);
			if (attr == 5)
			{
				packet.PutInt((uint)StatusEx);
			}
			packet.PutFloat(this.Height);
			packet.PutFloat(this.Fat);
			packet.PutFloat(this.Upper);
			packet.PutFloat(this.Lower);
			packet.PutInt(Region);
			packet.PutInt(loc.X);
			packet.PutInt(loc.Y);
			packet.PutByte(Direction);
			packet.PutInt(BattleState);
			packet.PutByte(WeaponSet);
			packet.PutInt(ColorA);               // Colors
			packet.PutInt(ColorB);               // ^
			packet.PutInt(ColorC);               // ^

			// Stats
			// --------------------------------------------------------------
			packet.PutFloat(CombatPower);
			packet.PutString(StandStyle);
			if (attr == 2)
			{
				packet.PutFloat(this.Life);
				packet.PutFloat(this.LifeInjured);
				packet.PutFloat(this.LifeMaxBase);
				packet.PutFloat(this.LifeMaxMod);
				packet.PutFloat(this.Mana);
				packet.PutFloat(this.ManaMaxBase);
				packet.PutFloat(this.ManaMaxMod);
				packet.PutFloat(this.Stamina);
				packet.PutFloat(this.StaminaMaxBase);
				packet.PutFloat(this.StaminaMaxMod);
				packet.PutFloat(this.StaminaFood);
				packet.PutFloat(0.5f);
				packet.PutShort((ushort)this.Level);
				packet.PutInt(this.LevelTotal);
				packet.PutShort(0);                  // Max Level
				packet.PutShort(0);					 // Rebirthes
				packet.PutShort(0);
				packet.PutLong(ExpTable.CalculateRemaining(this.Level, this.Experience) * 1000);
				packet.PutShort(Age);
				packet.PutFloat(this.StrBase);
				packet.PutFloat(this.StrMod);
				packet.PutFloat(this.DexBase);
				packet.PutFloat(this.DexMod);
				packet.PutFloat(this.IntBase);
				packet.PutFloat(this.IntMod);
				packet.PutFloat(this.WillBase);
				packet.PutFloat(this.WillMod);
				packet.PutFloat(this.LuckBase);
				packet.PutFloat(this.LuckMod);
				packet.PutFloat(0);					 // LifeMaxByFood
				packet.PutFloat(0);					 // ManaMaxByFood
				packet.PutFloat(0);					 // StaminaMaxByFood
				packet.PutFloat(0);					 // StrengthByFood
				packet.PutFloat(0);					 // DexterityByFood
				packet.PutFloat(0);					 // IntelligenceByFood
				packet.PutFloat(0);					 // WillByFood
				packet.PutFloat(0);					 // LuckByFood
				packet.PutShort(AbilityPoints);
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
				packet.PutShort(15);		         // MeleeAttackRateMod
				packet.PutShort(15);		         // RangeAttackRateMod
				packet.PutFloat(0);			         // CriticalBase
				packet.PutFloat(0);			         // CriticalMod
				packet.PutFloat(0);			         // ProtectBase
				packet.PutFloat(0);			         // ProtectMod
				packet.PutShort(0);			         // DefenseBase
				packet.PutShort(0);			         // DefenseMod
				packet.PutShort(0);			         // RateBase
				packet.PutShort(0);			         // RateMod
				packet.PutShort(0);			         // Rank1
				packet.PutShort(0);			         // Rank2
				packet.PutLong(0);			         // Score
				packet.PutShort(0);			         // AttackMinBaseMod
				packet.PutShort(8);			         // AttackMaxBaseMod
				packet.PutShort(0);			         // WAttackMinBaseMod
				packet.PutShort(0);			         // WAttackMaxBaseMod
				packet.PutFloat(10);		         // CriticalBaseMod
				packet.PutFloat(0);		             // ProtectBaseMod
				packet.PutShort(0);		             // DefenseBaseMod
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

				packet.PutInt(0);                    // StackingBuffer (all active buff)
				// loop
				//   packet.PutInt;
				//   packet.PutFloat
				//   packet.PutInt
				//   packet.PutInt
				//   packet.PutByte
				//   packet.PutFloat
			}
			else
			{
				packet.PutFloat(this.Life);
				packet.PutFloat(this.LifeMaxBase);
				packet.PutFloat(this.LifeMaxMod);
				packet.PutFloat(this.LifeInjured);

				packet.PutInt(0);
				packet.PutInt(0);
			}
		}
	}
}
