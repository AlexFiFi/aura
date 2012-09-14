using System;
using System.Collections.Generic;
using System.Linq;
using Common.Network;
using MabiNatives;

namespace Common.World
{
	public enum MabiStat
	{
		HEIGHT = 10, FAT = 11, UPPER = 12, LOWER = 13,
		CURRENT_HP = 25, MAX_HP_WOUND = 26, MAX_HP = 27, ADDITIONAL_MAX_HP = 28,
		CURRENT_MP = 29, MAX_MP = 30, ADDITIONAL_MAX_MP = 31, CURRENT_STAMINA = 32,
		MAX_STAMINA_HUNGER = 34, MAX_STAMINA = 33, ADDITIONAL_MAX_STAMINA = 34, STRENGTH = 44,
		ADDITIONAL_STRENGTH = 45, DEXTERITY = 46, ADDITIONAL_DEXTERITY = 47, INTELLIGENCE = 48,
		ADDITIONAL_INTELLIGENCE = 49, WILL = 50, ADDITIONAL_WILL = 51, LUCK = 52, ADDITIONAL_LUCK = 53,
		RSLOT_MIN_ATTACK = 71, RSLOT_MAX_ATTACK = 72, LSLOT_MIN_ATTACK = 73, LSLOT_MAX_ATTACK = 74,
		MIN_INJURY = 100, MAX_INJURY = 101, PROTECTION = 103, DEFENSE = 104, BALANCE_BOOST = 105
	}

	public abstract class MabiCreature : MabiWorldEntity
	{
		public string Name;
		public uint GuildPosition;
		public string GuildName;

		public uint Race;

		public byte Direction;

		// Attributes
		public ushort AbilityPoints;

		public uint Level = 1;
		public ulong Experience = 0;

		private Dictionary<MabiStat, float> _stats = new Dictionary<MabiStat, float>();

		// Stat Modifiers
		private List<MabiStat> _statChanges = new List<MabiStat>();
		private List<MabiStatMod> _statMods = new List<MabiStatMod>();

		public byte SkinColor;
		public byte Eye;
		public byte EyeColor;
		public byte Lip;

		public string StandStyle = "";

		public uint ColorA = 0x808080;
		public uint ColorB = 0x808080;
		public uint ColorC = 0x808080;

		public byte WeaponSet;
		public byte BattleState;
		public byte SitDownStyle;

		public List<MabiItem> Items = new List<MabiItem>();

		public ushort Title;

		public ushort ActiveSkill;
		public DateTime ActiveSkillLoadTime;

		//public List<ushort> ToggledSkills;

		// Movement
		private uint destX, destY;
		private uint prevX, prevY;

		private DateTime moveTime;
		private bool moveWalk; // run or walk

		// Combat
		public byte KnockCombo;

		private DateTime StunStart;
		private ushort StunLength;

		public bool RightSlotHit;

		public DateTime CreationTime;

		public MabiCreature Owner;

		public MabiCreature()
		{
		}

		public override ushort GetEntityDataType()
		{
			return 16;
		}

		// ONLY call right after rebirth or char creation- ONLY AT LEVEL ONE
		public void RecalculateBaseStats()
		{
			// Get base stats for age
			this.Set(MabiStat.MAX_HP, 10);
			this.Set(MabiStat.MAX_MP, 10);
			this.Set(MabiStat.MAX_STAMINA, 10);
			this.Set(MabiStat.CURRENT_HP, 10);
			this.Set(MabiStat.CURRENT_MP, 10);
			this.Set(MabiStat.CURRENT_STAMINA, 10);

			this.Set(MabiStat.STRENGTH, 10);
			this.Set(MabiStat.INTELLIGENCE, 10);
			this.Set(MabiStat.DEXTERITY, 10);
			this.Set(MabiStat.WILL, 10);
			this.Set(MabiStat.LUCK, 10);

			// Add stats from each skill
		}

		// Call as often as you want
		public void RecalculateAdditionalStats()
		{
			// Calculate additional stats based on what we're wearing, title, etc.
		}

		public float GetBalance()
		{
			return 50;
		}

		public float GetCritical()
		{
			return (float)0.5;
		}

		public void SetStun(ushort stunMilliseconds)
		{
			StunStart = DateTime.Now;
			StunLength = stunMilliseconds;
		}

		public bool IsStunned()
		{
			return (GetStunTime() > 0);
		}

		public ushort GetStunTime()
		{
			if (StunLength == 0 || StunStart == null)
			{
				return 0;
			}

			DateTime now = DateTime.Now;
			TimeSpan passedTime = now.Subtract(StunStart);
			if (passedTime.TotalMilliseconds > StunLength)
			{
				StunLength = 0;
				return 0;
			}

			return (ushort)(StunLength - passedTime.TotalMilliseconds);
		}

