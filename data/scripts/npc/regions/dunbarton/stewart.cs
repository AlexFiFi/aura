// Aura Script
// --------------------------------------------------------------------------
// Stewart - Magic School Teacher
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class StewartScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_stewart");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 120, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0x49C2AF, 0x609F, 0x447045);
		EquipItem(Pocket.Hair, 0xFAA, 0x997744, 0x997744, 0x997744);
		EquipItem(Pocket.Armor, 0x3A9A, 0xF7941D, 0xA0927D, 0xB80026);
		EquipItem(Pocket.Shoe, 0x4274, 0xB80026, 0x4F548D, 0x904959);
		EquipItem(Pocket.Head, 0x466D, 0x625F44, 0xC1C1C1, 0xCEA96B);
		EquipItem(Pocket.Robe, 0x4A3B, 0x993333, 0x221111, 0x664444);

		SetLocation(region: 18, x: 2671, y: 1771);

		SetDirection(99);
		SetStand("");

        Shop.AddTabs("Magic Items", "Spellbook", "Magic Weapons", "Quest");

        //----------------
        // Magic Items
        //----------------

        //Page 1
        Shop.AddItem("Magic Items", 63000);		//Phoenix Feather
        Shop.AddItem("Magic Items", 63000, 10);		//Phoenix Feather
        Shop.AddItem("Magic Items", 63001);		//Wings of a Goddess
        Shop.AddItem("Magic Items", 63001, 5);		//Wings of a Goddess
        Shop.AddItem("Magic Items", 62003);		//Blessed Magic Powder
        Shop.AddItem("Magic Items", 62003, 10);		//Blessed Magic Powder
        Shop.AddItem("Magic Items", 62001);		//Elite Magic Powder
        Shop.AddItem("Magic Items", 62001, 10);		//Elite Magic Powder
        Shop.AddItem("Magic Items", 62014);		//Spirit Weapon Restoration Potion

        //----------------
        // Spellbook
        //----------------

        //Page 1
        Shop.AddItem("Spellbook", 1009);		//A Guidebook on Firebolt
        Shop.AddItem("Spellbook", 1008);		//Icebolt Spell: Origin and Training
        Shop.AddItem("Spellbook", 1010);		//Basics of Lightning Magic: the Lightning Bolt
        Shop.AddItem("Spellbook", 1007);		//Healing: The Basics of Magic

        //----------------
        // Magic Weapons
        //----------------

        //Page 1
        Shop.AddItem("Magic Weapons", 40038);		//Lightning Wand
        Shop.AddItem("Magic Weapons", 40039);		//Ice Wand
        Shop.AddItem("Magic Weapons", 40040);		//Fire Wand
        Shop.AddItem("Magic Weapons", 40041);		//Combat Wand
        Shop.AddItem("Magic Weapons", 40090);		//Healing Wand
        Shop.AddItem("Magic Weapons", 40231);		//Crystal Lightning Wand
        Shop.AddItem("Magic Weapons", 40232);		//Crown Ice Wand
        Shop.AddItem("Magic Weapons", 40233);		//Phoenix Fire Wand
        Shop.AddItem("Magic Weapons", 40234);		//Tikka Wood Healing Wand

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70023); //Collecting Quests
        Shop.AddItem("Quest", 70023);
        Shop.AddItem("Quest", 70023);
        
		Phrases.Add("Hmm... I'll have to talk with Kristell about this.");
		Phrases.Add("Hmm... There aren't enough textbooks available.");
		Phrases.Add("I wonder if Aeira has prepared all the books.");
		Phrases.Add("It's not going to work like this.");
		Phrases.Add("Maybe I should ask Aranwen...");
		Phrases.Add("More and more people are not showing up...");
		Phrases.Add("Oh dear! I've already run out of magic materials.");
		Phrases.Add("Perhaps there's something wrong with my lecture?");

	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "He is a young man with nerdy spectacles and tangled hair.<br/>Beneath his glasses, his soft eyes are somewhat appealing,<br/>but his stained tunic and his hands with reek of herbs confirm that he is clumsy and unkempt.");
        MsgSelect(c, "How can I help you?", Button("Start a Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Upgrade Item", "@upgrade"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Mmm... How can I help you?");

            L_Keywords:
                Msg(c, Options.Name, "(Stewart is looking in my direction.)");
                ShowKeywords(c);
                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "I have a few items related to magic here.<br/>You can buy some if you need any.");
                OpenShop(c);
                End();
            }
            case "@repair":
            {
                MsgSelect(c,
                "Do you want to repair your magic weapon?<br/>All magic weapons are laden with Mana, so it's impossible to physically fix them.<br/>If you fix them the way blacksmiths fix swords, then they may lose all the magic powers that come with them.",
                Button("End Conversation", "@endrepair")
                );

                r = Wait();

                Msg(c, "Please handle with care..");
                End();
            }
            case "@upgrade":
            {
                MsgSelect(c,
                    "You want to upgrade something?<br/>First, let me see the item.<br/>Remember that the amount and type of upgrade varies with each item.",
                    Button("End Conversation", "@endupgrade")
                );

                r = Wait();

                Msg(c, "Come see me again next time if you have something else to upgrade.");
                End();
            }
        }
    }
}
