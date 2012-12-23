using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_EmainMachaScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_EmainMacha");
		SetRace(990019);

		SetLocation(region: 52, x: 36147, y: 46147);

		SetDirection(144);
	}
}
