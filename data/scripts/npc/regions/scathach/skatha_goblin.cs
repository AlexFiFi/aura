using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Skatha_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_skatha_goblin");
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x3E69A0, 0x213866, 0x808080);

		SetLocation(region: 4014, x: 32070, y: 43760);

		SetDirection(193);
	}
}
