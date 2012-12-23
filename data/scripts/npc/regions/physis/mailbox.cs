using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_IriaPhysisScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_IriaPhysis");
		SetRace(990020);

		SetLocation(region: 3200, x: 287527, y: 222681);

		SetDirection(222);
	}
}
