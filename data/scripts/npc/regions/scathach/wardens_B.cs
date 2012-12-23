using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_BScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_wardens_B");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 23, eyeColor: 29, lip: 21);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xB5C2, 0x5E000A, 0xFFEBA3);
		EquipItem(Pocket.Hair, 0x100E, 0x367071, 0x367071, 0x367071);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33891, y: 44839);

		SetDirection(191);
		SetStand("elf/male/anim/elf_npc_granites_stand_friendly");
	}
}
