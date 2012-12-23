using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KaynaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_kayna");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1.4f, upper: 1.4f, lower: 1f);
		SetFace(skin: 15, eye: 60, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xDE9853, 0x2E3B65, 0x6E6365);
		EquipItem(Pocket.Hair, 0x138F, 0x756939, 0x756939, 0x756939);
		EquipItem(Pocket.Armor, 0x3BFB, 0x825642, 0xB0ADAB, 0xEFE3B5);
		EquipItem(Pocket.Shoe, 0x42AA, 0x633C31, 0x6E9677, 0x808080);

		SetLocation(region: 23, x: 26559, y: 38870);

		SetDirection(227);
		SetStand("chapter4/human/female/anim/female_c4_npc_kayna");
	}
}
