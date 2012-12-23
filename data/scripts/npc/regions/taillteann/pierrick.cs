using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PierrickScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_pierrick");
		SetRace(47);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x660066;
		NPC.ColorC = 0xF6E07D;		

		EquipItem(Pocket.RightHand1, 0x9C6E, 0xBB2222, 0x38961F, 0x0);

		SetLocation(region: 300, x: 221497, y: 191138);

		SetDirection(216);
		SetStand("");
	}
}
