// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.Player;
using Aura.Data;
using Aura.World.World;

namespace Aura.World.Scripting
{
	public abstract class QuestPrerequisite
	{
		public abstract bool Met(MabiPC character);
	}

	public class PrerequisiteQuestCompleted : QuestPrerequisite
	{
		public uint Id { get; protected set; }

		public PrerequisiteQuestCompleted(uint id)
		{
			this.Id = id;
		}

		public override bool Met(MabiPC character)
		{
			var quest = character.GetQuestOrNull(this.Id);
			if (quest == null || quest.State != MabiQuestState.Complete)
				return false;

			return true;
		}
	}

	public class PrerequisiteReachedLevel : QuestPrerequisite
	{
		public ushort Level { get; protected set; }

		public PrerequisiteReachedLevel(ushort level)
		{
			this.Level = level;
		}

		public override bool Met(MabiPC character)
		{
			return (character.Level >= this.Level);
		}
	}

	/// <summary>
	/// Collection of prerequisites, met if all are met.
	/// </summary>
	public class PrerequisiteAnds : QuestPrerequisite
	{
		public List<QuestPrerequisite> Prerequisites { get; protected set; }

		public PrerequisiteAnds(params QuestPrerequisite[] prerequisites)
		{
			this.Prerequisites = new List<QuestPrerequisite>(prerequisites);
		}

		public override bool Met(MabiPC character)
		{
			foreach (var p in this.Prerequisites)
			{
				if (!p.Met(character))
					return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Collection of prerequisites, met if at least one of them is met.
	/// </summary>
	public class PrerequisiteOrs : QuestPrerequisite
	{
		public List<QuestPrerequisite> Prerequisites { get; protected set; }

		public PrerequisiteOrs(params QuestPrerequisite[] prerequisites)
		{
			this.Prerequisites = new List<QuestPrerequisite>(prerequisites);
		}

		public override bool Met(MabiPC character)
		{
			foreach (var p in this.Prerequisites)
			{
				if (p.Met(character))
					return true;
			}

			return false;
		}
	}
}
