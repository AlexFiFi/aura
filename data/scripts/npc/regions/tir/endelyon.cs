using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EndelyonScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_endelyon");
		SetRace(10001);
		SetBody(height: 1.06f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x6AC6A8, 0xE6CA60, 0x5E6A5E);
		EquipItem(Pocket.Hair, 0xBCE, 0x5E423E, 0x5E423E, 0x5E423E);
		EquipItem(Pocket.Armor, 0x3AA1, 0x303133, 0xC6D8EA, 0xDBC741);
		EquipItem(Pocket.Shoe, 0x4277, 0x303133, 0xA0927D, 0x4F548D);

		SetLocation(region: 1, x: 5975, y: 36842);

		SetDirection(0);
		SetStand("human/female/anim/female_natural_stand_npc_Endelyon");

		Phrases.Add("Hmm... Something doesn't feel right.");
		Phrases.Add("I really need some help here...");
		Phrases.Add("It's so hard to do this all by myself!");
		Phrases.Add("Why do people like such things?");

	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "An elegent young lady wears the simple black dress of a Lymilark priestess.",
			"Her face is set in a calm, demur expression, and her eyes exude warmth.",
			"A slight smile tugging at her lips hints at a strong will.");
		MsgSelect(c, "May I help you?", "Start Conversation", "@talk", "Shop", "@shop", "Modify", "@modify");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@endmodify":
				Msg(c, "Do you want me to stop...? Well, then... Next time...");
				break;
			case "@modify":
				MsgSelect(c, "Are you asking me...to modify your item?<br/>Honestly, I am not sure if I can, but if you still want me to, I'll give it a try<br/>Please choose an item to modify.",
					"End Conversation", "@endmodify");
				break;

			case "@shop":
				Msg(c, "What are you looking for?");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "I don't think we've ever met. Nice to meet you.");
				Msg(c, true, false, "(Endelyon is looking in my direction.)");
				ShowKeywords(c);
				break;
			default:
				Msg(c, "It doesn't sound familiar to me. I mean...");
				ShowKeywords(c);
				break;
		}
	}
}
