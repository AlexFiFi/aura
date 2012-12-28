using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Bangor_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
        
		SetName("_bangor_imp");

		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x2D2121;
		NPC.ColorB = 0x725D45;
		NPC.ColorC = 0xCA9045;		

		SetLocation(region: 31, x: 13330, y: 22358);

		SetDirection(160);
	}
}
