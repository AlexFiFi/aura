using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Courcleinhabitant_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 4, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetStand("");
        
		Phrases.Add("May Irinid bless you!");
		Phrases.Add("May the Great Spirit of Irinid bless you on the journey.");
		Phrases.Add("Oh, the Great Spirit...");
	}
}
