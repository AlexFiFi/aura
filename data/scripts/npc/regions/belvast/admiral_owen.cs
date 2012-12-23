using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Admiral_owenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_admiral_owen");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 147, eyeColor: 126, lip: 46);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1343, 0x737070, 0xFAB85B, 0x3A54);
		EquipItem(Pocket.Hair, 0x102F, 0xA7A59D, 0xA7A59D, 0xA7A59D);
		EquipItem(Pocket.Armor, 0x3E02, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x4214, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 22638, y: 29322);

		SetDirection(0);
		SetStand("chapter4/human/male/anim/male_c4_npc_owen");
	}
}
