using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class IlsaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ilsa");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 24, eyeColor: 25, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x485366, 0x7B0572, 0xFDD740);
		EquipItem(Pocket.Hair, 0xBD0, 0xC67139, 0xC67139, 0xC67139);
		EquipItem(Pocket.Armor, 0x3B20, 0x424563, 0xCDC9C4, 0xD65D9B);
		EquipItem(Pocket.Shoe, 0x42EF, 0x22212B, 0x8789F6, 0x2881F3);

		SetLocation(region: 433, x: 976, y: 2415);

		SetDirection(225);
		SetStand("human/female/anim/female_stand_npc_emain_Rua");
	}
}
