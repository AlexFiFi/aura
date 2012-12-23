using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MondScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_mond");
		SetRace(139);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 26, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.RightHand1, 0x9E17, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0x9E15, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 42094, y: 22868);

		SetDirection(3);
		SetStand("chapter4/monster/anim/imp/imp_c4_npc_bank");
	}
}
