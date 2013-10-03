using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class OslaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_osla");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 2, eyeColor: 47, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xF5A73F, 0xF69C35, 0xF98838);
		EquipItem(Pocket.Hair, 0xBDD, 0xE29B45, 0xE29B45, 0xE29B45);
		EquipItem(Pocket.Armor, 0x32DF, 0xD6C5BA, 0x947E6B, 0x0);
		EquipItem(Pocket.Shoe, 0x4466, 0x61534E, 0xF6D493, 0x644356);

		SetLocation(region: 52, x: 37398, y: 42382);

		SetDirection(28);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");
        
		Phrases.Add("...Ahhh... What's wrong with me...");
		Phrases.Add("...But I guess it's okay since I'm prettty...");
		Phrases.Add("Ahhh... I forgoooot.");
		Phrases.Add("Ahhh... I wrote it down somewhere");
		Phrases.Add("Hmm... where's my knight in shining armor?");
		Phrases.Add("Hmm...I don't look that bad...not bad at all...");
		Phrases.Add("I keep getting the numbers wrong...");
		Phrases.Add("I wonder where my prince charming is...");
		Phrases.Add("I wonder why I'm so out of it these days...");
		Phrases.Add("It's a problem if I have too many customers to take care of...");
		Phrases.Add("Narrrr....");
		Phrases.Add("Oh no... another bounced check...");
		Phrases.Add("What was I thinking...");
		Phrases.Add("Why do I keep forgetting things...");
		Phrases.Add("Your item is repaired! Please take it.");
	}
}
