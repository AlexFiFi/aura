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

		EquipItem(Pocket.Face, 0xF3C, 0xF78042);
		EquipItem(Pocket.Hair, 0xC1C, 0x201C1A);
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
		Disable(Options.Face | Options.Name);
		Msg(c, "A young lady is admiring her nails as you enter.",
			"When she notices you, she looks up expectantly, as if waiting for you to liven things up.",
			"Her big, blue eyes sparkle with charm and fun, and her subtle smile creates irresistable dimples.");
		Enable(Options.Face | Options.Name);
		MsgSelect(c, "May I help you?", "Start Conversation", "@talk", "Open My Account", "@bank", "Redeem Coupon", "@redeem", "Shop", "@shop");
	}

	public override void OnSelect(WorldClient c, string r, string i)
	{
		switch (r)
		{
			case "@redeem":
				MsgInput(c, "Are you here to redeem your coupon?<br/>Please enter the coupon number you wish to redeem.", "Exchange Coupon", "Enter your coupon number");
				break;
				
			case "@input":
				// Check code in "i"
				Msg(c, "I checked the number at our Head Office, and they say this coupon does not exist.", "Please double check the coupon number.");
				break;
				
			case "@cancel":
				// Missing real response.
				Msg(c, "Come back any time.");
				break;
				
			case "@shop":
				Msg(c, "So, does that mean you're looking for a Personal Shop License then?",
					"You must have something you want to sell around here!",
					"Hahaha...");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Is this your first time here? Nice to meet you.");
				Disable(Options.Name);
				Msg(c, "(Bebhinn is looking at me.)");
				Enable(Options.Name);
				ShowKeywords(c);
				break;
				
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
