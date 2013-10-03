using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Emain_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_imp");
		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x3C777E, 0x91A2B, 0x617373);

		SetLocation(region: 52, x: 42902, y: 61547);

		SetDirection(121);
	}
}
