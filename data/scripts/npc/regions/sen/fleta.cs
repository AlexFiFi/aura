using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class FletaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_fleta");
		SetRace(10001);
		SetBody(height: 0.1000001f, fat: 1.06f, upper: 1.09f, lower: 1f);
		SetFace(skin: 15, eye: 8, eyeColor: 155, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x470036, 0xF79622, 0xF46F81);
		EquipItem(Pocket.Hair, 0xBBC, 0xFFBC8B63, 0xFFBC8B63, 0xFFBC8B63);
		EquipItem(Pocket.Armor, 0x3AE6, 0xFF301D16, 0xFF1B100E, 0xFF6D5034);
		EquipItem(Pocket.Shoe, 0x426F, 0x151515, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 53, x: 104689, y: 109742);

		SetDirection(240);
		SetStand("");
	}
}
