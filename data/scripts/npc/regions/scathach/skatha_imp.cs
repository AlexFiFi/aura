using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Skatha_impScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_imp");
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x305A8C;
		NPC.ColorB = 0x2D3B4C;
		NPC.ColorC = 0x8FBDCC;		



		SetLocation(region: 4014, x: 32210, y: 43760);

		SetDirection(197);
		SetStand("chapter4/monster/anim/imp/mon_c4_imp_commerce");
	}
}
