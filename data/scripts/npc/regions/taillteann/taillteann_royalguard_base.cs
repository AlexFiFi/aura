using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_royalguard_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetStand("");
        
		Phrases.Add("I should be receiving a letter from home soon...");
		Phrases.Add("I was reminiscing about home and I didn't get much sleep last night.");
		Phrases.Add("Is it morning already...?");
		Phrases.Add("It hasn't been that long since I've eaten, but I'm hungry again...");
		Phrases.Add("Waking up to that bell every morning is a pain.");
		Phrases.Add("When is my shift going to end?");
	}
}
