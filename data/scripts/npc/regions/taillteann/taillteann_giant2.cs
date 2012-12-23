using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_giant2Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_giant2");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 19, eye: 53, eyeColor: 27, lip: 27);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x626B58, 0xA5C4E6, 0xBEAD0A);
		EquipItem(Pocket.Hair, 0x1F56, 0xCC9933, 0xCC9933, 0xCC9933);
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		SetLocation(region: 300, x: 205354, y: 191814);

		SetDirection(7);
		SetStand("");
	}
}
