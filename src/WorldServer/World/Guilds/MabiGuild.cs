// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Data;
using Aura.Shared.Const;
using Aura.World.Database;

namespace Aura.World.World.Guilds
{
	public class MabiGuild
	{
		public ulong Id;
		public string Name, LeaderName, Title;

		public string IntroMessage;
		public string WelcomeMessage;
		public string LeavingMessage;
		public string RejectionMessage;

		public GuildLevel GuildLevel;
		public GuildType Type;

		public uint Region;
		public uint X;
		public uint Y;
		public uint Area { get { return MabiData.RegionInfoDb.GetAreaId(this.Region, this.X, this.Y); } }
		public byte Rotation;

		public uint Gp;
		public uint Gold;
		public GuildStoneType StoneClass;

		public byte Options;

		public ulong Save()
		{
			return WorldDb.Instance.SaveGuild(this);
		}

		public bool HasOption(GuildOptionFlags opts)
		{
			return ((byte)opts & Options) != 0;
		}
	}

	public class MabiGuildMemberInfo
	{
		public ulong CharacterId;
		public GuildMemberRank MemberRank;
		public DateTime JoinedDate;
		public double Gp;
		public string ApplicationText;
		public GuildMessageFlags MessageFlags;

		public MabiGuildMemberInfo()
		{
			this.JoinedDate = DateTime.Now;
			this.ApplicationText = string.Empty;
		}

		public MabiGuildMemberInfo(ulong characterId, GuildMemberRank rank)
			: this()
		{
			this.CharacterId = characterId;
			this.MemberRank = rank;
		}

		public bool Has(GuildMessageFlags test)
		{
			return (MessageFlags & test) != 0;
		}
	}

	public enum GuildLevel : byte { Beginner = 0, Basic, Advanced, Great, Grand }
	public enum GuildType : byte { Battle = 0, Adventure, Manufacturing, Commerce, Social, Other }
	public enum GuildMemberRank { Leader = 0, Officer, BronzeCrown, SeniorMember, UknMember, Member, Applied = 254, Declined } /* Leader = 0 Officer = 1 Unknown = 2 Senior = 3 Member = 5*/
	public enum GuildStoneType : uint { Normal = 211, Hope = 40209, Courage = 40210 }

	public enum GuildMessageFlags : byte
	{
		None = 0x0,
		Accepted = 0x1,
		Rejected = 0x2,
		NewApp = 0x04,
		MemberLeft = 0x08,
		RankChanged = 0x10
	}

	public enum GuildOptionFlags : byte
	{
		None = 0x0,
		GuildHall = 0x1,
		Warp = 0x2
	}
}
