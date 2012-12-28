using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ShenaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_shena");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 16, eye: 1, eyeColor: 39, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xA10068, 0x91CA56, 0x1EBCA4);
		EquipItem(Pocket.Hair, 0xBDE, 0x960C21, 0x960C21, 0x960C21);
		EquipItem(Pocket.Armor, 0x3AE6, 0xFFF8EC, 0x1D100C, 0x977855);
		EquipItem(Pocket.Shoe, 0x426F, 0x0, 0xB4A67F, 0x842160);

		SetLocation(region: 52, x: 36027, y: 39195);

		SetDirection(48);
		SetStand("human/male/anim/male_natural_stand_npc_riocard");
        
		Phrases.Add("Alright, please file a single line from here!");
		Phrases.Add("Can you wait for 5 minutes?");
		Phrases.Add("Coming right up!");
		Phrases.Add("Did you like it??");
		Phrases.Add("Hello hello!");
		Phrases.Add("Here's the special menu!");
		Phrases.Add("Hey, right here!");
		Phrases.Add("Please wait for a bit.");
		Phrases.Add("Taste the very best in this kingdom!");
		Phrases.Add("Thank you very much! I'll see you soon.");
		Phrases.Add("We have a seat here!");
		Phrases.Add("Welcome to Loch Lios!!");
		Phrases.Add("Welcome to Loch Lios, a restaurant located right on the lakeside!");
		Phrases.Add("Yes, we serve hearty food!");
	}
}
