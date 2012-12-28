using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Belfast_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
		SetName("_belfast_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x494D75;
		NPC.ColorB = 0x5A2727;
		NPC.ColorC = 0xDEDEDE;		

		SetLocation(region: 4005, x: 54140, y: 22050);

		SetDirection(120);
	}
}
