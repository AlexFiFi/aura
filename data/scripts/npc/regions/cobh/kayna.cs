// Aura Script
// --------------------------------------------------------------------------
// Kayana - Blacksmith
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KaynaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kayna");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1.4f, upper: 1.4f, lower: 1f);
		SetFace(skin: 15, eye: 60, eyeColor: 27, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0xDE9853, 0x2E3B65, 0x6E6365);
		EquipItem(Pocket.Hair, 0x138F, 0x756939, 0x756939, 0x756939);
		EquipItem(Pocket.Armor, 0x3BFB, 0x825642, 0xB0ADAB, 0xEFE3B5);
		EquipItem(Pocket.Shoe, 0x42AA, 0x633C31, 0x6E9677, 0x808080);

		SetLocation(region: 23, x: 26559, y: 38870);

		SetDirection(227);
		SetStand("chapter4/human/female/anim/female_c4_npc_kayna");

        Shop.AddTabs("Weapons Tools", "Shoes Gloves", "Helmet", "Armor");

        //----------------
        // Weapons Tools
        //----------------

        //Page 1
        Shop.AddItem("Weapons Tools", 45001, 20); 	//Arrow
        Shop.AddItem("Weapons Tools", 45001, 100); 	//Arrow
        Shop.AddItem("Weapons Tools", 40023); 		//Gathering Knife
        Shop.AddItem("Weapons Tools", 40022); 		//Gathering Axe
        Shop.AddItem("Weapons Tools", 45002, 50); 	//Bolt
        Shop.AddItem("Weapons Tools", 45002, 200);	//Bolt
        Shop.AddItem("Weapons Tools", 40027); 		//Weeding Hoe
        Shop.AddItem("Weapons Tools", 40003);		//Short Bow
        Shop.AddItem("Weapons Tools", 40002);		//Wooden Blade
        Shop.AddItem("Weapons Tools", 40026);		//Sickle
        Shop.AddItem("Weapons Tools", 40025);		//Pickaxe
        Shop.AddItem("Weapons Tools", 45101, 20);	//Short Javelin
        Shop.AddItem("Weapons Tools", 40181); 		//Wood Atlatl
        Shop.AddItem("Weapons Tools", 40236);		//Elven Short Bow
        Shop.AddItem("Weapons Tools", 40006);		//Dagger
        Shop.AddItem("Weapons Tools", 40179);		//Spiked Knuckle
        Shop.AddItem("Weapons Tools", 40024);		//Blacksmith Hammer
        Shop.AddItem("Weapons Tools", 40005);		//Short Sword
        Shop.AddItem("Weapons Tools", 40243);		//Battle Short Sword
        Shop.AddItem("Weapons Tools", 40013);		//Long Bow
        Shop.AddItem("Weapons Tools", 40239);		//Feather Atlatl

	//Page 2
        Shop.AddItem("Weapons Tools", 45107);		//Bundle of Short Javelins (50)
        Shop.AddItem("Weapons Tools", 40175);		//Great Mallet
        Shop.AddItem("Weapons Tools", 40010);		//Longsword
        Shop.AddItem("Weapons Tools", 45102, 20);	//Long Javelin
        Shop.AddItem("Weapons Tools", 40244);		//Bear Knuckle
        Shop.AddItem("Weapons Tools", 40011);		//Broadsword
        Shop.AddItem("Weapons Tools", 40237);		//Elven Long Bow
        Shop.AddItem("Weapons Tools", 40180);		//Hobnail Knuckle
        Shop.AddItem("Weapons Tools", 40081);		//Leather Long Bow
        Shop.AddItem("Weapons Tools", 40174);		//Morning Star
        Shop.AddItem("Weapons Tools", 40240);		//Iron Mace
        Shop.AddItem("Weapons Tools", 45108);		//Bundle of Long Javelins (50)
        Shop.AddItem("Weapons Tools", 40242);		//Battle Sword
        Shop.AddItem("Weapons Tools", 40172);		//Great Sword
        Shop.AddItem("Weapons Tools", 40031);		//Crossbow
        Shop.AddItem("Weapons Tools", 40080);		//Gladius
        Shop.AddItem("Weapons Tools", 40745);		//Basic Control Bar

	//Page 3
        Shop.AddItem("Weapons Tools", 40238);		//Vales Great Sword
        Shop.AddItem("Weapons Tools", 46001);		//Round Shield
        Shop.AddItem("Weapons Tools", 40176);		//Battle Hammer
        Shop.AddItem("Weapons Tools", 46019);		//Tikka Shield
        Shop.AddItem("Weapons Tools", 40177);		//Warrior Axe
        Shop.AddItem("Weapons Tools", 46006);		//Kite Shield
        Shop.AddItem("Weapons Tools", 46020);		//Vales Shield

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 16000); 		//Leather Gloves
        Shop.AddItem("Shoes Gloves", 17015);		//Combat Shoes
        Shop.AddItem("Shoes Gloves", 17016);		//Field Combat Shoes
        Shop.AddItem("Shoes Gloves", 16014);		//Lorica Gloves
        Shop.AddItem("Shoes Gloves", 16501);		//Leather Protector
        Shop.AddItem("Shoes Gloves", 17501);		//Solleret Shoes;
        Shop.AddItem("Shoes Gloves", 16500);		//Ulna Protector Gloves
        Shop.AddItem("Shoes Gloves", 16028);		//Camelle Spirit Glove
        Shop.AddItem("Shoes Gloves", 16523);		//Graceful Gauntlet
        Shop.AddItem("Shoes Gloves", 17515);		//Graceful Greaves
        Shop.AddItem("Shoes Gloves", 16504);		//Counter Gauntlet
        Shop.AddItem("Shoes Gloves", 17064);		//Camelle Spirit Boots

        //----------------
        // Helmet
        //----------------

        //Page 1
        Shop.AddItem("Helmet", 18503);			//Cuirassier Helm
        Shop.AddItem("Helmet", 18502);			//Bone Helm
        Shop.AddItem("Helmet", 18501);			//Guardian Helm
        Shop.AddItem("Helmet", 18506);			//Wing Half Helm
        Shop.AddItem("Helmet", 18545);			//Graceful Helmet
        Shop.AddItem("Helmet", 18525);			//Waterdrop Cap
        Shop.AddItem("Helmet", 18520);			//Steel Headgear
        Shop.AddItem("Helmet", 18521);			//European Comb
        Shop.AddItem("Helmet", 18524);			//Four Wings Cap
        Shop.AddItem("Helmet", 18515);			//Twin Horn Cap

        //----------------
        // Armor
        //----------------

        //Page 1
        Shop.AddItem("Armor", 14001);			//Light Leather Mail (F)
        Shop.AddItem("Armor", 14010);			//Light Leather Mail (M)
        Shop.AddItem("Armor", 14028);			//Esteban Mail (M)
        Shop.AddItem("Armor", 14029);			//Esteban Mail (F)
        Shop.AddItem("Armor", 14019);			//Graceful Plate Armor
        Shop.AddItem("Armor", 14013);			//Lorica Segmentata
        Shop.AddItem("Armor", 14025);			//Camelle Spirit Armor (M)
        Shop.AddItem("Armor", 14026);			//Camelle Spirit Armor (F)
        Shop.AddItem("Armor", 14005);			//Drandos Leather Mail (F)
        Shop.AddItem("Armor", 14011);			//Drandos Leather Mail (M)
        Shop.AddItem("Armor", 14017);			//Three-Belt Leather Mail
        Shop.AddItem("Armor", 14016);			//Cross Belt Leather Coat
        
		Phrases.Add("Do you see anything you like?");
		Phrases.Add("Hey there! Come on over!");
		Phrases.Add("I don't like the look on that person's face.");
		Phrases.Add("If things keep on like this, I'm going to run out of space for arrows.");
		Phrases.Add("If you need anything, just let ole' Kayna know.");
		Phrases.Add("Look at those muscles. Cadoc is a hunk!");
		Phrases.Add("Oh my, I forgot to do something.");
		Phrases.Add("There haven't been that many customers this month.");
		Phrases.Add("Time is passing so slowly...");
		Phrases.Add("Welcome!");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "She's a middle age woman with round features.<br/>Her clothes are worn with the smoke and soot<br/>of her forge. Her two pigtails allow her pearl earrings<br/>to glitter in the sunlight.");
        MsgSelect(c, "Hi there! Anything I can help you with?", Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Upgrade Item", "@upgrade"));

        var r = Wait();
        switch (r)
        {
            case "@shop":
            {
                Msg(c, "What are you looking for?");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                MsgSelect(c,
                "Give the item you would like to repair.",
                Button("End Conversation", "@endrepair")
            );

                r = Wait();

                Msg(c, "Come again.");
                End();
            }
            case "@upgrade":
            {
                MsgSelect(c,
                    "What kind of upgrade would you like?",
                    Button("End Conversation", "@endupgrade")
                );

                r = Wait();

                Msg(c, "Come by again soon!");
                End();
            }
        }
    }
}
