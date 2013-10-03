// Aura Script
// --------------------------------------------------------------------------
// Neru - Commerce Imp
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

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