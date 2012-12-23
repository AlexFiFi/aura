using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Imp_kinuScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_imp_kinu");
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 247, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x817895;
		NPC.ColorB = 0xAB8E8E;
		NPC.ColorC = 0x4C4C6B;		



		SetLocation(region: 4005, x: 46469, y: 39990);

		SetDirection(238);
		SetStand("chapter4/monster/anim/imp/mon_c4_imp_commerce");
	}
}