		public double CalculateSpeed()
		{
			if (moveTime == null)
				return 0;

			uint x = Math.Max(destX, prevX) - Math.Min(destX, prevX);
			uint y = Math.Max(destY, prevY) - Math.Min(destY, prevY);

			double mdistance = Math.Round(Math.Sqrt(x * x + y * y));

			long mtime = (DateTime.Now.Ticks / 1000) - (moveTime.Ticks / 1000);
			if (mtime <= 0)
				return 0;

			double mspeed = mdistance / mtime;

			return mspeed;
		}

		public MabiItem GetItemInSlot(MabiItem.Slot slot)
		{
			foreach (MabiItem item in this.Items)
			{
				if (item.ItemInfo.uPocketId == (byte)slot)
					return item;
			}

			return null;
		}

		public MabiVertex GetDestination()
		{
			return new MabiVertex(destX, destY);
		}

		public override MabiVertex GetPosition()
		{
			if (destX == prevX && destY == prevY)
				return new MabiVertex((uint)destX, (uint)destY);

			double resultX;
			double resultY;

			// TODO: clean up this function later

			float mySpeed;
			if (moveWalk)
				mySpeed = MabiNativesManager.GetRaceWalkSpeed(Race) / 1000f;
			else
				mySpeed = MabiNativesManager.GetRaceRunSpeed(Race) / 1000f;

			uint xs = Math.Max(prevX, destX) - Math.Min(prevX, destX);
			uint ys = Math.Max(prevY, destY) - Math.Min(prevY, destY);
			//Console.WriteLine(string.Format("ys : {0}", ys));

			double movePer = (((DateTime.Now.Ticks / 10000) - (moveTime.Ticks / 10000)) * mySpeed) / Math.Sqrt(xs * xs + ys * ys);
			//Console.WriteLine(string.Format("Move per : {0}", movePer));

			if (destX > prevX)
			{
				resultX = prevX + Math.Round(xs * movePer);
				if (resultX > destX)
				{
					resultX = destX;
					destX = (uint)resultX;
				}
			}
			else
			{
				resultX = prevX - Math.Round(xs * movePer);
				if (resultX < destX)
				{
					resultX = destX;
					destX = (uint)resultX;
				}
			}

			if (destY > prevY)
			{
				resultY = prevY + Math.Round(ys * movePer);
				if (resultY > destY)
				{
					resultY = destY;
					destY = (uint)resultY;
				}
			}
			else
			{
				resultY = prevY - Math.Round(ys * movePer);
				if (resultY < destY)
				{
					resultY = destY;
					destY = (uint)resultY;
				}
			}

			return new MabiVertex((uint)resultX, (uint)resultY);
		}

		public void StartMove(uint x, uint y, bool walk)
		{
			MabiVertex prevloc = GetPosition();
			prevX = prevloc.X;
			prevY = prevloc.Y;

			if (x == prevX && y == prevY)
			{
				StopMove();
				return;
			}

			destX = x;
			destY = y;
			moveTime = DateTime.Now;
			moveWalk = walk;
		}

		public void StopMove()
		{
			var loc = GetPosition();

			destX = loc.X;
			destY = loc.Y;
		}

		public void SetPosition(uint x, uint y)
		{
			prevX = destX = x;
			prevY = destY = y;
		}

		public MabiItem GetItem(ulong itemid)
		{
			if (!Items.Exists(a => a.GetObjectId() == itemid))
				return null;

			return Items.First(a => a.GetObjectId() == itemid);
		}

		public abstract uint GetCreatureType();

		public void Set(MabiStat stat, float value)
		{
			_statChanges.Add(stat);
			_stats[stat] = value;
		}

		public float GetActualStatValue(MabiStat stat)
		{
			if (_stats.ContainsKey(stat))
				return _stats[stat];
			else
				return 0;
		}

		public float GetStatValue(MabiStat stat)
		{
			if (_stats.ContainsKey(stat))
				return _stats[stat] + GetModifyValue(stat);
			else
				return 0 + GetModifyValue(stat);
		}

		public void ModifyStat(MabiStat stat, float value)
		{
			if (_stats.ContainsKey(stat))
				_stats[stat] += value;
		}

