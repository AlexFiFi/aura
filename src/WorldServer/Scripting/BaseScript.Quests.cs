// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Tools;
using Common.World;
using World.Network;
using World.World;
using Common.Data;

namespace World.Scripting
{
	/// <summary>
	/// Quest methods for all scripts. Located in Based because it might be
	/// needed for stuff like Props.
	/// </summary>
	public partial class BaseScript : IDisposable
	{
		/// <summary>
		/// Returns true if the character has the quest.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected bool HasQuest(WorldClient client, uint id)
		{ return (this.HasQuest(client.Character as MabiPC, id)); }

		protected bool HasQuest(MabiPC character, uint id)
		{ return character.Quests.ContainsKey(id); }

		/// <summary>
		/// Returns true if quest is active and not done.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		protected bool QuestActive(WorldClient client, uint id)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null)
				return false;
			return (quest.State == MabiQuestState.Active);
		}

		/// <summary>
		/// Returns true if the character is done with the quest.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		protected bool QuestDone(WorldClient client, uint id)
		{
			MabiQuest quest = null;
			(client.Character as MabiPC).Quests.TryGetValue(id, out quest);
			if (quest == null)
				return false;
			return quest.IsDone;
		}

		/// <summary>
		/// Completes the given quest.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		protected void CompleteQuest(WorldClient client, uint id)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null)
				return;

			quest.State = MabiQuestState.Complete;
			WorldManager.Instance.CreatureCompletesQuest(client.Character, quest, true);
		}

		/// <summary>
		/// Returns true if the character is done with the quest, and has completed it.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		protected bool QuestCompleted(WorldClient client, uint id)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null || quest.State < MabiQuestState.Complete)
				return false;
			return true;
		}

		/// <summary>
		/// Returns true if character has completed the given objective.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <param name="objective"></param>
		/// <returns></returns>
		protected bool QuestObjectiveDone(WorldClient client, uint id, string objective)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null)
				return false;
			if (!quest.Progresses.ContainsKey(objective))
			{
				Logger.Warning("Quest '{0}' doesn't have an objective called '{1}'.", id, objective);
				return false;
			}
			return quest.Progresses[objective].Done;
		}

		/// <summary>
		/// Sets the given objective done.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <param name="objective"></param>
		protected void FinishQuestObjective(WorldClient client, uint id, string objective)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null)
				return;
			if (!quest.Progresses.ContainsKey(objective))
			{
				Logger.Warning("Quest '{0}' doesn't have an objective called '{1}'.", id, objective);
				return;
			}
			quest.SetObjectiveDone(objective);
			WorldManager.Instance.CreatureUpdateQuest(client.Character, quest);
		}

		/// <summary>
		/// Returns true if the current objective is the given one.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <param name="objective"></param>
		/// <returns></returns>
		protected bool QuestObjectiveIs(WorldClient client, uint id, string objective)
		{
			var quest = (client.Character as MabiPC).GetQuestOrNull(id);
			if (quest == null)
				return false;
			return quest.CurrentObjectiveIs(objective);
		}

		/// <summary>
		/// Starts a quest. Yep. That's it.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		public void StartQuest(WorldClient client, uint id)
		{
			if (this.HasQuest(client, id))
			{
				Logger.Warning("Trying to start quest '{0}' for '{1}' twice.", id, client.Character.Name);
				return;
			}

			// Check here, before we add a quest that doesn't even exist.
			if (!MabiData.QuestDb.Exists(id))
			{
				Logger.Warning("Quest '{0}' does not exist.", id);
				return;
			}

			var quest = new MabiQuest(id);
			(client.Character as MabiPC).Quests.Add(id, quest);

			WorldManager.Instance.CreatureReceivesQuest(client.Character, quest);
		}
	}
}
