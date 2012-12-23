using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_IriaConnousScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_IriaConnous");
		SetRace(990020);

		SetLocation(region: 3100, x: 366122, y: 425133);

		SetDirection(222);
	}
}
