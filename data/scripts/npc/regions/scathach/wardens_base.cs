using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 30824, y: 42563);

		SetDirection(69);

		Phrases.Add("Be on guard.");
		Phrases.Add("Can't...let guard down...");
		Phrases.Add("Cows have it easy.");
		Phrases.Add("Hey! Watch the sudden movements!");
		Phrases.Add("Hm.");
		Phrases.Add("Ho-hum.");
		Phrases.Add("How long has it been?");
		Phrases.Add("Huh? Need a hand?");
		Phrases.Add("Huh? What?");
		Phrases.Add("I could really go for a steak.");
		Phrases.Add("I have a bad feeling about this.");
		Phrases.Add("I should've been born a cow.");
		Phrases.Add("I wonder how they're doing...");
		Phrases.Add("I'm on to you.");
		Phrases.Add("Isn't the captain the best?");
		Phrases.Add("No sudden movements...");
		Phrases.Add("Shh!");
		Phrases.Add("Sigh...");
		Phrases.Add("So much noise! Jeez!");
		Phrases.Add("So...tired...");
		Phrases.Add("Stupid alchemists...");
		Phrases.Add("This is a dangerous place.");
		Phrases.Add("Those beasts at the main gate are such a headache.");
		Phrases.Add("What're you staring at?");
		Phrases.Add("Who goes there? You aren't one of ours, are you?");
		Phrases.Add("Whoa! What's that?");
		Phrases.Add("Yawn...");
		Phrases.Add("You! You're hiding something, aren't you?");
	}
}
