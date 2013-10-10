// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Util;
using Aura.Data;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Reload)]
	public class ReloadHandler : SkillHandler
	{
		private const string BulletCountTag = "GVBC";

		// TODO: Put that somewhere in the db... maybe we need a var system?
		private const ushort ReloadAmount = 64;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			// TODO: Check and reduce bullets in inv.

			// Var1 is the cast time, wth devCat?
			Send.SkillPrepare(creature.Client, creature, skill.Id, (uint)skill.RankInfo.Var1);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			// Gun update
			{
				creature.RightHand.Tags.SetShort(BulletCountTag, ReloadAmount);
				Send.ItemUpdate(creature.Client, creature, creature.RightHand);
			}

			//001 [........00000083] Int    : 131
			//002 [........0000000E] Int    : 14
			//003 [..............00] Byte   : 0
			//004 [............0000] Short  : 0
			Send.UseMotion(creature, 131, 14);

			Send.SkillUse(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}
	}
}
