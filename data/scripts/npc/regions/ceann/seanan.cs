using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SeananScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_seanan");
		SetRace(10002);
		SetBody(height: 0.65f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 26, eye: 30, eyeColor: 39, lip: 18);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xFBA184, 0xA4A61C, 0xDCD8EC);
		EquipItem(Pocket.Hair, 0xFF9, 0xFFDBD180, 0xFFDBD180, 0xFFDBD180);
		EquipItem(Pocket.Armor, 0x3B22, 0xFF3F9A5F, 0xFF28300E, 0xFF073635);
		EquipItem(Pocket.Shoe, 0x429F, 0xFF0A5A4F, 0xFF808000, 0xFF808000);

		SetLocation(region: 100, x: 45300, y: 42650);

		SetDirection(59);
		SetStand("");
        
		Phrases.Add("Hey, watch out!");
		Phrases.Add("I hate gusts of wind!");
		Phrases.Add("I want to ride on the ship!!");
		Phrases.Add("Is there anyone here that gets seasick?");
		Phrases.Add("Let's go let's go!");
		Phrases.Add("Okay, line up line up");
		Phrases.Add("Shall I go fishing?");
		Phrases.Add("Should I practice weaving?");
		Phrases.Add("What would tomorrow's weather be like?");
		Phrases.Add("When's the captain coming?");
		Phrases.Add("Wow, this is sooo boring");
	}
}
