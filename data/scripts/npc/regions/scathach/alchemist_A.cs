using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_AScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alchemist_A");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 140, eyeColor: 168, lip: 53);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x686D51, 0x79B38E, 0x2E3896);
		EquipItem(Pocket.Hair, 0xC37, 0xFCD685, 0xFCD685, 0xFCD685);
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33322, y: 42078);

		SetDirection(118);
		SetStand("");
	}
}
