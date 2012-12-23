using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_TaillteannScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Taillteann");
		SetRace(990019);

		SetLocation(region: 300, x: 216819, y: 193700);

		SetDirection(156);
	}
}
