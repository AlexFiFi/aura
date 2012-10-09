using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class RanaldScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ranald");
		SetRace(10002);
		SetBody(height: 1f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 20, eye: 0, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xF88B4A);
		EquipItem(Pocket.Hair, 0x103A, 0x4D4B53);
		EquipItem(Pocket.Armor, 0x3D24, 0xAC9271, 0x4D4F48, 0x7C6144);
		EquipItem(Pocket.Shoe, 0x4274, 0x9C7D6C, 0xFFC9A3, 0xF7941D);
		EquipItem(Pocket.LeftHand1, 0x9C4C, 0xDCDCDC, 0xC08B48, 0x808080);

		SetLocation(region: 1, x: 4651, y: 32166);

		SetDirection(195);
		SetStand("human/male/anim/male_natural_stand_npc_ranald02", "human/male/anim/male_natural_stand_npc_ranald_talk");

		Phrases.Add("I need a drink...");
		Phrases.Add("I guess I drank too much last night...");
		Phrases.Add("I need a nap...");
		Phrases.Add("I should drink in moderation...");
		Phrases.Add("I should sharpen my blade later.");
		Phrases.Add("It's really dusty here.");
		Phrases.Add("What's with the hair styles of today's kids?");
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(Options.Face | Options.Name);
		Msg(c, "From his appearance and posture, there is no doubt that he is well into middle age, but he is surprisingly well-built and in good shape.",
			"Long fringes of hair cover half of his forehead and right cheek. A strong nose bridge stands high between his shining hawkish eyes.",
			"His deep, low voice has the power to command other people's attention.");
		Enable(Options.Face | Options.Name);
		MsgSelect(c, "How can I help you?", "Start Conversation", "@talk", "Shop", "@shop", "Modify Item", "@modify", "Get Ciar Beginner Dungeon Pass", "@pass");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@modify":
				Msg(c, "Hmm... You want me to modify your item? You got some nerve!",
					"Ha ha. Just joking. Do you need to modify an item? Count on Ranald.",
					"Pick an item to modify.",
					"Oh, before that. Types or numbers of modifications are different depending on what item you want to modify. Always remember that.");
				break;

			case "@pass":
				GiveItem(c, 63139);
				Notice(c, "Recieved Ciar Beginner Dungeon Pass from Ranald.");
				Msg(c, "Ok, here's the pass.",
					"You can ask for it again if you need it.",
					"That doesn't mean you can fill up the iventory with a pile of passes.");
				break;

			case "@shop":
				Msg(c, "Tell me if you need a Quest Scroll.",
					"Working on these quests can also be a good way to train yourself.");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Hmm...", "Nice to meet you.");
				Disable(Options.Name);
				Msg(c, "(Ranald is paying attention to me.)");
				Enable(Options.Name);
				ShowKeywords(c);
				break;
				
			default:
				Msg(c, "Well, I don't really know...");
				ShowKeywords(c);
				break;
		}
	}
}
