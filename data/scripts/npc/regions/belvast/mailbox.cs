using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MailBox_BelfastScript : Mailbox
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_MailBox_Belfast");
		SetRace(990019);

		SetLocation(region: 4005, x: 42381, y: 24835);

		SetDirection(34);
	}
}
