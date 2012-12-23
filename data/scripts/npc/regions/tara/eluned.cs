using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ElunedScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_eluned");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 84, eyeColor: 98, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF43, 0xFCCF5E, 0x8AFE5, 0x5065);
		EquipItem(Pocket.Hair, 0xC23, 0x9B5A26, 0x9B5A26, 0x9B5A26);
		EquipItem(Pocket.Armor, 0x3C1A, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 430, x: 1026, y: 2033);

		SetDirection(230);
		SetStand("chapter3/human/female/anim/female_c3_npc_eluned");
	}
}
