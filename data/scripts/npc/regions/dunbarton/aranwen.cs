using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AranwenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_aranwen");
		SetRace(10001);
		SetBody(height: 1.15f, fat: 0.9f, upper: 1.1f, lower: 0.8f);
		SetFace(skin: 15, eye: 3, eyeColor: 192, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xBED5EE, 0xF48E71, 0x7E9C);
		EquipItem(Pocket.Hair, 0xBD2, 0xBDC2E5, 0xBDC2E5, 0xBDC2E5);
		EquipItem(Pocket.Armor, 0x32D0, 0xC6D8EA, 0xC6D8EA, 0x635985);
		EquipItem(Pocket.Glove, 0x4077, 0xC6D8EA, 0xB20859, 0xA7131C);
		EquipItem(Pocket.Shoe, 0x4460, 0xC6D8EA, 0xC6D8EA, 0x3F6577);
		EquipItem(Pocket.RightHand1, 0x9C4C, 0xC0C0C0, 0x8C84A4, 0x403C47);

		SetLocation(region: 14, x: 43378, y: 40048);

		SetDirection(125);
		SetStand("");
	}
}