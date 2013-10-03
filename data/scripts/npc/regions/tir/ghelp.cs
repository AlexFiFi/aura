// Aura Script
// --------------------------------------------------------------------------
// Ghelp - Commerce Ogre
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GhelpScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_ogre");
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);
		SetColor(0x3A2D28, 0x2D2121, 0x651313);
		SetLocation("tir", 6408, 17250, 56);
	}
}
