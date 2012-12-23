using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_goblinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_cobh_goblin");
		SetRace(322);
		SetBody(height: 0.7f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0xBC8942;
		NPC.ColorB = 0x243954;
		NPC.ColorC = 0x444444;		

		EquipItem(Pocket.RightHand1, 0x9DF4, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 23, x: 22150, y: 41340);

		SetDirection(40);
		SetStand("chapter4/monster/anim/goblin/mon_c4_goblin_commerce");
	}
}
