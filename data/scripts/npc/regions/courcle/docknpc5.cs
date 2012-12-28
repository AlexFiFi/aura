using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc5Script : Docknpc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_docknpc5");

		EquipItem(Pocket.Face, 0x1330, 0xA97E79, 0x187544, 0xFCD652);

		SetLocation(region: 3300, x: 86480, y: 161730);

		SetDirection(200);
	}
}
