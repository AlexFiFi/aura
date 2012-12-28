using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Emain_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_imp");
		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x3C777E;
		NPC.ColorB = 0x91A2B;
		NPC.ColorC = 0x617373;		

		SetLocation(region: 52, x: 42902, y: 61547);

		SetDirection(121);
	}
}
