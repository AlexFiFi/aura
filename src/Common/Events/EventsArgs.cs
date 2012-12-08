// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Common.World;
using Common.Constants;

namespace Common.Events
{
	/// <summary>
	/// Generally used event arg, for when there's only a few packets to send.
	/// </summary>
	//public class WorldEventArgs : EventArgs
	//{
	//    public List<MabiPacket> Packets = new List<MabiPacket>();
	//}

	/// <summary>
	/// Used in ErinnTime and RealTime, to pass the current time to the subscribers.
	/// </summary>
	public class TimeEventArgs : EventArgs
	{
		public byte Hour, Minute;
		public bool IsNight { get; private set; }

		public TimeEventArgs(byte hour, byte minute)
		{
			this.Hour = hour;
			this.Minute = minute;

			this.IsNight = !(hour >= 6 && hour < 18);
		}

		public override string ToString()
		{
			return (this.Hour.ToString().PadLeft(2, '0') + ":" + this.Minute.ToString().PadLeft(2, '0'));
		}
	}

	public class EntityEventArgs : EventArgs
	{
		public MabiEntity Entity;
		public EntityEventArgs(MabiEntity e)
		{
			Entity = e;
		}
	}

	public class MotionEventArgs : EventArgs
	{
		public uint Category, Type;
		public bool Loop;

		public MotionEventArgs(uint category, uint type, bool loop)
		{
			this.Category = category;
			this.Type = type;
			this.Loop = loop;
		}
	}

	public class ChatEventArgs : EventArgs
	{
		public string Message;

		public ChatEventArgs(string message)
		{
			this.Message = message;
		}
	}

	public class MoveEventArgs : EventArgs
	{
		public MabiVertex From, To;

		public MoveEventArgs(MabiVertex from, MabiVertex to)
		{
			this.From = from;
			this.To = to;
		}
	}

	public class ItemEventArgs : EventArgs
	{
		public MabiItem Item;
		public ItemEventArgs(MabiItem item)
		{
			this.Item = item;
		}
	}

	public class ItemUpdateEventArgs : ItemEventArgs
	{
		public bool IsNew;

		public ItemUpdateEventArgs(MabiItem item, bool isNew = false)
			: base(item)
		{
			this.IsNew = isNew;
		}
	}

	public class SkillUpdateEventArgs : EventArgs
	{
		public MabiSkill Skill;
		public bool IsNew;

		public SkillUpdateEventArgs(MabiSkill skill, bool isNew = false)
		{
			this.Skill = skill;
			this.IsNew = isNew;
		}
	}

	/// <summary>
	/// Combat related args, used to pass the required combat information from the combat class
	/// to the event raising methods, to create the packets, which are passed to the clients
	/// using a standard WorldEventArgs.
	/// </summary>
	public class CombatEventArgs : EventArgs
	{
		public uint CombatActionId, PrevCombatActionId = 0;
		public byte Hit = 1, HitsMax = 1;
		public List<CombatAction> CombatActions = new List<CombatAction>();
	}

	[Flags]
	public enum CombatActionType : byte { TakeDamage = 0x01, Hit = 0x02, /*? = 0x??,*/ Defense = 0x32, }

	public class SkillAction
	{
		public MabiEntity Target;
		public MabiCreature Creature;
		public ulong TargetId;

		public MabiVertex OldPosition;
	}

	public class CombatAction : SkillAction
	{
		public CombatActionType ActionType;
		public ushort StunTime;
		public SkillConst SkillId;
		public float CombatDamage;

		public bool Critical;
		public bool CleanHit;
		public bool FirstAttack;
		public bool Knockback;
		public bool Knockdown;
		public bool Finish;
		public bool DualWield;

		// Noticed in WM, seems to delay the knock back.
		public uint ReactionDelay;

		public uint GetAttackOption()
		{
			uint result = 0x20;

			if (ActionType.HasFlag(CombatActionType.TakeDamage))
			{
				// WM kill: 0x111C

				if (Critical) result += 0x01;
				if (CleanHit) result += 0x04;
				if (FirstAttack) result += 0x08;
				if (Finish) result += 0x1110;
				else if (Knockdown) result += 0x800;
				else if (Knockback) result += 0x400;
			}
			else if (ActionType.HasFlag(CombatActionType.Hit))
			{
				// WM kill: 0x22

				if (Finish) result += 0x06;
				else if (IsKnock()) result += 0x04;
				else if (DualWield) result += 0x40;
			}

			return result;
		}

		public byte GetDefenseOption()
		{
			byte result = 0x0;

			// += 0x01  heavy stander?
			// += 0x02  heavy stander?
			// += 0x04  mana deflector?

			return result;
		}

		/// <summary>
		/// Returns whether the enemy is sent flying (knockback/down or finish).
		/// </summary>
		/// <returns></returns>
		public bool IsKnock()
		{
			return (Knockback || Knockdown || Finish);
		}
	}
}
