// Aura Script
// --------------------------------------------------------------------------
// Endelyon - Priestess
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EndelyonScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_endelyon");
		SetRace(10001);
		SetBody(height: 1.06f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);
		SetStand("human/female/anim/female_natural_stand_npc_Endelyon");
		SetLocation("tir", 5975, 36842);

		EquipItem(Pocket.Face, 3900, 0x6AC6A8);
		EquipItem(Pocket.Hair, 3022, 0x5E423E);
		EquipItem(Pocket.Armor, 15009, 0x303133, 0xC6D8EA, 0xDBC741);
		EquipItem(Pocket.Shoe, 17015, 0x303133, 0xA0927D, 0x4F548D);
		
		Shop.AddTabs("Gifts");

		Phrases.Add("Hmm... Something doesn't feel right.");
		Phrases.Add("I really need some help here...");
		Phrases.Add("It's so hard to do this all by myself!");
		Phrases.Add("Why do people like such things?");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"An elegent young lady wears the simple black dress of a Lymilark priestess.",
			"Her face is set in a calm, demur expression, and her eyes exude warmth.",
			"A slight smile tugging at her lips hints at a strong will."
		);
		MsgSelect(c, "May I help you?", "Start Conversation", "@talk", "Shop", "@shop", "Modify", "@modify");
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "I don't think we've ever met. Nice to meet you.");
			
			L_Keywords:
				Msg(c, Options.Name, "(Endelyon is looking in my direction.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "It doesn't sound familiar to me. I mean...");
				goto L_Keywords;
			}
			
			case "@modify":
			{
				MsgSelect(c,
					"Are you asking me...to modify your item?<br/>Honestly, I am not sure if I can, but if you still want me to, I'll give it a try<br/>Please choose an item to modify.",
					"End Conversation", "@endmodify"
				);
				r = Wait();
				Msg(c, "Do you want me to stop...? Well, then... Next time...");
				End();
			}

			case "@shop":
			{
				Msg(c, "What are you looking for?");
				OpenShop(c);
				End();
			}
		}
	}
}
