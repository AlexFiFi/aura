// Aura Script
// --------------------------------------------------------------------------
// Annick - Pub Owner
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AnnickScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_annick");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 6, eyeColor: 37, lip: 55);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x8ED4, 0xA6D590, 0x5130);
		EquipItem(Pocket.Hair, 0xC39, 0x2D1C12, 0x2D1C12, 0x2D1C12);
		EquipItem(Pocket.Armor, 0x3DF2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 136, x: 939, y: 978);

		SetDirection(253);
		SetStand("chapter4/human/female/anim/female_c4_npc_annick");

        Shop.AddTabs("Liquor", "Food", "Etc.");

        //----------------
        // Liquor
        //----------------

        //Page 1
        Shop.AddItem("Liquor", 50504); 		//Barley Tea
        Shop.AddItem("Liquor", 50184); 		//Ice
        Shop.AddItem("Liquor", 50502); 		//Steamed Milk
        Shop.AddItem("Liquor", 50178);		//Orange Juice
        Shop.AddItem("Liquor", 50501);		//Lemon Tea
        Shop.AddItem("Liquor", 50500); 		//Basil Tea
        Shop.AddItem("Liquor", 50505); 		//Thyme Tea
        Shop.AddItem("Liquor", 50183); 		//Devenish Black Beer
        Shop.AddItem("Liquor", 50171); 		//Emain Macha Wine
        Shop.AddItem("Liquor", 50181); 		//Leighean Gin
        Shop.AddItem("Liquor", 50687); 		//Belvast Whiskey
        Shop.AddItem("Liquor", 50203); 		//Red Sunrise
        Shop.AddItem("Liquor", 50198); 		//Brifne Rocks
        Shop.AddItem("Liquor", 50201); 		//BnR
        Shop.AddItem("Liquor", 50509); 		//Corn Tea

        //----------------
        // Food
        //----------------

        //Page 1
        Shop.AddItem("Food", 50129); 		//Fried Shrimp
        Shop.AddItem("Food", 50109); 		//Roasted Chicken Wing
        Shop.AddItem("Food", 50197); 		//Spicy Fish Stew
        Shop.AddItem("Food", 50529); 		//Fried Smelt
        Shop.AddItem("Food", 50669); 		//Rock Bream Fish Stew

        //----------------
        // Etc.
        //----------------

        //Page 1
        Shop.AddItem("Etc.", 91563); 		//Hot Spring Ticket
        Shop.AddItem("Etc.", 91563, 5); 	//Hot Spring Ticket

		Phrases.Add("Ah, I love the smell of the sea!");
		Phrases.Add("Hahahah! You're a strange one, aren't ye?");
		Phrases.Add("Hey there! What're you looking at?");
		Phrases.Add("Hmm... Storm's comin', I reckon.");
		Phrases.Add("I miss my days on the sea.");
		Phrases.Add("I wonder what Miyir is doing right now...");
		Phrases.Add("I wonder what my men are up to?");
		Phrases.Add("If it wasn't for Admiral Owen, I would never be here!");
		Phrases.Add("This is the start of another day!");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "Light wrinkles are beginning to appear on her tanned face.<br/>She rushes around while talking and laughing.");
        MsgSelect(c, "Don't hesitate to ask if you're curious about anything.", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Ahoy, and welcome to the best pub in Port Cobh.");

            L_Keywords:
                Msg(c, Options.Name, "(Annick is looking at me.)");
                ShowKeywords(c);

                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
        case "@shop":
            {
                Msg(c, "So, you must be thirsty, right?<br/>I've got some special drinks<br/>and good fried fish back at my place,<br/>so come have a taste. Hahahaha!");
                OpenShop(c);
                End();
            }
        }
    }
}
