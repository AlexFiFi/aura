// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.Network;
using Aura.Shared.Network;

namespace Aura.World.World
{
	public class MabiCooldown
	{
		public CooldownType Type;
		public DateTime Expires;
		public uint Id;

		/// <summary>
		/// The error message to be shown. Use {0} to insert the time left.
		/// </summary>
		public string ErrorMessage;

		public MabiCooldown(CooldownType type, uint id, DateTime expires, string error)
		{
			this.Type = type;
			this.Id = id;
			this.Expires = expires;
			this.ErrorMessage = error;
		}

		public void SendErrorMessage(MabiCreature creature)
		{
			this.SendErrorMessage(creature.Client);
		}

		public void SendErrorMessage(Client client)
		{
			if (this.ErrorMessage != null)
				Send.Notice(client, NoticeType.MiddleTop, this.ErrorMessage, (this.Expires - DateTime.Now).ToString());
		}
	}

	public enum CooldownType : ushort
	{
		// SqlError = 0,
		Item = 1,
		Skill,
		Quest,
		Mission,
		PartnerAction
	}
}
