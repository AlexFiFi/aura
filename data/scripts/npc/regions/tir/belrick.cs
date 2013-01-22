// Aura Script
// --------------------------------------------------------------------------
// Belrick - Commerce Elephant
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class BelrickScript : CommerceElephantScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_elephant");
		SetLocation("tir", 6811, 17049, 56);
	}
}
