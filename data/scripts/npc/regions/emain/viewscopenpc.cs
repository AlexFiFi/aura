using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ViewscopenpcScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_viewscopenpc");
		SetRace(990004);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 52, x: 43713, y: 36412);

		SetDirection(194);
		SetStand("");
	}
}
