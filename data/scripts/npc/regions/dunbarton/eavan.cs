using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EavanScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_eavan");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 0.7f, upper: 0.7f, lower: 0.7f);
		SetFace(skin: 15, eye: 3, eyeColor: 3, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xD1DFF2, 0x38BD96, 0xF69365);
		EquipItem(Pocket.Hair, 0xBCE, 0xFFEEAA, 0xFFEEAA, 0xFFEEAA);
		EquipItem(Pocket.Armor, 0x3AC1, 0xFFCCCC, 0x80C5D3, 0xA7ACB4);
		EquipItem(Pocket.Glove, 0x3E8F, 0xFFFFFF, 0xE6F2E2, 0x6161AC);
		EquipItem(Pocket.Shoe, 0x4270, 0xDDAACC, 0xF79B2F, 0xE10175);

		SetLocation(region: 14, x: 40024, y: 41041);

		SetDirection(192);
		SetStand("human/female/anim/female_natural_stand_npc_Eavan");
	}
}
