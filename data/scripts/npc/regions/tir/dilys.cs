// Aura Script
// --------------------------------------------------------------------------
// Dilys - Healer 
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DilysScript : NPCScript
{
	const uint _healCost = 90;
	const uint _petHealCost = 180;

	public override void OnLoad()
	{
		SetName("_dilys");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 27, lip: 48);
		SetStand("human/female/anim/female_natural_stand_npc_Dilys_retake", "human/female/anim/female_natural_stand_npc_Dilys_talk");
		SetLocation("tir_healer", 1107, 1050, 195);

		EquipItem(Pocket.Face, 3908, 0xFFE170);
		EquipItem(Pocket.Hair, 3141, 0x633C31);
		EquipItem(Pocket.Armor, 15653, 0xFFFFFF, 0x61854B, 0xFFFFFF);
		EquipItem(Pocket.Glove, 16098, 0x61854B);
		EquipItem(Pocket.Shoe, 17285, 0xE8E8E8);

		Shop.AddTabs("Potions", "First Aid Kits", "Etc.");

        //----------------
        // Potions
        //----------------

        //Page 1
        Shop.AddItem("Potions", 51037, 10);	//Base Potion
        Shop.AddItem("Potions", 51001);		//HP 10 Potion
        Shop.AddItem("Potions", 51011);		//Stamina 10 Potion
        Shop.AddItem("Potions", 51000);		//Potion Concoction Kit
        Shop.AddItem("Potions", 51201, 1);	//Marionette 30 Potion
        Shop.AddItem("Potions", 51201, 10);	//Marionette 30 Potion
        Shop.AddItem("Potions", 51201, 20);	//Marionette 30 Potion
        Shop.AddItem("Potions", 51202, 1);	//Marionette 50 Potion
        Shop.AddItem("Potions", 51202, 10);	//Marionette 50 Potion
        Shop.AddItem("Potions", 51202, 20);	//Marionette 50 Potion
        Shop.AddItem("Potions", 51002, 1);	//HP 30 Potion
        Shop.AddItem("Potions", 51002, 10);	//HP 30 Potion
        Shop.AddItem("Potions", 51002, 20);	//HP 30 Potion
        Shop.AddItem("Potions", 51012, 1);	//Stamina 30 Potion
        Shop.AddItem("Potions", 51012, 10);	//Stamina 30 Potion
        Shop.AddItem("Potions", 51012, 20);	//Stamina 30 Potion

        //----------------
        // First Aid Kits
        //----------------

        //Page 1
        Shop.AddItem("First Aid Kits", 60005, 10);	//Bandage
        Shop.AddItem("First Aid Kits", 60005, 20);	//Bandage
        Shop.AddItem("First Aid Kits", 63000, 10);	//Phoenix Feather
        Shop.AddItem("First Aid Kits", 63000, 20);	//Phoenix Feather
        Shop.AddItem("First Aid Kits", 63032);		//Pet First-Aid Kit
        Shop.AddItem("First Aid Kits", 63716, 10);	//Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63716, 20);	//Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63715, 10);	//Fine Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63715, 20);	//Fine Marionette Repair Set

        //----------------
        // Etc.
        //----------------

        //Page 1
        Shop.AddItem("Etc.", 91563, 1);		//Hot Spring Ticket
        Shop.AddItem("Etc.", 91563, 5);		//Hot Spring Ticket

		Phrases.Add("I wish I could see the stars.");
		Phrases.Add("It's such a hassle to get all those ingredients for just one meal.");
		Phrases.Add("Men are all the same.");
		Phrases.Add("Perhaps I should order a safe this month.");
		Phrases.Add("Should I go to the market?");
		Phrases.Add("What should I cook for dinner tonight?");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "A tall, slim lady tinkers with various ointments, herbs, and bandages.<br/>She looks wise beyond her years, maybe because of the green healer dress she's wearing.<br/>Her dark hair is neatly combed, and her gentle brown eyes puts everyone who speaks to her at ease.<br/>She smiles faintly, waiting for you to speak.");

		MsgSelect(c, "Welcome to the Healer's House", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Get Treatment", "@heal"), Button("Heal Pet", "@healpet"));
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Welcome!");
				
			L_Keywords:
				Msg(c, Options.Name, "(Dilys is waiting for me to say something.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}

			case "@shop":
			{
				Msg(c, "What potion do you need?");
				OpenShop(c);
				End();
			}
				
			case "@heal":
			{
				if (c.Character.Life >= c.Character.LifeMax)
				{
					Msg(c, "You don't have a mark on you! You really shouldn't pretend to be sick... There are people out there that need my help!");
					End();
				}
				
				MsgSelect(c,
					"Goodness, <username/>! Are you hurt? I must treat your wounds immediately.<br/>I can't understand why everyone gets injured so much around here...<br/>The fee is " + _healCost + " Gold but don't think about money right now. What's important is that you get treated.",
					Button("Recieve Treatment", "@recieveheal"), Button("Decline", "@end")
				);
				
				r = Wait();
				if(r == "@recieveheal")
				{
					if (!c.Character.HasGold(_healCost))
					{
						Msg(c, "Oh no, I don't think you have enough gold.<br/>Hmmm... The gold covers the cost of the bandages and medicine...<br/>How about doing a part-time job?");
						End();
					}
					
					c.Character.RemoveGold(_healCost);
					c.Character.FullHealLife();
					
					Msg(c, "Good, I've put on some bandages and your treatment is done.<br/>If you get injured again, don't hesitate to visit me.");
				}
				
				break;
			}

			case "@healpet":
			{
				if (c.Character.Pet == null)
				{
					Msg(c, "You may want to summon your animal friend first.<br/>If you don't have a pet, then please don't waste my time.");
					End();
				}
				else if (c.Character.Pet.IsDead)
				{
					Msg(c, "Uh oh, you'll need to revive the pet first.<br/>Use a Phoenix Feather to revive your pet.");
					End();
				}
				else if (c.Character.Pet.Life >= c.Character.Pet.LifeMax)
				{
					Msg(c, "Your pet's as healthy as you are!<br/>I don't appreciate pranks.");
					End();
				}
				
				MsgSelect(c,
					"Oh no! " + c.Character.Name + ", your animal friend is badly hurt and needs to be treated right away.<br/>I don't know why so many animals are getting injured lately. It makes me worry.<br/>The treatment will cost " + _petHealCost + " Gold, but don't think of the price. Your pet needs help immediatly",
					Button("Recieve Treatment", "@recieveheal"), Button("Decline the Treatment", "@end")
				);
					
				r = Wait();
				if(r == "@recieveheal")
				{
					if (!c.Character.HasGold(_petHealCost))
					{
						Msg(c, "Oh no, I don't think you have enough gold.<br/>Hmmm... The gold covers the cost of the bandages and medicine...<br/>How about doing a part-time job?");
						End();
					}
					
					c.Character.RemoveGold(_petHealCost);
					c.Character.Pet.FullHealLife();
					
					Msg(c, "Okay, all bandaged up and ready to go!<br/>If your dear animal friend gets hurt again, do not hesitate to visit me.");
				}
				End();
			}
		}
	}
}
