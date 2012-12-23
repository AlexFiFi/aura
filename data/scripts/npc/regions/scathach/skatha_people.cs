using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Skatha_peopleScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_people");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 162, eyeColor: 114, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF52, 0x116A94, 0x6C75, 0xE5B354);
		EquipItem(Pocket.Hair, 0xC3F, 0x8B6559, 0x8B6559, 0x8B6559);
		EquipItem(Pocket.Armor, 0x3E2D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x48F2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4015, x: 32951, y: 40325);

		SetDirection(194);
		SetStand("chapter4/human/female/anim/female_c4_npc_skatha_human_stand");
	}
}
