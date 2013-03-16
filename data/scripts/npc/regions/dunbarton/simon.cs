using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SimonScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_simon");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.8f, upper: 0.8f, lower: 0.8f);
		SetFace(skin: 15, eye: 8, eyeColor: 25, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1326, 0x9B6D85, 0x494966, 0x30B075);
		EquipItem(Pocket.Hair, 0xFB8, 0x998866, 0x998866, 0x998866);
		EquipItem(Pocket.Armor, 0x3AC5, 0xD6D8DE, 0x31208E, 0xFF9B3B);
		EquipItem(Pocket.Shoe, 0x4275, 0x9C7B6B, 0xF79825, 0x7335);

		SetLocation(region: 17, x: 1314, y: 921);

		SetDirection(24);
		SetStand("human/male/anim/male_natural_stand_npc_Simon");
        
        Phrases.Add("Heeheehee... She's got some fashion sense.");
		Phrases.Add("Let's see... Which ones do I have to finish by today?");
		Phrases.Add("Oops! I was supposed to do that tomorrow, wasn't I? Heehee...");
		Phrases.Add("That man over there... What he's wearing is so 20 minutes ago.");
		Phrases.Add("This... is too last-minute.");
		Phrases.Add("Time just flies today. Heh.");
		Phrases.Add("Travelers... How are they so careless about their appearance?");
		Phrases.Add("Ugh! This world is so devoid of beauty.");
	}
}
