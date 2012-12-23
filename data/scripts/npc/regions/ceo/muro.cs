using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MuroScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_muro");
		SetRace(10105);
		SetBody(height: 0.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 133, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Shoe, 0x426D, 0x441A19, 0x695C66, 0xBADB);
		EquipItem(Pocket.RightHand1, 0x9C47, 0xB3BEA4, 0x745D2F, 0x406C);
		EquipItem(Pocket.LeftHand1, 0xB3B1, 0x624D4F, 0x7F7237, 0x346722);

		SetLocation(region: 56, x: 11035, y: 11011);

		SetDirection(159);
		SetStand("");
	}
}
