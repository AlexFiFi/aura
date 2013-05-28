// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Data;
using Aura.Shared.Const;

namespace Aura.World.Scripting
{
	// Little factory for quest objectives. Methods return a partially filled
	// QuestObjectiveInfo, using the given parameters.

	public partial class QuestScript : BaseScript
	{
		protected QuestObjectiveInfo Kill(uint amount, params uint[] races)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.Kill;
			result.Races.AddRange(races);
			result.Amount = amount;
			return result;
		}

		protected QuestObjectiveInfo Collect(uint itemId, uint amount)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.Collect;
			result.Id = itemId;
			result.Amount = amount;
			return result;
		}

		protected QuestObjectiveInfo ReachRank(SkillConst skillId, SkillRank rank)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.ReachRank;
			result.Id = (uint)skillId;
			result.Amount = (uint)rank;
			return result;
		}

		protected QuestObjectiveInfo Talk(string target)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.Talk;
			result.Target = target;
			return result;
		}

		protected QuestObjectiveInfo Deliver(string target, uint itemId)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.Deliver;
			result.Target = target;
			result.Id = itemId;
			return result;
		}

		protected QuestObjectiveInfo ReachLevel(uint level)
		{
			var result = new QuestObjectiveInfo();
			result.Type = ObjectiveType.ReachLevel;
			result.Id = level;
			return result;
		}
	}
}
