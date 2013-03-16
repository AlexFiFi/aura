using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GlewyasScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_glewyas");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 100, eyeColor: 26, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x133B, 0xFDC0AB, 0x7E0A6C, 0x96D1F1);
		EquipItem(Pocket.Hair, 0x1012, 0xD4B6A0, 0xD4B6A0, 0xD4B6A0);
		EquipItem(Pocket.Armor, 0x3C4C, 0xE5D2C8, 0xFFFFFF, 0x0);
		EquipItem(Pocket.Shoe, 0x42F2, 0x213547, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4753, 0x2F5D74, 0xB6B6B6, 0xC08FD5);

		SetLocation(region: 410, x: 20505, y: 18728);

		SetDirection(230);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");

		Phrases.Add("A stew must boil to get the flavor into all the meat.");
		Phrases.Add("Ah, today's produce is fresh and crisp. Simply divine.");
		Phrases.Add("Arzhela, your hair glows like lightly sauteed potatoes!");
		Phrases.Add("Come to me, all ye who are hungry, and I shall make your taste buds sing!");
		Phrases.Add("Cooking a rare and sumptuous recipe is worth any sacrifice!");
		Phrases.Add("Cooking is like a competitive sport!");
		Phrases.Add("Don't live a medium rare life.");
		Phrases.Add("Eeeeeek! Rat!!");
		Phrases.Add("Eh? What's this I smell?");
		Phrases.Add("I'm actually hungry. How ironic.");
		Phrases.Add("Oh, Azrhela, my goddess!");
		Phrases.Add("Perhaps I should get a cat...");
		Phrases.Add("Real men like it RAW!");
		Phrases.Add("What shall I whip up for tomorrow's party?");
		Phrases.Add("With an open heart and an empty stomach, I say unto you: Allez Cuisine!");
	}
}
