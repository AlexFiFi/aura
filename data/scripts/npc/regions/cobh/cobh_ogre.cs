using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobh_ogre");
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0xBC8942;
		NPC.ColorB = 0x444444;
		NPC.ColorC = 0x243954;		

		SetLocation(region: 23, x: 22400, y: 41150);

		SetDirection(46);
	}
}
