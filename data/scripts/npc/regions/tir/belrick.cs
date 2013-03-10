// Aura Script
// --------------------------------------------------------------------------
// Belrick - Commerce Elephant
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BelrickScript : CommerceElephantScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_elephant");
		SetLocation("tir", 6811, 17049, 56);
	}
}
