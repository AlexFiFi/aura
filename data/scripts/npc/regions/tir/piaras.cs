// Aura Script
// --------------------------------------------------------------------------
// Piaras - Innkeeper
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
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
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");
		SetLocation("tir_inn", 1344, 1225, 182);

		EquipItem(Pocket.Face, 4900, 0x8FB397);
		EquipItem(Pocket.Hair, 4004, 0x3F4959);
		EquipItem(Pocket.Armor, 15003, 0x355047, 0xF6E2B1, 0xFBFBF3);
		EquipItem(Pocket.Shoe, 17012, 0x9C936F, 0x724548, 0x50685C);

		Phrases.Add("Ah... The weather is just right to go on a journey.");
		Phrases.Add("Do you ever wonder who lives up that mountain?");
		Phrases.Add("Hey, you need to take your part-time job more seriously!");
		Phrases.Add("I haven't seen Malcolm around here today. He used to come by every day.");
		Phrases.Add("Nora, where are you? Nora?");
		Phrases.Add("The Inn is always bustling with people.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"His straight posture gives him a strong, resolute impression even though he's only slightly taller than average height. Clean shaven, well groomed hair, spotless appearance and dark green vest make him look like a dandy.",
			"His neat looks, dark and thick eyebrows and the strong jaw line harmonized with the deep baritone voice complete the impression of an affable gentleman."
		);
		MsgSelect(c, "Welcome to my Inn.", "Start Conversation", "@talk", "Shop", "@shop");
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Hello, nice to meet you.<br/>I am Piaras.");
				
			L_Keywords:
				Msg(c, Options.Name, "(Piaras is waiting for me to say something.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "I'd love to listen to you, but about something else.");
				goto L_Keywords;
			}
			
			case "@shop":
			{
				Msg(c, "May I ask what you're looking for?");
				OpenShop(c);
				End();
			}
		}
	}
}
