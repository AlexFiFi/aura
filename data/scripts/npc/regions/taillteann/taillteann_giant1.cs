using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_giant1Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_giant1");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 23, eye: 45, eyeColor: 27, lip: 28);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x670061, 0x840F69, 0xCF9461);
		EquipItem(Pocket.Hair, 0x1F51, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		SetLocation(region: 300, x: 205354, y: 191114);

		SetDirection(7);
		SetStand("");
	}
}
