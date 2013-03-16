using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KaynaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kayna");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1.4f, upper: 1.4f, lower: 1f);
		SetFace(skin: 15, eye: 60, eyeColor: 27, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0xDE9853, 0x2E3B65, 0x6E6365);
		EquipItem(Pocket.Hair, 0x138F, 0x756939, 0x756939, 0x756939);
		EquipItem(Pocket.Armor, 0x3BFB, 0x825642, 0xB0ADAB, 0xEFE3B5);
		EquipItem(Pocket.Shoe, 0x42AA, 0x633C31, 0x6E9677, 0x808080);

		SetLocation(region: 23, x: 26559, y: 38870);

		SetDirection(227);
		SetStand("chapter4/human/female/anim/female_c4_npc_kayna");
        
		Phrases.Add("Do you see anything you like?");
		Phrases.Add("Hey there! Come on over!");
		Phrases.Add("I don't like the look on that person's face.");
		Phrases.Add("If things keep on like this, I'm going to run out of space for arrows.");
		Phrases.Add("If you need anything, just let ole' Kayna know.");
		Phrases.Add("Look at those muscles. Cadoc is a hunk!");
		Phrases.Add("Oh my, I forgot to do something.");
		Phrases.Add("There haven't been that many customers this month.");
		Phrases.Add("Time is passing so slowly...");
		Phrases.Add("Welcome!");
	}
}
