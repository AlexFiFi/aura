using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Evidence_05Script : Evidence_base_Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_evidence_05");
        
		SetLocation(region: 411, x: 10316, y: 8108);

		SetDirection(40);
	}
}
