using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GoroScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_goro");
		SetRace(10105);
		SetBody(height: 0.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 32, eye: 3, eyeColor: 7, lip: 2);

		EquipItem(Pocket.Shoe, 0x426D, 0x441A19, 0x695C66, 0xBADB);
		EquipItem(Pocket.LeftHand1, 0x9C47, 0x405062, 0x7F7237, 0x729E);
		EquipItem(Pocket.RightHand1, 0xB3B1, 0x4F4F4B, 0x746C54, 0x4D1D77);

		SetLocation(region: 28, x: 1283, y: 3485);

		SetDirection(198);
		SetStand("monster/anim/goblin/natural/goblin01_natural_stand_friendly");

		Phrases.Add("Here, you may enter Alby Arena.");
		Phrases.Add("Test your strength here, in Alby Arena!");
		Phrases.Add("Wait, do not attack.");
		Phrases.Add("We exchange Stars with Arena Coins.");
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(c, Options.FaceAndName);
		Msg(c, "With his rough skin, menacing face, and his constant hard-breathing,",
			"he has the sure look of a Goblin.",
			"Yet, there is something different about this one.",
			"Strangely, it appears to have a sense of noble demeanor that does not match its rugged looks.");
		Enable(c, Options.FaceAndName);
		MsgSelect(c, "How can I help you?", "Start Conversation", "@talk", "Shop", "@shop");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@shop":
				OpenShop(c);
				Msg(c, "What do you need?",
				"You must be fully prepared if you wish to enter the Battle Arena.");
				break;

			case "@talk":
				Msg(c, "Welcome, what a smart man you are.<br/>My name is Goro.<br/>Ah, do not be surprised. I have no intention of hurting you.");
				Disable(c, Options.Name);
				Msg(c, "(Goro is looking at me)");
				Enable(c, Options.Name);
				ShowKeywords(c);
				break;
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
