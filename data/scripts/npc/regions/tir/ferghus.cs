// Aura Script
// --------------------------------------------------------------------------
// Ferghus - Blacksmith
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class FerghusScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ferghus");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.4f, lower: 1.1f);
		SetFace(skin: 23, eye: 3, eyeColor: 112, lip: 4);
		SetStand("human/male/anim/male_natural_stand_npc_Ferghus_retake", "human/male/anim/male_natural_stand_npc_Ferghus_talk");
		SetLocation("tir", 18075, 29960, 80);

		EquipItem(Pocket.Face, 0x1356, 0xF79435);
		EquipItem(Pocket.Hair, 0x1039, 0x2E303F);
		EquipItem(Pocket.Armor, 0x3D22, 0x1F2340, 0x988486, 0x9E9FAC);
		EquipItem(Pocket.Shoe, 0x4383, 0x77564A, 0xF2A03A, 0x8A243D);
		EquipItem(Pocket.LeftHand1, 0x9C58, 0x808080, 0x212121, 0x808080);

		Phrases.Add("(Spits out a loogie)");
		Phrases.Add("Beard! Oh, beard! A true man never forgets how to grow a beard, yeah!");
		Phrases.Add("How come they are so late? I've been expecting armor customers for hours now.");
		Phrases.Add("Hrrrm");
		Phrases.Add("I am running out of Iron Ore. I guess I should wait for more.");
		Phrases.Add("I feel like working while singing songs.");
		Phrases.Add("I probably did too much hammering yesterday. Now my arm is sore.");
		Phrases.Add("I really need a pair of bellows... The sooner the better.");
		Phrases.Add("Ouch, I yawned too big. I nearly ripped my mouth open!");
		Phrases.Add("Scratching");
		Phrases.Add("What am I going to make today?");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"His bronze complexion shines with the glow of vitality. His distinctive facial outline ends with a strong jaw line covered with dark beard.", 
			"The first impression clearly shows he is a seasoned blacksmith with years of experience.",
			"The wide-shouldered man keeps humming with a deep voice while his muscular torso swings gently to the rhythm of the tune."
		);
		MsgSelect(c, "Welcome to my Blacksmith's Shop", "Start Conversation", "@talk", "Shop", "@shop", "Repair Item", "@repair", "Upgrade Item", "@upgrade");
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Are you new here? Good to see you.");
			
			L_Keywords:
				Msg(c, Options.Name, "(Ferghus is looking in my direction.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "*Yawn* I don't know.");
				goto L_Keywords;
			}

			case "@shop":
			{
				Msg(c, "Looking for a weapon?<br/>Or armor?");
				OpenShop(c);
				End();
			}

			case "@repair":
			{
				MsgSelect(c,
					"If you want to have armor, kits of weapons repaired, you've come to the right place.<br/>I sometimes make mistakes, but I offer the best deal for repair work.<br/>For rare and expensive items, I think you should go to a big city. I can't guarantee anything.",
					"End Conversation", "@endrepair"
				);
				
				r = Wait();
				
				Msg(c, "By the way, do you know you can bless your items with the Holy Water of Lymilark?<br/>I don't know why, but I make fewer mistakes<br/>while repairing blessed items. Haha.");
				Msg(c, "Well, come again when you have items to fix.");
				End();
			}

			case "@upgrade":
			{
				MsgSelect(c,
					"Will you select items to be modified?<br/>The number and types of modifications are different depending on the items.<br/>When I modify them, my hands never slip or make mistakes. So don't worry, trust me.",
					"End Conversation", "@endupgrade"
				);
				
				r = Wait();
				
				Msg(c, "If you have something to modify, let me know anytime.");
				End();
			}
		}
	}
}
