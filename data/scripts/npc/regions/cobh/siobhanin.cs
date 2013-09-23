// Aura Script
// --------------------------------------------------------------------------
// Siobhanin - General Store
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SiobhaninScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_siobhanin");
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 41, eyeColor: 168, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1AF4, 0x3348, 0xF8A561, 0xF49D33);
		EquipItem(Pocket.Hair, 0x177A, 0xE3A400, 0xE3A400, 0xE3A400);
		EquipItem(Pocket.Armor, 0x3CD4, 0xE6D5CA, 0x455562, 0x63696E);
		EquipItem(Pocket.Shoe, 0x4325, 0x9EB6BE, 0x8C5A00, 0x95BF49);

		SetLocation(region: 23, x: 26852, y: 36426);

		SetDirection(37);
		SetStand("chapter4/elf/male/anim/elf_npc_siobhanin");

        Shop.AddTabs("General Goods", "Cooking Tools","Gift", "Event");

        //----------------
        // General Goods
        //----------------

        //Page 1
        Shop.AddItem("General Goods", 40093);		//Pet Instructor Stick
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 64018, 10);	//Paper (Big)
        Shop.AddItem("General Goods", 64018, 100);	//Paper (Big)
        Shop.AddItem("General Goods", 62021, 100);	//Six-sided Die
        Shop.AddItem("General Goods", 63020);		//Empty Bottle
        Shop.AddItem("General Goods", 60045);		//Handicraft Kit
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 50078);		//Ticking Quiz Bomb
        Shop.AddItem("General Goods", 2001);		//Gold Pouch
        Shop.AddItem("General Goods", 40215);		//Snare Drum
        Shop.AddItem("General Goods", 2006);		//Big Gold Pouch
        Shop.AddItem("General Goods", 40017);		//Mandolin
        Shop.AddItem("General Goods", 40017);		//Mandolin
        Shop.AddItem("General Goods", 40017);		//Mandolin

        //Page 2
        Shop.AddItem("General Goods", 2005);		//Item Bag (7x5)
        Shop.AddItem("General Goods", 2029);		//Item Bag (8x6)
        Shop.AddItem("General Goods", 2024);		//Item Bag (7x6)
        Shop.AddItem("General Goods", 2026);		//Item Bag(44)
        Shop.AddItem("General Goods", 18158);		//Conky Glasses
        Shop.AddItem("General Goods", 91364);		//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 91364, 10);	//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 2038);		//Item Bag (8x10)
        Shop.AddItem("General Goods", 18028);		//Folding Glasses
        Shop.AddItem("General Goods", 91365);		//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 91365, 10);	//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 85571);		//Reforging Tool
        Shop.AddItem("General Goods", 91366);		//Seal Scroll (30-day)
        Shop.AddItem("General Goods", 91366, 10);	//Seal Scroll (30-day)

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52011);			//Socks
        Shop.AddItem("Gift", 52018);			//Hammer
        Shop.AddItem("Gift", 52008);			//Anthology
        Shop.AddItem("Gift", 52009);			//Cubic Puzzle
        Shop.AddItem("Gift", 52017);			//Underwear Set

        //----------------
        // Cooking Appliances
        //----------------

        //Page 1
        Shop.AddItem("Cooking Tools", 40042);		//Cooking Knife
        Shop.AddItem("Cooking Tools", 40044);		//Ladle
        Shop.AddItem("Cooking Tools", 40043);		//Rolling Pin
        Shop.AddItem("Cooking Tools", 46005);		//Cooking Table
        Shop.AddItem("Cooking Tools", 46004);		//Cooking Pot
        
		Phrases.Add("Admiral Owen really is quite an amazing person!");
		Phrases.Add("Don't worry about buying something. You can look all you want.");
		Phrases.Add("Ha-ha!");
		Phrases.Add("I'll give you 4 for 30 Gold!");
		Phrases.Add("My back is getting tired from standing here.");
		Phrases.Add("Sigh...");
		Phrases.Add("That one is 50 Gold.");
		Phrases.Add("What brings you here?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "A young elven boy with snow-white skin and startlingly<br/>blue eyes. His delicate fingers work at tidying the collar<br/>of his neat shirt. His smiling lips seem to ask: How much<br/>have you found out so far?");
        Msg(c, "Come look at my items. I have so much to offer!", Button("Shop", "@shop"), Button("Upgrade Item", "@upgrade"));

        var r = Select(c);
        switch (r)
        {
            case "@shop":
            {
                Msg(c, "Take a look around.");
                OpenShop(c);
                End();
            }
            case "@upgrade":
            {
                Msg(c,
                    "All right, what would you like to upgrade?",
                    Button("End Conversation", "@endupgrade")
                );

                r = Select(c);

                Msg(c, "Yes, come again!");
                End();
            }
        }
    }
}
