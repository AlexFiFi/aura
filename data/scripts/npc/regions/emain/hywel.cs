using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class HywelScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hywel");
		SetRace(999997);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x956B3F;
		NPC.ColorB = 0xDBDBDB;
		NPC.ColorC = 0xAFAFAF;		



		SetLocation(region: 139, x: 2121, y: 2458);

		SetDirection(157);
		SetStand("");
        
		Phrases.Add("Honey, I'm comin' for you...");
		Phrases.Add("What's that boy of mine up to?");
	}
}
