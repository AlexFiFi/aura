using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Worker_belfast_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(123);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 4005, x: 54358, y: 23439);

		SetDirection(193);
		SetStand("");

		Phrases.Add("Heave ho...");
		Phrases.Add("Hmm, I think we are missing some...");
		Phrases.Add("I need a break...");
		Phrases.Add("I need to find a way to make some money...");
		Phrases.Add("I need to work hard and save up...");
		Phrases.Add("I should be able to get a job once the dock is open, right?");
		Phrases.Add("I used to be really successful... Then the pirates came.");
		Phrases.Add("I want to be rich as soon as possible.");
		Phrases.Add("I want to be rich, too.");
		Phrases.Add("I want to set sail on the sea...");
		Phrases.Add("If I work hard, I can save up a lot, right?");
		Phrases.Add("I'm not scared of those ruddy pirates!");
		Phrases.Add("It's so hard to save up money.");
		Phrases.Add("Just a quick break...I'll work harder afterwards!");
		Phrases.Add("Let's move this!");
		Phrases.Add("Move this here...");
		Phrases.Add("The weather is so nice!");
		Phrases.Add("Those blasted pirates!");
		Phrases.Add("Unemployment is so frustrating!");
		Phrases.Add("When will I be done with this?");
		Phrases.Add("When will the construction at the dock end?");
		Phrases.Add("Working is tough, but it can be fun.");
	}
}
