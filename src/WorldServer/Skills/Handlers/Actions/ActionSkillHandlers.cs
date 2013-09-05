// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Umbrella)]
	public class UmbrellaSkillHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			if (creature.RightHand == null)
				return SkillResults.Failure;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.OpenUmbrella, creature.Id).PutInt(creature.RightHand.Info.Class), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CloseUmbrella, creature.Id), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}
}
