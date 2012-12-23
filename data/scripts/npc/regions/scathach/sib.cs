using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SibScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_sib");
		SetRace(9001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 38, eyeColor: 83, lip: 47);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1710, 0xEDE20F, 0xEFEB42, 0xAEB1);
		EquipItem(Pocket.Hair, 0xC2C, 0xF7EFFF, 0xF7EFFF, 0xF7EFFF);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33124, y: 42574);

		SetDirection(95);
		SetStand("chapter4/human/female/anim/female_c4_npc_sive_stand");
	}
}
