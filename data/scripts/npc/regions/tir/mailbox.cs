using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_TirChonaillScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_TirChonaill");
		SetRace(990019);
		SetLocation(region: 1, x: 11351, y: 39397);

		SetDirection(222);
	}
}
