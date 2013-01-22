// Aura Script
// --------------------------------------------------------------------------
// Neru - Commerce Imp
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class NeruScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_imp");
		SetFace(skin: 26, eye: 3, eyeColor: 7, lip: 2);
		SetColor(0x7D0000, 0x2D2121, 0xC19000);
		SetLocation("tir", 6045, 17233, 56);
	}
}