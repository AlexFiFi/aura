using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_01Script : Soldier_Belfast_BaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_soldier_belfast_01");

		SetLocation(region: 4005, x: 23484, y: 48333);

		SetDirection(224);
	}
}
