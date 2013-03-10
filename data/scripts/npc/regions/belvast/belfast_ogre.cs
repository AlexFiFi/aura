using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Belfast_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
		SetName("_belfast_ogre");
		SetFace(skin: 30, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x5A2727;
		NPC.ColorB = 0x1B0808;
		NPC.ColorC = 0x4E4F84;		

		SetLocation(region: 4005, x: 54070, y: 22324);

		SetDirection(135);
	}
}
