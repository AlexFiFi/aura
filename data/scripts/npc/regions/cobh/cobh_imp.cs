using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_impScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_cobh_imp");
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x203249;
		NPC.ColorB = 0xBC8942;
		NPC.ColorC = 0x84671C;		



		SetLocation(region: 23, x: 22065, y: 41600);

		SetDirection(38);
		SetStand("chapter4/monster/anim/imp/mon_c4_imp_commerce");
	}
}
