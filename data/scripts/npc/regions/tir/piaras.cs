// Aura Script
// --------------------------------------------------------------------------
// Piaras - Innkeeper
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

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

		Shop.AddTabs("Book", "Gift");

        //----------------
        // Book
        //----------------

        //Page 1
        Shop.AddItem("Book", 1055);		//The Road to Becoming a Magic Warrior
        Shop.AddItem("Book", 1056);		//How to Enjoy Field Hunting
        Shop.AddItem("Book", 1124);		//An Easy Guide to Taking Up Residence in a Home
        Shop.AddItem("Book", 1037);		//Experiencing the Miracle of Resurrection with 100 Gold
        Shop.AddItem("Book", 1041);		//A Story About Eggs
        Shop.AddItem("Book", 1038);		//Nora Talks about the Tailoring Skill
        Shop.AddItem("Book", 1039);		//Easy Part-Time Jobs
        Shop.AddItem("Book", 1062);		//The Greedy Snow Imp
        Shop.AddItem("Book", 1048);		//My Fluffy Life with Wool
        Shop.AddItem("Book", 1049);		//The Holy Water of Lymilark
        Shop.AddItem("Book", 1057);		//Introduction to Field Bosses
        Shop.AddItem("Book", 1054);		//Behold the Dungeon - Advice for Young Generations
        Shop.AddItem("Book", 1058);		//Understanding Wisps
        Shop.AddItem("Book", 1015);		//Seal Stone Research Almanac : Rabbie Dungeon
        Shop.AddItem("Book", 1016);		//Seal Stone Research Almanac : Ciar Dungeon
        Shop.AddItem("Book", 1017);		//Seal Stone Research Almanac : Dugald Aisle
        Shop.AddItem("Book", 1505);		//The World of Handicrafts

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52011);		//Socks
        Shop.AddItem("Gift", 52018);		//Hammer
        Shop.AddItem("Gift", 52008);		//Anthology
        Shop.AddItem("Gift", 52009);		//Cubic Puzzle
        Shop.AddItem("Gift", 52017);		//Underwear Set

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

		MsgSelect(c, "Welcome to my Inn.", Button("Start Conversation", "@talk"), Button("Shop", "@shop"));
		
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
