using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DeviScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_devi");
		SetRace(45);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0xCF9B5F;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 300, x: 215391, y: 194644);

		SetDirection(171);
		SetStand("chapter3/human/male/anim/male_c3_npc_devi");
        
        Phrases.Add("Aah, so bored, so bored.");
		Phrases.Add("Aarrgh, this is torture...");
		Phrases.Add("Anything interesting going on? Didn't think so.");
		Phrases.Add("Dang it, I'm out of wine. I'll have to sneak in some more.");
		Phrases.Add("I can't believe not a single person has sent me a letter...");
		Phrases.Add("I'm so annoyed...");
		Phrases.Add("Not a single pretty girl in town.");
		Phrases.Add("Send me into battle instead of chaining me to this bank.");
	}
}
