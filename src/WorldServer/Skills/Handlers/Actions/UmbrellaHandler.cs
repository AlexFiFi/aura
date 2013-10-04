// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.World;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Umbrella)]
	public class UmbrellaSkillHandler : StartStopSkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			if (creature.RightHand == null)
				return SkillResults.Failure;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.OpenUmbrella, creature.Id).PutInt(creature.RightHand.Info.Class), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CloseUmbrella, creature.Id), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}
}
