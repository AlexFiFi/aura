using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Bangor_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
        
		SetName("_bangor_imp");

		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x2D2121, 0x725D45, 0xCA9045);

		SetLocation(region: 31, x: 13330, y: 22358);

		SetDirection(160);
	}
}
