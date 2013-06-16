// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Player;
using Aura.World.World;
using Aura.World.Events;
using Aura.World.Network;
using System.Collections.Generic;

namespace Aura.World.Scripting
{
	public partial class QuestScript : BaseScript
	{
		public QuestInfo Info = new QuestInfo();
		public Receive ReceiveMethod = Receive.Manually;

		public List<QuestPrerequisite> Prerequisites = new List<QuestPrerequisite>();

		public uint Id { get { return this.Info.Id; } }

		public void SetId(uint id)
		{
			this.Info.Id = id;
		}

		public void SetName(string name)
		{
			this.Info.Name = name;
		}

		public void SetDescription(string description)
		{
			this.Info.Description = description;
		}

		public void SetInfo(string info)
		{
			this.Info.AdditionalInfo = info;
		}

		public void SetReceive(Receive val)
		{
			this.ReceiveMethod = val;
		}

		public void SetCancelable()
		{
			this.Info.Cancelable = true;
		}

		public override void OnLoadDone()
		{
			if (this.Info.Objectives.Count > 0)
				(this.Info.Objectives[0] as QuestObjectiveInfo).Unlocked = true;

			if (this.ReceiveMethod == Receive.Auto)
				EventManager.Instance.PlayerEvents.PlayerLoggedIn += this.OnPlayerLoggedIn;
		}

		public override void Dispose()
		{
			base.Dispose();

			// Just remove the subscriptions, no problem if they
			// weren't subscribed to them.
			EventManager.Instance.PlayerEvents.PlayerLoggedIn -= this.OnPlayerLoggedIn;
			EventManager.Instance.PlayerEvents.KilledByPlayer -= this.OnKilledByPlayer;
			EventManager.Instance.CreatureEvents.CreatureItemAction -= this.OnCreatureItemAction;
		}

		public void OnPlayerLoggedIn(object sender, PlayerEventArgs args)
		{
			var character = args.Player;

			//if (this.ReceiveMethod == Receive.Auto)
			//{
			//    if (!this.HasQuest(character, this.Id))
			//    {
			//        var quest = new MabiQuest(this.Id);
			//        character.Quests[this.Id] = quest;
			//        WorldManager.Instance.CreatureReceivesQuest(character, quest);
			//    }
			//}

			// Check prerequisits, for new quests, or ones that aren't given
			// automatically upon completion of the previous one.
			this.CheckPrerequisites(character);
		}

		/// <summary>
		/// Looks for follow up quests and starts them,
		/// if the prerequisites are met.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="quest"></param>
		public virtual void OnCompleted(WorldClient client, MabiQuest quest)
		{
			var character = client.Character as MabiPC;

			foreach (var followUp in ScriptManager.Instance.GetFollowUpQuestScripts(this.Id))
				followUp.CheckPrerequisites(character);
		}

		/// <summary>
		/// Checks all prerequisites, and starts the quest if all are met.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool CheckPrerequisites(MabiPC character)
		{
			if (this.HasQuest(character, this.Id))
				return false;

			foreach (var p in this.Prerequisites)
			{
				if (!p.Met(character))
					return false;
			}

			this.StartQuest(character, this.Id);

			return true;
		}

		/// <summary>
		/// Handles objective updates of type Kill.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void OnKilledByPlayer(object sender, EventArgs args)
		{
			var ea = args as CreatureKilledEventArgs;
			var character = ea.Killer as MabiPC;

			// Has quest?
			var quest = character.GetQuestOrNull(this.Id);
			if (quest == null)
				return;

			// Quest done?
			var progress = quest.CurrentProgress;
			if (progress == null)
				return;

			var objective = this.Info.Objectives[progress.Objective] as QuestObjectiveInfo;

			// Kill objective?
			if (objective.Type != ObjectiveType.Kill)
				return;

			// Correct monster?
			if (!objective.Races.Contains(ea.Victim.Race))
				return;

			// Target amount reached?
			if (progress.Count >= objective.Amount)
				return;

			progress.Count++;

			if (progress.Count >= objective.Amount)
				quest.SetObjectiveDone(progress.Objective);

			character.UpdateQuest(quest);
		}

		/// <summary>
		/// Handles objective updates of type Collect.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void OnCreatureItemAction(object sender, EventArgs args)
		{
			var ea = args as ItemActionEventArgs;
			var character = sender as MabiPC;

			// Sender actually a player?
			if (character == null)
				return;

			// Has quest?
			var quest = character.GetQuestOrNull(this.Id);
			if (quest == null)
				return;

			// Quest done?
			var progress = quest.CurrentProgress;
			if (progress == null)
			{
				// If there's only one objective we'll let this one pass,
				// and check the objective type of that one objective.
				// For example, Church PTJ only has one collect objective,
				// and we have to be able to "un-done" the quest,
				// if required items are dropped or something.
				if (quest.Progresses.Count == 1)
					progress = quest.Progresses[0] as MabiQuestProgress;
				else
					return;
			}

			var objective = this.Info.Objectives[progress.Objective] as QuestObjectiveInfo;

			// Collect objective?
			if (objective.Type != ObjectiveType.Collect)
				return;

			// Correct item?
			if (objective.Id != ea.Class)
				return;

			progress.Count = character.CountItem(objective.Id);

			if (!progress.Done && progress.Count >= objective.Amount)
			{
				if (quest.Progresses.Count > 1)
					progress.Count = objective.Amount;
				quest.SetObjectiveDone(progress.Objective);
			}
			if (progress.Done && progress.Count < objective.Amount)
			{
				quest.SetObjectiveUndone(progress.Objective);
			}

			character.UpdateQuest(quest);
		}

		public void AddObjective(string ident, string description, uint region, uint x, uint y, QuestObjectiveInfo info)
		{
			this.AddObjective(ident, description, false, region, x, y, info);
		}

		public void AddObjective(string ident, string description, bool unlocked, uint region, uint x, uint y, QuestObjectiveInfo info)
		{
			foreach (string o in this.Info.Objectives.Keys)
			{
				if (o == ident)
				{
					Logger.Warning("Multiple objectives with the same name ({1}) found in quest '{0}'. All but the first one will be ignored.", this.Id, ident);
					return;
				}
			}

			info.Description = description;
			info.Unlocked = unlocked;

			info.Region = region;
			info.X = x;
			info.Y = y;

			// Subscribe to KilledByPlayer once for this quest, if needed for this objective.
			if (info.Type == ObjectiveType.Kill)
			{
				EventManager.Instance.PlayerEvents.KilledByPlayer -= this.OnKilledByPlayer;
				EventManager.Instance.PlayerEvents.KilledByPlayer += this.OnKilledByPlayer;
			}

			// Subscribe to OnCreatureItemAction once for this quest, if needed for this objective.
			if (info.Type == ObjectiveType.Collect)
			{
				EventManager.Instance.CreatureEvents.CreatureItemAction -= this.OnCreatureItemAction;
				EventManager.Instance.CreatureEvents.CreatureItemAction += this.OnCreatureItemAction;
			}

			this.Info.Objectives.Add(ident, info);
		}

		public void AddReward(QuestRewardInfo info)
		{
			this.AddReward(0, info);
		}

		public void AddReward(byte group, QuestRewardInfo info)
		{
			info.Group = group;

			this.Info.Rewards.Add(info);
		}

		public void AddPrerequisite(QuestPrerequisite prereq)
		{
			this.Prerequisites.Add(prereq);
		}
	}

	public enum Receive : byte { Manually, Auto }
}
