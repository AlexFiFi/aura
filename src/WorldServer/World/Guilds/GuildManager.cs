// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.Database;
using Aura.World.Network;
using Aura.Shared.Util;
using Aura.Shared.Network;

namespace Aura.World.World.Guilds
{
	public static class GuildManager
	{
		public static bool CreateGuild(string name, GuildType type, MabiCreature leader, IEnumerable<MabiCreature> otherMembers)
		{
			if (WorldDb.Instance.GetGuildForChar(leader.Id) != null)
			{
				Send.MsgBox(leader.Client, leader, "You are already a member of a guild");
				return false;
			}

			foreach (var member in otherMembers)
			{
				if (WorldDb.Instance.GetGuildForChar(member.Id) != null)
				{
					Send.MsgBox(leader.Client, leader, "{0} is already a member of a guild", member.Name);
					return false;
				}
			}

			if (!WorldDb.Instance.GuildNameOkay(name))
			{
				Send.MsgBox(leader.Client, leader, "That name is not valid or is already in use.");
				return false;
			}

			// TODO: checks in here...

			var pos = leader.GetPosition();

			var guild = new MabiGuild();
			guild.IntroMessage = "Guild stone for the " + name + " guild";
			guild.LeavingMessage = "You have left the " + name + " guild";
			guild.RejectionMessage = "You have been denied admission to the " + name + " guild.";
			guild.WelcomeMessage = "Welcome to the " + name + " guild!";
			guild.Name = name;
			guild.Region = leader.Region;
			guild.X = pos.X;
			guild.Y = pos.Y;
			guild.Rotation = leader.Direction;
			guild.StoneClass = GuildStoneType.Normal;
			guild.Type = type;

			var guildId = guild.Save();

			leader.GuildMemberInfo = new MabiGuildMemberInfo(leader.Id, GuildMemberRank.Leader);
			WorldDb.Instance.SaveGuildMember(leader.GuildMemberInfo, guildId);
			foreach (var member in otherMembers)
			{
				member.GuildMemberInfo = new MabiGuildMemberInfo(member.Id, GuildMemberRank.SeniorMember);
				WorldDb.Instance.SaveGuildMember(member.GuildMemberInfo, guildId);
			}

			// Reload guild to make sure it gets initialized and gets an id
			guild = WorldDb.Instance.GetGuild(guildId);

			leader.Guild = guild;

			WorldManager.Instance.Broadcast(PacketCreator.GuildMembershipChanged(guild, leader, GuildMemberRank.Leader), SendTargets.Range, leader);

			foreach (var member in otherMembers)
			{
				member.Guild = guild;
				WorldManager.Instance.Broadcast(PacketCreator.GuildMembershipChanged(guild, member, GuildMemberRank.SeniorMember), SendTargets.Range, member);
			}

			AddGuildStone(guild);

			Send.ChannelNotice(NoticeType.Top, 20000, "{0} Guild has been created. Guild leader: {1}", name, leader.Name);

			return true;
		}

		// TODO: Make it a script.
		public static void LoadGuildStones()
		{
			var guilds = WorldDb.Instance.LoadGuilds();

			foreach (var guild in guilds)
			{
				AddGuildStone(guild);
			}

			Logger.ClearLine();
			Logger.Info("Done loading {0} guilds.", guilds.Count);
		}

		public static void AddGuildStone(MabiGuild guild)
		{
			var extra = string.Format("<xml guildid=\"{0}\"{1}/>", guild.Id, guild.HasOption(GuildOptionFlags.Warp) ? " gh_warp=\"true\"" : "");
			var prop = new MabiProp("", guild.Name, extra, (uint)guild.StoneClass, guild.Region, guild.X, guild.Y, guild.Rotation);
			
			WorldManager.Instance.AddProp(prop);
			WorldManager.Instance.SetPropBehavior(new MabiPropBehavior(prop, GuildstoneTouch));
		}

		private static void GuildstoneTouch(WorldClient client, MabiCreature creature, MabiProp p)
		{
			// TODO: Better way to get this ID... Pake could be used to fake it
			string gid = p.ExtraData.Substring(p.ExtraData.IndexOf("guildid=\""));
			gid = gid.Substring(9);
			gid = gid.Substring(0, gid.IndexOf("\""));
			ulong bid = ulong.Parse(gid);

			var g = WorldDb.Instance.GetGuild(bid);
			if (g != null)
			{
				if (creature.Guild != null)
				{
					if (g.Id == creature.Guild.Id && creature.GuildMemberInfo.MemberRank < GuildMemberRank.Applied)
						client.Send(new MabiPacket(Op.OpenGuildPanel, creature.Id).PutLong(g.Id).PutBytes(0, 0, 0)); // 3 Unknown bytes...
					else
						client.Send(new MabiPacket(Op.GuildInfo, creature.Id).PutLong(g.Id).PutStrings(g.Name, g.LeaderName)
							.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < GuildMemberRank.Applied))
							.PutString(g.IntroMessage));
				}
				else
					client.Send(new MabiPacket(Op.GuildInfoNoGuild, creature.Id).PutLong(g.Id).PutStrings(g.Name, g.LeaderName)
						.PutInt((uint)WorldDb.Instance.GetGuildMemberInfos(g).Count(m => m.MemberRank < GuildMemberRank.Applied))
						.PutString(g.IntroMessage));
			}
		}
	}
}
