using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Emain_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x3C687E, 0x617373, 0xDEDEDE);

		SetLocation(region: 52, x: 42913, y: 61774);

		SetDirection(121);
	}
}
