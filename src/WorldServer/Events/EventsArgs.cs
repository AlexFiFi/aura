// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.World;

namespace Aura.World.Events
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
		public MabiTime Time;

		public TimeEventArgs(MabiTime time)
		{
			this.Time = time;
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

	public class ItemActionEventArgs : EventArgs
	{
		public uint Class;
		public ItemActionEventArgs(uint cls)
		{
			this.Class = cls;
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

	public class CreatureKilledEventArgs : EventArgs
	{
		public MabiCreature Victim;
		public MabiCreature Killer;

		public CreatureKilledEventArgs(MabiCreature victim, MabiCreature killer)
		{
			this.Victim = victim;
			this.Killer = killer;
		}
	}
}
