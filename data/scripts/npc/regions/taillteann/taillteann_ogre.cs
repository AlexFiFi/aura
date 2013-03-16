using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_ogre");
		SetFace(skin: 30, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x898282, 0x2D293F, 0x4A1D61);

		SetLocation(region: 300, x: 240065, y: 192755);

		SetDirection(196);
	}
}
