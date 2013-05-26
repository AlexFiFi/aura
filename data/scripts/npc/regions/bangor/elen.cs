// Aura Script
// --------------------------------------------------------------------------
// Elen - Blacksmith
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ElenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_elen");
		SetRace(10001);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 25, eye: 3, eyeColor: 54, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x2C6B74, 0xF25CA0, 0xB5901E);
		EquipItem(Pocket.Hair, 0xBBD, 0xFFE680, 0xFFE680, 0xFFE680);
		EquipItem(Pocket.Armor, 0x3AB5, 0xFFFFFF, 0x942370, 0xEFE1C2);
		EquipItem(Pocket.Shoe, 0x427B, 0x2B6280, 0x67676C, 0x5DAA);
		EquipItem(Pocket.Head, 0x4668, 0x7D2224, 0xFFFFFF, 0x88CD);
		EquipItem(Pocket.RightHand1, 0x9C58, 0xFACB5F, 0x4F3C26, 0xFAB052);

		SetLocation(region: 31, x: 11353, y: 12960);

		SetDirection(15);
		SetStand("human/female/anim/female_natural_stand_npc_elen");

        Shop.AddTabs("Weapon", "Shoes Gloves", "Helmet", "Armor", "Event", "Arrowhead");

        //----------------
        // Weapon
        //----------------

        //Page 1
        Shop.AddItem("Weapon", 40001);		//Wooden Stick
        Shop.AddItem("Weapon", 40019);		//Broad Stick
        Shop.AddItem("Weapon", 45001, 20); 	//Arrow
        Shop.AddItem("Weapon", 45001, 100); 	//Arrow
        Shop.AddItem("Weapon", 40023); 		//Gathering Knife
        Shop.AddItem("Weapon", 40022); 		//Gathering Axe
        Shop.AddItem("Weapon", 45002, 50); 	//Bolt
        Shop.AddItem("Weapon", 45002, 200);	//Bolt
        Shop.AddItem("Weapon", 40027); 		//Weeding Hoe
        Shop.AddItem("Weapon", 40003);		//Short Bow
        Shop.AddItem("Weapon", 40002);		//Wooden Blade
        Shop.AddItem("Weapon", 40020);		//Wooden Club
        Shop.AddItem("Weapon", 40026);		//Sickle
        Shop.AddItem("Weapon", 40025);		//Pickaxe
        Shop.AddItem("Weapon", 40006);		//Dagger
        Shop.AddItem("Weapon", 40179);		//Spiked Knuckle
        Shop.AddItem("Weapon", 40005);		//Short Sword
        Shop.AddItem("Weapon", 40007);		//Hatchet
        Shop.AddItem("Weapon", 40024);		//Blacksmith Hammer
        Shop.AddItem("Weapon", 40014);		//Composite Bow
        Shop.AddItem("Weapon", 40010);		//Longsword
        Shop.AddItem("Weapon", 40180);		//Hobnail Knuckle
        Shop.AddItem("Weapon", 40016);		//Warhammer
        Shop.AddItem("Weapon", 40013);		//Long Bow
        Shop.AddItem("Weapon", 40243);		//Battle Short Sword
        Shop.AddItem("Weapon", 40244);		//Bear Knuckle
        Shop.AddItem("Weapon", 40015);		//Fluted Short Sword
        Shop.AddItem("Weapon", 40011);		//Broadsword

        //Page 2
        Shop.AddItem("Weapon", 40012);		//Bastard Sword
        Shop.AddItem("Weapon", 40242);		//Battle Sword
        Shop.AddItem("Weapon", 46001);		//Round Shield
        Shop.AddItem("Weapon", 40030);		//Two-handed Sword
        Shop.AddItem("Weapon", 40033);		//Claymore
        Shop.AddItem("Weapon", 46006);		//Kite shield

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 16004);	//Studded Bracelet
        Shop.AddItem("Shoes Gloves", 16008);	//Cores' Thief Gloves
        Shop.AddItem("Shoes Gloves", 16000);	//Leather Gloves
        Shop.AddItem("Shoes Gloves", 17021);	//Lorica Sandals
        Shop.AddItem("Shoes Gloves", 17014);	//Leather Shoes
        Shop.AddItem("Shoes Gloves", 16009);	//Tork's Hunter Gloves
        Shop.AddItem("Shoes Gloves", 17001);	//Ladies Leather Boots
        Shop.AddItem("Shoes Gloves", 17005);	//Hunter Boots
        Shop.AddItem("Shoes Gloves", 17015);	//Combat Shoes
        Shop.AddItem("Shoes Gloves", 17016);	//Field Combat Shoes
        Shop.AddItem("Shoes Gloves", 17020);	//Thief Shoes
        Shop.AddItem("Shoes Gloves", 16005);	//Wood Plate Cannon
        Shop.AddItem("Shoes Gloves", 16014);	//Lorica Gloves
        Shop.AddItem("Shoes Gloves", 16017);	//Standard Gloves
        Shop.AddItem("Shoes Gloves", 16007);	//Cores Ninja Gloves
        Shop.AddItem("Shoes Gloves", 17506);	//Long Greaves
        Shop.AddItem("Shoes Gloves", 16500);	//Ulna Protector Gloves
        Shop.AddItem("Shoes Gloves", 16501);	//Leather Protector
        Shop.AddItem("Shoes Gloves", 17501);	//Solleret Shoes

        //Page 2
        Shop.AddItem("Shoes Gloves", 17500);	//High Polean Plate Boots
        Shop.AddItem("Shoes Gloves", 16504);	//Counter Gauntlet
        Shop.AddItem("Shoes Gloves", 16505);	//Fluted Gauntlet
        Shop.AddItem("Shoes Gloves", 17505);	//Plate Boots

        //----------------
        // Helmet
        //----------------

        //Page 1
        Shop.AddItem("Helmet", 18513);		//Spiked Cap
        Shop.AddItem("Helmet", 18503);		//Cuirassier Helm
        Shop.AddItem("Helmet", 18500);		//Ring Mail Helm
        Shop.AddItem("Helmet", 18504);		//Cross Full Helm
        Shop.AddItem("Helmet", 18502);		//Bone Helm
        Shop.AddItem("Helmet", 18501);		//Guardian Helm
        Shop.AddItem("Helmet", 18505);		//Spiked Helm
        Shop.AddItem("Helmet", 18506);		//Wing Half Helm
        Shop.AddItem("Helmet", 18508);		//Slit Full Helm
        Shop.AddItem("Helmet", 18509);		//Bascinet

        //----------------
        // Armor
        //----------------

        //Page 1
        Shop.AddItem("Armor", 14006);		//Linen Cuirass (F)
        Shop.AddItem("Armor", 14009);		//Linen Cuirass (M)
        Shop.AddItem("Armor", 14001);		//Light Leather Mail (F)
        Shop.AddItem("Armor", 14010);		//Light Leather Mail (M)
        Shop.AddItem("Armor", 14004);		//Cloth Mail
        Shop.AddItem("Armor", 14008);		//Full Leather Armor Set
        Shop.AddItem("Armor", 14003);		//Studded Cuirassier
        Shop.AddItem("Armor", 14007);		//Padded Armor with Breastplate
        Shop.AddItem("Armor", 14013);		//Lorica Segmentata
        Shop.AddItem("Armor", 14005);		//Drandos Leather Mail (F)
        Shop.AddItem("Armor", 14011);		//Drandos Leather Mail (M)
        Shop.AddItem("Armor", 13017);		//Surcoat Chain Mail

        //Page 2
        Shop.AddItem("Armor", 13001);		//Melka Chain Mail
        Shop.AddItem("Armor", 13010);		//Round Pauldron Chainmail
        Shop.AddItem("Armor", 13022);		//Rose Plate armor (Type P)
        Shop.AddItem("Armor", 13053);		//Spika Silver Plate Armor (Giants)
        Shop.AddItem("Armor", 13031);		//Spika Silver Plate Armor

        //----------------
        // Arrowhead
        //----------------

        //Page 1
        Shop.AddItem("Arrowhead", 64011);	//Bundle of Arrowheads
        Shop.AddItem("Arrowhead", 64015);	//Bundle of Bolt Heads
        Shop.AddItem("Arrowhead", 64013);	//Bundle of Fine Arrowheads
        Shop.AddItem("Arrowhead", 64016);	//Bundle of Fine Bolt Heads
        Shop.AddItem("Arrowhead", 64014);	//Bundle of the Finest Arrowheads
        Shop.AddItem("Arrowhead", 64017);	//Bundle of the Finest Bolt Heads
        
		Phrases.Add("Come over here if you are interested in blacksmith work.");
		Phrases.Add("Grandpa worries too much.");
		Phrases.Add("Heh. That boy over there is kind of cute. I'd get along with him really well.");
		Phrases.Add("How about some excitement in this town?");
		Phrases.Add("If my beauty mesmerizes you, at least have the guts to come and tell me so.");
		Phrases.Add("I'm not too bad at blacksmith work myself, you know.");
		Phrases.Add("It's rather slow today...");
		Phrases.Add("Lets see... I still have some left...");
		Phrases.Add("Mom always neglects me...");
		Phrases.Add("Nothing is free!");
		Phrases.Add("The real fun is in creating, not repairing.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "Her lovely blonde hair, pushed back with a red and white headband to keep it out of her face, comes down to her waist in a wave and covers her entire back.<br/>Her small face with dark emerald eyes shines brightly and her full lips create an inquisitive look.<br/>The sleeveless shirt she is wearing due to the heat of the shop exposes her soft tanned skin, showing how healthy she is.");
        Msg(c, "Mmm? Is there something you would like to say to me?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Modify Item", "@modify"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Welcome! But... I've never seen you around here before.");

            L_Keywords:
                Msg(c, Options.Name, "(Elen is looking in my direction.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "Welcome to the best Blacksmith's Shop in Bangor.<br/>We deal with almost anything made of metal. Is there anything in particular you're looking for?");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                Msg(c,
                "Is there something you want to repair?<br/>I'm far from being as good as my grandpa,<br/>but I am a blacksmith myself, so I'll so my best to live up to the title.",
                Button("End Conversation", "@endrepair")
            );

                r = Select(c);

                Msg(c, "If you don't trust me, talk to grandpa.<br/>He's the best blacksmith in town.");
                End();
            }
            case "@modify":
            {
                Msg(c,
                    "Mmm? You are asking me for an item modification?<br/>Ha ha. If you are,<br/>I'll do it just for you!<br/>You know that armor can't be worn by anyone else once it's modified, right?",
                    Button("End Conversation", "@endmodify")
                );
                r = Select(c);
                Msg(c, "Then, can i get back to my other tasks?<br/>Just let me know if you have something else to modify.");
                End();
            }
        }
    }
}
