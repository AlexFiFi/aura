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
	// Little factory for quest prerequisites. Methods return a
	// QuestPrerequisite, using the given parameters.

	public partial class QuestScript : BaseScript
	{
		protected QuestPrerequisite QuestCompleted(uint questId)
		{
			return new PrerequisiteQuestCompleted(questId);
		}

		protected QuestPrerequisite ReachedLevel(ushort level)
		{
			return new PrerequisiteReachedLevel(level);
		}
	}
}
