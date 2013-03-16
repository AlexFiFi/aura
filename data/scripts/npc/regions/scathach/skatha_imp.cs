using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Skatha_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_skatha_imp");
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x305A8C, 0x2D3B4C, 0x8FBDCC);

		SetLocation(region: 4014, x: 32210, y: 43760);

		SetDirection(197);
	}
}
