// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using Common.Network;
using Common.Events;
using Common.Tools;
using Common.Data;
using System.Threading;

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
		public string StandStyleTalk = "";

		public uint ColorA = 0x808080;
		public uint ColorB = 0x808080;
		public uint ColorC = 0x808080;

		public byte WeaponSet;
		public byte BattleState;
		public bool IsFlying;

		public List<MabiItem> Items = new List<MabiItem>();

		public ushort Title;

		public List<MabiSkill> Skills = new List<MabiSkill>();
		public ushort ActiveSkillId;
		public MabiItem ActiveSkillItem;
		public MabiCreature ActiveSkillTarget;
		public byte ActiveSkillStacks;
		public DateTime ActiveSkillPrepareEnd;
		//public MabiSkill ActiveSkill;
		public uint SoulCount;

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

		public DateTime CreationTime;

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

		public MabiCreature()
		{
		}

		/// <summary>
		/// Calculates the damage of left-and-right slots together
		/// </summary>
		/// <returns></returns>
		public float GetWeaponDamage()
		{
			var balance = this.GetRndBalance();
			return (this.GetRndDamage(Pocket.LeftHand1, balance) + this.GetRndDamage(Pocket.RightHand1, balance));
		}

		/// <summary>
		/// Calculates damage within the creature's possibilities.
		/// </summary>
		/// <param name="slot">The slot to use while calculating damage.</param>
		/// <param name="balance">The balance value to use, or NaN to generate one.</param>
		/// <returns></returns>
		public float GetRndDamage(Pocket slot, float balance = float.NaN)
		{
			return this.GetRndDamage(this.GetItemInPocket(slot));
		}

		public float GetRndDamage(MabiItem weapon, float balance = float.NaN)
		{
			float min = 0, max = 0;

			if (weapon != null) // && (slotItem.Type != ItemType.Weapon || slotItem.Type != ItemType.Weapon2))
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
				balance = this.GetRndBalance();

			return min + ((max - min) * balance);
		}
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

			// Do NPCs even regen? o.o
			_statMods.Add(new MabiStatMod(Stat.Life, 0.1875f, this.LifeMax));
			_statMods.Add(new MabiStatMod(Stat.Mana, 0.075f, this.ManaMax));
			_statMods.Add(new MabiStatMod(Stat.Stamina, 0.6f, this.StaminaMax));

			this.HookUp();
		}

		protected override void HookUp()
		{
			ServerEvents.Instance.ErinnTimeTick += this.RestoreStats;
			base.HookUp();
		}

		public override void Dispose()
		{
			ServerEvents.Instance.ErinnTimeTick -= this.RestoreStats;
			base.Dispose();
		}

		protected virtual void RestoreStats(object sender, TimeEventArgs e)
		{
			if (!this.IsDead())
			{
				foreach (var stat in _statMods)
				{
					switch (stat.StatusAttribute)
					{
						case Stat.Life: this.Life += stat.ChangePerSecond; break;
						case Stat.Mana: this.Mana += stat.ChangePerSecond; break;
						case Stat.Stamina: this.Stamina += stat.ChangePerSecond; break;
					}
				}

				//if (e.IsNight)
				//{
				//    this.Mana += 0.15f;
				//}
			}
			//EntityEvents.Instance.OnCreatureStatUpdates(this);
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
		/// Calculates random balance in range of the creature's possibilities.
		/// </summary>
		/// <returns></returns>
		public float GetRndBalance()
		{
			var rnd = RandomProvider.Get();

			var balance = 0.8f; // TODO: Proper base value here. We'll need the weapon.
			balance += ((1.0f - balance) - ((1.0f - balance) * 2 * (float)rnd.NextDouble())) * (float)rnd.NextDouble();

			return balance;
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

		public bool HasSkill(ushort id)
		{
			return this.Skills.Exists(a => a.Info.Id == id);
		}

		public bool HasSkill(SkillConst id)
		{
			return this.HasSkill((ushort)id);
		}

		public MabiItem GetItemInPocket(Pocket slot, bool correctForWeaponSet = true)
		{
			if (correctForWeaponSet && (slot == Pocket.RightHand1 || slot == Pocket.LeftHand1 || slot == Pocket.Arrow1))
				slot += this.WeaponSet;

			return this.GetItemInPocket((byte)slot);
		}

		public MabiItem GetItemInPocket(byte slot)
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

		/// <summary>
		/// Returns null if there is no space for this item.
		/// </summary>
		/// <param name="newItem"></param>
		/// <param name="pocket"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Returns item with the given Id from inventory, or null if it's not found.
		/// </summary>
		/// <param name="itemid"></param>
		/// <returns></returns>
		public MabiItem GetItem(ulong itemid)
		{
			return this.Items.FirstOrDefault(a => a.Id == itemid);
		}

		/// <summary>
		/// Adds one or multiple items with the given id to the creature's
		/// inventory. Tries to fill sacs first, inventory afterwards, and
		/// all remaining will be added to the temp inventory.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		public MabiItem GiveItem(uint itemId, uint amount, uint color1 = 0, uint color2 = 0, uint color3 = 0, bool useDBColors = true, bool drop = false)
		{
			MabiItem result = null;

			foreach (var item in this.Items)
			{
				if ((item.Type == ItemType.Sac && item.StackItem == itemId) || (item.Info.Class == itemId && item.StackType == BundleType.Stackable))
				{
					if (item.Info.Amount >= item.StackMax)
						continue;

					var prev = item.Info.Amount;
					var diff = item.StackMax - item.Info.Amount;
					if (diff >= amount)
					{
						item.Info.Amount += (ushort)amount;
						amount = 0;
					}
					else
					{
						item.Info.Amount = item.StackMax;
						amount -= (uint)diff;
					}

					if (prev != item.Info.Amount)
					{
						EntityEvents.Instance.OnCreatureItemUpdate(this, item);
						result = item;
					}
				}
			}

			while (amount > 0)
			{
				var item = new MabiItem(itemId);
				if (!useDBColors)
				{
					item.Info.ColorA = color1;
					item.Info.ColorB = color2;
					item.Info.ColorC = color3;
				}
				var max = Math.Max((ushort)1, item.StackMax); // This way, we can't drag the server into an infinate loop
				if (amount <= max)
				{
					item.Info.Amount = (ushort)amount;
					amount = 0;
				}
				else
				{
					item.Info.Amount = max;
					amount -= max;
				}

				if (drop)
				{
					EntityEvents.Instance.OnCreatureDropItem(this, item);
				}
				else
				{
					var pocket = Pocket.Inventory;
					var space = this.GetFreeItemSpace(item, pocket);
					if (space == null)
					{
						pocket = Pocket.Temporary;
						space = new MabiVertex(0, 0);
					}

					item.Move(pocket, space.X, space.Y);
					this.Items.Add(item);

					EntityEvents.Instance.OnCreatureItemUpdate(this, item, true);
				}

				result = item;
			}

			return result;
		}

		/// <summary>
		/// Removes the given amount of items with the given id from the
		/// creature's inventory. Tries inventory first, sacs afterwards.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		public void RemoveItem(uint itemId, uint amount)
		{
			var toRemove = new List<MabiItem>();

			// Items first
			foreach (var item in this.Items)
			{
				if (item.Info.Class == 2000)
				{
					var prev = item.Info.Amount;
					if (amount <= item.Info.Amount)
					{
						item.Info.Amount -= (ushort)amount;
						amount = 0;
					}
					else
					{
						amount = amount - item.Info.Amount;
						item.Info.Amount = 0;
					}

					if (prev != item.Info.Amount)
					{
						if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
							toRemove.Add(item);
						EntityEvents.Instance.OnCreatureItemUpdate(this, item);
					}
				}
			}

			// Sacs afterwards
			if (amount > 0)
			{
				foreach (var item in this.Items)
				{
					if (item.Type == ItemType.Sac && item.StackItem == 2000)
					{
						var prev = item.Info.Amount;
						if (amount <= item.Info.Amount)
						{
							item.Info.Amount -= (ushort)amount;
							amount = 0;
						}
						else
						{
							amount = amount - item.Info.Amount;
							item.Info.Amount = 0;
						}

						if (prev != item.Info.Amount)
						{
							if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
								toRemove.Add(item);
							EntityEvents.Instance.OnCreatureItemUpdate(this, item);
						}
					}
				}
			}

			foreach (var item in toRemove)
				this.Items.Remove(item);
		}

		/// <summary>
		/// Adds the given amount of gold to the inventory. See GiveItem.
		/// </summary>
		/// <param name="amount"></param>
		public MabiItem GiveGold(uint amount)
		{
			return this.GiveItem(2000, amount);
		}

		/// <summary>
		/// Removes the given amount of gold from the inventory. See RemoveItem.
		/// </summary>
		/// <param name="amount"></param>
		public void RemoveGold(uint amount)
		{
			this.RemoveItem(2000, amount);
		}

		/// <summary>
		/// Returns wheather the amount of all items with the given id in the
		/// inventory exceeds the given amount. Sacs are counted as well.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool HasItem(uint itemId, uint amount = 1)
		{
			var total = 0;
			foreach (var item in this.Items)
			{
				if (item.Info.Class == itemId || item.StackItem == itemId)
					total += item.Info.Amount;
			}

			return (total >= amount);
		}

		/// <summary>
		/// Returns wheather the creature has the given amount of gold in
		/// its inventory. See HasItem.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool HasGold(uint amount)
		{
			return this.HasItem(2000, amount);
		}

		/// <summary>
		/// Decrements item amount by one and sends update packets.
		/// </summary>
		/// <param name="item"></param>
		public void DecItem(MabiItem item)
		{
			if (!this.Items.Contains(item))
				return;

			item.Info.Amount--;

			if (item.StackType != BundleType.Sac && item.Info.Amount < 1)
				this.Items.Remove(item);

			EntityEvents.Instance.OnCreatureItemUpdate(this, item);
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
				this.Skills.Add(skill);
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
			return this.Skills.FirstOrDefault(a => a.Info.Id == skillId);
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
			}

			if (!this.IsFlying)
				return (!_moveIsWalk ? ri.SpeedRun : ri.SpeedWalk);
			else
				return ri.FlightInfo.FlightSpeed;
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
			if (!this.IsMoving())
				return _position.Copy();

			var passed = (DateTime.Now - _moveStartTime).TotalSeconds;
			if (passed >= _moveDuration)
				//return this.SetPosition(_destination.X, _destination.Y);
				return new MabiVertex(_destination.X, _destination.Y, _destination.H);

			var xt = _position.X + (_movementX * passed);
			var yt = _position.Y + (_movementY * passed);
			var ht = 0.0;
			if (this.IsFlying)
				ht = _position.H + (_movementH < 0 ?
					Math.Max(_movementH * passed, _destination.H) :
					Math.Min(_movementH * passed, _destination.H));

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
				_movementH = (pos.H < dest.H ? this.RaceInfo.FlightInfo.DecentSpeed : this.RaceInfo.FlightInfo.AscentSpeed);
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

		public bool IsMoving()
		{
			return (!_position.Equals(_destination));
		}

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

		public bool ShouldSurvive()
		{
			// TODO: Actual, proper calculation.
			return ((this.Will * 10) + RandomProvider.Get().Next(1001)) > 999;
		}

		public virtual void Die()
		{
			this.State |= CreatureStates.Dead;
		}

		public void Revive()
		{
			this.Injuries = 0;
			this.Life = this.LifeInjured / 2;
			this.State &= ~CreatureStates.Dead;
		}

		public void GiveExp(ulong val)
		{
			this.Experience = Math.Min(this.Experience + val, ExpTable.GetForLevel(ExpTable.GetMaxLevel()));

			if (this.Experience < ExpTable.GetTotalForNextLevel(this.Level))
				return;

			var max = ExpTable.GetMaxLevel();
			var lvl = this.Level;

			var levelStats = MabiData.StatsLevelUpDb.Find(this.Race, this.Age);

			while (this.Level < max && this.Experience >= ExpTable.GetTotalForNextLevel(this.Level))
			{
				this.Level++;

				// For now we'll let pets level up, even without stat update.
				if (levelStats != null)
				{
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
				else
				{
					Logger.Unimplemented("Level up stats missing for race '" + this.Race.ToString() + "'.");
				}
			}

			if (lvl < this.Level)
			{
				this.FullHeal();
				EntityEvents.Instance.OnCreatureLevelsUp(this);
				// TODO: stats update
			}
		}

		public bool IsDead()
		{
			return ((this.State & CreatureStates.Dead) != 0);
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
				mod.AddData(packet);

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
			uint num = 0;

			packet.PutInt((uint)Stat.Life);
			packet.PutFloat(this.Life);
			num++;

			packet.PutInt((uint)Stat.LifeInjured);
			packet.PutFloat(this.LifeInjured);
			num++;

			packet.PutInt((uint)Stat.LifeMax);
			packet.PutFloat(this.LifeMaxBase);
			num++;

			packet.PutInt((uint)Stat.LifeMaxMod);
			packet.PutFloat(this.LifeMaxMod);
			num++;

			packet.PutInt((uint)Stat.Mana);
			packet.PutFloat(this.Mana);
			num++;

			packet.PutInt((uint)Stat.ManaMax);
			packet.PutFloat(this.ManaMaxBase);
			num++;

			packet.PutInt((uint)Stat.ManaMaxMod);
			packet.PutFloat(this.ManaMaxMod);
			num++;

			packet.PutInt((uint)Stat.Stamina);
			packet.PutFloat(this.Stamina);
			num++;

			packet.PutInt((uint)Stat.Food);
			packet.PutFloat(this.StaminaHunger);
			num++;

			packet.PutInt((uint)Stat.StaminaMax);
			packet.PutFloat(this.StaminaMax);
			num++;

			packet.PutInt((uint)Stat.StaminaMaxFood);
			packet.PutFloat(this.Hunger);
			num++;

			//packet.PutInt((uint)Stat.StaminaMaxMod);
			//packet.PutFloat(this.StaminaMaxMod);
			//num++;

			// TODO: Only update required stats.

			packet.PutInt((uint)Stat.Level);
			packet.PutInt(this.Level);
			num++;

			//packet.PutInt((uint)Stat.LevelTotal);
			//packet.PutInt(this.LevelTotal);

			packet.PutInt((uint)Stat.Experience);
			packet.PutLong(ExpTable.CalculateRemaining(this.Level, this.Experience) * 1000);
			num++;

			packet.PutInt((uint)Stat.StrMod);
			packet.PutFloat(this.StrMod);
			num++;

			packet.PutInt((uint)Stat.DexMod);
			packet.PutFloat(this.DexMod);
			num++;

			packet.PutInt((uint)Stat.IntMod);
			packet.PutFloat(this.IntMod);
			num++;

			packet.PutInt((uint)Stat.LuckMod);
			packet.PutFloat(this.LuckMod);
			num++;

			packet.PutInt((uint)Stat.WillMod);
			packet.PutFloat(this.WillMod);
			num++;

			packet.Put<uint>(num, 1);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
		}

#pragma warning disable 0162
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
			if (Op.Version > 140400)
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

			// Apperantly added in 170400
			if (Op.Version >= 170400)
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
			if (Op.Version > 140400 && Op.Version <= 170300)
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

			// Apperantly added in 170400, new event?
			if (Op.Version >= 170400)
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
			if (Op.Version > 140400)
			{
				if (this.EntityType == EntityType.NPC)
				{
					packet.PutShort(0);		         // OnlyShowFilter
					packet.PutShort(0);		         // HideFilter
				}
			}

			// Commerce
			// --------------------------------------------------------------
			if (Op.Version > 140400)
			{
				packet.PutByte(1);					 // IsInCommerceCombat
				packet.PutLong(0);					 // TransportCharacterId
				packet.PutFloat(1);					 // ScaleHeight
			}

			// Apperantly added in 170400
			if (Op.Version >= 170400)
			{
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutFloat(1);
				packet.PutLong(0);
				packet.PutShort(0);
				packet.PutByte(255);

				// Shamala Transformation
				// ----------------------------------------------------------
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
				packet.PutString(StandStyleTalk);
			}

			if (Op.Version > 140400)
				packet.PutByte(0);			     // BombEventState

			// Apperantly added in 170400... or has it always been there?
			if (Op.Version >= 170400)
				packet.PutByte(0);
		}
#pragma warning restore 0162

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
			packet.PutInt((uint)State);
			if (attr == 5)
			{
				packet.PutInt((uint)StateEx);
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

			// Stand styles mess up some models pretty bad =P
			if (this.Shamala == null)
				packet.PutString(StandStyle);
			else
				packet.PutString("");

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
				packet.PutFloat(this.StaminaHunger);
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

				packet.PutInt((uint)_statMods.Count);
				foreach (var mod in _statMods)
					mod.AddData(packet);
			}
			else
			{
				packet.PutFloat(this.Life);
				packet.PutFloat(this.LifeMaxBase);
				packet.PutFloat(this.LifeMaxMod);
				packet.PutFloat(this.LifeInjured);

				packet.PutInt((uint)_statMods.Count);
				foreach (var mod in _statMods)
					mod.AddData(packet);

				packet.PutInt(0);
			}
		}
	}
}
