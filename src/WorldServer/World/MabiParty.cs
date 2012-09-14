// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using Common.Network;
using Common.World;

namespace World.World
{
	public class MabiParty
	{
		public string Name;
		public string Password;
		public uint Type;
		public uint MaxMembers;
		public List<MabiCreature> Members;
		public MabiCreature Leader;

		public MabiParty(string name, string password, uint type, uint maxmembers, MabiCreature leader)
		{
			Name = name;
			Password = password;
			Type = type;
			MaxMembers = maxmembers;
			Leader = leader;
			Members = new List<MabiCreature>();
			Members.Add(leader);
		}

		public string GetMemberWantedString()
		{
			return string.Format("p{0}{1:d2}{2:d2}n{3}", Type, Members.Count, MaxMembers, Name);
		}

		public void AddPartyInfo(MabiPacket packet)
		{
			packet.PutLong(0x0040000600000001); // Party ID
			packet.PutString(Name);
			packet.PutLong(Leader.Id);
			packet.PutByte(1);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutLong(0);
			packet.PutInt(MaxMembers);
			packet.PutInt(Type);
			packet.PutString(Name);
			packet.PutString(""); // Password?
			packet.PutInt((uint)Members.Count);

			uint memberNumber = 1;
			foreach (MabiCreature member in Members)
			{
				packet.PutInt(memberNumber++);
				packet.PutLong(member.Id);
				packet.PutString(member.Name);
				packet.PutByte(1); // ?
				packet.PutInt(member.Region);
				MabiVertex loc = member.GetPosition();
				packet.PutInt(loc.X);
				packet.PutInt(loc.Y);
				packet.PutByte(0); // ?
				packet.PutInt(100); // Health %
				packet.PutInt(100); // Health %
				packet.PutLong(0); // ?
			}

			packet.PutByte(0);
		}
	}
}
