using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Evidence_04Script : Evidence_base_Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_evidence_04");

		SetLocation(region: 411, x: 10944, y: 10304);

		SetDirection(40);
	}
}
