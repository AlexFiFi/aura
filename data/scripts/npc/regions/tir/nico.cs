// Aura Script
// --------------------------------------------------------------------------
// Nico - Commerce Goblin
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NicoScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);
		SetColor(0x811E1E, 0x3A2D28, 0xDDAD1F);
		SetLocation("tir", 6221, 17173, 56);
	}
}
