using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Tara_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
        base.OnLoad();
        SetName("_tara_ogre");
		SetFace(skin: 131, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x8B6C5F;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0x136980;		

		SetLocation(region: 401, x: 74634, y: 128938);

		SetDirection(121);
	}
}
