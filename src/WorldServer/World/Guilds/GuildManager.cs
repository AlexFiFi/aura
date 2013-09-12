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
using System.Text.RegularExpressions;

namespace Aura.World.World.Guilds
{
	public static class GuildManager
	{
		public static bool CreateGuild(string name, GuildType type, MabiCreature leader, IEnumerable<MabiCreature> otherMembers)
		{
			if (WorldDb.Instance.GetGuildForChar(leader.Id) != null)
			{
				Send.MsgBox(leader.Client, leader, Localization.Get("guild.already_you")); // You are already a member of a guild
				return false;
			}

			foreach (var member in otherMembers)
			{
				if (WorldDb.Instance.GetGuildForChar(member.Id) != null)
				{
					Send.MsgBox(leader.Client, leader, Localization.Get("guild.already"), member.Name); // {0} is already a member of a guild
					return false;
				}
			}

			if (!WorldDb.Instance.GuildNameOkay(name))
			{
				Send.MsgBox(leader.Client, leader, Localization.Get("guild.name_used")); // That name is not valid or is already in use.
				return false;
			}

			// TODO: checks in here...

			var pos = leader.GetPosition();

			var guild = new MabiGuild();
			guild.IntroMessage = string.Format(Localization.Get("guild.intro"), name);         // Guild stone for the {0} guild
			guild.LeavingMessage = string.Format(Localization.Get("guild.leaving"), name);	   // You have left the {0} guild
			guild.RejectionMessage = string.Format(Localization.Get("guild.rejection"), name); // You have been denied admission to the {0} guild.
			guild.WelcomeMessage = string.Format(Localization.Get("guild.welcome"), name);	   // Welcome to the {0} guild!
			guild.Name = name;
			guild.Region = leader.Region;
			guild.X = pos.X;
			guild.Y = pos.Y;
			guild.Rotation = leader.Direction;
			guild.StoneClass = GuildStoneType.Normal;
			guild.Type = type;

			var guildId = guild.Save();

			leader.GuildMember = new MabiGuildMember(leader.Id, guildId, GuildMemberRank.Leader);
			leader.GuildMember.Save();
			foreach (var member in otherMembers)
			{
				member.GuildMember = new MabiGuildMember(member.Id, guildId, GuildMemberRank.SeniorMember);
				member.GuildMember.Save();
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

			Send.ChannelNotice(NoticeType.Top, 20000, Localization.Get("guild.created"), name, leader.Name); // {0} Guild has been created. Guild leader: {1}

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
			var match = Regex.Match(p.ExtraData, "guildid=\"([0-9]+)\"");
			if (!match.Success)
				return;

			var guildId = ulong.Parse(match.Groups[1].Value);

			var guild = WorldDb.Instance.GetGuild(guildId);
			if (guild != null)
			{
				if (creature.Guild != null)
				{
					if (guild.Id == creature.Guild.Id && creature.GuildMember.MemberRank < GuildMemberRank.Applied)
					{
						client.Send(new MabiPacket(Op.OpenGuildPanel, creature.Id).PutLong(guild.Id).PutBytes(0, 0, 0));
					}
					else
					{
						client.Send(
							new MabiPacket(Op.GuildInfo, creature.Id)
							.PutLong(guild.Id)
							.PutString(guild.Name)
							.PutString(guild.LeaderName)
							.PutInt(CountAcceptedMembers(guild.Id))
							.PutString(guild.IntroMessage)
						);
					}
				}
				else
				{
					client.Send(
						new MabiPacket(Op.GuildInfoNoGuild, creature.Id)
						.PutLong(guild.Id)
						.PutStrings(guild.Name)
						.PutStrings(guild.LeaderName)
						.PutInt(CountAcceptedMembers(guild.Id))
						.PutString(guild.IntroMessage)
					);
				}
			}
		}

		public static List<MabiGuildMember> GetMembers(ulong guildId)
		{
			return WorldDb.Instance.GetGuildMembers(guildId);
		}

		public static uint CountAcceptedMembers(ulong guildId)
		{
			return (uint)GetMembers(guildId).Count(member => member.MemberRank < GuildMemberRank.Applied);
		}
	}
}
