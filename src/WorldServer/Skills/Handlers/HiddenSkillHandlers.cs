// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.World;
using Common.Events;
using Common.Network;
using System;

namespace World.Skills
{
	public class HiddenResurrectionHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.Send(new MabiPacket(Op.SkillReady, creature.Id).PutShort(skill.Info.Id).PutString(""));

			return SkillResults.Okay | SkillResults.NoReply;
		}

		public override SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			if (!target.IsDead())
				return SkillResults.InvalidTarget;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.HealingMotion).PutString("healing_phoenix").PutLong(target.Id), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			if (creature.ActiveSkillTarget == null || creature.ActiveSkillItem == null || creature.ActiveSkillItem.Info.Class != 63000 || creature.ActiveSkillItem.Info.Amount < 1)
				return SkillResults.InvalidTarget;

			if (!creature.ActiveSkillTarget.IsDead())
				return SkillResults.InvalidTarget;

			creature.DecItem(creature.ActiveSkillItem);

			WorldManager.Instance.CreatureRevive(creature.ActiveSkillTarget);

			creature.Client.Send(new MabiPacket(Op.SkillComplete, creature.Id).PutShort(skill.Info.Id).PutLong(creature.ActiveSkillTarget.Id).PutInts(0, 1));

			return SkillResults.Okay;
		}
	}
}
