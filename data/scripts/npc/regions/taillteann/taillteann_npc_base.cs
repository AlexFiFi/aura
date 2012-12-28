using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_npc_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

        SetStand("");
        
        Phrases.Add("...");
		Phrases.Add("I like windy days.");
		Phrases.Add("I should take a walk. I ate too much.");
		Phrases.Add("I spent too much money yesterday on Enchants.");
		Phrases.Add("I want a pretty black dress.");
		Phrases.Add("Nice weather we're having.");
		Phrases.Add("Why isn't he here yet?");
    }
}
