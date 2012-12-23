using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AusteynScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_austeyn");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 16, eye: 8, eyeColor: 84, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1328, 0x81A6, 0x806588, 0x8DA62C);
		EquipItem(Pocket.Hair, 0xFBB, 0xD1D9E3, 0xD1D9E3, 0xD1D9E3);
		EquipItem(Pocket.Armor, 0x3A9B, 0x36485A, 0xBDC2B1, 0x626C76);
		EquipItem(Pocket.Shoe, 0x4271, 0x36485A, 0xFFE1B9, 0x9A004E);

		SetLocation(region: 20, x: 660, y: 770);

		SetDirection(251);
		SetStand("");
	}
}
