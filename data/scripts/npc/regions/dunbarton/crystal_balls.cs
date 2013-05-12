// Aura Script
// --------------------------------------------------------------------------
// Crystal Balls
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CrystalBall1Script : CrystalBallBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_crystalbead01");
		SetLocation(72, 9050, 8430);
	}
}

public class CrystalBall2Script : CrystalBallBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_crystalbead02");
		SetLocation(72, 9050, 9350);
	}
}
public class CrystalBall3Script : CrystalBallBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_crystalbead03");
		SetLocation(72, 11250, 8430);
	}
}

public class CrystalBall4Script : CrystalBallBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_crystalbead04");
		SetLocation(72, 11250, 9350);
	}
}
