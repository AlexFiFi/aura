using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Tara_impScript : CommerceImpScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_imp");
		SetFace(skin: 25, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x3A2D28;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0x136980;		

		SetLocation(region: 401, x: 74648, y: 128488);

		SetDirection(121);
	}
}
