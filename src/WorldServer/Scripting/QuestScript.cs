// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Player;
using Aura.World.World;
using Aura.World.Events;
using Aura.World.Network;

namespace Aura.World.Scripting
{
	public partial class QuestScript : BaseScript
	{
		public QuestInfo Info = new QuestInfo();
		public Receive ReceiveMethod = Receive.Manually;

		public uint Id { get { return this.Info.Class; } }

		public void SetId(uint id)
		{
			this.Info.Class = id;
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

			if (this.ReceiveMethod == Receive.OnLogin)
				ServerEvents.Instance.PlayerLoggedIn += this.OnPlayerLoggedIn;
		}

		public override void Dispose()
		{
			base.Dispose();

			// Just remove the subscribtions, no problem if they
			// weren't subscribed to them.
			ServerEvents.Instance.PlayerLoggedIn -= this.OnPlayerLoggedIn;
			ServerEvents.Instance.KilledByPlayer -= this.OnKilledByPlayer;
			EntityEvents.Instance.CreatureItemAction -= this.OnCreatureItemAction;
		}

		public void OnPlayerLoggedIn(object sender, EventArgs args)
		{
			if (this.ReceiveMethod == Receive.OnLogin)
			{
				var character = sender as MabiPC;
				if (!this.HasQuest(character, this.Info.Class))
				{
					var quest = new MabiQuest(this.Info.Class);
					character.Quests[this.Info.Class] = quest;
					WorldManager.Instance.CreatureReceivesQuest(character, quest);
				}
			}
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
			var quest = character.GetQuestOrNull(this.Info.Class);
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

			WorldManager.Instance.CreatureUpdateQuest(character, quest);
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
			var quest = character.GetQuestOrNull(this.Info.Class);
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

			WorldManager.Instance.CreatureUpdateQuest(character, quest);
		}

		public void AddObjective(string ident, string description, uint region, uint x, uint y, QuestObjectiveInfo info)
		{
			this.AddObjective(ident, description, false, region, x, y, info);
		}

		public void AddObjective(string ident, string description, bool unlocked, uint region, uint x, uint y, QuestObjectiveInfo info)
		{
			info.Description = description;
			info.Unlocked = unlocked;

			info.Region = region;
			info.X = x;
			info.Y = y;

			// Subscribe to KilledByPlayer once for this quest, if needed for this objective.
			if (info.Type == ObjectiveType.Kill)
			{
				ServerEvents.Instance.KilledByPlayer -= this.OnKilledByPlayer;
				ServerEvents.Instance.KilledByPlayer += this.OnKilledByPlayer;
			}

			// Subscribe to OnCreatureItemAction once for this quest, if needed for this objective.
			if (info.Type == ObjectiveType.Collect)
			{
				EntityEvents.Instance.CreatureItemAction -= this.OnCreatureItemAction;
				EntityEvents.Instance.CreatureItemAction += this.OnCreatureItemAction;
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

		public virtual void OnCompleted(WorldClient client, MabiQuest quest)
		{ }
	}

	public enum Receive : byte { Manually, OnLogin }
}
