using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;

public class NoraScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_nora");
		SetRace(10001);
		SetBody(height: .85f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 3900, 14604266, 10666036, 18968);
		EquipItem(Pocket.Hair, 3025, 13867649, 13867649, 13867649);
		EquipItem(Pocket.Armor, 15010, 3434862, 16641770, 13031658);
		EquipItem(Pocket.Shoe, 17006, 3434862, 10245519, 9444693);

		SetLocation(region: 1, x: 15933, y: 33363);

		SetDirection(186);
		SetStand("human/female/anim/female_natural_stand_npc_nora02");

		Phrases.Add("I hope the clothes dry quickly.");
		Phrases.Add("I would love to listen to some music, but I don't see any musicians around.");
		Phrases.Add("No way! There's no such thing as a huge spider.");
		Phrases.Add("Oh no! Rats!");
		Phrases.Add("Perhaps I should consider taking a day off.");
		Phrases.Add("Please wait.");
		Phrases.Add("Wait a second.");
		Phrases.Add("Wow! Look at that owl! Beautiful!");

		Shop.AddTabs("Tailoring", "Sewing Patterns", "Gift", "Quest", "Cooking Appliances");
	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "A girl wearing a well-ironed green apron leans forward, gazing cheerfully at her sorroundings.",
			"Her bright eyes are azure blue and a faint smile plays on her lips.",
			"Cross-shaped earrings dangle from her ears, dancing playfully between her honey-blonde hair.",
			"Her hands are always busy, as she engages in some chore or another, though she often looks into the distance as if deep in thought.");
		MsgSelect(c, "How can I help you?", "Start Conversation", "@talk", "Shop", "@shop", "Repair Item", "@repair");
	}

	public override void OnSelect(WorldClient c, string r, string i = null)
	{
		switch (r)
		{
			case "@shop":
				Msg(c, "Are you looking for a Tailoring Kit and materials?",
					"If so, you've come to the right place.");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Welcome!");
				Msg(c, true, false, "(Nora is looking in my direction.)");
				ShowKeywords(c);
				break;

			case "@repair":
				Msg(c, "Do you want to repair your clothes?",
					"Well I can't say I'm perfect at it,",
					"but I'll do my best.",
					"Just in case, when in doubt, you can always go to a professional tailor.");
				break;
				
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}