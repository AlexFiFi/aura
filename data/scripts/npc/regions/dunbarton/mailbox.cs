using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_DunbartonScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Dunbarton");
		SetRace(990019);

		SetLocation(region: 14, x: 35745, y: 37086);

		SetDirection(0);
	}
}
