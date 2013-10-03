// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;

namespace Aura.Login.Database
{
	public class Skill
	{
		public ushort Id { get; set; }
		public byte Rank { get; set; }

		public Skill(SkillConst id, SkillRank rank = SkillRank.Novice)
		{
			this.Id = (ushort)id;
			this.Rank = (byte)rank;
		}
	}
}
