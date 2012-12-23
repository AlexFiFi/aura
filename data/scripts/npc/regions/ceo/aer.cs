using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AerScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_aer");
		SetRace(19);
		SetBody(height: 1.3f, fat: 1f, upper: 1f, lower: 1.2f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 68, x: 5599, y: 8550);

		SetDirection(192);
		SetStand("");
	}
}
