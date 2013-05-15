// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.World;
using Aura.World.Events;

namespace Aura.World.Skills
{
	[Flags]
	public enum SkillResults
	{
		None,

		Okay = 0x0001,
		AttackStunned = 0x0002,
		OutOfRange = 0x0004,
		InsufficientStamina = 0x0008,
		InsufficientMana = 0x0010,
		//NoReply = 0x0020,
		Unimplemented = 0x0040,
		InvalidTarget = 0x0080,
		Failure = 0x0100,

		/// <summary>
		/// Okay | NoReply
		/// </summary>
		//OkayNoReply = 0x0021,
	}

	public abstract class SkillHandler
	{
		// Normal skills
		// 1. Loading (Prepare)
		// 2. Loading done (Ready)
		// -  Optionally it can be canceled (Cancel)
		// 3. Target selection or similar (Use)
		// 4. Skill used (Complete)

		public virtual SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			Logger.Unimplemented("Skill prepare handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill ready handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Logger.Unimplemented("Skill use handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Logger.Unimplemented("Skill complete handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			//Logger.Unimplemented("Skill cancel handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			//return SkillResults.Unimplemented;

			// Canceling should be straight forward, let's just accept it.
			// Override for special stuff.
			return SkillResults.Okay;
		}

		// Special case, for skills that only load, and are used through combat mastery.
		public virtual SkillResults UseCombat(MabiCreature creature, ulong targetId, MabiSkill skill)
		{
			Logger.Unimplemented("Skill use combat handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		// Skills like Rest, that don't load and are simply (de)activated.
		public virtual SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill start handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill stop handler for '{0}' ({1}).", skill.Id, skill.Info.Id);
			return SkillResults.Unimplemented;
		}
	}
}
