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
		/// <summary>
		/// Fills stack and sends update.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skill"></param>
		public static void FillStack(MabiCreature creature, MabiSkill skill)
		{
			IncStack(creature, skill, skill.RankInfo.StackMax);
		}

		/// <summary>
		/// Increases stack and sends update.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skill"></param>
		/// <param name="amount"></param>
		public static void IncStack(MabiCreature creature, MabiSkill skill, byte amount = 1)
		{
			if (creature.ActiveSkillStacks + amount > skill.RankInfo.StackMax)
				creature.ActiveSkillStacks = skill.RankInfo.StackMax;
			else
				creature.ActiveSkillStacks += amount;
			Send.SkillStackSet(creature.Client, creature, skill.Id, creature.ActiveSkillStacks);
		}

		/// <summary>
		/// Removes stack and sends update.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skill"></param>
		public static void ClearStack(MabiCreature creature, MabiSkill skill)
		{
			DecStack(creature, skill, byte.MaxValue);
		}

		/// <summary>
		/// Decreases stack and sends update.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skill"></param>
		/// <param name="amount"></param>
		public static void DecStack(MabiCreature creature, MabiSkill skill, byte amount = 1)
		{
			if (creature.ActiveSkillStacks > amount)
				creature.ActiveSkillStacks -= amount;
			else
				creature.ActiveSkillStacks = 0;
			Send.SkillStackUpdate(creature.Client, creature, skill.Id, creature.ActiveSkillStacks);
		}

		public static void GiveSkillExp(MabiCreature creature, MabiSkill skill, float exp)
		{
			if (skill.Info.Experience < 100000)
			{
				skill.Info.Experience += (int)exp * 1000;
				if (skill.IsRankable)
					skill.Info.Flag |= (ushort)SkillFlags.Rankable;
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

		/// <summary>
		/// Calculates the target ID for a particular area. Used in Fireball, Icespear, WM, others?
		/// </summary>
		public static ulong GetAreaTargetID(uint region, uint x, uint y)
		{
			return 0x3000000000000000 + ((ulong)region << 32) + ((x / 20) << 16) + (y / 20);
		}

		public static void GetAreaTargetComponents(ulong areaTarget, out uint region, out uint x, out uint y)
		{
			areaTarget -= 0x3000000000000000;

			y = (uint)(areaTarget & ushort.MaxValue) * 20;
			areaTarget >>= 16;
			x = (uint)(areaTarget & ushort.MaxValue) * 20;
			areaTarget >>= 16;
			region = (uint)areaTarget;
		}
	}
}
