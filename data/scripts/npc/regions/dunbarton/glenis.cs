// Aura Script
// --------------------------------------------------------------------------
// Glenis - Dunbarton Grocer
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GlenisScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_glenis");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 0.3f, upper: 1.4f, lower: 1.2f);
		SetFace(skin: 15, eye: 7, eyeColor: 119, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3E, 0x84C8, 0x323C99, 0xF35D80);
		EquipItem(Pocket.Hair, 0xBCC, 0xBC756C, 0xBC756C, 0xBC756C);
		EquipItem(Pocket.Armor, 0x3AA2, 0x764E63, 0xCCD8ED, 0xE7957A);
		EquipItem(Pocket.Shoe, 0x4274, 0x764E63, 0xFC9C5F, 0xD2CCE5);

		SetLocation(region: 14, x: 37566, y: 41605);

		SetDirection(129);
		SetStand("human/female/anim/female_natural_stand_npc_Glenis");

        Shop.AddTabs("Food", "Gift", "Quest", "Event");

        //----------------
        // Food
        //----------------

        //Page 1
        Shop.AddItem("Food", 50004);		//Bread
        Shop.AddItem("Food", 50002);		//Slice of Cheese
        Shop.AddItem("Food", 50206);		//Chocolate
        Shop.AddItem("Food", 50127);		//Shrimp
        Shop.AddItem("Food", 50114);		//Garlic
        Shop.AddItem("Food", 50114, 10);	//Garlic
        Shop.AddItem("Food", 50131);		//Sugar
        Shop.AddItem("Food", 50131, 10);	//Sugar
        Shop.AddItem("Food", 50132);		//Salt
        Shop.AddItem("Food", 50132, 10);	//Salt
        Shop.AddItem("Food", 50156);		//Pepper
        Shop.AddItem("Food", 50156, 10);	//Pepper
        Shop.AddItem("Food", 50153);		//Frying Powder
        Shop.AddItem("Food", 50148);		//Yeast
        Shop.AddItem("Food", 50148, 10);	//Yeast
        Shop.AddItem("Food", 50112);		//Strawberry
        Shop.AddItem("Food", 50112, 10);	//Strawberry
        Shop.AddItem("Food", 50142);		//Onion
        Shop.AddItem("Food", 50142, 10);	//Onion
        Shop.AddItem("Food", 50121);		//Butter
        Shop.AddItem("Food", 50121, 10);	//Butter
        Shop.AddItem("Food", 50108);		//Chicken Wing
        Shop.AddItem("Food", 50108, 10);	//Chicken Wing
        Shop.AddItem("Food", 50045);		//Pine Nut
        Shop.AddItem("Food", 50047);		//Camellia Seeds
        Shop.AddItem("Food", 50046);		//Juniper Berry
        Shop.AddItem("Food", 50135);		//Rice
        Shop.AddItem("Food", 50135, 10);	//Rice
        Shop.AddItem("Food", 50145);		//Olive Oil
        Shop.AddItem("Food", 50145, 10);	//Olive Oil
        Shop.AddItem("Food", 50005);		//Large Meat
        Shop.AddItem("Food", 50001);		//Big Lump of Cheese
        Shop.AddItem("Food", 50006);		//Slice of Meat
        Shop.AddItem("Food", 50122);		//Bacon
        Shop.AddItem("Food", 50122, 10);	//Bacon
        Shop.AddItem("Food", 50133);		//Beef
        Shop.AddItem("Food", 50123);		//Roasted Bacon
        Shop.AddItem("Food", 50134);		//Sliced Bread
        Shop.AddItem("Food", 50120);		//Steamed Rice
        Shop.AddItem("Food", 50102);		//Potato Salad
        Shop.AddItem("Food", 50104);		//Egg Salad
        Shop.AddItem("Food", 50101);		//Potato Egg Salad

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
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70070); //Cooking Quests
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        Shop.AddItem("Quest", 70070);
        
		Phrases.Add("Come buy your food here.");
		Phrases.Add("Flora! Are the ingredients ready?");
		Phrases.Add("Have a nice day today!");
		Phrases.Add("Please come again!");
		Phrases.Add("Thank you for coming!");
		Phrases.Add("This is Glenis' Restaurant.");
		Phrases.Add("This is today's special! Mushroom soup!");
		Phrases.Add("We are serving breakfast now.");
		Phrases.Add("Welcome!");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName,
            "With her round face and large, sparkling eyes, this middle aged woman appears to have a big heart.",
            "Her face, devoid of makeup, is dominated by her large eyes and a playful smile.",
	    "Over her lace collar she wears and old but well-polished locket."
        );
        MsgSelect(c, "Welcome!<br/>This is Glenis' Restaurant.", Button("Start a Conversation", "@talk"), Button("Shop", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
                {
                    Msg(c, "Come on in... Welcome to our restaurant.");

                L_Keywords:
                    Msg(c, Options.Name, "(Glenis is waiting for me to say something.)");
                    ShowKeywords(c);
                    var keyword = Wait();

                    Msg(c, "Can we change the subject?");
                    goto L_Keywords;
                }
            case "@shop":
                {
                    Msg(c, "Are you looking for any kind of food in particular?<br/>Take your pick.");
                    OpenShop(c);
                    End();
                }
}
}
}
