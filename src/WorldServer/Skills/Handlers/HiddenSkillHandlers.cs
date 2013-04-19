// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.World;

namespace Aura.World.Skills
{
	public class HiddenResurrectionHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			creature.Client.Send(new MabiPacket(Op.SkillReady, creature.Id).PutShort(skill.Info.Id).PutString(""));

			return SkillResults.Okay | SkillResults.NoReply;
		}

		public override SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			if (!target.IsDead)
				return SkillResults.InvalidTarget;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.UseMagic).PutString("healing_phoenix").PutLong(target.Id), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			if (creature.ActiveSkillTarget == null || creature.ActiveSkillItem == null || creature.ActiveSkillItem.Info.Class != 63000 || creature.ActiveSkillItem.Info.Amount < 1)
				return SkillResults.InvalidTarget;

			if (!creature.ActiveSkillTarget.IsDead)
				return SkillResults.InvalidTarget;

			creature.DecItem(creature.ActiveSkillItem);

			WorldManager.Instance.CreatureRevive(creature.ActiveSkillTarget);

			creature.Client.Send(new MabiPacket(Op.SkillComplete, creature.Id).PutShort(skill.Info.Id).PutLong(creature.ActiveSkillTarget.Id).PutInts(0, 1));

			return SkillResults.Okay;
		}
	}
}
