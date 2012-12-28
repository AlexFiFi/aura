using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_TaraScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Tara");
		SetRace(990019);

		SetLocation(region: 401, x: 109136, y: 94295);

		SetDirection(194);
	}
}
