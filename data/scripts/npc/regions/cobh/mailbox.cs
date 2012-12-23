using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_CobhScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Cobh");
		SetRace(990019);

		SetLocation(region: 23, x: 27657, y: 41599);

		SetDirection(0);
	}
}
