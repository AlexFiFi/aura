using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobh_goblin");
		SetRace(322);
		SetFace(skin: 27, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0xBC8942;
		NPC.ColorB = 0x243954;
		NPC.ColorC = 0x444444;		

		SetLocation(region: 23, x: 22150, y: 41340);

		SetDirection(40);
	}
}
