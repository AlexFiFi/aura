using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Belfast_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
		SetName("_belfast_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x494D75, 0x5A2727, 0xDEDEDE);

		SetLocation(region: 4005, x: 54140, y: 22050);

		SetDirection(120);
	}
}
