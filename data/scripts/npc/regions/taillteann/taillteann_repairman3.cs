using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_repairman3Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_repairman3");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 49, eyeColor: 126, lip: 27);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0xFAAFB8, 0xF8AD5D, 0xF5A33B);
		EquipItem(Pocket.Hair, 0x1F4F, 0x663333, 0x663333, 0x663333);
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		SetLocation(region: 300, x: 205912, y: 190960);

		SetDirection(73);
		SetStand("");
	}
}
