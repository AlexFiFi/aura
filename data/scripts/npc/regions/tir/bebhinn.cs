// Aura Script
// --------------------------------------------------------------------------
// Bebhinn - Banker 
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BebhinnScript : NPCScript
{
	const bool KR = false; // White Bebhinn

	public override void OnLoad()
	{
		SetName("_bebhinn");
		SetRace(10001);
		SetStand("human/female/anim/female_natural_stand_npc_Bebhinn");
		SetLocation("tir_bank", 1364, 1785, 228);

		if(!KR)
		{
			SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
			SetFace(skin: 27, eye: 59, eyeColor: 55, lip: 1);
			EquipItem(Pocket.Face, 3900, 0xF78042);
			EquipItem(Pocket.Hair, 3100, 0x201C1A);
			EquipItem(Pocket.Armor, 90106, 0xFFE4BF, 0x1E649D, 0x175884);
			EquipItem(Pocket.Shoe, 17040, 0x996633, 0x6175AD, 0x808080);
		}
		else
		{
			SetBody(height: 0.88f, fat: 1f, upper: 1f, lower: 1f);
			SetFace(skin: 17, eye: 0, eyeColor: 127, lip: 0);
			EquipItem(Pocket.Face, 3900, 0x019E49, 0x031A5E, 0x8B78B7);
			EquipItem(Pocket.Hair, 3024, 0xF78F87, 0xF78F87, 0xF78F87);
			EquipItem(Pocket.Armor, 15026, 0xE9E3F1, 0x8D82AA, 0x4E435F);
			EquipItem(Pocket.Shoe, 17004, 0x4E435F, 0xA0927D, 0x4F548D);
		}

		Shop.AddTabs("License");

        //----------------
        // License
        //----------------

        //Page 1
        Shop.AddItem("License", 60101); //Tir Chonaill Merchant License
        Shop.AddItem("License", 81010); //Purple Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81011); //Pink Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81012); //Green Personal Shop Brownie Work-For-Hire Contract

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

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "A young lady is admiring her nails as you enter.<br/>When she notices you, she looks up expectantly, as if waiting for you to liven things up.<br/>Her big, blue eyes sparkle with charm and fun, and her subtle smile creates irresistable dimples.");

		Msg(c, "May I help you?", Button("Start Conversation", "@talk"), Button("Open My Account", "@bank"), Button("Redeem Coupon", "@redeem"), Button("Shop", "@shop"));
		
		var r = Select(c);
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Is this your first time here? Nice to meet you.");
				
			L_Keywords:
				Msg(c, Options.Name, "(Bebhinn is looking at me.)");
				ShowKeywords(c);
				var keyword = Select(c);
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}
				
			case "@bank":
			{
				Msg(c, "(Unimplemented)");
				End();
			}
				
			case "@redeem":
			{
				Msg(c, "Are you here to redeem your coupon?<br/>Please enter the coupon number you wish to redeem.", Input("Exchange Coupon", "Enter your coupon number"));
				var input = Select(c);
				if(input == "@cancel")
					End();
				
				if(!CheckCode(c, input))
				{
					Msg(c, "I checked the number at our Head Office, and they say this coupon does not exist.<br/>Please double check the coupon number.");
					End();
				}
				
				// Unofficial response.
				Msg(c, "There you go, have a nice day.");
				End();
			}
				
			case "@shop":
			{
				Msg(c, "So, does that mean you're looking for a Personal Shop License then?<br/>You must have something you want to sell around here!<br/>Hahaha...");
				OpenShop(c);
				End();
			}
		}
	}
}