		private float getMaxValue(MabiStat attribute)
		{
			switch (attribute)
			{
				// stat modifiers only work on these!!
				case MabiStat.CURRENT_HP:
					return GetStatValue(MabiStat.MAX_HP) + GetStatValue(MabiStat.ADDITIONAL_MAX_HP);
				case MabiStat.MAX_HP_WOUND:
					return GetStatValue(MabiStat.MAX_HP) + GetStatValue(MabiStat.ADDITIONAL_MAX_HP);
				case MabiStat.CURRENT_MP:
					return GetStatValue(MabiStat.MAX_MP) + GetStatValue(MabiStat.ADDITIONAL_MAX_MP);
				case MabiStat.CURRENT_STAMINA:
					return GetStatValue(MabiStat.MAX_STAMINA) + GetStatValue(MabiStat.ADDITIONAL_MAX_STAMINA);
				case MabiStat.MAX_STAMINA_HUNGER:
					return GetStatValue(MabiStat.MAX_STAMINA) + GetStatValue(MabiStat.ADDITIONAL_MAX_STAMINA);
				default:
					return 0;
			}
		}

		public void AddStatModifier(MabiStat attribute, UInt32 time, float change)
		{
			float maxvalue = getMaxValue(attribute);

			MabiStatMod mod = new MabiStatMod(attribute, time, change, maxvalue);

			_statMods.Add(mod);
		}

		public float GetModifyValue(MabiStat stat)
		{
			float modifyVal = 0;

			foreach (MabiStatMod mod in _statMods)
			{
				if (mod.StatusAttribute == stat)
				{
					modifyVal += mod.GetCurrentChange();
				}
			}

			return modifyVal;
		}

		public void AddPublicStatData(MabiPacket packet)
		{
			packet.PutInt(8); // Number of stats

			packet.PutInt((uint)MabiStat.HEIGHT);
			packet.PutFloat(this.GetStatValue(MabiStat.HEIGHT));

			packet.PutInt((uint)MabiStat.FAT);
			packet.PutFloat(this.GetStatValue(MabiStat.FAT));

			packet.PutInt((uint)MabiStat.UPPER);
			packet.PutFloat(this.GetStatValue(MabiStat.UPPER));

			packet.PutInt((uint)MabiStat.LOWER);
			packet.PutFloat(this.GetStatValue(MabiStat.LOWER));

			packet.PutInt((uint)MabiStat.CURRENT_HP);
			packet.PutFloat(GetStatValue(MabiStat.CURRENT_HP));

			packet.PutInt((uint)MabiStat.MAX_HP);
			packet.PutFloat(GetStatValue(MabiStat.MAX_HP));

			packet.PutInt((uint)MabiStat.ADDITIONAL_MAX_HP);
			packet.PutFloat(GetStatValue(MabiStat.ADDITIONAL_MAX_HP));

			packet.PutInt((uint)MabiStat.MAX_HP_WOUND);
			packet.PutFloat(GetStatValue(MabiStat.MAX_HP) - GetStatValue(MabiStat.MAX_HP_WOUND));

			packet.PutInt((uint)_statMods.Count);

			foreach (MabiStatMod mod in _statMods)
			{
				mod.AddData(packet);
			}

			packet.PutInt(0); // No modifier removers
			packet.PutInt(0);
		}

		public void AddPrivateStatData(MabiPacket packet)
		{
			packet.PutInt(11); // Number of status attributes we're sending

			packet.PutInt((uint)MabiStat.CURRENT_HP);
			packet.PutFloat(GetStatValue(MabiStat.CURRENT_HP));

			packet.PutInt((uint)MabiStat.MAX_HP_WOUND);
			packet.PutFloat(GetStatValue(MabiStat.MAX_HP) - GetStatValue(MabiStat.MAX_HP_WOUND));

			packet.PutInt((uint)MabiStat.MAX_HP);
			packet.PutFloat(GetStatValue(MabiStat.MAX_HP));

			packet.PutInt((uint)MabiStat.ADDITIONAL_MAX_HP);
			packet.PutFloat(GetStatValue(MabiStat.ADDITIONAL_MAX_HP));

			packet.PutInt((uint)MabiStat.CURRENT_MP);
			packet.PutFloat(GetStatValue(MabiStat.CURRENT_MP));

			packet.PutInt((uint)MabiStat.MAX_MP);
			packet.PutFloat(GetStatValue(MabiStat.MAX_MP));

			packet.PutInt((uint)MabiStat.ADDITIONAL_MAX_MP);
			packet.PutFloat(GetStatValue(MabiStat.ADDITIONAL_MAX_MP));

			packet.PutInt((uint)MabiStat.CURRENT_STAMINA);
			packet.PutFloat(GetStatValue(MabiStat.CURRENT_STAMINA));

			packet.PutInt((uint)MabiStat.MAX_STAMINA_HUNGER);
			packet.PutFloat(GetStatValue(MabiStat.MAX_STAMINA) - GetStatValue(MabiStat.MAX_STAMINA_HUNGER));

			packet.PutInt((uint)MabiStat.MAX_STAMINA);
			packet.PutFloat(GetStatValue(MabiStat.MAX_STAMINA));

			packet.PutInt((uint)MabiStat.ADDITIONAL_MAX_STAMINA);
			packet.PutFloat(GetStatValue(MabiStat.ADDITIONAL_MAX_STAMINA));

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
		}

