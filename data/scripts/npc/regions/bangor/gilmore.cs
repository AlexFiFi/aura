// Aura Script
// --------------------------------------------------------------------------
// Gilmore - General Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GilmoreScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_gilmore");
		SetRace(10002);
		SetBody(height: 0.8000003f, fat: 0.4f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 7, eyeColor: 76, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1327, 0x719235, 0x496D4A, 0xF2A945);
		EquipItem(Pocket.Hair, 0xFBA, 0x896D43, 0x896D43, 0x896D43);
		EquipItem(Pocket.Armor, 0x3A9B, 0xB6CAAA, 0x584232, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xA68DC3, 0x1B24B);
		EquipItem(Pocket.Head, 0x466C, 0x0, 0xC8C6C4, 0xDFE9A7);

		SetLocation(region: 31, x: 10383, y: 10055);

		SetDirection(224);
		SetStand("human/male/anim/male_natural_stand_npc_gilmore");

        Shop.AddTabs("General Goods", "Blacksmith", "Clothing", "Gift", "Event");

        //----------------
        // General Goods
        //----------------

        //Page 1
        Shop.AddItem("General Goods", 40093);		//Pet Instructor Stick
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 64018, 10);	//Big Paper
        Shop.AddItem("General Goods", 64018, 100);	//Big Paper
        Shop.AddItem("General Goods", 1124);		//A easy guide to taking up residence in a home (Book)
        Shop.AddItem("General Goods", 62021, 100);	//Six-sided Die
        Shop.AddItem("General Goods", 63020);		//Empty bottle
        Shop.AddItem("General Goods", 1053); 		//What we call 'Iron' (Book)
        Shop.AddItem("General Goods", 60045);		//Handicraft Kit
        Shop.AddItem("General Goods", 16024);		//Pet Instructor Glove
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 50078);		//Ticking Quiz Bomb
        Shop.AddItem("General Goods", 2001);		//Gold Pouch
        Shop.AddItem("General Goods", 40216);		//Cymbals
        Shop.AddItem("General Goods", 40018);		//Ukulele
        Shop.AddItem("General Goods", 2006);		//Big Gold Pouch
        Shop.AddItem("General Goods", 40017);		//Mandolin
        Shop.AddItem("General Goods", 40017);		//Mandolin
        Shop.AddItem("General Goods", 2005);		//Item Bag (7x5)

        //Page 2
        Shop.AddItem("General Goods", 2029);		//Item Bag (8x6)
        Shop.AddItem("General Goods", 2024);		//Item Bag (7x6)
        Shop.AddItem("General Goods", 91364);		//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 91364, 10);	//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 2038);		//Item Bag (8x10)
        Shop.AddItem("General Goods", 91365);		//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 91365, 10);	//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 85571);		//Reforging Tool
        Shop.AddItem("General Goods", 91366);		//Seal Scroll (30-day)
        Shop.AddItem("General Goods", 91366, 10);	//Seal Scroll (30-day)

        //----------------
        // Blacksmith
        //----------------

        //Page 1
        Shop.AddItem("Blacksmith", 64581);		//These are ALL Black Smith Manuals of Random Variety,
        Shop.AddItem("Blacksmith", 64581);		//they are in the CORRECT order.
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64581);
        Shop.AddItem("Blacksmith", 64500);
        Shop.AddItem("Blacksmith", 64500);

        //----------------
        // Clothing
        //----------------

        //Page 1
        Shop.AddItem("Clothing", 16029);		//Leather Stitched Glove
        Shop.AddItem("Clothing", 17068);		//Dotted Stitch Boots
        Shop.AddItem("Clothing", 15044);		//Carpenter Clothes
        Shop.AddItem("Clothing", 18039);		//Archer Feather Cap
        Shop.AddItem("Clothing", 17031);		//Outdoor Ankle Boots
        Shop.AddItem("Clothing", 15062);		//Zigzag Tunic
        Shop.AddItem("Clothing", 18047);		//Cores' Felt Hat
        Shop.AddItem("Clothing", 16033);		//Adolph Glove
        Shop.AddItem("Clothing", 17071);		//Knee-high Boots
        Shop.AddItem("Clothing", 17035);		//Four-line Boots
        Shop.AddItem("Clothing", 15063);		//Layered Frilled Dress
        Shop.AddItem("Clothing", 18045);		//Starry Wizard Hat

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52011);			//Socks
        Shop.AddItem("Gift", 52018);			//Hammer
        Shop.AddItem("Gift", 52008);			//Anthology
        Shop.AddItem("Gift", 52009);			//Cubic Puzzle
        Shop.AddItem("Gift", 52017);			//Underwear Set
        
		Phrases.Add("Business is slow nowadays. Perhaps I should raise the rent.");
		Phrases.Add("Cheap stuff means cheap quality.");
		Phrases.Add("Get lost unless you are going to buy something!");
		Phrases.Add("I have plenty of goods. As long as you have the Gold.");
		Phrases.Add("If you don't like me, you can buy goods somewhere else.");
		Phrases.Add("My goods don't just grow on trees.");
		Phrases.Add("So you think you can buy goods somewhere else?");
		Phrases.Add("They are so much trouble. Those thieving jerks...");
		Phrases.Add("What a pain. More kids just keep coming to the store.");
		Phrases.Add("Why should I put up with criticism from people who are not even my customers?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "This wiry man with a slight hump has his hands folded behind his back, and light brown hair hangs over his wide, wrinkled forehead.<br/>The reading glasses, so thick that you can't see what's behind them, rest on the wrinkles of his nose and flash every time he turns his face.<br/>Over his firmly sealed, stubborn-looking lips, he has a light-brown mustache.<br/>Frowning, he tilts down his head and stares at you over his reading glasses with grumpy brown eyes.");
        Msg(c, "What brings you here?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Welcome. I haven't seen you before. I assume you have Gold?<br/>Because, you have no business with me unless you have Gold.");

            L_Keywords:
                Msg(c, Options.Name, "(Gilmore is paying attention to me.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "I don't know what it is you're looking for,<br/>but you'd better stop now if all you are going to do is look around.");
                OpenShop(c);
                End();
            }
            case "@upgrade":
            {
                Msg(c,
                    "...<br/>Is there something you need to upgrade?<br/>Sigh... Fine, let's see it..",
                    Button("End Conversation", "@endupgrade")
                );

                r = Select(c);

                Msg(c, "Is that it? Well then...");
                End();
            }
        }
    }
}
