using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Emain_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_ogre");
		SetFace(skin: 119, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x837979, 0xB1927, 0xEAEAEA);

		SetLocation(region: 52, x: 42915, y: 61991);

		SetDirection(121);
	}
}
