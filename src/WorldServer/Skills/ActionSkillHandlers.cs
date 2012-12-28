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
	class UmbrellaSkillHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			try
			{
				WorldManager.Instance.Broadcast(new MabiPacket(Op.OpenUmbrella, creature.Id).PutInt(creature.GetItemInPocket(Pocket.RightHand1, true).Info.Class), SendTargets.Range, creature);
				return SkillResults.Okay;
			}
			catch
			{
				return SkillResults.Failure;
			}
		}
		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CloseUmbrella, creature.Id), SendTargets.Range, creature);
			return SkillResults.Okay;
		}
	}
}
