using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GlenisScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_glenis");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 0.3f, upper: 1.4f, lower: 1.2f);
		SetFace(skin: 15, eye: 7, eyeColor: 119, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3E, 0x84C8, 0x323C99, 0xF35D80);
		EquipItem(Pocket.Hair, 0xBCC, 0xBC756C, 0xBC756C, 0xBC756C);
		EquipItem(Pocket.Armor, 0x3AA2, 0x764E63, 0xCCD8ED, 0xE7957A);
		EquipItem(Pocket.Shoe, 0x4274, 0x764E63, 0xFC9C5F, 0xD2CCE5);

		SetLocation(region: 14, x: 37566, y: 41605);

		SetDirection(129);
		SetStand("human/female/anim/female_natural_stand_npc_Glenis");
	}
}
