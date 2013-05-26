// Aura Script
// --------------------------------------------------------------------------
// Walter - General Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class WalterScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_walter");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.2f, upper: 1f, lower: 1.2f);
		SetFace(skin: 22, eye: 13, eyeColor: 27, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1327, 0xC5018A, 0xF79291, 0xFD852E);
		EquipItem(Pocket.Hair, 0xFBB, 0x554433, 0x554433, 0x554433);
		EquipItem(Pocket.Armor, 0x3AC4, 0x665033, 0xDDDDDD, 0xD5DBE4);
		EquipItem(Pocket.Shoe, 0x4271, 0x9D7012, 0xD3E3F4, 0xEEA23D);

		SetLocation(region: 14, x: 35770, y: 39528);

		SetDirection(252);
		SetStand("");

        Shop.AddTabs("General Goods", "Tailoring", "Sewing Patterns", "Gift", "Cooking Appliances", "Event");

        //----------------
        // General Goods
        //----------------

        //Page 1
        Shop.AddItem("General Goods", 40093);		//Pet Instructor Stick
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 64018, 10);	//Big Paper
        Shop.AddItem("General Goods", 64018, 100);	//Big Paper
        Shop.AddItem("General Goods", 62021, 100);	//Six-sided Die
        Shop.AddItem("General Goods", 63020);		//Empty bottle
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
        Shop.AddItem("General Goods", 2037);		//Kiosk
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
        // Tailoring
        //----------------

        //Page 1
        Shop.AddItem("Tailoring", 60001);		//Tailoring Kit
        Shop.AddItem("Tailoring", 60015);		//Cheap Finishing Thread
        Shop.AddItem("Tailoring", 60015, 5);		//Cheap Finishing Thread
        Shop.AddItem("Tailoring", 60031);		//Regular Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60019);		//Cheap Fabric
        Shop.AddItem("Tailoring", 60019, 5);		//Cheap Fabric
        Shop.AddItem("Tailoring", 60016);		//Common Finishing Thread
        Shop.AddItem("Tailoring", 60016, 5);		//Common Finishing Thread
        Shop.AddItem("Tailoring", 60017);		//Fine Finishing Thread
        Shop.AddItem("Tailoring", 60055);		//Fine Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60017, 5);		//Fine Finishing Thread
        Shop.AddItem("Tailoring", 60046);		//Finest Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60018);		//Finest Finishing Thread
        Shop.AddItem("Tailoring", 60057);		//Fine Fabric Weaving Gloves
        Shop.AddItem("Tailoring", 60056);		//Finest Fabric Weaving Gloves
        Shop.AddItem("Tailoring", 60020);		//Common Fabric
        Shop.AddItem("Tailoring", 60018, 5);		//Finest Finishing Thread
        Shop.AddItem("Tailoring", 60020, 5);		//Common Fabric

        //----------------
        // Sewing Patterns
        //----------------

        //Page 1
        Shop.AddItem("Sewing Patterns", 60000);		//All the sewing patterns
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60000);
        Shop.AddItem("Sewing Patterns", 60044);
        Shop.AddItem("Sewing Patterns", 60044);
        Shop.AddItem("Sewing Patterns", 60044);
        Shop.AddItem("Sewing Patterns", 60044);
        Shop.AddItem("Sewing Patterns", 60044);

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
        Shop.AddItem("Cooking Appliances", 40042);	//Cooking Knife
        Shop.AddItem("Cooking Appliances", 40044);	//Ladle
        Shop.AddItem("Cooking Appliances", 40043);	//Rolling Pin
        Shop.AddItem("Cooking Appliances", 46005);	//Cooking Table
        Shop.AddItem("Cooking Appliances", 46004);	//Cooking Pot
        
        	Phrases.Add("Ahem!");
		Phrases.Add("Ahem... Ow...my throat...");
		Phrases.Add("Hello there!");
		Phrases.Add("Hmm...");
		Phrases.Add("Is there any specific item you're looking for?");
		Phrases.Add("Please don't touch that.");
		Phrases.Add("That one is 20 Gold.");
		Phrases.Add("That's 30 Gold for four.");
		Phrases.Add("That's 50 Gold for three.");
		Phrases.Add("What are you looking for?");
		Phrases.Add("What do you need?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "A middle-aged man with a dark complexion and average height, Walter is wearing suspenders and stroking his stubby fingers.<br/>Under his dark-brown eyes, his tightly sealed lips are covered by a thick mustache.<br/>You can see his mustache and his Adam's apple slightly move as if he is about to say something.");
        Msg(c, "Um? What do you want?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Modify Item", "@modify"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "..Welcome");

            L_Keywords:
                Msg(c, Options.Name, "(Walter is slowly looking me over.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "What are you looking for?");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                Msg(c,
                "Repair? What is it that you want to repair? let's have a look.<br/>I can take care of general goods like instruments, glasses, and tools.<br/>My skills are not what they used to be, so I won't charge you a lot...",
                Button("End Conversation", "@endrepair")
                );

                r = Select(c);

                Msg(c, "If you're not careful with it, it will break easily.<br/>So take good care of it.");
                End();
            }
            case "@modify":
            {
                Msg(c,
                    "...<br/>Give me what you want to modify.<br/>I'm sure you have checked the number and type of the modification you want?",
                    Button("End Conversation", "@endmodify")
                );
                r = Select(c);
                Msg(c, "This is it? Well, then...");
                End();
            }
        }
    }
}
