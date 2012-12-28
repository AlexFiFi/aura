using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc4Script : Docknpc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_docknpc4");

		EquipItem(Pocket.Face, 0x1330, 0x332F, 0xAA441D, 0x424061);

		SetLocation(region: 3300, x: 256240, y: 160690);

		SetDirection(219);
	}
}
