using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AtrataScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_atrata");
		SetRace(9001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 35, eyeColor: 238, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x170C, 0x1B71A9, 0x6F4D00, 0x91066E);
		EquipItem(Pocket.Hair, 0x138A, 0xB141D, 0xB141D, 0xB141D);
		EquipItem(Pocket.Shoe, 0x4280, 0x3A142A, 0x5C9AFC, 0x47ABF1);
		EquipItem(Pocket.Head, 0x46D0, 0xFFFFFF, 0x979CF9, 0x4600D8);
		EquipItem(Pocket.Robe, 0x4A4E, 0xEF96A0, 0x5B0E2B, 0x17041F);
		EquipItem(Pocket.RightHand1, 0xB3C7, 0x3A142A, 0x5C9AFC, 0x47ABF1);

		SetLocation(region: 3100, x: 379708, y: 421574);

		SetDirection(181);
		SetStand("elf/female/anim/elf_npc_atrata_stand_friendly");
	}
}
