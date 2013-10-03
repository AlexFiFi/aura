using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Cobh_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobh_imp");
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x203249, 0xBC8942, 0x84671C);

		SetLocation(region: 23, x: 22065, y: 41600);

		SetDirection(38);
	}
}
