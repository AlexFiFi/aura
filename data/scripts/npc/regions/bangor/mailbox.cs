using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_BangorScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Bangor");
		SetRace(990019);

		SetLocation(region: 31, x: 11328, y: 9239);

		SetDirection(0);
	}
}
