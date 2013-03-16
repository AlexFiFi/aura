using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Emain_elephantScript : CommerceElephantScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_elephant");

		SetColor(0x8490AE, 0xBBC2D1, 0x808080);

		SetLocation(region: 52, x: 43121, y: 62350);

		SetDirection(127);
	}
}
