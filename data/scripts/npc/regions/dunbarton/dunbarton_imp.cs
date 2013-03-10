using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Dunbarton_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dunbarton_imp");
		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x315D2E;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0x3A2D28;		

		SetLocation(region: 14, x: 42640, y: 46464);

		SetDirection(36);
	}
}