		public override void AddData(MabiPacket packet)
		{
			packet.PutLong(GetObjectId());
			packet.PutByte(5);
			packet.PutString(Name);
			packet.PutString("");
			packet.PutString("");
			packet.PutInt(Race);
			packet.PutByte(SkinColor);
			packet.PutByte(Eye);
			packet.PutByte(EyeColor);
			packet.PutByte(Lip);
			packet.PutInt((uint)GetCreatureType());//0xE0000000);
			packet.PutInt(0);

			packet.PutFloat(this.GetStatValue(MabiStat.HEIGHT));
			packet.PutFloat(this.GetStatValue(MabiStat.FAT));
			packet.PutFloat(this.GetStatValue(MabiStat.UPPER));
			packet.PutFloat(this.GetStatValue(MabiStat.LOWER));

			packet.PutInt(RegionId);

			MabiVertex loc = GetPosition();
			packet.PutInt(loc.X);
			packet.PutInt(loc.Y);

			packet.PutByte(Direction);
			packet.PutInt(BattleState);
			packet.PutByte(WeaponSet);
			packet.PutInt(ColorA);
			packet.PutInt(ColorB);
			packet.PutInt(ColorC);
			packet.PutFloat(1998); // CP
			packet.PutString(StandStyle);
			packet.PutFloat(GetStatValue(MabiStat.CURRENT_HP));
			packet.PutFloat(15);
			packet.PutFloat(0);
			packet.PutFloat(15);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutShort(Title);
			packet.PutLong(0);
			packet.PutShort(0);
			packet.PutString("");

			packet.PutByte(0);
			//Items
			packet.PutInt((uint)Items.Count);

			foreach (MabiItem myItem in Items)
			{
				packet.PutLong(myItem.GetObjectId());
				packet.PutBin(myItem.ItemInfo);
			}

			packet.PutShort(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutByte(0);

			packet.PutByte(0);

			packet.PutString("");
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutInt(0);

			packet.PutByte(1);

			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutLong(0);

			packet.PutInt(0);
			packet.PutLong(0);

			packet.PutString("");
			packet.PutInt(1);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutString("");
			packet.PutByte(0);
			packet.PutShort(0);
			packet.PutShort(0);
			packet.PutString((Owner != null ? Owner.Name : "")); // owner name
			packet.PutLong((Owner != null ? Owner.GetObjectId() : 0)); // owner id
			packet.PutByte(0);
			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(1);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutLong(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutByte(1);
			packet.PutLong(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			/*packet.AddByte(0);
			packet.AddInt(0);
			packet.AddInt(0);
			packet.AddId(0);
			packet.AddInt(0);
			packet.AddByte(0);
			packet.AddInt(0);
			packet.AddId(0);
			packet.AddId(0);
			packet.AddByte(1);
			packet.AddId(0);
			packet.AddInt(0);
			packet.AddByte(0);
			packet.AddByte(0);
			packet.AddByte(0);
			packet.AddInt(0);
			packet.AddId(0);
			packet.AddFloat(0);
			packet.AddInt(100);
			packet.AddByte(9);*/
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutLong(0);

			packet.PutFloat(0);
			packet.PutInt(100);
			packet.PutByte(9);
			packet.PutByte(0);
			packet.PutLong(0);

			//packet.PutString("");
			//packet.PutShort(0);

			packet.PutShort(0);
			packet.PutShort(0);

			//packet.PutString("");

			packet.PutByte(1);
			packet.PutLong(0);
			packet.PutFloat(1);
			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutShort(0);
			packet.PutByte(0);
			packet.PutLong(0);
			packet.PutInt(0xFFFFFFFF);
			packet.PutLong(0);
			packet.PutInt(0);
			if (loc.X != destX || loc.Y != destY)
			{
				packet.PutByte((moveWalk) ? (byte)2 : (byte)1);
				packet.PutInt(destX);
				packet.PutInt(destY);
			}
			else
			{
				packet.PutByte(0);
			}
		}
	}
}
