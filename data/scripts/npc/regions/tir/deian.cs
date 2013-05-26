// Aura Script
// --------------------------------------------------------------------------
// Deian - Shepherd Boy
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DeianScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_deian");
		SetRace(10002);
		SetBody(height: .85f);
		SetFace(skin: 23, eye: 19, eyeColor: 0, lip: 0);
		SetStand("human/male/anim/male_natural_stand_npc_deian", "human/male/anim/male_natural_stand_npc_deian_talk");
		SetLocation("tir", 27953, 42287, 158);

		EquipItem(Pocket.Face, 4900, 0x1AB67C);
		EquipItem(Pocket.Hair, 4156, 0xE7CB60);
		EquipItem(Pocket.Armor, 15656, 0xE2EDC7, 0x4F5E44);
		EquipItem(Pocket.Glove, 16099, 0x343F2D);
		EquipItem(Pocket.Shoe, 17287, 0x4C392A);
		EquipItem(Pocket.Head, 18407, 0x343F2D);
		EquipItem(Pocket.RightHand1, 40001, 0x755748, 0x5E9A49, 0x5E9A49);

		Shop.AddTabs("Party Quest", "Gathering Tools");

        //----------------
        // Party Quest
        //----------------

        //Page 1
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest
        Shop.AddItem("Party Quest", 70025);		//Party Quest

        //----------------
        // Gathering Tools
        //----------------

        //Page 1
        Shop.AddItem("Gathering Tools", 40023);		//Gathering Knife

		Phrases.Add("Baa! Baa!");
		Phrases.Add("Geez, these sheep are a pain in the neck.");
		Phrases.Add("Hey, this way!");
		Phrases.Add("I don't understand. I have one extra...");
		Phrases.Add("I used to think they were cute. But it gets annoying when you have too many of them.");
		Phrases.Add("I wonder if I could buy a house with my savings yet...");
		Phrases.Add("I'm so bored. There's just nothing exciting around here.");
		Phrases.Add("It's amazing how fast they grow feeding on grass.");
		Phrases.Add("What the... Now there's one missing!");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "An adolescent boy carrying a shepherd's staff watches over a flock of sheep.<br/>Now and then, he hollers at some sheep that've wandered too far, and his voice cracks every time.<br/>His skin is tanned and his muscles are strong from his daily work.<br/>Though he's young, he peers at you with so much confidence it almost seems like arrogance.");
			
		Msg(c, "What can I do for you?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Upgrade Item", "@upgrade"));
		
		var r = Select(c);
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Nice to meet you, I am Deian.<br/>You don't look that old, maybe a couple of years older than I am?<br/>Let's just say we're the same age. You don't mind, do ya?");
				
			L_Keywords:
				Msg(c, Options.Name, "(Shepherd Boy Deian is paying attention to me.)");
				ShowKeywords(c);
				
				var keyword = Select(c);
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}

			case "@shop":
			{
				Msg(c, "I got nothing much, except for some quest scrolls. Are you interested?");
				OpenShop(c);
				End();
			}

			case "@upgrade":
			{
				Msg(c, "Upgrades! Who else would know more about that than the great Deian? Hehe...<br/>Now, what do you want to upgrade?<br/>Don't forget to check how many times you can upgrade that item and what type of upgrade it is before you give it to me...");
				End();
			}
		}
	}
}
