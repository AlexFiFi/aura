using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CommerceElephantScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(377);

		SetBody(height: 2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;

		SetStand("");

		Phrases.Add("Bhoo hoo...");
		Phrases.Add("Bhoo!");
		Phrases.Add("Bhoo! Bhoo!");
		Phrases.Add("Boo!");
		Phrases.Add("Boooo!");

	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "It has been trumpeting and shuffling non-stop.",
			"As it stomps the ground with its giant feel, it bellows again.",
			"Now, it stares softly at you with its two innocent eyes.");
		Msg(c, "Boooo?", "Bhoo!");
		Msg(c, "Bhooo!", "Bhoo, bhoooo!");
	}

}
