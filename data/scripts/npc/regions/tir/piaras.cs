using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PiarasScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_piaras");
		SetRace(10002);
		SetBody(height: 1.28f, fat: 0.9f, upper: 1.2f, lower: 1f);
		SetFace(skin: 22, eye: 1, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x8FB397);
		EquipItem(Pocket.Hair, 0xFA4, 0x3F4959);
		EquipItem(Pocket.Armor, 0x3A9B, 0x355047, 0xF6E2B1, 0xFBFBF3);
		EquipItem(Pocket.Shoe, 0x4274, 0x9C936F, 0x724548, 0x50685C);

		SetLocation(region: 7, x: 1344, y: 1225);

		SetDirection(182);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");

		Phrases.Add("Ah... The weather is just right to go on a journey.");
		Phrases.Add("Do you ever wonder who lives up that mountain?");
		Phrases.Add("Hey, you need to take your part-time job more seriously!");
		Phrases.Add("I haven't seen Malcolm around here today. He used to come by every day.");
		Phrases.Add("Nora, where are you? Nora?");
		Phrases.Add("The Inn is always bustling with people.");
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(c, Options.FaceAndName);
		Msg(c, "His straight posture gives him a strong, resolute impression even though he's only slightly taller than average height. Clean shaven, well groomed hair, spotless appearance and dark green vest make him look like a dandy.",
			"His neat looks, dark and thick eyebrows and the strong jaw line harmonized with the deep baritone voice complete the impression of an affable gentleman.");
		Enable(c, Options.FaceAndName);
		MsgSelect(c, "Welcome to my Inn.", "Start Conversation", "@talk", "Shop", "@shop");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@shop":
				Msg(c, "May I ask what you're looking for?");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Hello, nice to meet you.", "I am Piaras.");
				Disable(c, Options.Name);
				Msg(c, "(Piaras is waiting for me to say something.)");
				Enable(c, Options.Name);
				ShowKeywords(c);
				break;
				
			default:
				Msg(c, "I'd love to listen to you, but about something else.");
				ShowKeywords(c);
				break;
		}
	}
}
