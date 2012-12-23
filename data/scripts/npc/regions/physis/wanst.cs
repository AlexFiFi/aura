using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class WanstScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_wanst");
		SetRace(30);
		SetBody(height: 1f, fat: 1.1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3200, x: 295393, y: 229140);

		SetDirection(163);
		SetStand("giant/male/anim/giant_npc_wanst");
	}
}
