using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Skatha_goblinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_goblin");
		SetRace(348);
		SetBody(height: 0.7f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x3E69A0;
		NPC.ColorB = 0x213866;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.RightHand1, 0x9DF4, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4014, x: 32070, y: 43760);

		SetDirection(193);
		SetStand("chapter4/monster/anim/goblin/mon_c4_goblin_commerce");
	}
}
