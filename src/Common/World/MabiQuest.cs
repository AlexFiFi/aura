// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Common.Network;
using Common.Data;
using Common.Tools;

namespace Common.World
{
	public class MabiQuestProgress
	{
		public string Objective;
		public uint Count;
		public bool Done;
		public bool Unlocked;

		public MabiQuestProgress(string objective, bool unlockedByDefault)
		{
			this.Objective = objective;
			this.Unlocked = unlockedByDefault;
		}

		public MabiQuestProgress(string objective, uint count, bool done, bool unlocked)
		{
			this.Objective = objective;
			this.Count = count;
			this.Done = done;
			this.Unlocked = unlocked;
		}
	}

	public class MabiQuest
	{
		private static ulong _questIds = Constants.Id.QuestsTmp;

		public ulong Id;
		public uint Class;
		public MabiQuestState State;

		public OrderedDictionary Progresses = new OrderedDictionary();

		/// <summary>
		/// Every quest has an invisible item that goes with it.
		/// The id is always quest id - 0x0010000000000000.
		/// </summary>
		public ulong ItemId { get { return this.Id - Constants.Id.QuestItemOffset; } }

		// Abstract way of accessing the info, but an easy and clean one.
		// Automatic querying of QuestDb with warning msg and caching.
		private QuestInfo _info;
		public QuestInfo Info
		{
			get
			{
				if (_info != null)
					return _info;

				var qi = MabiData.QuestDb.Find(this.Class);
				if (qi == null)
				{
					Logger.Warning("Unknown quest id '{0}'.", this.Class);
					return null;
				}

				return (_info = qi);
			}
		}

		/// <summary>
		/// Returns true if all objectives are done.
		/// </summary>
		public bool IsDone
		{
			get
			{
				int n = 0;
				for (int i = 0; i < this.Progresses.Count; ++i)
				{
					if ((this.Progresses[i] as MabiQuestProgress).Done)
						n++;
				}
				return (n >= this.Progresses.Count);
			}
		}

		/// <summary>
		/// Used in quest items, although seemingly not required.
		/// </summary>
		public string ToolTip
		{
			get
			{
				return string.Format("QSTTIP:s:N_{0}|D_{1}|A_|R_{2}|T_0;", this.Info.Name, this.Info.Description, string.Join(", ", this.Info.Rewards));
			}
		}

		private MabiItem _questitem;
		public MabiItem QuestItem
		{
			get
			{
				if (_questitem != null)
					return _questitem;
				return (_questitem = new MabiItem(this));
			}
		}

		public MabiQuest(uint cls, ulong id = 0)
		{
			this.Class = cls;
			this.Id = (id == 0 ? _questIds++ : id);

			// Loading quest from db, progress comes from there as well.
			if (id > 0)
				return;

			if (this.Info == null)
				return;

			// Fill progress list based on the objectives of this quest.
			foreach (DictionaryEntry de in this.Info.Objectives)
			{
				var key = de.Key as string;
				var val = de.Value as QuestObjectiveInfo;
				this.Progresses.Add(key, new MabiQuestProgress(key, val.Unlocked));
			}
		}

		public MabiQuestProgress CurrentProgress
		{
			get
			{
				// Go through objective progress
				foreach (MabiQuestProgress p in this.Progresses.Values)
				{
					// First not done objective is the current one.
					if (!p.Done)
						return p;
				}

				// Quest done?
				return null;
			}
		}

		public void SetObjectiveDone(string objective)
		{
			for (int i = 0; i < this.Progresses.Count; ++i)
			{
				var p = this.Progresses[i] as MabiQuestProgress;
				if (p.Objective == objective)
				{
					p.Done = true;
					p.Unlocked = false;
					//p.Value.Count = (this.Info.Objectives[p.Key] as QuestObjectiveInfo).Amount;

					// Unlock the next objective.
					if (i + 1 < this.Progresses.Count)
					{
						p = this.Progresses[i + 1] as MabiQuestProgress;
						p.Unlocked = true;
					}
				}
			}
		}

		public void SetObjectiveUndone(string objective)
		{
			for (int i = 0; i < this.Progresses.Count; ++i)
			{
				var p = this.Progresses[i] as MabiQuestProgress;
				if (p.Objective == objective)
				{
					p.Done = false;
					p.Unlocked = true;
					return;
				}
			}
		}

