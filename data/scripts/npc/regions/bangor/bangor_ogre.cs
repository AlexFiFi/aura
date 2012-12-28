using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Bangor_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
		SetName("_bangor_ogre");
        SetFace(skin: 27, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x6C716C;
		NPC.ColorB = 0x2D2121;
		NPC.ColorC = 0x935F15;		

		SetLocation(region: 31, x: 13130, y: 22719);

		SetDirection(160);
	}
}
