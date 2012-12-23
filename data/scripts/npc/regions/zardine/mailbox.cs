using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_IriaZardineScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_IriaZardine");
		SetRace(990019);

		SetLocation(region: 3400, x: 329447, y: 176173);

		SetDirection(178);
	}
}
