using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class WeideScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_weide");
		SetRace(29);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0xEDE5D4, 0x808080);

		EquipItem(Pocket.RightHand1, 0xB3C7, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 287360, y: 222560);

		SetDirection(225);
		SetStand("giant/male/anim/giant_npc_weide");
        
		Phrases.Add("Every effect has a cause that goes before it.");
		Phrases.Add("I am giving my all for the Seebarsch royal family.");
		Phrases.Add("I feel like I'm losing hair these days...");
		Phrases.Add("I forgot to shave my beard in honor of the Seebarsch royal family.");
		Phrases.Add("I'm too old now to pick up a sword and fight.");
		Phrases.Add("In difficult times like this, only purebred Physis Giants can help.");
		Phrases.Add("Kirine...so superficial...");
		Phrases.Add("My eyesight is getting so bad. Maybe it's time for me to retire...");
		Phrases.Add("That brat Zeder must be talking about me again.");
		Phrases.Add("The older you get, the harder it is to adjust to new things.");
		Phrases.Add("There was a time when I was on the front lines.");
		Phrases.Add("You didn't get the numbers wrong again, did you?");
	}
}
