using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_imp");
		SetFace(skin: 27, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x4A1D61;
		NPC.ColorB = 0x2D2830;
		NPC.ColorC = 0xD0C9C9;

		SetLocation(region: 300, x: 240427, y: 192732);

		SetDirection(196);
	}
}
