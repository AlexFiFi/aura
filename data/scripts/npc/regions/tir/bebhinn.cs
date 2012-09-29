using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BebhinnScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_bebhinn");
		SetRace(10001);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 59, eyeColor: 55, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xF78042, 0x2A53, 0x6F6A57);
		EquipItem(Pocket.Hair, 0xC1C, 0x201C1A, 0x201C1A, 0x201C1A);
		EquipItem(Pocket.Armor, 0x15FFA, 0xFFE4BF, 0x1E649D, 0x175884);
		EquipItem(Pocket.Shoe, 0x4290, 0x996633, 0x6175AD, 0x808080);

		SetLocation(region: 2, x: 1364, y: 1785);

		SetDirection(228);
		SetStand("human/female/anim/female_natural_stand_npc_Bebhinn");

		Shop.AddTabs("Personal Shop License");

		Phrases.Add("Any city would be better than here, right?");
		Phrases.Add("I prefer rainy days over clear days.");
		Phrases.Add("It's soooo boring.");
		Phrases.Add("No matter what, I am going hiking this weekend.");
		Phrases.Add("Should I move out to the city?");
		Phrases.Add("So many good-looking men stopped by today...");
		Phrases.Add("There's nothing worse than a man who makes a woman wait.");
		Phrases.Add("Where would be a good place to spend my vacation?");
		Phrases.Add("Wow... I'm so pretty... Hehe.");

	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "A young lady is admiring her nails as you enter.",
			"When she notices you, she looks up expectantly, as if waiting for you to liven things up.",
			"Her big, blue eyes sparkle with charm and fun, and her subtle smile creates irresistable dimples.");
		MsgSelect(c, "May I help you?", "Start Conversation", "@talk", "Open My Account", "@bank", "Redeem Coupon", "@redeem", "Shop", "@shop");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@redeem":
				Msg(c, "Are you here to redeem your coupon?",
					"Please enter the coupon number you wish to redeem.");
				break;
			case "@shop":
				Msg(c, "So, does that mean you're looking for a Personal Shop License then?",
					"You must have something you want to sell around here!",
					"Hahaha...");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Is this your first time here? Nice to meet you.");
				Msg(c, true, false, "(Bebhinn is looking at me.)");
				ShowKeywords(c);
				break;
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
