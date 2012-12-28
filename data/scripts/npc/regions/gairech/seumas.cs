using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SeumasScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_seumas");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 24, eye: 7, eyeColor: 39, lip: 4);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1356, 0x9582BD, 0x821F60, 0xC57782);
		EquipItem(Pocket.Hair, 0xFAF, 0x2D2B13, 0x2D2B13, 0x2D2B13);
		EquipItem(Pocket.Armor, 0x3AC4, 0x514940, 0xCDAD7C, 0xE6E6E6);
		EquipItem(Pocket.Glove, 0x3E91, 0x827148, 0xA2003A, 0x73A5);
		EquipItem(Pocket.Shoe, 0x427E, 0x0, 0x100041, 0xB60180);
		EquipItem(Pocket.Head, 0x4668, 0x964D25, 0xCAA859, 0x1A958);
		EquipItem(Pocket.RightHand1, 0x9C59, 0x454545, 0x745D2F, 0xEEA140);

		SetLocation(region: 30, x: 38334, y: 48677);

		SetDirection(238);
		SetStand("human/anim/tool/Rhand_A/female_tool_Rhand_A02_mining");
        
		Phrases.Add("(gasp, gasp))");
		Phrases.Add("75... 76... 77...");
		Phrases.Add("A little bit more... A little bit more and I'll rest...");
		Phrases.Add("I keep getting stuck on these rocks...");
		Phrases.Add("La la la!");
		Phrases.Add("Let's go! Let's do it!");
		Phrases.Add("Oh, no! I forgot.");
		Phrases.Add("Start all over again!");
		Phrases.Add("This is nothing!");
		Phrases.Add("We'd better hurry.");
		Phrases.Add("Yo-ho! Yo-ho!");
	}
}
