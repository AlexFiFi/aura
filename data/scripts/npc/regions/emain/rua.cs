using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class RuaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_rua");
		SetRace(16);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 57, x: 6981, y: 4998);

		SetDirection(82);
		SetStand("");
	}
}
