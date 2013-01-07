// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Runtime.InteropServices;
using Common.Constants;
using Common.Data;
using Common.Tools;

namespace Common.World
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SkillInfo
	{
		public ushort Id;
		public ushort Version;
		public byte Rank;
		public byte MaxRank;
		private byte __unknown6;
		private byte __unknown7;
		public int Experience;
		public ushort Count;
		public ushort Flag;
		public long LastPromotionTime;
		public ushort PromotionCount;
		private ushort __unknown26;
		public int PromotionExp;
		public ushort ConditionCount1;
		public ushort ConditionCount2;
		public ushort ConditionCount3;
		public ushort ConditionCount4;
		public ushort ConditionCount5;
		public ushort ConditionCount6;
		public ushort ConditionCount7;
		public ushort ConditionCount8;
		public ushort ConditionCount9;
		private ushort __unknown50;
	}

	public class MabiSkill
	{
		public SkillInfo Info;
		public SkillRankInfo RankInfo = new SkillRankInfo();

		private readonly uint _race;

		public SkillConst Id { get { return (SkillConst)this.Info.Id; } }
		public SkillRank Rank { get { return (SkillRank)this.Info.Rank; } }

		//public MabiItem ActiveItem { get; set; }
		//public MabiCreature ActiveTarget { get; set; }
		//public byte ActiveStacks { get; set; }

		public bool IsRankable
		{
			get
			{
				return this.Info.Experience >= 100000;
			}
		}

		public MabiSkill(ushort skillId, byte rank, uint race = 10001)
		{
			this.Info.Id = skillId;
			this.Info.Rank = rank;
			this.Info.MaxRank = (byte)SkillRank.R1;
			this.Info.Flag = (ushort)(SkillFlags.Default | SkillFlags.Shown);

			// Set to 1, so we don't get the perfect training msg
			// all the time. Calculated in reverse, count is set to
			// the max training and reduced afterwards (good idea devCat...).
			this.Info.ConditionCount1 = 1;
			this.Info.ConditionCount2 = 1;
			this.Info.ConditionCount3 = 1;
			this.Info.ConditionCount4 = 1;
			this.Info.ConditionCount5 = 1;
			this.Info.ConditionCount6 = 1;
			this.Info.ConditionCount7 = 1;
			this.Info.ConditionCount8 = 1;
			this.Info.ConditionCount9 = 1;

			_race = 10001;

			this.LoadRankInfo();
		}

		public MabiSkill(SkillConst skill, SkillRank rank = SkillRank.Novice, uint race = 10001)
			: this((ushort)skill, (byte)rank, race)
		{
		}

		public void LoadRankInfo()
		{
			var skillInfo = MabiData.SkillDb.Find(this.Info.Id);
			if (skillInfo == null)
			{
				Logger.Warning("Unknown skill '" + this.Info.Id.ToString() + "'.");
				return;
			}

			var rankInfo = skillInfo.GetRankInfo(this.Info.Rank, _race);
			if (rankInfo == null)
			{
				Logger.Warning("Unable to find info for skill '" + this.Info.Id.ToString() + "', rank '" + this.Info.Rank.ToString() + "'.");

				if (skillInfo.RankInfo.Count < 1)
					return;

				rankInfo = skillInfo.RankInfo[0];
				Logger.Warning("Using the first found rank info instead.");
			}

			this.RankInfo = rankInfo;
		}
	}
}
