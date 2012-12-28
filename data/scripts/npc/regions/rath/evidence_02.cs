using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Evidence_02Script : Evidence_base_Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_evidence_02");

		SetLocation(region: 411, x: 8009, y: 7801);

		SetDirection(40);
	}
}
