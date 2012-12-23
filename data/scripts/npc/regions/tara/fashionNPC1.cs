using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class FashionNPC1Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_fashionNPC1");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 27, eyeColor: 31, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x7B251, 0x5F97, 0x9169AD);
		EquipItem(Pocket.Hair, 0xBD9, 0xFFCC66, 0xFFCC66, 0xFFCC66);
		EquipItem(Pocket.Armor, 0x3C0E, 0x451A1A, 0xF7F3DE, 0xFFF38C);
		EquipItem(Pocket.Shoe, 0x42AF, 0x0, 0x0, 0x808080);

		SetLocation(region: 401, x: 83737, y: 105418);

		SetDirection(31);
		SetStand("human/female/anim/female_natural_stand_npc_Eavan");
	}
}
