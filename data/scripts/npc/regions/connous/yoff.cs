using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class YoffScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_yoff");
		SetRace(17105);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		SetLocation(region: 3103, x: 1200, y: 3700);

		SetDirection(185);
		SetStand("elf/male/anim/elf_npc_yoff_stand_friendly");
        
		Phrases.Add("Fearful people will die a million deaths before their actual death, but courageous people only die once.");
		Phrases.Add("I can't remember the last time I'd gotten a good night's sleep...");
		Phrases.Add("If you are looking to challenge yourself, the Arena may be just the place.");
		Phrases.Add("It's the kind of place where you feel like you're facing your own shadows.");
		Phrases.Add("Knives... They'll haunt you in the form of a cut.");
		Phrases.Add("The heart is surprisingly fragile. It breaks like a defective sword.");
		Phrases.Add("This is the entrance to Arena.");
		Phrases.Add("This isn't what I've always looked like. I mean, yeah, I don't think this is what I must have always looked like. Make sense?");
		Phrases.Add("When your eyes are all messed up, darkness is probably better for you. Everything gets blurry anyway.");
	}
}
