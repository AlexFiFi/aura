using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GranitesScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_granites");
		SetRace(9002);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 40, eyeColor: 126, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0x395F74, 0x19A78, 0xF40F32);
		EquipItem(Pocket.Hair, 0x1773, 0xABDBD7, 0xABDBD7, 0xABDBD7);
		EquipItem(Pocket.Shoe, 0x4292, 0x6E96CA, 0x5A5A5A, 0x7170B2);
		EquipItem(Pocket.Robe, 0x4A4E, 0x63967, 0x7283C4, 0xD1239);
		EquipItem(Pocket.RightHand1, 0x9D0C, 0xFF808080, 0xFF07497A, 0xFF07497A);
		EquipItem(Pocket.LeftHand1, 0x9C44, 0xFF808080, 0xFF07497A, 0xFF07497A);

		SetLocation(region: 3100, x: 368232, y: 419384);

		SetDirection(224);
		SetStand("elf/male/anim/elf_npc_granites_stand_friendly");
	}
}
