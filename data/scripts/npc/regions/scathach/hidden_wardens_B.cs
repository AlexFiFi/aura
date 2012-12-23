using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_BScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hidden_wardens_B");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 3, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x7ECEC7, 0x1CB1B8, 0x6089);
		EquipItem(Pocket.Hair, 0xFB1, 0x70403E, 0x70403E, 0x70403E);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 75400, y: 38100);

		SetDirection(76);
		SetStand("");
	}
}
