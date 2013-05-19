// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.World;

namespace Aura.World.Skills
{
	public enum SourceOptions : ushort
	{
		None = 0x00,

		/// <summary>
		/// Difference between KBH 1 and 2 unknown. (0x02)
		/// </summary>
		KnockBackHit1 = 0x02,

		/// <summary>
		/// (0x04)
		/// </summary>
		KnockBackHit2 = 0x04,

		/// <summary>
		/// Fireball Explosion (0x08)
		/// </summary>
		UseEffect = 0x08,

		/// <summary>
		/// Req for some skills, didn't show for fireball (0x20)
		/// </summary>
		Result = 0x20,

		/// <summary>
		/// Set when using two weapons (0x40)
		/// </summary>
		DualWield = 0x40,

		/// <summary>
		/// Shows "First Hit"? (0x400)
		/// </summary>
		FirstHit = 0x400,
	}

	public enum TargetOptions : ushort
	{
		None = 0x00,
		Critical = 0x01,
		CleanHit = 0x04,
		FirstAttack = 0x08,
		Finished = 0x10,
		Result = 0x20,
		KnockDownFinish = 0x100,
		Smash = 0x200, // Seems be used with Smash sometimes?
		KnockBack = 0x400,
		KnockDown = 0x800,
		FinishingHit = 0x1000,
		// ??? = 0x100000 // logged using mana shield
		// ??? = 0x4000000 // logged on a counter hit / using mana shield

		/// <summary>
		/// Finished | KnockDownFinish | FinishingHit
		/// </summary>
		FinishingKnockDown = 0x1110,
	}

	// Most likely flags
	public enum CombatActionType : byte
	{
		/// <summary>
		/// Target takes simple hit (1)
		/// </summary>
		TakeHit = 0x01,

		/// <summary>
		/// Simple hit by Source (2)
		/// </summary>
		Hit = 0x02,

		/// <summary>
		/// Both hit at the same time (6)
		/// </summary>
		SimultaneousHit = 0x06,

		/// <summary>
		/// Smash/Counter (32)
		/// </summary>
		HardHit = 0x32,

		/// <summary>
		/// Target type Defense (33)
		/// </summary>
		Defended = 0x33,

		// Target type with Mana Shield
		// ??? = 0x41,

		/// <summary>
		/// Passive Damage, Shadow Bunshin/Fireball (42)
		/// </summary>
		SpecialHit = 0x42,

		/// <summary>
		/// Target type for Counter (53)
		/// </summary>
		CounteredHit = 0x53,

		/// <summary>
		/// Magicbolt, range, Doing Counter? (72)
		/// </summary>
		RangeHit = 0x72,

		//DefendedHit = 0x73, // ?
	}

	public class CombatActionPack
	{
		public MabiCreature Attacker { get; set; }
		public uint CombatActionId { get; set; }
		public uint PrevCombatActionId { get; set; }
		public byte Hit { get; set; }
		public byte HitsMax { get; set; }
		public SkillConst SkillId { get; set; }

		public List<CombatAction> Actions = new List<CombatAction>();

		private CombatActionPack()
		{
			this.CombatActionId = CombatHelper.GetNewActionId();
			this.Hit = 1;
			this.HitsMax = 1;
		}

		public CombatActionPack(MabiCreature attacker, SkillConst skillId)
			: this()
		{
			this.Attacker = attacker;
			this.SkillId = skillId;
		}

		public void Add(params CombatAction[] actions)
		{
			this.Actions.AddRange(actions);
		}

		public MabiPacket GetPacket()
		{
			var p = new MabiPacket(Op.CombatActionBundle, Id.Broadcast);
			p.PutInt(this.CombatActionId);
			p.PutInt(this.PrevCombatActionId);
			p.PutByte(this.Hit);
			p.PutByte(this.HitsMax);
			p.PutByte(0);

			p.PutSInt(this.Actions.Count);
			foreach (var action in this.Actions)
			{
				p.PutIntBin(action.GetPacket(this.CombatActionId).Build(false));
				//Logger.Debug(action.GetPacket(this.CombatActionId));
			}

			//Logger.Debug(p);

			return p;
		}
	}

	public abstract class CombatAction
	{
		public MabiCreature Creature { get; set; }
		public CombatActionType Type { get; set; }
		public ushort StunTime { get; set; }
		public SkillConst SkillId { get; set; }
		public MabiVertex OldPosition { get; set; }

