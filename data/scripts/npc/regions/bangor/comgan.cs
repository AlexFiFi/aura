// Aura Script
// --------------------------------------------------------------------------
// Comgan - Healer Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ComganScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_comgan");
		SetRace(10002);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 55, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0x977726, 0xFFFCDE, 0x35003F);
		EquipItem(Pocket.Hair, 0xFA3, 0xFFFFFFF, 0xFFFFFFF, 0xFFFFFFF);
		EquipItem(Pocket.Armor, 0x3AD4, 0x400000, 0xF0EA9D, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4277, 0x0, 0xF4638B, 0xF9EF64);

		SetLocation(region: 31, x: 15329, y: 12122);

		SetDirection(154);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");

        Shop.AddTabs("Potions", "First Aid Kits", "Quest", "Party Quest", "Etc.");

        //----------------
        // Potions
        //----------------

        //Page 1
        Shop.AddItem("Potions", 51001);		//HP 10 Potion
        Shop.AddItem("Potions", 51011);		//Stamina 10 Potion
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
        Shop.AddItem("First Aid Kits", 63001);		//Wings of a Goddess
        Shop.AddItem("First Aid Kits", 63001, 5);	//Wings of a Goddess
        Shop.AddItem("First Aid Kits", 63716, 10);	//Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63716, 20);	//Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63715, 10);	//Fine Marionette Repair Set
        Shop.AddItem("First Aid Kits", 63715, 20);	//Fine Marionette Repair Set

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70111);		//Hunting Quest
        Shop.AddItem("Quest", 70118);		//Hunting Quest
        Shop.AddItem("Quest", 70106);		//Hunting Quest
        Shop.AddItem("Quest", 70105);		//Hunting Quest
        Shop.AddItem("Quest", 70139);		//Hunting Quest

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest
        Shop.AddItem("Party Quest", 70025);	//Party Quest

        //----------------
        // Etc.
        //----------------

        //Page 1
        Shop.AddItem("Etc.", 91563, 1);		//Hot Spring Ticket
        Shop.AddItem("Etc.", 91563, 5);		//Hot Spring Ticket

		Phrases.Add("...");
		Phrases.Add("I guess only people like me would understand what I'm saying...");
		Phrases.Add("I need to build a Church soon...");
		Phrases.Add("Lord Lymilark...");
		Phrases.Add("Oh Lymilark, please provide me with strength and courage... Like you did that day...");
		Phrases.Add("Selling gifts to build a church would be... Is that feasible?");
		Phrases.Add("There are more important things in life than what we merely see...");
		Phrases.Add("What must I do...");
		Phrases.Add("What should I do to convert more people?");
		Phrases.Add("What should I do...");
		Phrases.Add("Why do people ignore what I say...");

	}
    
    public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "This boy is wearing a priest's robe with wide necklines showing that he has on many layers of clothing.<br/>The color of his thick hair looks like feather clouds floating above the Bangor sky.<br/>Blue eyes like a deep, trainquil ocean add a gentle radiance to his slightly tilted face.<br/>His gentle smile shows his godly manner.");
		MsgSelect(c, "Do you... believe in God?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Have we met?");
				
			L_Keywords:
				Msg(c, Options.Name, "(Comgan is slowly looking me over.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}

			case "@shop":
			{
				Msg(c, "What is it that you need?<br/>Please take a look.");
				OpenShop(c);
				End();
			}
		}
	}
}
