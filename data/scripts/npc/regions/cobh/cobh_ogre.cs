using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Cobh_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobh_ogre");
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0xBC8942, 0x444444, 0x243954);

		SetLocation(region: 23, x: 22400, y: 41150);

		SetDirection(46);
	}
}