		public abstract bool IsKnock { get; }

		/// <summary>
		/// Returns true if the given type equals the combat action's type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Is(CombatActionType type)
		{
			return (this.Type == type);
		}

		/// <summary>
		/// Returns a packet with the base information for every action
		/// </summary>
		/// <param name="actionId"></param>
		/// <returns></returns>
		public virtual MabiPacket GetPacket(uint actionId)
		{
			var result = new MabiPacket(Op.CombatAction, this.Creature.Id);
			result.PutInt(actionId);
			result.PutLong(this.Creature.Id);
			result.PutByte((byte)this.Type);
			result.PutShort(this.StunTime);
			result.PutShort((ushort)this.SkillId);
			result.PutShort(0);
			return result;
		}
	}

	/// <summary>
	/// Contains information about the source action part of the
	/// CombatActionPack. This part is sent first, before the target actions.
	/// </summary>
	public class SourceAction : CombatAction
	{
		public SourceOptions Options { get; set; }
		public ulong TargetId { get; set; }

		/// <summary>
		/// Returns true if the specified option is set.
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public bool Has(SourceOptions option)
		{
			return ((this.Options & option) != 0);
		}

		/// <summary>
		/// Returns true if KnockBackHit is set.
		/// </summary>
		public override bool IsKnock
		{
			get { return this.Has(SourceOptions.KnockBackHit2) || this.Has(SourceOptions.KnockBackHit1); }
		}

		public SourceAction(CombatActionType type, MabiCreature creature, SkillConst skillId, ulong targetId)
		{
			this.Type = type;
			this.Creature = creature;
			this.SkillId = skillId;
			this.TargetId = targetId;
		}

		public override MabiPacket GetPacket(uint actionId)
		{
			var pos = this.Creature.GetPosition();

			var result = base.GetPacket(actionId);
			result.PutLong(this.TargetId);
			result.PutInt((uint)this.Options);
			result.PutByte(0);
			result.PutByte((byte)(!this.Has(SourceOptions.KnockBackHit2) ? 2 : 1));
			result.PutInts(pos.X, pos.Y);
			// prop id

			return result;
		}
	}

	/// <summary>
	/// Contains information about the target action part of CombatActionPack.
	/// Multiple target actions are used, depending on the amount of targets.
	/// </summary>
	public class TargetAction : CombatAction
	{
		public TargetOptions Options { get; set; }
		public MabiCreature Attacker { get; set; }
		public uint Delay { get; set; }
		public float Damage { get; set; }
		public float ManaDamage { get; set; }

		/// <summary>
		/// Returns true if the specified option is set.
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public bool Has(TargetOptions option)
		{
			return ((this.Options & option) != 0);
		}

		/// <summary>
		/// Returns true if any option involving knocking back/down is
		/// active, including finishers.
		/// </summary>
		public override bool IsKnock
		{
			get { return this.Has(TargetOptions.KnockDownFinish) || this.Has(TargetOptions.Smash) || this.Has(TargetOptions.KnockBack) || this.Has(TargetOptions.KnockDown) || this.Has(TargetOptions.Finished); }
		}

		public TargetAction(CombatActionType type, MabiCreature creature, MabiCreature attacker, SkillConst skillId)
		{
			this.Type = type;
			this.Creature = creature;
			this.Attacker = attacker;
			this.SkillId = skillId;
		}

		public override MabiPacket GetPacket(uint actionId)
		{
			var pos = this.Creature.GetPosition();
			var enemyPos = this.Attacker.GetPosition();

			var result = base.GetPacket(actionId);

			if (this.Is(CombatActionType.Defended) || this.Is(CombatActionType.CounteredHit))
			{
				result.PutLong(this.Attacker.Id);
				result.PutInt(0);
				result.PutByte(0);
				result.PutByte(1);
				result.PutInt(pos.X);
				result.PutInt(pos.Y);
			}

			result.PutInt((uint)this.Options);
			result.PutFloat(this.Damage);
			result.PutFloat(this.ManaDamage);
			result.PutInt(0);

			result.PutFloat((float)enemyPos.X - pos.X);
			result.PutFloat((float)enemyPos.Y - pos.Y);
			if (this.IsKnock)
				result.PutFloats(pos.X, pos.Y);

			result.PutByte(0); // PDef
			result.PutInt(this.Delay);
			result.PutLong(this.Attacker.Id);

			return result;
		}
	}
}
