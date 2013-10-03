using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Belfast_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
		SetName("_belfast_imp");
		SetFace(skin: 247, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x421B1B, 0x2C2927, 0x494D75);

		SetLocation(region: 4005, x: 54112, y: 21839);

		SetDirection(133);
	}
}
