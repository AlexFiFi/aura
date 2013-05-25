// Aura Script
// --------------------------------------------------------------------------
// Manus - Healer Shop of Dunbarton
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ManusScript : NPCScript
{
	const uint _healCost = 90;
	const uint _petHealCost = 180;

	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_manus");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 12, eyeColor: 27, lip: 18);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0x18B475, 0x87912F, 0x7A4D00);
		EquipItem(Pocket.Hair, 0x1000, 0x2B2822, 0x2B2822, 0x2B2822);
		EquipItem(Pocket.Armor, 0x3AB6, 0xCFD0B5, 0x6600, 0x6600);
		EquipItem(Pocket.Shoe, 0x428B, 0x223846, 0x574662, 0x808080);

		SetLocation(region: 19, x: 881, y: 1194);

		SetDirection(0);
		SetStand("");

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
        Shop.AddItem("Etc.", 1044);		//Reshaping Your Body
        Shop.AddItem("Etc.", 1047);		//On Effective Treatment of Wounds
        Shop.AddItem("Etc.", 91563, 1);		//Hot Spring Ticket
        Shop.AddItem("Etc.", 91563, 5);		//Hot Spring Ticket
        
		Phrases.Add("A healthy body for a healthy mind!");
		Phrases.Add("Alright! Here we go! Woo-hoo!");
		Phrases.Add("Come! A special potion concocted by Manus for sale now!");
		Phrases.Add("Here, let's have a look.");
		Phrases.Add("I wish there was something I could spend this extra energy on...");
		Phrases.Add("Perhaps Stewart could tell me about this...");
		Phrases.Add("There's nothing like a massage for relief when your muscles are tight! Hahaha!");
		Phrases.Add("Why did you let it go this bad?!");
		Phrases.Add("You should exercise more. You're so thin.");

	}
	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "This man is wearing a green and white healer's dress.<br/>His thick, dark hair is immaculately combed and reaches down to his neck,<br/>his straight bangs accentuating a strong jaw and prominent cheeckbones.");
		Msg(c, "You've never been here before, have you? Where does it hurt?");
		MsgSelect(c, "Ha! Tell me everything you need!", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "You look familiar. Haven't we met before?");
				
			L_Keywords:
				Msg(c, Options.Name, "(Manus is looking at me.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}

			case "@shop":
			{
				Msg(c, "Is there something i can help you with?");
				OpenShop(c);
				End();
			}
			case "@heal":
			{
				if (c.Character.Life >= c.Character.LifeMax)
				{
					Msg(c, "I don't see any injuries that you need treatment for.<br/>If you have a broken heart, I think you need to go see a Counselor instead of a Healer...");
					End();
				}
				
				MsgSelect(c,
					"Goodness, " + c.Character.Name + "! Are you hurt? I must treat your wounds immediately.<br/>I can't understand why everyone gets injured so much around here...<br/>The fee is " + _healCost + " Gold but don't think about money right now. What's important is that you get treated.",
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
					Msg(c, "Would you like to show me your animal friend first?");
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
					"Oh my goodness... " + c.Character.Name + ", your animal friend needs to be treated right away.<br/>It will cost " + _petHealCost + " Gold to go ahead with it... would you like to treat your pet?",
					Button("Recieve Treatment", "@recieveheal"), Button("Decline the Treatment", "@end")
				);
					
				r = Wait();
				if(r == "@recieveheal")
				{
					if (!c.Character.HasGold(_petHealCost))
					{
						Msg(c, "Ummm... " + c.Character.Name + "?<br/>I am sorry, but i think you are short on cash. I can't treat your pet then.");
						End();
					}
					
					c.Character.RemoveGold(_petHealCost);
					c.Character.Pet.FullHealLife();
					
					Msg(c, "The treatment is complete.<br/>I wish you'd treat your pet with more respect, as much as you'd have for yourself.");
				}
				End();
			}
		}
	}
}
