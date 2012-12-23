using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SiobhaninScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_siobhanin");
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 41, eyeColor: 168, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0x3348, 0xF8A561, 0xF49D33);
		EquipItem(Pocket.Hair, 0x177A, 0xE3A400, 0xE3A400, 0xE3A400);
		EquipItem(Pocket.Armor, 0x3CD4, 0xE6D5CA, 0x455562, 0x63696E);
		EquipItem(Pocket.Shoe, 0x4325, 0x9EB6BE, 0x8C5A00, 0x95BF49);

		SetLocation(region: 23, x: 26852, y: 36426);

		SetDirection(37);
		SetStand("chapter4/elf/male/anim/elf_npc_siobhanin");
	}
}
