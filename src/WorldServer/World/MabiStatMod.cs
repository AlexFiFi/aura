// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Shared.Const;
using Aura.Shared.Network;
using System.Collections.Generic;
using System.Threading;
using Aura.Shared.Util;

namespace Aura.World.World
{
	public enum StatModSource
	{
		Skill,
		SkillRank,
		LevelUp,
		Item,
		Title,
		Talent,
		CompanionBonus
	}

	public class MabiStatMods
	{
		private Timer _expiryTimer = null; // TODO: Busy wait (?) Events may be better
		public readonly Dictionary<Stat, List<StatMod>> Mods = new Dictionary<Stat, List<StatMod>>();
		private readonly Dictionary<Stat, float> _cache = new Dictionary<Stat, float>();

		public void Add(Stat stat, float amount, StatModSource source, ulong id)
		{
			this.Add(stat, amount, source, id, DateTime.MaxValue);
		}

		public void Add(Stat stat, float amount, StatModSource source, ulong id, DateTime expires)
		{
			if (expires < DateTime.Now)
				throw new ArgumentException("Expires has already passed...");

			lock (this.Mods)
			{
				if (!this.Mods.ContainsKey(stat))
					this.Mods.Add(stat, new List<StatMod>());

				this.Mods[stat].Add(new StatMod(stat, amount, source, id, expires));

				UpdateCache(stat);
			}

			if (expires != DateTime.MaxValue)
			{
				InitExpiryTimer();
			}
		}

		public void Remove(Stat stat, StatModSource source, ulong id)
		{
			lock (this.Mods)
			{
				if (!this.Mods.ContainsKey(stat))
					this.Mods.Add(stat, new List<StatMod>());

				var mod = this.Mods[stat].FirstOrDefault(a => a.Source == source && a.Id == id);

				if (mod == null)
					return;

				this.Mods[stat].Remove(mod);

				UpdateCache(stat);

				if (mod.Expires != DateTime.MaxValue)
					InitExpiryTimer();
			}
		}

		public float GetMod(Stat stat)
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(stat))
					_cache.Add(stat, 0);

				return _cache[stat];
			}
		}

		protected void UpdateCache(Stat? stat = null)
		{
			lock (this.Mods)
			{
				lock (_cache)
				{
					if (stat != null)
					{
						if (!_cache.ContainsKey((Stat)stat))
							_cache.Add((Stat)stat, 0);

						_cache[(Stat)stat] = this.Mods[(Stat)stat].Sum(a => a.Amount);
					}
					else
					{
						foreach (var statEntry in this.Mods.Keys)
						{
							this.UpdateCache(statEntry);
						}
					}
				}
			}
		}

		protected void HandleExpiredStat(object state)
		{
			lock (this.Mods)
			{
				foreach (var modList in this.Mods.Values)
				{
					for (int i = modList.Count - 1; i >= 0; i--)
					{
						if (modList[i].Expires <= DateTime.Now)
							modList.RemoveAt(i);
					}
				}

				InitExpiryTimer();
			}
		}

		protected void InitExpiryTimer()
		{
			DateTime due = DateTime.MaxValue;
			lock (this.Mods)
			{
				foreach (var modList in this.Mods.Values)
				{
					var tempDue = modList.Min(a => a.Expires);
					if (tempDue < due)
						due = tempDue;
				}
			}

			if (_expiryTimer != null)
				_expiryTimer.Dispose();

			if (due != DateTime.MaxValue)
				_expiryTimer = new Timer(new TimerCallback(HandleExpiredStat), null, (long)(due - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
			else
				_expiryTimer = null;
		}
	}

	public class StatMod
	{
		public Stat StatEffect { get; protected set; }
		public float Amount { get; protected set; }
		public DateTime Expires { get; protected set; }
		public StatModSource Source { get; protected set; }
		public ulong Id { get; protected set; }

		public StatMod(Stat stat, float amount, StatModSource source, ulong id, DateTime expires)
		{
			this.StatEffect = stat;
			this.Amount = amount;
			this.Source = source;
			this.Id = id;
			this.Expires = expires;
		}
	}

	public class MabiStatRegen
	{
		private static int _statRegenIndex = 30;

		public uint ModId;
		public Stat Stat;
		public DateTime Start;
		public uint Duration;
		public float ChangePerSecond;
		public float MaxValue;

		public uint TimeLeft
		{
			get
			{
				var now = DateTime.Now;
				var passed = now.Subtract(this.Start);

				if (passed.Milliseconds > this.Duration)
					return 0;
				else
					return this.Duration - (uint)passed.Milliseconds;
			}
		}

		public MabiStatRegen(Stat status, uint duration, float changePerSecond, float maxValue)
		{
			this.ModId = (uint)Interlocked.Increment(ref _statRegenIndex);
			this.Stat = status;
			this.Start = DateTime.Now;
			this.Duration = duration;
			this.ChangePerSecond = changePerSecond;
			this.MaxValue = maxValue;
		}

		public MabiStatRegen(Stat status, float changepersecond, float maxValue)
			: this(status, uint.MaxValue, changepersecond, maxValue)
		{
		}

		public void Terminate()
		{
			this.Duration = 0;
		}

		public void AddToPacket(MabiPacket packet)
		{
			packet.PutInt(this.ModId);
			packet.PutFloat(this.ChangePerSecond);
			packet.PutInt(this.TimeLeft);
			packet.PutInt((uint)this.Stat);
			packet.PutByte(0); // ?
			packet.PutFloat(this.MaxValue);
		}
	}
}
