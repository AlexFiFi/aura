using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;

public class DuncanScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_trefor");
		SetRace(10002);
		SetBody(height: 1.35f);
		SetFace(skin: 20, eye: 0, eyeColor: 27, lip: 0);

		EquipItem(Pocket.Face, 4909, 0x93005C);
		EquipItem(Pocket.Hair, 4023, 0xD43F34);
		EquipItem(Pocket.Armor, 14076, 0x1F1F1F, 0x303F36, 0x1F1F1F);
		EquipItem(Pocket.Glove, 16097, 0x303F36, 0x000000, 0x000000);
		EquipItem(Pocket.Shoe, 17282, 0x1F1F1F, 0x1F1F1F);
		EquipItem(Pocket.Head, 18405, 0x191919, 0x293D52);
		EquipItem(Pocket.LeftHand2, 40005, 0xB6B6C2, 0x404332, 0x22B653);

		SetLocation(region: 1, x: 8692, y: 52637);

		SetDirection(220);
		SetStand("human/male/anim/male_natural_stand_npc_trefor02", "human/male/anim/male_natural_stand_npc_trefor_talk");

		Phrases.Add("(Fart)...");
		Phrases.Add("(Spits out a loogie)");
		Phrases.Add("Ah-choo!");
		Phrases.Add("Ahem");
		Phrases.Add("Burp.");
		Phrases.Add("Cough cough...");
		Phrases.Add("I heard people can go bald when they wear a helmet for too long...");
		Phrases.Add("I need to get married...");
		Phrases.Add("It's been a while since I took a shower");
		Phrases.Add("Seems like I caught a cold...");
		Phrases.Add("Soo itchy... and I can't scratch it!");
		Phrases.Add("This helmet's really making me sweat");
	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "Quite a specimen of physical fitness appears before you wearing well-polished armor that fits closely the contours of his body.",
			"A medium-length sword hangs delicately from the scabbard at his waist. While definitely a sight to behold, it's difficult to see much of his face because of his lowered visor, but one cannot help but notice the flash in his eyes occasionally catching the light between the slits on his helmet.",
			"His tightly pursed lips seem to belie his desire to not shot any emotion.");
		MsgSelect(c, "How can I help you?", "Start Conversation", "@talk", "Shop", "@shop", "Upgrade Item", "@upgrade", "Get Alby Beginner Dungeon Pass", "@pass");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@talk":
				Msg(c, "Hmm? Are you a new traveler?");
				Msg(c, true, false, "(Trefor is waiting for me to say something.)");
				ShowKeywords(c);
				break;

			case "@shop":
				Msg(c, "Do you need a Quest Scroll?");
				OpenShop(c);
				break;

			case "@upgrade":
				Msg(c, "Do you want to modify an item?",
					"You don't need to go too far; I'll do it for you. Select an item that you'd like me to modify.",
					"I'm sure you know that the number of times it can be modified, as well as the types of modifications available depend on the item, right?");
				break;

			case "@pass":
				GiveItem(c, 63140);
				Notice(c, "Recieved Alby Beginner Dungeon Pass from Trefor");
				Msg(c, "Do you need an Alby Beginner Dungeon Pass?",
					"No problem. Here you go.",
					"Drop by anytime when you need more.",
					"I'm a generous man, ha ha.");
				break;
				
			default:
				Msg(c, "Oh, is that so?");
				ShowKeywords(c);
				break;
		}
	}
}