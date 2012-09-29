using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GhelpScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_ogre");
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x3A2D28;
		NPC.ColorB = 0x2D2121;
		NPC.ColorC = 0x651313;


		SetLocation(region: 1, x: 6408, y: 17250);

		SetDirection(56);
	}
}
