using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Scathach_alchemist_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

        	SetStand("");

		Phrases.Add("A bit chilly today, isn't it?");
		Phrases.Add("Ah, I'm so hungry.");
		Phrases.Add("Be careful, now.");
		Phrases.Add("Believe me, I'm not here by choice.");
		Phrases.Add("Captain Odran! Yoo-hoo!");
		Phrases.Add("Captain, over here! I'm over here!");
		Phrases.Add("Captain. Captain! CAPTAIN!!");
		Phrases.Add("Dullards everywhere!");
		Phrases.Add("Have you eaten?");
		Phrases.Add("Is it because I'm a Royal Alchemist? Is that why you're ignoring me?");
		Phrases.Add("Is that a bug...? Disgusting!");
		Phrases.Add("My throat hurts.");
		Phrases.Add("Savages.");
		Phrases.Add("Such filth!");
		Phrases.Add("That's cute.");
		Phrases.Add("The patrolmen are so...uncouth.");
		Phrases.Add("Welcome.");
		Phrases.Add("What shall I study today?");
		Phrases.Add("Why must I stay in this terrible place?");
		Phrases.Add("Why must I suffer so?");
		Phrases.Add("You can't even get decent tea out here!");
		Phrases.Add("You can't ignore me forever, captain!");
    }
}
