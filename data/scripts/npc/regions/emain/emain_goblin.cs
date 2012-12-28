using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Emain_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emain_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x3C687E;
		NPC.ColorB = 0x617373;
		NPC.ColorC = 0xDEDEDE;		

		SetLocation(region: 52, x: 42913, y: 61774);

		SetDirection(121);
	}
}
