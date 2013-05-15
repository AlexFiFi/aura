// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.World.Util;

namespace Aura.World.Skills
{
	public static class SkillHelper
	{
		public static void InitStack(MabiCreature creature, MabiSkill skill)
		{
			creature.ActiveSkillStacks = skill.RankInfo.Stack;
			creature.Client.SendSkillStackSet(creature, skill.Id, creature.ActiveSkillStacks);
		}

		public static void DecStack(MabiCreature creature, MabiSkill skill)
		{
			if (creature.ActiveSkillStacks > 0)
				creature.ActiveSkillStacks--;
			creature.Client.SendSkillStackUpdate(creature, skill.Id, creature.ActiveSkillStacks);
		}

		public static void GiveSkillExp(MabiCreature creature, MabiSkill skill, float exp)
		{
			if (skill.Info.Experience < 100000)
			{
				skill.Info.Experience += (int)exp * 1000;
				if (skill.IsRankable)
					skill.Info.Flag |= (ushort)SkillFlags.Rankable;
				if (creature.Client != null)
					creature.Client.Send(new MabiPacket(Op.SkillTrainingUp, creature.Id).PutBin(skill.Info).PutFloat(exp).PutByte(1).PutString("" /* (Specialized Skill Bonus: x2) */));
			}
		}

		/// <summary>
		/// Adds 1 prof to the given item and sends an update to the
		/// creature's client, if needed.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="item"></param>
		public static void GiveItemExp(MabiCreature creature, MabiItem item)
		{
			//if (item != null && item.OptionInfo.Experience < 10000)
			//{
			//    item.OptionInfo.Experience += WorldConf.ItemExp;
			//    if (item.OptionInfo.Experience > 1000)
			//        item.OptionInfo.Experience = 1000;
			//    creature.Client.Send(PacketCreator.ItemProfUpdate(creature, item));
			//}
		}
	}
}
