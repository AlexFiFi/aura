using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_03Script : Soldier_Belfast_BaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_soldier_belfast_03");
        
		SetLocation(region: 4005, x: 26983, y: 28858);

		SetDirection(193);
	}
}
