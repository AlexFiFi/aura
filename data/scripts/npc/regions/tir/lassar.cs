using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LassarScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_lassar");
		SetRace(10001);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 153, eyeColor: 25, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0xF67F3D);
		EquipItem(Pocket.Hair, 0xC48, 0xD25D5D);
		EquipItem(Pocket.Armor, 0x3D29, 0x394254, 0x394254, 0x574747);
		EquipItem(Pocket.Shoe, 0x4385, 0x394254, 0x0, 0x0);
		EquipItem(Pocket.LeftHand1, 0x9DE2, 0x808080, 0x0, 0x0);
		EquipItem(Pocket.RightHand1, 0xB3C7, 0x808080, 0x0, 0x0);

		SetLocation(region: 9, x: 2020, y: 1537);

		SetDirection(202);
		SetStand("human/female/anim/female_natural_stand_npc_lassar02", "human/female/anim/female_natural_stand_npc_lassar_talk");

		Phrases.Add("....");
		Phrases.Add("And I have to supervise an advancement test.");
		Phrases.Add("Come to think of it, I have to come up with questions for the test.");
		Phrases.Add("Funny, I had never thought I would do this kind of work when I was young.");
		Phrases.Add("I hope I can go gather some herbs soon...");
		Phrases.Add("I should put more clothes on. It's kind of cold.");
		Phrases.Add("I think I'll wait a little longer...");
		Phrases.Add("Is there any problem in my teaching method?");
		Phrases.Add("Perhaps I could take today off...");
		Phrases.Add("The weather will be fine for some time, it seems.");
		Phrases.Add("Will things get better tomorrow, I wonder?");
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(c, Options.FaceAndName);
		Msg(c, "Waves of her red hair come down to her shoulders.",
			"Judging by her somewhat small stature, well-proportioned body, and a neat two-piece school uniform, it isn't had to tell that she is a teacher.",
			"The intelligent look in her eyes, the clear lip line and eyebrows present her as a charming lady.");
		Enable(c, Options.FaceAndName);
		MsgSelect(c, "Is there anything I can help you with?", "Start Conversation", "@talk", "Shop", "@shop", "Repair Item", "@repair", "Upgrade Item", "@upgrade");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@repair":
				Msg(c, "You want to repair a magic weapon?",
					"Don't ask Ferghus to repair magic weapons. Although he won't even do it...",
					"I can't imagine what would happen...if you tried to repair it like a regular weapon.");
				break;

			case "@shop":
				Msg(c, "If you want to learn magic, you've come to the right place.");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Ummm... Are you " + c.Character.Name + ", by any chance?");
				Msg(c, "Hahaha! You look just like what Bebhinn described.",
					"Excuse my laughing.",
					"Good to meet you.",
					"I am Lassar.");
				Disable(c, Options.Name);
				Msg(c, "(Lassar is waiting for me to say something.)");
				Enable(c, Options.Name);
				ShowKeywords(c);
				break;

			case "@upgrade":
				Msg(c, "You're looking to upgrade something?",
					"Hehe, how smart of you to come to a magic school teacher.",
					"Let me see what you're trying to upgrade.",
					"You know that the amount and type of upgrade available differs with each item, right?");
				break;

			default:
				Msg(c, "Why don't you ask other people? I'm afraid I would be of little help.");
				ShowKeywords(c);
				break;
		}
	}
}
