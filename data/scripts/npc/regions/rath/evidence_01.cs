using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Evidence_01Script : Evidence_base_Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_evidence_01");

		SetLocation(region: 411, x: 6717, y: 6743);

		SetDirection(40);
	}
}
