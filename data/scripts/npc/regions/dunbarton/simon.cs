// Aura Script
// --------------------------------------------------------------------------
// Simon - Clothing Shop Owner
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SimonScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_simon");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.8f, upper: 0.8f, lower: 0.8f);
		SetFace(skin: 15, eye: 8, eyeColor: 25, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1326, 0x9B6D85, 0x494966, 0x30B075);
		EquipItem(Pocket.Hair, 0xFB8, 0x998866, 0x998866, 0x998866);
		EquipItem(Pocket.Armor, 0x3AC5, 0xD6D8DE, 0x31208E, 0xFF9B3B);
		EquipItem(Pocket.Shoe, 0x4275, 0x9C7B6B, 0xF79825, 0x7335);

		SetLocation(region: 17, x: 1314, y: 921);

		SetDirection(24);
		SetStand("human/male/anim/male_natural_stand_npc_Simon");

        Shop.AddTabs("Robes", "Hats", "Shoes Gloves", "Clothes", "Fine Clothes", "Event");

        //----------------
        // Robes
        //----------------

        //Page 1
        Shop.AddItem("Robes", 19003);		//Tricolor Robe
        Shop.AddItem("Robes", 19003);		//Tricolor Robe
        Shop.AddItem("Robes", 19003);		//Tricolor Robe
        Shop.AddItem("Robes", 19003);		//Tricolor Robe

        //----------------
        // Hats
        //----------------

        //Page 1
        Shop.AddItem("Hats", 18012);		//Tork Merchant Cap
        Shop.AddItem("Hats", 18007);		//Popo Cap
        Shop.AddItem("Hats", 18013);		//Cores' Cap
        Shop.AddItem("Hats", 18004);		//Mongo's Fashion Cap
        Shop.AddItem("Hats", 18010);		//Mongo's Smart Cap
        Shop.AddItem("Hats", 18042);		//Cores' Oriental Hat
        Shop.AddItem("Hats", 18003);		//Lirina's Cap
        Shop.AddItem("Hats", 18002);		//Mongo Cap
        Shop.AddItem("Hats", 18124);		//Sandra's Sniper Suit Cap
        Shop.AddItem("Hats", 18000);		//Tork's Cap
        Shop.AddItem("Hats", 18009);		//Mongo's Archer Cap
        Shop.AddItem("Hats", 18011);		//Mongo Jester Cap
        Shop.AddItem("Hats", 18008);		//Stripe Cap
        Shop.AddItem("Hats", 18014);		//Mongo's Hat
        Shop.AddItem("Hats", 18046);		//Tiara
        Shop.AddItem("Hats", 18051);		//Cores' Ribbon Hat

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 16015);		//Bracelet
        Shop.AddItem("Shoes Gloves", 16024);		//Pet Instructor Glove
        Shop.AddItem("Shoes Gloves", 17019);		//Blacksmith Shoes
        Shop.AddItem("Shoes Gloves", 17067);		//X Tie-Up Shoes
        Shop.AddItem("Shoes Gloves", 16031);		//Tight Tri-lined Glove
        Shop.AddItem("Shoes Gloves", 17003);		//Leather Shoes (Type 4)
        Shop.AddItem("Shoes Gloves", 17017);		//Leather Coat Shoes
        Shop.AddItem("Shoes Gloves", 17060);		//Sandra's Sniper Suit Boots (M)
        Shop.AddItem("Shoes Gloves", 17061);		//Sandra's Sniper Suit Boots (F)
        Shop.AddItem("Shoes Gloves", 17010);		//Cores' Boots (M)
        Shop.AddItem("Shoes Gloves", 16026);		//Sandra's Sniper Suit Gloves
        Shop.AddItem("Shoes Gloves", 17024);		//Open-toe Platform Sandal
        Shop.AddItem("Shoes Gloves", 16006);		//Guardian Gloves
        Shop.AddItem("Shoes Gloves", 17029);		//Belt Buckle Boots
        Shop.AddItem("Shoes Gloves", 16013);		//Swordsman Gloves
        Shop.AddItem("Shoes Gloves", 17023);		//Enamel Shoes
        Shop.AddItem("Shoes Gloves", 16016);		//Light Gloves
        Shop.AddItem("Shoes Gloves", 17013);		//Thick Sandals
        Shop.AddItem("Shoes Gloves", 16017);		//Standard Gloves
        Shop.AddItem("Shoes Gloves", 16019);		//Lirina Striped Gloves
        Shop.AddItem("Shoes Gloves", 17040);		//Ella's Strap Boots
        Shop.AddItem("Shoes Gloves", 17069);		//Leo Shoes

	//Page 2
        Shop.AddItem("Shoes Gloves", 16032);		//Elven Glove
        Shop.AddItem("Shoes Gloves", 17041);		//Vine-print Hunting Boots

        //----------------
        // Clothes
        //----------------

        //Page 1
        Shop.AddItem("Clothes", 15022);		//Popo's Skirt
        Shop.AddItem("Clothes", 15023);		//Tork's Hunter Suit (F)
        Shop.AddItem("Clothes", 15035);		//Tork's Hunter Suit (M)
        Shop.AddItem("Clothes", 15033);		//Mongo's Traveler Suit
        Shop.AddItem("Clothes", 15027);		//Mongo's Long Skirt
        Shop.AddItem("Clothes", 15041);		//Female Business Suit
        Shop.AddItem("Clothes", 15044);		//Carpenter Clothes
        Shop.AddItem("Clothes", 15051);		//Belted Casual Wear
        Shop.AddItem("Clothes", 15052);		//Terks' Two-Tone Tunic
        Shop.AddItem("Clothes", 15020);		//Cores' Healer Dress

        //----------------
        // Fine Clothes
        //----------------

        //Page 1
        Shop.AddItem("Fine Clothes", 15016);	//Ceremonial Stocking
        Shop.AddItem("Fine Clothes", 15026);	//Lirina's Long Skirt
        Shop.AddItem("Fine Clothes", 15042);	//High Neck One-piece Dress
        Shop.AddItem("Fine Clothes", 15045);	//Ruffled Tuxedo Ensemble
        Shop.AddItem("Fine Clothes", 15032);	//Lirina's Shorts
        Shop.AddItem("Fine Clothes", 15153);	//Sandra's Sniper Suit (M)
        Shop.AddItem("Fine Clothes", 15154);	//Sandra's Sniper Suit (F)
        Shop.AddItem("Fine Clothes", 15151);	//Mario NY Modern Vintage Ensemble (M)
        Shop.AddItem("Fine Clothes", 15152);	//Mario NY Modern Vintage Ensemble (F)
        Shop.AddItem("Fine Clothes", 15053);	//Flat Collar One-Piece Dress
        Shop.AddItem("Fine Clothes", 15014);	//Messenger Wear
        Shop.AddItem("Fine Clothes", 15029);	//Tork's Blacksmith Suit

	//Page 2
        Shop.AddItem("Fine Clothes", 15019);	//Cores Ninja Suit (F)
        Shop.AddItem("Fine Clothes", 15017);	//Chinese dress
        Shop.AddItem("Fine Clothes", 15067);	//Oriental Warrior Suit
        Shop.AddItem("Fine Clothes", 15064);	//Idol Ribbon Dress

        
        Phrases.Add("Heeheehee... She's got some fashion sense.");
		Phrases.Add("Let's see... Which ones do I have to finish by today?");
		Phrases.Add("Oops! I was supposed to do that tomorrow, wasn't I? Heehee...");
		Phrases.Add("That man over there... What he's wearing is so 20 minutes ago.");
		Phrases.Add("This... is too last-minute.");
		Phrases.Add("Time just flies today. Heh.");
		Phrases.Add("Travelers... How are they so careless about their appearance?");
		Phrases.Add("Ugh! This world is so devoid of beauty.");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "With a long face, narrow shoulders, and a pale complexion, this man crosses his delicate hands in front of the chest and sways left and right.<br/>His demeanor is exaggerated and the voice nasal. He seems to have a habit of glancing sideways with those light brown eyes.<br/>His fashionable shirt has an intricate pattern and was made with great care.");
        MsgSelect(c, "What do you want?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Modify Item", "@modify"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Haha. That's right.<br/>You have to come often to be recognized.");

            L_Keywords:
                Msg(c, Options.Name, "(Simon is paying attention to me.)");
                ShowKeywords(c);

                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "Hmm. Take your time to look around.");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                MsgSelect(c,
                "Want to mend your clothes?<br/>Rest assured, I am the best this kingdom has to offer. I never make mistakes.<br/>Because of that, I charge a higher repair fee.<br/>If you can stomach a cheap repair, go find someone else. I only work with top quality.",
                Button("End Conversation", "@endrepair")
                );

                r = Wait();

                Msg(c, "No more?<br/>Then, bye!");
                End();
            }
            case "@modify":
            {
                MsgSelect(c,
                    "Hmm... You want to modify your clothes? Like custom-made?<br/>Well, show me what you want modified. I'll make sure it fits you like a glove.<br/>But, you know that once I modify it, no one else can wear it anymore, right?",
                    Button("End Conversation", "@endmodify")
                );
                r = Wait();
                Msg(c, "If you have something to modify, come see me anytime.");
                End();
            }
        }
    }
}
