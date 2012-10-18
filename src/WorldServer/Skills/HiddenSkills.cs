using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;
using World.Network;
using Common.Network;
using Common.Events;

namespace World.World
{
	public static partial class Skills
	{
		public static SkillResult HiddenResurrectionPrepare(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			return SkillResult.Okay | SkillResult.SuppressFlash; //Suppress the "flashing"
		}
		public static SkillResult HiddenResurrectionUse(MabiCreature creature, MabiEntity target, SkillAction skillAction, MabiSkill skill, uint var1, uint var2)
		{
			creature.ActiveSkillTarget = WorldManager.Instance.GetCreatureById(target.Id);
			if (creature.ActiveSkillTarget == null || !creature.ActiveSkillTarget.IsDead())
			{
				creature.Client.Send(PacketCreator.MsgBox(creature, "Invalid character name or the character is not knocked out."));
				return SkillResult.None;
			}
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(14).PutString("healing_phoenix").PutLong(target.Id), SendTargets.Range, creature);
			return SkillResult.Okay;
		}
		public static SkillResult HiddenResurectionCompleted(MabiCreature creature, MabiSkill skill, string parameters = "")
		{
			if (creature.ActiveSkillItem == null || creature.ActiveSkillItem.Info.Amount < 1)
				return SkillResult.None;

			if (creature.ActiveSkillTarget.IsDead())
			{
				creature.ActiveSkillItem.Info.Amount--;
				creature.Client.Send(PacketCreator.ItemAmount(creature, creature.ActiveSkillItem));

				WorldManager.Instance.CreatureRevive(creature.ActiveSkillTarget);

				creature.Client.Send(new MabiPacket(Op.SkillComplete, creature.Id).PutShort(skill.Info.Id).PutLong(creature.ActiveSkillTarget.Id).PutInts(0, 1));
			}
			else
			{
				creature.Client.Send(PacketCreator.MsgBox(creature, "Invalid character name or the character is not knocked out."));
				return SkillResult.None;				
			}

			return SkillResult.Okay;
		}
	}
}
