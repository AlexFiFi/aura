using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_Housing_SenMagScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Housing_SenMag");
		SetRace(990019);

		SetLocation(region: 202, x: 42172, y: 49415);

		SetDirection(202);
	}
}
