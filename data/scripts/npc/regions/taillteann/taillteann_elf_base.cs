using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_elf_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetStand("");
        
        Phrases.Add("Does Granat never grow tired?");
		Phrases.Add("I wonder if I'll be permitted to go home this holiday.");
		Phrases.Add("I'm quite hungry.");
		Phrases.Add("It's cooler here than in Filia.");
	}
}
