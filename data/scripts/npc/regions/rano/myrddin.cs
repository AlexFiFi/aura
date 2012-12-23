using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MyrddinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_myrddin");
		SetRace(10002);
		SetBody(height: 1.03f, fat: 1f, upper: 1.03f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 48, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xFDAD86, 0xFFE3D0, 0xB29C);
		EquipItem(Pocket.Hair, 0xFC3, 0xFFC9966B, 0xFFC9966B, 0xFFC9966B);
		EquipItem(Pocket.Armor, 0x3AF7, 0xFFFFFFFF, 0xFF1C2444, 0xFF14295A);
		EquipItem(Pocket.Shoe, 0x42A1, 0xFF0C1123, 0xFF1A5350, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x46B2, 0xFF000000, 0xFF28486F, 0xFF4378B6);

		SetLocation(region: 3001, x: 309582, y: 117816);

		SetDirection(9);
		SetStand("human/male/anim/male_natural_stand_npc_Ranald");
	}
}
