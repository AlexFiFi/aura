using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AsconScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ascon");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 0.7f, upper: 0.7f, lower: 0.7f);
		SetFace(skin: 18, eye: 138, eyeColor: 27, lip: 46);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1340, 0xF79929, 0x5A695F, 0xB1DF);
		EquipItem(Pocket.Hair, 0x1028, 0xE2E1B7, 0xE2E1B7, 0xE2E1B7);
		EquipItem(Pocket.Armor, 0x3DF1, 0xB6D8EA, 0x1F5A99, 0x633C31);

		SetLocation(region: 23, x: 36869, y: 36212);

		SetDirection(101);
		SetStand("chapter4/human/male/anim/male_c4_npc_ascon");
	}
}
