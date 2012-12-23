using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_CScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alchemist_C");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 23, eye: 30, eyeColor: 8, lip: 24);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x3CA040, 0x55C190, 0x1F63AE);
		EquipItem(Pocket.Hair, 0x1007, 0x414141, 0x414141, 0x414141);
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33951, y: 43279);

		SetDirection(209);
		SetStand("");
	}
}