		public void AddData(MabiPacket packet)
		{
			if (this.Info == null)
				return;

			packet.PutLong(this.Id);
			packet.PutByte(0);

			packet.PutLong(this.ItemId);

			packet.PutByte(2); // 0 = blue icon, 7 (shadow? changes structure slightly)

			packet.PutInt(this.Info.Class); // 200076, range important for the tabs.

			packet.PutString(this.Info.Name);
			packet.PutString(this.Info.Description);
			packet.PutString(this.Info.AdditionalInfo);

			packet.PutInt(1);
			packet.PutInt(70024); // Item "Hunting Quest" ?
			packet.PutByte(this.Info.Cancelable);
			packet.PutByte(0);
			packet.PutByte(0); // 1 = blue icon
			packet.PutByte(0);
			packet.PutString(""); // data\gfx\image\gui_temporary_quest.dds
			packet.PutInt(0);     // 4, x y ?
			packet.PutInt(0);
			packet.PutString(""); // <xml soundset="4" npc="GUI_NPCportrait_Lanier"/>
			packet.PutString("QMBEXP:f:1.000000;QMBGLD:f:1.000000;QMSMEXP:f:1.000000;QMSMGLD:f:1.000000;QMAMEXP:f:1.000000;QMAMGLD:f:1.000000;QMBHDCTADD:4:0;QMGNRB:f:1.000000;QMGNRB:f:1.000000;");

			packet.PutInt(0);
			packet.PutInt(0);
			// Alternative, PTJ
			//020 [........00000002] Int    : 2
			//021 [........0000000C] Int    : 12
			//022 [........00000010] Int    : 16
			//023 [........00000015] Int    : 21
			//024 [000039BF89671150] Long   : 63494806770000 // Timestamp

			packet.PutSInt(this.Info.Objectives.Count);
			foreach (DictionaryEntry de in this.Info.Objectives)
			{
				var key = de.Key as string;
				var objective = de.Value as QuestObjectiveInfo;
				var progress = this.Progresses[key] as MabiQuestProgress;

				packet.PutByte((byte)objective.Type);
				packet.PutString(objective.Description);
				packet.PutString(objective.ToString());

				// 3  - TARGECHAR:s:shamala;TARGETCOUNT:4:1; - Ask Shamala about collecting transformations
				// 14 - TARGETITEM:4:40183;TARGETCOUNT:4:1; - Break a nearby tree
				// 1  - TGTSID:s:/brownphysisfoxkid/;TARGETCOUNT:4:10;TGTCLS:2:0; - Hunt 10 Young Brown Physis Foxes
				// 9  - TGTSKL:2:23002;TGTLVL:2:1;TARGETCOUNT:4:1; - Combat Mastery rank F reached
				// 19 - TGTCLS:2:3906;TARGETCOUNT:4:1; - Win at least one match in the preliminaries or the finals of the Jousting Tournament.
				// 18 - TGTCLS:2:3502;TARGETCOUNT:4:1; - Read the Author's Notebook.
				// 4  - TARGECHAR:s:duncan;TARGETITEM:4:75473;TARGETCOUNT:4:1; - Deliver the Author's Notebook to Duncan.
				// 15 - TGTLVL:2:15;TARGETCOUNT:4:1; - Reach Lv. 15
				// 2  - TARGETITEM:4:52027;TARGETCOUNT:4:10;QO_FLAG:4:1; - Harvest 10 Bundles of Wheat
				// 22 - TGTSID:s:/ski/start/;TARGETITEM:4:0;EXCNT:4:0;TGITM2:4:0;EXCNT2:4:0;TARGETCOUNT:4:1; - Click on the Start Flag.
				// 47 - TARGETCOUNT:4:1;TGTCLS:4:730205; - Clear the Monkey Mash Mission.
				// 52 - QO_FLAG:b:true;TARGETCOUNT:4:1; - Collect for the Transformation Diary
				// 50 - TARGETRACE:4:9;TARGETCOUNT:4:1; - Transform into a Kiwi.
				// 54 - TARGETRACE:4:9;TARGETCOUNT:4:1; - Collect Frail Green Kiwi perfectly.

				// Type theory:
				// 1  : Kill x of y
				// 2  : Collect x of y
				// 3  : Talk to x
				// 4  : Bring x to y
				// 9  : Reach rank x on skill y
				// 14 : ?
				// 15 : Reach lvl x
				// 18 : Do something with item x ?
				// 19 : Clear something, like jousting or a dungeon?

				// Progress
				packet.PutInt(progress.Count);
				packet.PutByte(progress.Done);
				packet.PutByte(progress.Unlocked);

				// Target location
				if (objective.Region > 0)
				{
					packet.PutByte(1);
					packet.PutInt(objective.Region);
					packet.PutInt(objective.X);
					packet.PutInt(objective.Y);
				}
				else
				{
					packet.PutByte(0);
				}
			}

			packet.PutByte(1);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(1);

			// Rewards
			packet.PutByte((byte)this.Info.Rewards.Count);
			foreach (var reward in this.Info.Rewards)
			{
				packet.PutByte((byte)reward.Type);
				packet.PutString(reward.ToString());
				packet.PutByte(reward.Group);
				packet.PutByte(1);
				packet.PutByte(1);
			}

			packet.PutByte(0);
		}

		public void AddProgressData(MabiPacket packet)
		{
			packet.PutLong(this.Id);
			packet.PutByte(1);

			packet.PutSInt(this.Progresses.Count);
			foreach (MabiQuestProgress p in this.Progresses.Values)
			{
				packet.PutInt(p.Count);
				packet.PutByte(p.Done);
				packet.PutByte(p.Unlocked);
			}

			packet.PutByte(0);
			packet.PutByte(0);
		}
	}

	public enum MabiQuestState : byte { Active, Complete }
}
