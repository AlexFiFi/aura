// Aura Script
// --------------------------------------------------------------------------
// Caitin - Grocery Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CaitinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_caitin");
		SetRace(10001);
		SetBody(height: 1.3f);
		SetFace(skin: 16, eye: 2, eyeColor: 39, lip: 0);
		SetStand("human/female/anim/female_natural_stand_npc_Caitin_new", "human/female/anim/female_natural_stand_npc_Caitin_talk");
		SetLocation("tir_grocery", 1831, 1801, 64);

		EquipItem(Pocket.Face, 3900, 0x1AB67C, 0xF09D3B, 0x007244);
		EquipItem(Pocket.Hair, 3002, 0x683E33, 0x683E33, 0x683E33);
		EquipItem(Pocket.Armor, "Popo's Skirt", 0x708B3D, 0xFBE39B, 0x6D685F);
		EquipItem(Pocket.Shoe, "Cloth Shoes", 0x2A2A2A);

        Shop.AddTabs("Grocery", "Gift", "Quest", "Event");

        //----------------
        // Grocery
        //----------------

        //Page 1
        Shop.AddItem("Grocery", 5004);		//Bread
        Shop.AddItem("Grocery", 5002);		//Slice of Cheese
        Shop.AddItem("Grocery", 50111);		//Carrot
        Shop.AddItem("Grocery", 50131);		//Sugar
        Shop.AddItem("Grocery", 50131, 10);	//Sugar x10
        Shop.AddItem("Grocery", 50132);		//Salt
        Shop.AddItem("Grocery", 50132, 10);	//Salt x10
        Shop.AddItem("Grocery", 50156);		//Pepper
        Shop.AddItem("Grocery", 50156, 10);	//Pepper x10
        Shop.AddItem("Grocery", 50153);		//Frying Powder
        Shop.AddItem("Grocery", 50148);		//Yeast
        Shop.AddItem("Grocery", 50148, 10);	//Yeast x10
        Shop.AddItem("Grocery", 50130);		//Whipping Cream
        Shop.AddItem("Grocery", 50130, 5);	//Whipping Cream x5
        Shop.AddItem("Grocery", 50112);		//Strawberry
        Shop.AddItem("Grocery", 50112, 10);	//Strawberry x10
        Shop.AddItem("Grocery", 50121);		//Butter
        Shop.AddItem("Grocery", 50121, 10);	//Butter x10
        Shop.AddItem("Grocery", 50142);		//Onion
        Shop.AddItem("Grocery", 50142, 10);	//Onion x10
        Shop.AddItem("Grocery", 50138);		//Cabbage
        Shop.AddItem("Grocery", 50139);		//Mushroom
        Shop.AddItem("Grocery", 50145);		//Olive Oil
        Shop.AddItem("Grocery", 50145, 10);	//Olive Oil x10
        Shop.AddItem("Grocery", 50005);		//Large Meat
        Shop.AddItem("Grocery", 50001);		//Big Lump of Cheese
        Shop.AddItem("Grocery", 50006, 5);	//Slice of Meat x5
        Shop.AddItem("Grocery", 50134);		//Sliced Bread
        Shop.AddItem("Grocery", 50120);		//Steamed Rice
        Shop.AddItem("Grocery", 50104);		//Egg Salad

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52010);		//Ramen
        Shop.AddItem("Gift", 52021);		//Slice of Cake
        Shop.AddItem("Gift", 52019);		//Heart Cake
        Shop.AddItem("Gift", 52022);		//Wine
        Shop.AddItem("Gift", 52023);		//Wild Ginseng

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest


		Phrases.Add("*Yawn*");
		Phrases.Add("Hmm... Sales are low today... That isn't good.");
		Phrases.Add("I am a little tired.");
		Phrases.Add("I have to finish these bills... I'm already behind schedule.");
		Phrases.Add("I must have had a bad dream.");
		Phrases.Add("It's about time for customers to start coming in.");
		Phrases.Add("My body feels stiff all over.");
		Phrases.Add("These vegetables are spoiling...");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, "What can I do for you?", Button("Start Conversation", "@talk"), Button("Shop"));
		
		var r = Select(c);
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Nice to meet you.");
				
			L_Keywords:
				Msg(c, Options.Name, "(Caitin is looking in my direction.)");
				//Msg(c, Options.Name, "(That was a great conversation!)");
				ShowKeywords(c);
				
				var keyword = Select(c);
				switch(keyword)
				{
					case "personal_info":
					{
						Msg(c, "My grandmother named me.<br/>I work here at the Grocery Store, so I know one important thing.<br/>You have to eat to survive!<br/>Food helps you regain your Stamina.");
						Msg(c, "That doesn't mean you can eat just everything.<br/>You shouldn't have too much greasy food<br/>because you could gain a lot of weight.");
						Msg(c, "Huh? You have food with you but don't know how to eat it?<br/>Okay, open the Inventory and right-click on the food.<br/>Then, click \"Use\" to eat.<br/>If you have bread in your Inventory, and your Stamina is low,<br/>try eating it now.");
						break;
					}
					
					default:
					{
						Msg(c, "Can we change the subject?");
						break;
					}
				}
				
				goto L_Keywords;
			}
			
			case "@shop":
			{
				Msg(c, "Welcome to the Grocery Store.<br/>There is a variety of fresh food and ingredients for you to choose from.");
				OpenShop(c);
				End();
			}
		}
	}
}
