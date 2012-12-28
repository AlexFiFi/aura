using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_02Script : Soldier_Belfast_BaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_soldier_belfast_02");
        
		SetLocation(region: 4005, x: 24072, y: 48696);

		SetDirection(216);
	}
}
