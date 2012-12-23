using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CastaneaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_castanea");
		SetRace(9001);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 18, eye: 39, eyeColor: 3, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x170C, 0xCBA0C8, 0xB97B7E, 0x53686D);
		EquipItem(Pocket.Hair, 0x138B, 0x926287, 0x926287, 0x926287);
		EquipItem(Pocket.Armor, 0x3B41, 0xFF808080, 0xFF07497A, 0xFF07497A);
		EquipItem(Pocket.Shoe, 0x42A9, 0xD0B18, 0xB0506, 0x947200);
		EquipItem(Pocket.Robe, 0x4A42, 0x2C0309, 0x0, 0x611532);

		SetLocation(region: 3100, x: 379510, y: 425770);

		SetDirection(149);
		SetStand("elf/female/anim/elf_npc_castanea_stand_friendly");
	}
}
