using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PadanScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_padan");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 23, eye: 87, eyeColor: 162, lip: 35);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x133B, 0x456668, 0xF35D6E, 0x296D5E);
		EquipItem(Pocket.Hair, 0x100C, 0xFFCF6B, 0xFFCF6B, 0xFFCF6B);
		EquipItem(Pocket.Armor, 0x3C1F, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 401, x: 82180, y: 122453);

		SetDirection(144);
		SetStand("");

		Phrases.Add("Can you be any more sluggish?");
		Phrases.Add("For King Ethur Mac Cuill!");
		Phrases.Add("It looks like it's going to rain, my shoulders are starting to ache.");
		Phrases.Add("It worries me that we are getting fewer and fewer recruits for the Royal Guard.");
		Phrases.Add("Our main enemy is the Fomor!");
		Phrases.Add("State your name and rank.");
		Phrases.Add("Those Fomors...");
		Phrases.Add("Wake up! Don't you hear the bell?!");
		Phrases.Add("Wake up! Wake up, soldier!");
		Phrases.Add("When we need a shadow hero...");
		Phrases.Add("Who would've thought that Shadow Realm would expand all the way to Tara...?");
	}
}
