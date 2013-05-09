// Aura Script
// --------------------------------------------------------------------------
// Goro - Battle Arena Goblin
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GoroScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_goro");
		SetRace(10105);
		SetBody(height: 0.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 32, eye: 3, eyeColor: 7, lip: 2);
		SetLocation("alby_arena_lobby", 1283, 3485, 198);
		SetStand("monster/anim/goblin/natural/goblin01_natural_stand_friendly");

		EquipItem(Pocket.Shoe, 0x426D, 0x441A19, 0x695C66, 0xBADB);
		EquipItem(Pocket.LeftHand1, 0x9C47, 0x405062, 0x7F7237, 0x729E);
		EquipItem(Pocket.RightHand1, 0xB3B1, 0x4F4F4B, 0x746C54, 0x4D1D77);

		Shop.AddTabs("Arena", "Potions");

        //----------------
        // Arena
        //----------------

        //Page 1
        Shop.AddItem("Arena", 63019, 10);	//Alby Battle Arena Coin
        Shop.AddItem("Arena", 63019, 20);	//Alby Battle Arena Coin
        Shop.AddItem("Arena", 63019, 50);	//Alby Battle Arena Coin
        Shop.AddItem("Arena", 63019, 100);	//Alby Battle Arena Coin

        //----------------
        // Potions
        //----------------

        //Page 1
        Shop.AddItem("Potions", 60005, 10);	//Bandage
        Shop.AddItem("Potions", 60005, 20);	//Bandage
        Shop.AddItem("Potions", 63000, 10);	//Phoenix Feather
        Shop.AddItem("Potions", 63000, 20);	//Phoenix Feather
        Shop.AddItem("Potions", 51002);		//HP 30 Potion
        Shop.AddItem("Potions", 51002, 20);	//HP 30 Potion
        Shop.AddItem("Potions", 51012);		//Stamina 30 Potion
        Shop.AddItem("Potions", 51012, 20);	//Stamina 30 Potion

		Phrases.Add("Here, you may enter Alby Arena.");
		Phrases.Add("Test your strength here, in Alby Arena!");
		Phrases.Add("Wait, do not attack.");
		Phrases.Add("We exchange Stars with Arena Coins.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"With his rough skin, menacing face, and his constant hard-breathing,",
			"he has the sure look of a Goblin.",
			"Yet, there is something different about this one.",
			"Strangely, it appears to have a sense of noble demeanor that does not match its rugged looks."
		);

		MsgSelect(c, "How can I help you?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Welcome, what a smart man you are.<br/>My name is Goro.<br/>Ah, do not be surprised. I have no intention of hurting you.");
			
			L_Keywords:
				Msg(c, Options.Name, "(Goro is looking at me)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}
			
			case "@shop":
			{
				Msg(c, "What do you need?<br/>You must be fully prepared if you wish to enter the Battle Arena.");
				OpenShop(c);
				End();
			}
		}
	}
}
