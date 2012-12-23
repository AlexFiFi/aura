using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LegatusScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_legatus");
		SetRace(37);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3400, x: 322199, y: 203607);

		SetDirection(170);
		SetStand("monster/anim/dragon/dragon_standing_friendly_02");
	}
}
