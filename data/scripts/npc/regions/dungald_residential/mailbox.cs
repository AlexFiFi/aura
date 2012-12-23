using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_Housing_DugaldScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Housing_Dugald");
		SetRace(990019);

		SetLocation(region: 200, x: 20108, y: 18733);

		SetDirection(231);
	}
}
