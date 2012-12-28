using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Dunbarton_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dunbarton_goblin");
		SetFace(skin: 20, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x487346;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0x9C802D;		

		SetLocation(region: 14, x: 42858, y: 46475);

		SetDirection(32);
	}
}
