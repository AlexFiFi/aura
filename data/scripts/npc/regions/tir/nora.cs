// Aura Script
// --------------------------------------------------------------------------
// Nora - Inn Helper
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NoraScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_nora");
		SetRace(10001);
		SetBody(height: 0.85f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);
		SetStand("human/female/anim/female_natural_stand_npc_nora02");
		SetLocation("tir", 15933, 33363, 186);

		EquipItem(Pocket.Face, 3900, 0xDED7EA, 0xA2C034, 0x004A18);
		EquipItem(Pocket.Hair, 3025, 0xD39A81, 0xD39A81, 0xD39A81);
		EquipItem(Pocket.Armor, 15010, 0x34696E, 0xFDEEEA, 0xC6D8EA);
		EquipItem(Pocket.Shoe, 17006, 0x34696E, 0x9C558F, 0x901D55);

		Shop.AddTabs("Tailoring", "Sewing Patterns", "Gift", "Quest", "Cooking Appliances");

        //----------------
        // Tailoring
        //----------------

        //Page 1
        Shop.AddItem("Tailoring", 60001);		//Tailoring Kit
        Shop.AddItem("Tailoring", 60015);		//Cheap Finishing Thread
        Shop.AddItem("Tailoring", 60015, 5);		//Cheap Finishing Thread
        Shop.AddItem("Tailoring", 60031);		//Regular Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60019);		//Cheap Fabric
        Shop.AddItem("Tailoring", 60019, 5);		//Cheap Fabric
        Shop.AddItem("Tailoring", 60016);		//Common Finishing Thread
        Shop.AddItem("Tailoring", 60016, 5);		//Common Finishing Thread
        Shop.AddItem("Tailoring", 60017);		//Fine Finishing Thread
        Shop.AddItem("Tailoring", 60055);		//Fine Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60017, 5);		//Fine Finishing Thread
        Shop.AddItem("Tailoring", 60046);		//Finest Silk Weaving Gloves
        Shop.AddItem("Tailoring", 60018);		//Finest Finishing Thread
        Shop.AddItem("Tailoring", 60057);		//Fine Fabric Weaving Gloves
        Shop.AddItem("Tailoring", 60056);		//Finest Fabric Weaving Gloves
        Shop.AddItem("Tailoring", 60020);		//Common Fabric
        Shop.AddItem("Tailoring", 60018);		//Finest Finishing Thread
        Shop.AddItem("Tailoring", 60020, 5);		//Common Fabric

        //----------------
        // Sewing Patterns
        //----------------

        //Page 1
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern
        Shop.AddItem("Sewing Patterns", 60000);		//Sewing Pattern

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52014);		//Teddy Bear
        Shop.AddItem("Gift", 52016);		//Bunny Doll
        Shop.AddItem("Gift", 52015);		//Pearl Necklace
        Shop.AddItem("Gift", 52025);		//Gift Ring

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest
        Shop.AddItem("Quest", 70023);		//Collecting Quest

        //----------------
        // Cooking Appliances
        //----------------

        //Page 1
        Shop.AddItem("Cooking Appliances", 40042);		//Cooking Knife
        Shop.AddItem("Cooking Appliances", 40044);		//Ladle
        Shop.AddItem("Cooking Appliances", 40043);		//Rolling Pin
        Shop.AddItem("Cooking Appliances", 46005);		//Cooking Table
        Shop.AddItem("Cooking Appliances", 46004);		//Cooking Pot

		Phrases.Add("I hope the clothes dry quickly.");
		Phrases.Add("I would love to listen to some music, but I don't see any musicians around.");
		Phrases.Add("No way! There's no such thing as a huge spider.");
		Phrases.Add("Oh no! Rats!");
		Phrases.Add("Perhaps I should consider taking a day off.");
		Phrases.Add("Please wait.");
		Phrases.Add("Wait a second.");
		Phrases.Add("Wow! Look at that owl! Beautiful!");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "A girl wearing a well-ironed green apron leans forward, gazing cheerfully at her sorroundings.<br/>Her bright eyes are azure blue and a faint smile plays on her lips.<br/>Cross-shaped earrings dangle from her ears, dancing playfully between her honey-blonde hair.<br/>Her hands are always busy, as she engages in some chore or another, though she often looks into the distance as if deep in thought.");

		Msg(c, "How can I help you?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"));
		
		var r = Select(c);
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Welcome!");
				
			L_Keywords:
				Msg(c, Options.Name, "(Nora is looking in my direction.)");
				ShowKeywords(c);
				
				var keyword = Select(c);
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}
			
			case "@shop":
			{
				Msg(c, "Are you looking for a Tailoring Kit and materials?<br/>If so, you've come to the right place.");
				OpenShop(c);
				End();
			}

			case "@repair":
			{
				Msg(c, "Do you want to repair your clothes?<br/>Well I can't say I'm perfect at it,<br/>but I'll do my best.<br/>Just in case, when in doubt, you can always go to a professional tailor.");
				End();
			}
		}
	}
}
