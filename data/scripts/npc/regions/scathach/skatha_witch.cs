using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Skatha_witchScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_witch");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 163, eyeColor: 38, lip: 48);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF54, 0x36FA9, 0x8C96CB, 0x704E50);
		EquipItem(Pocket.Hair, 0xC3F, 0xECE6DC, 0xECE6DC, 0xECE6DC);
		EquipItem(Pocket.Armor, 0x3E2F, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x48F4, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4015, x: 32951, y: 40325);

		SetDirection(194);
		SetStand("chapter4/human/female/anim/female_c4_npc_skatha_stand");
	}
}
