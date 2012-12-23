using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ColmScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_colm");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 162, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x19A7F, 0xFBA852, 0xF35F47);
		EquipItem(Pocket.Hair, 0xFA3, 0x80B6CB, 0x80B6CB, 0x80B6CB);
		EquipItem(Pocket.Armor, 0x3B17, 0xFFE3B5, 0x595F49, 0xACACAC);
		EquipItem(Pocket.Shoe, 0x4291, 0xBD9B55, 0x676058, 0x808080);

		SetLocation(region: 435, x: 1775, y: 1720);

		SetDirection(161);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");
	}
}
