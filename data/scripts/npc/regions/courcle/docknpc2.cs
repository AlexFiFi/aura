using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc2Script : Docknpc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_docknpc2");

		EquipItem(Pocket.Face, 0x1330, 0xB4DBA6, 0x680046, 0x75A8A4);

		SetLocation(region: 3300, x: 148070, y: 163780);

		SetDirection(194);
	}
}
