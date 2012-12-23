using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_AScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hidden_wardens_A");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 150, eyeColor: 28, lip: 26);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x576869, 0x9355, 0x18B2D1);
		EquipItem(Pocket.Hair, 0x1F46, 0x1F3C1B, 0x1F3C1B, 0x1F3C1B);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 61500, y: 41200);

		SetDirection(17);
		SetStand("");
	}
}
