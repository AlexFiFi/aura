// Aura Script
// --------------------------------------------------------------------------
// Edern - Blacksmith
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EdernScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_edern");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1.4f, upper: 2f, lower: 1.4f);
		SetFace(skin: 25, eye: 9, eyeColor: 38, lip: 2);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1328, 0xF89A47, 0xF9B6CA, 0xF89947);
		EquipItem(Pocket.Hair, 0xFBC, 0xC0BC92, 0xC0BC92, 0xC0BC92);
		EquipItem(Pocket.Armor, 0x3ABF, 0x9B5033, 0x9A835F, 0x321007);
		EquipItem(Pocket.Glove, 0x407E, 0xEABE7D, 0x808080, 0x57685E);
		EquipItem(Pocket.Shoe, 0x4460, 0x2B1C09, 0x857756, 0x321007);
		EquipItem(Pocket.RightHand1, 0x9C58, 0xFACB5F, 0x4F3C26, 0xFAB052);

		SetLocation(region: 31, x: 10972, y: 13373);

		SetDirection(76);
		SetStand("human/male/anim/male_natural_stand_npc_edern");

        Shop.AddTabs("Weapon", "Advanced Weapon", "Armor");

        //----------------
        // Weapon
        //----------------

        //Page 1
        Shop.AddItem("Weapon", 40081);		//Leather Long Bow
        Shop.AddItem("Weapon", 40079);		//Mace
        Shop.AddItem("Weapon", 40078);		//Bipennis
        Shop.AddItem("Weapon", 40080);		//Gladius
        Shop.AddItem("Weapon", 40404);		//Physis Wooden Lance

        //----------------
        // Armor
        //----------------

        //Page 1
        Shop.AddItem("Armor", 16525);		//Arish Ashuvain Gauntlet
        Shop.AddItem("Armor", 17518);		//Arish Ashuvain Boots (M)
        Shop.AddItem("Armor", 17519);		//Arish Ashuvain Boots (F)
        Shop.AddItem("Armor", 13047);		//Kirinusjin's Half-plate Armor (M)
        Shop.AddItem("Armor", 13048);		//Kirinusjin's Half-plate Armor (F)
        Shop.AddItem("Armor", 13045);		//Arish Ashuvain Armor (M)
        Shop.AddItem("Armor", 13046);		//Arish Ashuvain Armor (F)
        Shop.AddItem("Armor", 13043);		//Leminia's Holy Moon Armor (M)
        Shop.AddItem("Armor", 13044);		//Leminia's Holy Moon Armor (F)
        
		Phrases.Add("A true blacksmith never complains.");
		Phrases.Add("Hahaha...");
		Phrases.Add("Hey! Don't just stand there and make me nervous. If you've got something to say, say it!");
		Phrases.Add("Hey, you! You there! Don't just snoop around. Come in!");
		Phrases.Add("How I wish for a hard-working young man or woman to help...");
		Phrases.Add("I hope Elen learns all my trade skills soon...");
		Phrases.Add("I'll have to have some food first.");
		Phrases.Add("Kids nowadays don't want to do hard work... Grrr...");
		Phrases.Add("Let's see... Elen's mom was supposed to come in sometime.");
		Phrases.Add("So many lazy kids. That's a problem.");
		Phrases.Add("So many people underestimate blacksmith work.");
		Phrases.Add("The town is lively as usual.");
		Phrases.Add("Yes... This is the true scent of metal.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "Between the long strands of white hair, you can see the wrinkles on his face and neck that show his old age.<br/>But his confidence and well-built torso with copper skin reveal that this man is anything but fragile.<br/>His eyes encompass both the passion of youth and the wisdom of old age.<br/>The thick brows that shoot upward with wrinkles add a fierce look, but his eyes are of soft amber tone.");
        Msg(c, "You must have something to say to me.", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Modify Item", "@modify"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Welcome! You look familiar.");

            L_Keywords:
                Msg(c, Options.Name, "(Edern is slowly looking me over.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "Are you looking for something?<br/>If you're looking for normal equipment, talk to Elen.<br/>I only deal with special equipment that you can't find anywhere else.");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                Msg(c,
                "If it's not urgent, would you mind talking to Elen?<br/>If it's something you particularly treasure, I can repair it myself.",
                Button("End Conversation", "@endrepair")
            );

                r = Select(c);

                Msg(c, "You can figure out a person by looking at his equipment.<br/>Please do be careful with your equipment.");
                End();
            }
            case "@modify":
            {
                Msg(c,
                    "Then give me the item to be modified<br/>I ask this for your own good, but, while the weapons are not affected,<br/>armor that has been modified will be yours only. You know that, right?<br/>It won't fit anyone else.",
                    Button("End Conversation", "@endmodify")
                );
                r = Select(c);
                Msg(c, "Then come back to me when you have something you want to modify.");
                End();
            }
        }
    }
}
