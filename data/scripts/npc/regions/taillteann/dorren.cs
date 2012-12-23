using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class DorrenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_dorren");
		SetRace(48);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0xF7F3DE;		



		SetLocation(region: 300, x: 216610, y: 200328);

		SetDirection(186);
		SetStand("chapter3/human/female/anim/female_c3_npc_dorren");
	}
}
