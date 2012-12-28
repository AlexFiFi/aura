using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_04Script : Soldier_Belfast_BaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_soldier_belfast_04");

		SetLocation(region: 4005, x: 26983, y: 30470);

		SetDirection(63);
	}
}
