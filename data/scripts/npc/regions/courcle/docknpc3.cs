using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc3Script : Docknpc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_docknpc3");

		EquipItem(Pocket.Face, 0x1330, 0xFABBCC, 0xFDE25B, 0x117C78);

		SetLocation(region: 3300, x: 175860, y: 130750);

		SetDirection(48);
	}
}
