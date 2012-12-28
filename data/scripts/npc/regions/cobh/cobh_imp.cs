using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobh_imp");
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x203249;
		NPC.ColorB = 0xBC8942;
		NPC.ColorC = 0x84671C;		

		SetLocation(region: 23, x: 22065, y: 41600);

		SetDirection(38);
	}
}
