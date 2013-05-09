// Aura Script
// --------------------------------------------------------------------------
// Jenifer - Cafe Owner
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class JeniferScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_jenifer");
		SetRace(10001);
		SetBody(height: 1.1f, fat: 1.1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 17, eye: 4, eyeColor: 119, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3D, 0xB4BDE0, 0xD4E1F3, 0x717172);
		EquipItem(Pocket.Hair, 0xBB9, 0x240C1A, 0x240C1A, 0x240C1A);
		EquipItem(Pocket.Armor, 0x3AAC, 0xF98C84, 0xFBDDD7, 0x351311);
		EquipItem(Pocket.Shoe, 0x4275, 0x0, 0x366961, 0xDAD6EB);

		SetLocation(region: 31, x: 14628, y: 8056);

		SetDirection(26);
		SetStand("human/female/anim/female_natural_stand_npc_lassar");

        Shop.AddTabs("Food", "Gift", "Cooking Tools", "Event");

        //----------------
        // Food
        //----------------

        //Page 1
        Shop.AddItem("Food", 50004);		//Bread
        Shop.AddItem("Food", 50002);		//Slice of Cheese
        Shop.AddItem("Food", 50005);		//Large Meat
        Shop.AddItem("Food", 50001);		//Big Lump of Cheese
        Shop.AddItem("Food", 50006, 5);		//Slice of Meat

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
        // Cooking Tools
        //----------------

        //Page 1
        Shop.AddItem("Cooking Tools", 40042);	//Cooking Knife
        Shop.AddItem("Cooking Tools", 40044);	//Ladle"
        Shop.AddItem("Cooking Tools", 40043);	//Rolling Pin
        Shop.AddItem("Cooking Tools", 46005);	//Cooking Table
        Shop.AddItem("Cooking Tools", 46004);	//Cooking Pot
        
        Phrases.Add("Ah, I'm so bored...");
		Phrases.Add("Ah. What an unbelievably beautiful weather...");
		Phrases.Add("I could never keep this place clean... It always gets dirty.");
		Phrases.Add("I thought there was something else that needed to be done...");
		Phrases.Add("I wish it would rain so I could take a day off.");
		Phrases.Add("I'm gonna get drunk if I drink too much...");
		Phrases.Add("I'm so tired...");
		Phrases.Add("It would be nice if Riocard drank...");
		Phrases.Add("Perhaps I should lose some weight...");
		Phrases.Add("Riocard! Did you finish everything I asked you to do?");
		Phrases.Add("Riocard. Come play with me.");
		Phrases.Add("Today's fortune is... no profit?");
		Phrases.Add("Wait a minute...");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName,
            "Well-groomed purple hair, a face as smooth as flawless porcelain,",
            "and brown eyes with thick mascara complemented by a mole that adds beauty to her oval faced.",
            "The jasmine scent fills the air every time her light sepia healer dress moves,",
            "and her red cross earrings dangle and shine as her smile spreads across her lips."
        );
        MsgSelect(c, "Mmm? How can I help you?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Welcome to the Bangor Pub. Are you a first-time visitor?");

            L_Keywords:
                Msg(c, Options.Name, "(Jennifer is looking in my direction.)");
                ShowKeywords(c);

                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }

			case "@shop":
            {
                Msg(c, "What do you need?<br/>Right now, we're just selling a few food items.");
                OpenShop(c);
                End();
            }

			case "@repair":
            {
                MsgSelect(c,
                "I can fix accessories.<br/>I know this sounds funny coming from me, but it's not very good to repair accessories.<br/>First off, it costs to much.<br/>You might be better off buying a new one than repairing it. But if you still want to repair it...",
                Button("End Conversation", "@endrepair")
            );
            
            r = Wait();
            
            Msg(c, "It must be very precious to you if you want to repair an accessory.<br/>I totally understand. But it takes on another kind of charm as it tarnishes, you know?");
            Msg(c, "Well, see you again.");
            End();
        }
        }
    }
}
