using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Bangor_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bangor_ogre");
        SetFace(skin: 27, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x6C716C, 0x2D2121, 0x935F15);

		SetLocation(region: 31, x: 13130, y: 22719);

		SetDirection(160);
	}
}
