using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Evidence_03Script : Evidence_base_Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_evidence_03");

		SetLocation(region: 411, x: 6413, y: 10109);

		SetDirection(40);
	}
}
