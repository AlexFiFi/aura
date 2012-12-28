using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cordelia_madamScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cordelia_madam");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1.1f, upper: 0.9f, lower: 1.1f);
		SetFace(skin: 15, eye: 27, eyeColor: 170, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xCABADB, 0xFDDF58, 0x5F6B51);
		EquipItem(Pocket.Hair, 0xBDF, 0xA2E2C5, 0xA2E2C5, 0xA2E2C5);
		EquipItem(Pocket.Armor, 0x3AAE, 0xFFFFFF, 0x1065A8, 0xE4AE55);
		EquipItem(Pocket.Shoe, 0x426F, 0xD6C1B3, 0x388529, 0x808080);

		SetLocation(region: 4005, x: 30244, y: 32671);

		SetDirection(9);
		SetStand("chapter4/human/female/anim/female_c4_npc_cordelia");

		Phrases.Add("Ho ho, how fun!");
		Phrases.Add("I just heard something amazing!");
		Phrases.Add("Is there any interesting gossip?");
		Phrases.Add("Shh, don't go telling others about this!");
		Phrases.Add("We will continue with the next part of the story this time tomorrow!");
	}
}
