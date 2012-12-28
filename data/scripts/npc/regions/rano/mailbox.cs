using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_IriaRanoScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_IriaRano");
		SetRace(990021);

		SetLocation(region: 3001, x: 164133, y: 169740);

		SetDirection(253);
	}
}
