using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Goblin_hicoScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_goblin_hico");
		SetRace(322);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x749F56;
		NPC.ColorB = 0x908926;
		NPC.ColorC = 0x333333;		



		SetLocation(region: 4005, x: 45547, y: 24662);

		SetDirection(216);
		SetStand("");
	}
}
