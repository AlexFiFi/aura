// Aura Script
// --------------------------------------------------------------------------
// Malcolm - General Shop 
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MalcolmScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_malcolm");
		SetRace(10002);
		SetBody(height: 1.22f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 26, eyeColor: 162, lip: 0);
		SetStand("human/male/anim/male_natural_stand_npc_Malcolm_retake", "human/male/anim/male_natural_stand_npc_Malcolm_talk");
		SetLocation("tir_general", 1238, 1655, 59);

		EquipItem(Pocket.Face, 0x1324, 0xBA0562);
		EquipItem(Pocket.Hair, 0x103B, 0xECBC58);
		EquipItem(Pocket.Armor, 0x3D27, 0xD8C9B7, 0x112A13, 0x131313);
		EquipItem(Pocket.Shoe, 0x4387, 0x544838, 0x0, 0x0);
		EquipItem(Pocket.LeftHand1, 0x9E2B, 0x808080, 0x0, 0x0);
		EquipItem(Pocket.RightHand1, 0x9C51, 0x3F7246, 0xC0B584, 0x3F4B40);

        Shop.AddTabs("General Goods", "Hats", "Shoes Gloves", "Casual", "Formal", "Event");

        //----------------
        // General Goods
        //----------------

        //Page 1
        Shop.AddItem("General Goods", 40093);		//Pet Instructor Stick
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 61001);		//Score Scroll
        Shop.AddItem("General Goods", 64018, 10);   	//Big Paper
        Shop.AddItem("General Goods", 64018, 100);  	//Big Paper
        Shop.AddItem("General Goods", 62021, 100);	//Six-sided Die
        Shop.AddItem("General Goods", 63020);		//Empty bottle
        Shop.AddItem("General Goods", 19001);		//Robe
        Shop.AddItem("General Goods", 19001);		//Robe
        Shop.AddItem("General Goods", 19001);		//Robe
        Shop.AddItem("General Goods", 1006); 		//Introduction to Music Composition
        Shop.AddItem("General Goods", 60045); 		//Handicraft Kit
        Shop.AddItem("General Goods", 60034, 300); 	//Bait Tin
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 40004);		//Lute
        Shop.AddItem("General Goods", 19002);		//Slender Robe
        Shop.AddItem("General Goods", 19002);		//Slender Robe
        
        //Page 2 
        Shop.AddItem("General Goods", 19002);		//Slender Robe
        Shop.AddItem("General Goods", 50078);		//Ticking Quiz Bomb
        Shop.AddItem("General Goods", 2001);		//Gold Pouch
        Shop.AddItem("General Goods", 40045);		//Fishing Rod
        Shop.AddItem("General Goods", 40018);		//Ukulele
        Shop.AddItem("General Goods", 40018);		//Ukulele
        Shop.AddItem("General Goods", 40018);		//Ukulele
        Shop.AddItem("General Goods", 2006);		//Big Gold Pouch
        Shop.AddItem("General Goods", 40214);		//Bass Drum
        Shop.AddItem("General Goods", 40214);		//Bass Drum
        Shop.AddItem("General Goods", 40214);		//Bass Drum
        Shop.AddItem("General Goods", 2005);		//Item Bag (7x5)

        //Page 3
        Shop.AddItem("General Goods", 2029);		//Item Bag (8x6)
        Shop.AddItem("General Goods", 2024);		//Item Bag (7x6)
        Shop.AddItem("General Goods", 91364);		//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 91364, 10);	//Seal Scroll (1-day)
        Shop.AddItem("General Goods", 2038);		//Item Bag (8x10)
        Shop.AddItem("General Goods", 18029);		//Wood-rimmed Glasses
        Shop.AddItem("General Goods", 18029);		//Wood-rimmed Glasses
        Shop.AddItem("General Goods", 91365);		//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 91365, 10);	//Seal Scroll (7-day)
        Shop.AddItem("General Goods", 85571);		//Reforging Tool
        Shop.AddItem("General Goods", 91366);		//Seal Scroll (30-day)
        Shop.AddItem("General Goods", 91366, 10);	//Seal Scroll (30-day)

        //----------------
        // Hats
        //----------------

        //Page 1
        Shop.AddItem("Hats", 18024);	//Hairband
        Shop.AddItem("Hats", 18023);	//Mongo's Thief Cap
        Shop.AddItem("Hats", 18025);	//Popo's Merchant Cap
        Shop.AddItem("Hats", 18016);	//Hat
        Shop.AddItem("Hats", 18019);	//Lirina's Feather Cap
        Shop.AddItem("Hats", 18027);	//Lirina's Merchant Cap
        Shop.AddItem("Hats", 18017);	//Tail Cap
        Shop.AddItem("Hats", 18015);	//Leather Hat
        Shop.AddItem("Hats", 18020);	//Mongo's Feather Cap
        Shop.AddItem("Hats", 18018);	//Leather Tail Cap
        Shop.AddItem("Hats", 18021);	//Merchant Cap
        Shop.AddItem("Hats", 18026);	//Mongo's Merchant Cap

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 17012);	//Leather Shoes
        Shop.AddItem("Shoes Gloves", 16024);	//Pet Instructor Glove
        Shop.AddItem("Shoes Gloves", 17025);	//Sandal
        Shop.AddItem("Shoes Gloves", 16029);	//Leather Stitched Glove
        Shop.AddItem("Shoes Gloves", 16002);	//Linen Gloves
        Shop.AddItem("Shoes Gloves", 17066);	//One-button Ankle Shoes
        Shop.AddItem("Shoes Gloves", 16001);	//Quilting Gloves
        Shop.AddItem("Shoes Gloves", 16030);	//Big Band Glove
        Shop.AddItem("Shoes Gloves", 17006);	//Cloth Shoes
        Shop.AddItem("Shoes Gloves", 17000);	//Women's Flats
        Shop.AddItem("Shoes Gloves", 17007);	//Leather Shoes
        Shop.AddItem("Shoes Gloves", 17002);	//Swordswoman Shoes
        Shop.AddItem("Shoes Gloves", 17008);	//Cores' Boots
        Shop.AddItem("Shoes Gloves", 17027);	//Long Sandals
        Shop.AddItem("Shoes Gloves", 16003);	//Sesamoid Gloves
        Shop.AddItem("Shoes Gloves", 16011);	//Cores' Healer Gloves
        Shop.AddItem("Shoes Gloves", 16012);	//Swordswoman Gloves
        Shop.AddItem("Shoes Gloves", 17036);	//Spika Two-piece Boots
        Shop.AddItem("Shoes Gloves", 16034);	//Two-lined Belt Glove
        Shop.AddItem("Shoes Gloves", 17071);	//Knee-high Boots
        Shop.AddItem("Shoes Gloves", 17038);	//Mini Ribbon Sandals

        //----------------
        // Casual
        //----------------

        //Page 1
        Shop.AddItem("Casual", 15000);	//Popo's Shirt and Pants
        Shop.AddItem("Casual", 15003);	//Vest and Pants Set
        Shop.AddItem("Casual", 15021);	//Elementary School Uniform
        Shop.AddItem("Casual", 15018);	//Mongo's Traveler Suit
        Shop.AddItem("Casual", 15043);	//Track Suit Set
        Shop.AddItem("Casual", 15031);	//Magic School Uniform
        Shop.AddItem("Casual", 15024);	//Popo's Dress
        Shop.AddItem("Casual", 15012);	//Ceremonial Dress

        //----------------
        // Formal
        //----------------

        //Page 1
        Shop.AddItem("Formal", 15025);	//Magic School Uniform (F)
        Shop.AddItem("Formal", 15061);	//Wave-print Side-slit Tunic
        Shop.AddItem("Formal", 15059);	//Terks' Tank Top and Shorts
        Shop.AddItem("Formal", 15005);	//Adventurer's Suit
        Shop.AddItem("Formal", 15007);	//Traditional Tir Chonaill Costume
        Shop.AddItem("Formal", 15028);	//Cores' Thief Suit
        Shop.AddItem("Formal", 15011);	//Sleeveless and Bell-Bottoms
        Shop.AddItem("Formal", 15013);	//China Dress

		Phrases.Add("Aww! My legs hurt. My feet are all swollen from standing all day long.");
		Phrases.Add("Dear love, you live right next door, yet I cannot see you... I can't sleep at night thinking of you...");
		Phrases.Add("Ha ha, look at what that person is wearing. (laugh)");
		Phrases.Add("I wonder what Nora is doing now...");
		Phrases.Add("It isn't easy running a shop alone... Maybe I should hire a clerk.");
		Phrases.Add("Maybe I should wrap it up and call it a day... (confused)");
		Phrases.Add("So much work, so little time... I'm in trouble!");
		Phrases.Add("These travelers will buy something sooner or later.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"While his thin face makes him look weak,",
			"and his soft and delicate hands seem much too feminine,",
			"his cool long blonde hair gives him a suave look.",
			"He looks like he just came out of a workshop since he's wearing a heavy leather apron."
		);

		MsgSelect(c, "What can I do for you?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"));
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Welcome to the General Shop. This must be your first visit here.");
				
			L_Keywords:
				Msg(c, Options.Name, "(Malcolm is waiting for me to say something.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Sorry, I don't know.<br/>Hmm... Maybe I should have a travel diary to write things down.");
				goto L_Keywords;
			}
			
			case "@repair":
			{
				Msg(c, "What item do you want to repair?<br/>You can repair various items such as Music Instruments and Glasses.");
				End();
			}

			case "@shop":
			{
				Msg(c, "Welcome to Malcolm's General Shop.<br/>Look around as much as you wish. Clothes, accessories and other goods are in stock.");
				OpenShop(c);
				End();
			}
		}
	}
}
