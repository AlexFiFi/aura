// Aura Script
// --------------------------------------------------------------------------
// Aeira - Book Store
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AeiraScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aeira");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 2, eyeColor: 27, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xAB6523, 0x6C696C, 0x650800);
		EquipItem(Pocket.Hair, 0xBCE, 0x664444, 0x664444, 0x664444);
		EquipItem(Pocket.Armor, 0x3AC2, 0xEBAE98, 0x354E34, 0xE3E4EE);
		EquipItem(Pocket.Shoe, 0x4280, 0xA0505E, 0xF8784F, 0x6E41);
		EquipItem(Pocket.Head, 0x466C, 0x746C54, 0xC0C0C0, 0x7C8C);

		SetLocation(region: 14, x: 44978, y: 43143);

		SetDirection(158);
		SetStand("human/female/anim/female_natural_stand_npc_Aeira");

        Shop.AddTabs("Skill Book", "Life Skill Book", "Literature");

        //----------------
        // Skill Book
        //----------------

        //Page 1
        Shop.AddItem("Skill Book", 1006);		//Introduction to Music Composition
        Shop.AddItem("Skill Book", 1012);		//Campfire Manual
        Shop.AddItem("Skill Book", 1505);		//The World of Handicrafts
        Shop.AddItem("Skill Book", 1302);		//Your First Glass of Wine Vol. 1
        Shop.AddItem("Skill Book", 1303);		//Your First Glass of Wine Vol. 2
        Shop.AddItem("Skill Book", 1011);		//Improving Your Composing Skill
        Shop.AddItem("Skill Book", 1304);		//Wine for the Everyman
        Shop.AddItem("Skill Book", 1018);		//The History of Music in Erinn (1)
        Shop.AddItem("Skill Book", 1305);		//Tin's Liquor Drop
        Shop.AddItem("Skill Book", 1083);		//Campfire Skill : Beyond the Kit
        Shop.AddItem("Skill Book", 1064);		//Master Chef's Cooking Class: Baking
        Shop.AddItem("Skill Book", 1065);		//Master Chef's Cooking Class: Simmering
        Shop.AddItem("Skill Book", 1019);		//The History of Music in Erinn (2)
        Shop.AddItem("Skill Book", 1066);		//About Kneading
        Shop.AddItem("Skill Book", 1020);		//Composition Lessons with Helene (1)
        Shop.AddItem("Skill Book", 1123);		//The Great Camping Companion: Camp Kit
        Shop.AddItem("Skill Book", 1007);		//Healing: The Basics of Magic
        Shop.AddItem("Skill Book", 1029);		//A Campfire Memory
        Shop.AddItem("Skill Book", 1114);		//The History of Music in Erinn (3)
        Shop.AddItem("Skill Book", 1111);		//The Path of Composing
        Shop.AddItem("Skill Book", 1013);		//Music Theory

        //----------------
        // Life Skill Book
        //----------------

        //Page 1
        Shop.AddItem("Life Skill Book", 1055);		//The Road to Becoming a Magic Warrior
        Shop.AddItem("Life Skill Book", 1056);		//How to Enjoy Field Hunting
        Shop.AddItem("Life Skill Book", 1092);		//Enchant, Another Mysterious Magic
        Shop.AddItem("Life Skill Book", 1124);		//An Easy Guide to Taking Up Residence in a Home
        Shop.AddItem("Life Skill Book", 1102);		//Your Pet
        Shop.AddItem("Life Skill Book", 1052);		//How to Milk a Cow
        Shop.AddItem("Life Skill Book", 1050);		//An Unemployed Man's Memoir of Clothes
        Shop.AddItem("Life Skill Book", 1040);		//Facial Expressions Require Practice too
        Shop.AddItem("Life Skill Book", 1046);		//Fire Arrow, The Ultimate Archery
        Shop.AddItem("Life Skill Book", 1021);		//The Tir Chonaill Environs
        Shop.AddItem("Life Skill Book", 1022);		//The Dunbarton Environs
        Shop.AddItem("Life Skill Book", 1043);		//Wizards Love the Dark
        Shop.AddItem("Life Skill Book", 1057);		//Introduction to Field Bosses
        Shop.AddItem("Life Skill Book", 1058);		//Understanding Wisps
        Shop.AddItem("Life Skill Book", 1015);		//Seal Stone Research Almanac : Rabbie Dungeon
        Shop.AddItem("Life Skill Book", 1016);		//Seal Stone Research Almanac : Ciar Dungeon
        Shop.AddItem("Life Skill Book", 1017);		//Seal Stone Research Almanac : Dugald Aisle
        Shop.AddItem("Life Skill Book", 1033);		//Guidebook for Dungeon Exploration - Theory
        Shop.AddItem("Life Skill Book", 1034);		//Guidebook for Dungeon Exploration - Practicum
        Shop.AddItem("Life Skill Book", 1035);		//An Adventurer's Memoir
        Shop.AddItem("Life Skill Book", 1077);		//Wanderer of the Fiodh Forest
        Shop.AddItem("Life Skill Book", 1090);		//How Am I Going to Survive Like This?
        Shop.AddItem("Life Skill Book", 1031);		//Understanding Elementals
        Shop.AddItem("Life Skill Book", 1036);		//Records of the Bangor Seal Stone Investigation
        Shop.AddItem("Life Skill Book", 1072);		//Cooking on Your Own Vol. 1
        Shop.AddItem("Life Skill Book", 1073);		//Cooking on Your Own Vol. 2

        //----------------
        // Literature
        //----------------

        //Page 1
        Shop.AddItem("Literature", 1023);		//The Story of Spiral Hill
        Shop.AddItem("Literature", 1025);		//Mystery of the Dungeon
        Shop.AddItem("Literature", 1026);		//A Report on Astralium
        Shop.AddItem("Literature", 1027);		//I Hate Cuteness
        Shop.AddItem("Literature", 1028);		//Tracy's Secret
        Shop.AddItem("Literature", 1032);		//The Shadow Mystery
        Shop.AddItem("Literature", 1140);		//It's a 'paper airplane' that flies.
        Shop.AddItem("Literature", 1001);		//The Story of a White Doe
        Shop.AddItem("Literature", 1059);		//A Campfire Story
        Shop.AddItem("Literature", 1060);		//Imp's Diary
        Shop.AddItem("Literature", 1061);		//The Tale of Ifan the Rich
        Shop.AddItem("Literature", 1042);		//Animal-loving Healer
        Shop.AddItem("Literature", 1103);		//The Story of a Lizard
        Shop.AddItem("Literature", 1104);		//The Origin of Moon Gates
        Shop.AddItem("Literature", 74028);		//The Forgotten Legend of Fiodh Forest
        Shop.AddItem("Literature", 74029);		//The Tragedy of Emain Macha
        Shop.AddItem("Literature", 74027);		//The Knight of Light Lugh, The Hero of Mag Tuireadh
        
        Phrases.Add("*cough* The books are too dusty...");
		Phrases.Add("*Whistle*");
		Phrases.Add("Hahaha.");
		Phrases.Add("Hmm. I can't really see...");
		Phrases.Add("Hmm. The Bookstore is kind of small.");
		Phrases.Add("I wonder if this book would sell?");
		Phrases.Add("I wonder what Stewart is up to?");
		Phrases.Add("Kristell... She's unfair.");
		Phrases.Add("Oh, hello!");
		Phrases.Add("Umm... So...");
		Phrases.Add("Whew... I should just finish up the transcription.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "This girl seems to be in her late teens with big thick glasses resting at the tip of her nose.<br/>Behind the glasses are two large, round brown eyes shining brilliantly.<br/>Wearing a loose-fitting dress, she has a ribbon made of soft and thin material around her neck.");
        Msg(c, "So, what can i help you with?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Hahaha. I... Umm... I think I've met you before...<br/>Your name was...<br/>Oh, I'm sorry, <username/>. My mind went blank for a second. Hehehe.");

            L_Keywords:
                Msg(c, Options.Name, "(Aeira is slowly looking me over.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "Welcome to the Bookstore.");
                OpenShop(c);
                End();
            }
        }
    }
}
