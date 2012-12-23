using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_IriaCourcleScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_IriaCourcle");
		SetRace(990021);

		SetLocation(region: 3300, x: 251450, y: 183913);

		SetDirection(250);
	}
}
