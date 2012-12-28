using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_goblin");
		SetRace(322);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0xBA8E7F;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0x286C81;		

		SetLocation(region: 401, x: 74641, y: 128688);

		SetDirection(121);
	}
}
