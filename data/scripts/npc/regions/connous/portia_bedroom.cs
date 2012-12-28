using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Portia_bedroomScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_portia_bedroom");
		SetRace(10001);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 148, eyeColor: 135, lip: 43);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF51, 0x736B4B, 0x7150, 0x294B);
		EquipItem(Pocket.Hair, 0xC3A, 0xFADE9C, 0xFADE9C, 0xFADE9C);
		EquipItem(Pocket.Armor, 0x3DFE, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3106, x: 4375, y: 3613);

		SetDirection(172);
		SetStand("chapter4/human/female/anim/female_c4_npc_posser");
	}
}
