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
	// Little factory for quest rewards. Methods return a partially filled
	// QuestRewardInfo, using the given parameters.

	public partial class QuestScript : BaseScript
	{
		protected QuestRewardInfo Item(uint itemId, uint amount)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.Item;
			result.Id = itemId;
			result.Amount = amount;
			return result;
		}

		protected QuestRewardInfo Skill(SkillConst skillId, SkillRank rank)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.Skill;
			result.Id = (uint)skillId;
			result.Amount = (uint)rank;
			return result;
		}

		protected QuestRewardInfo Gold(uint amount)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.Gold;
			result.Amount = amount;
			return result;
		}

		protected QuestRewardInfo Exp(uint amount)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.Exp;
			result.Amount = amount;
			return result;
		}

		protected QuestRewardInfo ExplExp(uint amount)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.ExplExp;
			result.Amount = amount;
			return result;
		}

		protected QuestRewardInfo AP(uint amount)
		{
			var result = new QuestRewardInfo();
			result.Type = RewardType.AP;
			result.Amount = amount;
			return result;
		}
	}
}
