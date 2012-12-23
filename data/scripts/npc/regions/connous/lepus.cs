using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LepusScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_lepus");
		SetRace(9002);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 36, eyeColor: 91, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1AF4, 0x5D2661, 0x6E5F65, 0xA12C91);
		EquipItem(Pocket.Hair, 0x1775, 0x48B4D6, 0x48B4D6, 0x48B4D6);
		EquipItem(Pocket.Armor, 0x3B40, 0xFFA72B, 0x182C18, 0x112320);
		EquipItem(Pocket.Shoe, 0x427D, 0xAE820E, 0x630837, 0xEEE5B7);
		EquipItem(Pocket.Head, 0x46CF, 0x86E360, 0xAC9ACB, 0xB1A301);

		SetLocation(region: 3100, x: 363093, y: 426159);

		SetDirection(200);
		SetStand("elf/male/anim/elf_npc_lepus_stand_friendly");
	}
}
