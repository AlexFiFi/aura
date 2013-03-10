using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Skatha_ogreScript : CommerceOgreScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_skatha_ogre");
		SetFace(skin: 30, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x9CB4D0;
		NPC.ColorB = 0x264870;
		NPC.ColorC = 0x343952;		

		SetLocation(region: 4014, x: 31870, y: 43840);

		SetDirection(189);
	}
}
