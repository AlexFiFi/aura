using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class RabScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_rab");
		SetRace(20);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x404040;
		NPC.ColorC = 0xC0C0C0;		



		SetLocation(region: 53, x: 103263, y: 110129);

		SetDirection(118);
		SetStand("");
	}
}
