// Aura Script
// --------------------------------------------------------------------------
// Nerys - Weapons Dealer
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NerysScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_nerys");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 4, eyeColor: 31, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xF79E59, 0xF79D38, 0x573295);
		EquipItem(Pocket.Hair, 0xBCF, 0x994433, 0x994433, 0x994433);
		EquipItem(Pocket.Armor, 0x3AC3, 0x94C1C5, 0x6C9D9A, 0xBE8C92);
		EquipItem(Pocket.Glove, 0x3E88, 0x818775, 0x117C7D, 0xA3DC);
		EquipItem(Pocket.Shoe, 0x4269, 0x823021, 0x82C991, 0xF2597B);

		SetLocation(region: 14, x: 44229, y: 35842);

		SetDirection(139);
		SetStand("human/female/anim/female_natural_stand_npc_Nerys");

        Shop.AddTabs("Weapon", "Shoes Gloves", "Helmet", "Armor", "Event", "Arrowhead");

        //----------------
        // Weapon
        //----------------

        //Page 1
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
        Shop.AddItem("Weapon", 40006);		//Dagger
        Shop.AddItem("Weapon", 40179);		//Spiked Knuckle
        Shop.AddItem("Weapon", 40243);		//Battle Short Sword
        Shop.AddItem("Weapon", 40013);		//Long Bow
        Shop.AddItem("Weapon", 40015);		//Fluted Short Sword
        Shop.AddItem("Weapon", 40014);		//Composite Bow
        Shop.AddItem("Weapon", 40010);		//Longsword
        Shop.AddItem("Weapon", 40016);		//Warhammer
        Shop.AddItem("Weapon", 40244);		//Bear Knuckle
        Shop.AddItem("Weapon", 40011);		//Broadsword
        Shop.AddItem("Weapon", 40012);		//Bastard Sword
        Shop.AddItem("Weapon", 40180);		//Hobnail Knuckle

        //Page 2
        Shop.AddItem("Weapon", 40242);		//Battle Sword
        Shop.AddItem("Weapon", 40031);		//Crossbow
        Shop.AddItem("Weapon", 40745);		//Basic Control Bar
        Shop.AddItem("Weapon", 40404);		//Physis Wooden Lance
        Shop.AddItem("Weapon", 46001);		//Round Shield
        Shop.AddItem("Weapon", 46006);		//Kite shield

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 16009);	//Tork's Hunter Gloves
        Shop.AddItem("Shoes Gloves", 16005);	//Wood Plate Cannon
        Shop.AddItem("Shoes Gloves", 16017);	//Standard Gloves
        Shop.AddItem("Shoes Gloves", 16007);	//Cores Ninja Gloves
        Shop.AddItem("Shoes Gloves", 17506);	//Long Greaves
        Shop.AddItem("Shoes Gloves", 16501);	//Leather Protector
        Shop.AddItem("Shoes Gloves", 17501);	//Solleret Shoes
        Shop.AddItem("Shoes Gloves", 16500);	//Ulna Protector Gloves
        Shop.AddItem("Shoes Gloves", 17500);	//High Polean Plate Boots
        Shop.AddItem("Shoes Gloves", 16504);	//Counter Gauntlet
        Shop.AddItem("Shoes Gloves", 16505);	//Fluted Gauntlet

        //----------------
        // Helmet
        //----------------

        //Page 1
        Shop.AddItem("Helmet", 18513);		//Spiked Cap
        Shop.AddItem("Helmet", 18500);		//Ring Mail Helm
        Shop.AddItem("Helmet", 18504);		//Cross Full Helm
        Shop.AddItem("Helmet", 18502);		//Bone Helm
        Shop.AddItem("Helmet", 18501);		//Guardian Helm
        Shop.AddItem("Helmet", 18506);		//Wing Half Helm
        Shop.AddItem("Helmet", 18508);		//Slit Full Helm
        Shop.AddItem("Helmet", 18505);		//Spiked Helm
        Shop.AddItem("Helmet", 18509);		//Bascinet
        Shop.AddItem("Helmet", 18525);		//Waterdrop Cap
        Shop.AddItem("Helmet", 18522);		//Pelican Protector
        Shop.AddItem("Helmet", 18520);		//Steel Headgear
        Shop.AddItem("Helmet", 18521);		//European Comb
        Shop.AddItem("Helmet", 18515);		//Twin Horn Cap
        Shop.AddItem("Helmet", 18524);		//Four Wings Cap
        Shop.AddItem("Helmet", 18519);		//Panache Head Protector

        //----------------
        // Armor
        //----------------

        //Page 1
        Shop.AddItem("Armor", 14006);		//Linen Cuirass (F)
        Shop.AddItem("Armor", 14009);		//Linen Cuirass (M)
        Shop.AddItem("Armor", 14007);		//Padded Armor with Breastplate
        Shop.AddItem("Armor", 14013);		//Lorica Segmentata
        Shop.AddItem("Armor", 14005);		//Drandos Leather Mail (F)
        Shop.AddItem("Armor", 14011);		//Drandos Leather Mail (M)
        Shop.AddItem("Armor", 14017);		//Three-Belt Leather Mail
        Shop.AddItem("Armor", 14016);		//Cross Belt Leather Coat
        Shop.AddItem("Armor", 13017);		//Surcoat Chain Mail
        Shop.AddItem("Armor", 13001);		//Melka Chain Mail
        Shop.AddItem("Armor", 13010);		//Round Pauldron Chainmail

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
        
        Phrases.Add("At this rate, I won't have enough arrows next month...");
		Phrases.Add("Do you need something else?");
		Phrases.Add("I should have gone on the trip myself...");
		Phrases.Add("Manus is showing off his muscles again...");
		Phrases.Add("See something you like?");
		Phrases.Add("There are so many weapon repair requests this month.");
		Phrases.Add("This way, people. This way.");
		Phrases.Add("Wait, I shouldn't be doing this right now.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "This lady has a slender build and wears comfortable clothing.<br/>The subtle softness of her short red hair is brought out by being tightly combed back.<br/>Thick ruby earrings matching her hair dangle from her ears and<br/>slightly waver and glitter every time she looks up.");
        Msg(c, "Tell me if you need anything.", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Modify Item", "@modify"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "What are you looking for?");

            L_Keywords:
                Msg(c, Options.Name, "(Nerys is slowly looking me over.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "You brought your money with you, right?");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                Msg(c,
                "You can repair weapons, armor, and equipment here.<br/>I use expensive repair tools, so the fee is fairly hihg. Is that okay with you?<br/>I do make fewer mistakes because of that, though.",
                Button("End Conversation", "@endrepair")
                );

                r = Select(c);

                Msg(c, "If the repair fee is too much for you,<br/>try using some Holy Water of Lymilar.<br/>It should be a big help.<br/>Now, come again if there's anything that needs to be repaired.");
                End();
            }
            case "@modify":
            {
                Msg(c,
                    "Modification? Pick an item.<br/>I don't have to explain to you about<br/>the number of possible modification and the types, do I?",
                    Button("End Conversation", "@endmodify")
                );
                r = Select(c);
                Msg(c, "Is that all for today? Well, come back anytime you need me.");
                End();
            }
        }
    }
}
