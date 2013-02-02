// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Tools;
using Common.Events;
using Common.World;
using World.World;

namespace World.Scripting
{
	public class QuestScript : BaseScript
	{
		public QuestInfo Info = new QuestInfo();
		public Receive ReceiveMethod = Receive.Manually;

		private bool _killedSubscription = false;
		private bool _itemActionSubscription = false;

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

			if (this.ReceiveMethod == Receive.OnLogin)
				ServerEvents.Instance.PlayerLoggedIn -= this.OnPlayerLoggedIn;

			if (_killedSubscription)
				ServerEvents.Instance.KilledByPlayer -= this.OnKilledByPlayer;

			if (_itemActionSubscription)
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
			if (objective.Id != ea.Victim.Race)
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

		public void AddObjective(string ident, string description, ObjectiveType type, params dynamic[] args)
		{
			this.AddObjective(ident, description, false, type, args);
		}

		/// <summary>
		/// Adds and objective to the quest.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="description"></param>
		/// <param name="unlocked">Whether the objective is visible (not "???") by default.</param>
		public void AddObjective(string ident, string description, bool unlocked, ObjectiveType type, params dynamic[] args)
		{
			var qoi = new QuestObjectiveInfo();
			qoi.Type = type;
			qoi.Description = description;
			qoi.Unlocked = unlocked;

			int i = 0;

			// Args parsing based on type.
			switch (type)
			{
				// ii
				case ObjectiveType.Kill:
				case ObjectiveType.Collect:
				case ObjectiveType.ReachRank:
					if (args.Length < 2)
					{
						ArgErrorLog(type, args.Length, 2, this.ScriptPath);
						return;
					}
					qoi.Id = (uint)args[i++];
					qoi.Amount = (uint)args[i++];
					break;

				// s
				case ObjectiveType.Talk:
					if (args.Length < 1)
					{
						ArgErrorLog(type, args.Length, 1, this.ScriptPath);
						return;
					}
					qoi.Target = args[i++];
					break;

				// si
				case ObjectiveType.Deliver:
					if (args.Length < 2)
					{
						ArgErrorLog(type, args.Length, 2, this.ScriptPath);
						return;
					}
					qoi.Target = args[i++];
					qoi.Id = (uint)args[i++];
					break;

				// i
				case ObjectiveType.ReachLevel:
					if (args.Length < 1)
					{
						ArgErrorLog(type, args.Length, 1, this.ScriptPath);
						return;
					}
					qoi.Id = (uint)args[i++];
					break;

				default:
					Logger.Warning("Unsupported objective type '{0}'.", type);
					return;
			}

			// Subscribe to KilledByPlayer once for this quest, if needed for this objective.
			if (type == ObjectiveType.Kill && !_killedSubscription)
			{
				ServerEvents.Instance.KilledByPlayer += this.OnKilledByPlayer;
				_killedSubscription = true;
			}

			// Subscribe to OnCreatureItemAction once for this quest, if needed for this objective.
			if (type == ObjectiveType.Collect && !_itemActionSubscription)
			{
				EntityEvents.Instance.CreatureItemAction += this.OnCreatureItemAction;
				_itemActionSubscription = true;
			}

			// Check for 3 more args afterwards (target location).
			if (args.Length - i >= 3)
			{
				qoi.Region = (uint)args[i++];
				qoi.X = (uint)args[i++];
				qoi.Y = (uint)args[i++];
			}

			this.Info.Objectives.Add(ident, qoi);
		}

		private void ArgErrorLog(Enum type, int args, int expected, string path)
		{
			Logger.Error("Insufficient amount of paramters for '{4}.{0}' ({1}/{2}) in '{3}'.", type, args, expected, path, type.GetType());
		}

		public void AddReward(RewardType type, params dynamic[] args)
		{
			this.AddReward(0, type, args);
		}

		public void AddReward(byte group, RewardType type, params dynamic[] args)
		{
			var qri = new QuestRewardInfo();
			qri.Type = type;
			qri.Group = group;

			switch (type)
			{
				// ii
				case RewardType.Item:
				case RewardType.Skill:
					if (args.Length < 2)
					{
						ArgErrorLog(type, args.Length, 2, this.ScriptPath);
						return;
					}
					qri.Id = (uint)args[0];
					qri.Amount = (uint)args[1];
					break;

				// i
				case RewardType.Gold:
				case RewardType.Exp:
				case RewardType.ExplExp:
				case RewardType.AP:
					if (args.Length < 1)
					{
						ArgErrorLog(type, args.Length, 1, this.ScriptPath);
						return;
					}
					qri.Amount = (uint)args[0];
					break;

				default:
					Logger.Warning("Unsupported reward type '{0}'.", type);
					return;
			}

			this.Info.Rewards.Add(qri);
		}
	}

	public enum Receive : byte { Manually, OnLogin }
}
