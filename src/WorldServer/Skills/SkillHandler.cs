// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.World;
using World.World;
using World.Network;
using Common.Tools;
using Common.Network;
using Common.Events;
using Common.Constants;

namespace World.Skills
{
	[Flags]
	public enum SkillResults
	{
		Okay = 1,
		AttackStunned = 2,
		OutOfRange = 4,
		InsufficientStamina = 8,
		InsufficientMana = 16,
		NoReply = 32,
		Unimplemented = 256,
		InvalidTarget = 512,
		Failure = 1024,
	}

	public abstract class SkillHandler
	{
		// Normal skills
		// 1. Loading (Prepare)
		// 2. Loading done (Ready)
		// -  Optionally it can be canceled (Cancel)
		// 3. Target selection or similar (Use)
		// 4. Skill used (Complete)
		public virtual SkillResults Prepare(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill prepare handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill ready handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			Logger.Unimplemented("Skill use handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill complete handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			//Logger.Unimplemented("Skill cancel handler for '" + skill.Info.Id.ToString() + "'.");
			//return SkillResults.Unimplemented;

			// Canceling should be straight forward, let's just accept it.
			// Override for special stuff.
			return SkillResults.Okay;
		}

		// Special case, for skills that only load, and are used through combat mastery.
		public virtual SkillResults UseCombat(MabiCreature creature, MabiCreature target, CombatAction sourceAction, MabiSkill skill)
		{
			Logger.Unimplemented("Skill use combat handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		// Skills like Rest, that don't load and are simply (de)activated.
		public virtual SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill start handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		public virtual SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			Logger.Unimplemented("Skill stop handler for '" + skill.Info.Id.ToString() + "'.");
			return SkillResults.Unimplemented;
		}

		// Helpers
		protected void Flash(MabiCreature creature)
		{
			this.SkillInit(creature, "flashing");
		}

		protected void SkillInit(MabiCreature creature, string str = "")
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.SkillInit).PutString(str), SendTargets.Range, creature);
		}

		protected void SetActive(MabiCreature creature, MabiSkill skill)
		{
			creature.ActiveSkillId = skill.Info.Id;
		}

		protected void InitStack(MabiCreature creature, MabiSkill skill)
		{
			creature.ActiveSkillStacks = skill.RankInfo.Stack;

			if (creature.Client != null)
				creature.Client.Send(new MabiPacket(Op.SkillStackSet, creature.Id).PutBytes(creature.ActiveSkillStacks, creature.ActiveSkillStacks).PutShort(creature.ActiveSkillId));
		}

		protected void DecStack(MabiCreature creature, MabiSkill skill)
		{
			creature.ActiveSkillStacks--;

			if (creature.Client != null)
				creature.Client.Send(new MabiPacket(Op.SkillStackUpdate, creature.Id).PutBytes(creature.ActiveSkillStacks, 1, 0).PutShort(skill.Info.Id));
		}

		/// <summary>
		/// Set target aggro if new enemy.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="target"></param>
		protected void SetAggro(MabiCreature creature, MabiCreature target)
		{
			if (target.Target != creature)
			{
				target.Target = creature;
				target.BattleState = 1;
				WorldManager.Instance.CreatureChangeStance(target, 0);
			}
		}
	}
}
