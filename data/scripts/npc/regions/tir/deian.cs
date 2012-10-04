using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;

public class DeianScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_deian");
		SetRace(10002);
		SetBody(height: .85f);
		SetFace(skin: 23, eye: 19, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 4900, 0x1AB67C);
		EquipItem(Pocket.Hair, 4156, 0xE7CB60);
		EquipItem(Pocket.Armor, 15656, 0xE2EDC7, 0x4F5E44);
		EquipItem(Pocket.Glove, 16099, 0x343F2D);
		EquipItem(Pocket.Shoe, 17287, 0x4C392A);
		EquipItem(Pocket.Head, 18407, 0x343F2D);
		EquipItem(Pocket.LeftHand1, 40001, 0x755748, 0x5E9A49, 0x5E9A49);

		SetLocation(region: 1, x: 27953, y: 42287);

		SetDirection(158);
		SetStand("human/male/anim/male_natural_stand_npc_deian", "human/male/anim/male_natural_stand_npc_deian_talk");

		Shop.AddTabs("Quest");

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

	public override void OnTalk(WorldClient c)
	{
		Msg(c, "An adolescent boy carrying a shepherd's staff watches over a flock of sheep.",
			"Now and then, he hollers at some sheep that've wandered too far, and his voice cracks every time.",
			"His skin is tanned and his muscles are strong from his daily work.",
			"Though he's young, he peers at you with so much confidence it almost seems like arrogance.");
		MsgSelect(c, "What can I do for you?", "Start Conversation", "@talk", "Shop", "@shop", "Upgrade Item", "@upgrade");
	}

	public override void OnSelect(WorldClient c, string r, string i = null)
	{
		switch (r)
		{
			case "@shop":
				Msg(c, "I got nothing much, except for some quest scrolls. Are you interested?");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Nice to meet you, I am Deian.",
					"You don't look that old, maybe a couple of years older than I am?",
					"Let's just say we're the same age. You don't mind, do ya?");
				Msg(c, true, false, "(Shepherd Boy Deian is paying attention to me.)");
				ShowKeywords(c);
				break;

			case "@upgrade":
				Msg(c, "Upgrades! Who else would know more about that than the great Deian? Hehe...",
					"Now, what do you want to upgrade?",
					"Don't forget to check how many times you can upgrade that item and what type of upgrade it is before you give it to me...");
				break;

			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}