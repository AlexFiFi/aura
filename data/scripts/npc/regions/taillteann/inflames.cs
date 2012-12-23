using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class InflamesScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_inflames");
		SetRace(999998);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 300, x: 214825, y: 203451);

		SetDirection(0);
		SetStand("");
	}
}
