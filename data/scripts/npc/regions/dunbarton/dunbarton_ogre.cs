using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Dunbarton_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dunbarton_ogre");
		SetFace(skin: 131, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x543C34;
		NPC.ColorB = 0x172417;
		NPC.ColorC = 0x224421;		

		SetLocation(region: 14, x: 43075, y: 46476);

		SetDirection(48);
	}
}
