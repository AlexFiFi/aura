using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Ffion_crystalbeadScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ffion_crystalbead");
		SetRace(990005);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0xFFFFFF;
		NPC.ColorB = 0x43587C;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3001, x: 164944, y: 160203);

		SetDirection(31);
		SetStand("");
	}
}
