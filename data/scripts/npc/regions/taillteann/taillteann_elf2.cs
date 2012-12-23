using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_elf2Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_elf2");
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 43, eyeColor: 31, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0x55146B, 0x7B344A, 0x8D0058);
		EquipItem(Pocket.Hair, 0x1775, 0xFFC7C6, 0xFFC7C6, 0xFFC7C6);
		EquipItem(Pocket.Armor, 0x3BEE, 0x647692, 0x78829D, 0xBFBFBF);
		EquipItem(Pocket.RightHand1, 0x9D20, 0xD8B77E, 0x0, 0x0);
		EquipItem(Pocket.RightHand2, 0x9D33, 0xFFFFFF, 0x5D4519, 0x5D5537);

		SetLocation(region: 300, x: 226962, y: 193788);

		SetDirection(150);
		SetStand("");
	}
}
