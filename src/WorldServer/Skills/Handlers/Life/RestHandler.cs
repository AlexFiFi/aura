// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using System;
using Aura.Data;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.Rest)]
	public class RestHandler : StartStopTagsSkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			ulong chairOId = 0;
			if (tags.Has("ITEMID"))
				chairOId = (ulong)tags.Get("ITEMID");

			if (chairOId > 0)
			{
				// Check item
				var item = creature.GetItem(chairOId);
				if (item != null && item.Type == ItemType.Misc)
				{
					// Get chair prop id
					var propId = 0u;
					var chairInfo = MabiData.ChairDb.Find(item.Info.Class);
					if (chairInfo != null)
						propId = (!creature.IsGiant ? chairInfo.PropId : chairInfo.GiantPropId);

					var pos = creature.GetPosition();

					// Effect
					if (chairInfo.Effect != 0)
						Send.Effect(chairInfo.Effect, creature, true);

					// Chair prop
					var prop = new MabiProp(propId, creature.Region, pos.X, pos.Y, MabiMath.DirToRad(creature.Direction));
					prop.State = "stand";
					WorldManager.Instance.AddProp(prop);

					// Move char
					Send.AssignChair(creature, prop.Id, 1);

					// Update chair
					prop.ExtraData = string.Format("<xml OWNER='{0}' SITCHAR='{0}'/>", creature.Id);
					Send.PropUpdate(prop);

					creature.Temp.CurrentChair = chairInfo;
					creature.Temp.SittingProp = prop;
				}
			}

			creature.State |= CreatureStates.SitDown;
			Send.SitDown(creature);

			SkillHelper.GiveSkillExp(creature, skill, 20);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill, MabiTags tags)
		{
			creature.State &= ~CreatureStates.SitDown;
			Send.StandUp(creature);

			if (creature.Temp.SittingProp != null)
			{
				// Effect
				if (creature.Temp.CurrentChair.Effect != 0)
					Send.Effect(Effect.CherryBlossoms, creature, false);

				// Update chair
				creature.Temp.SittingProp.ExtraData = string.Format("<xml OWNER='0' SITCHAR='0'/>");
				Send.PropUpdate(creature.Temp.SittingProp);

				Send.AssignChair(creature, 0, 0);

				// Remove chair in 1s
				creature.Temp.SittingProp.DisappearTime = DateTime.Now.AddSeconds(1);

				creature.Temp.SittingProp = null;
			}

			return SkillResults.Okay;
		}
	}
}
