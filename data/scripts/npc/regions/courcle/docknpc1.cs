using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Docknpc1Script : Docknpc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_docknpc1");

		EquipItem(Pocket.Face, 0x1330, 0xC6AE69, 0xB4BB, 0x4012);

		SetLocation(region: 3300, x: 53760, y: 224730);

		SetDirection(195);
	}
}
