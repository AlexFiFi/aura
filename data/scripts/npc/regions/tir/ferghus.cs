// Aura Script
// --------------------------------------------------------------------------
// Ferghus - Blacksmith
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

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

		Shop.AddTabs("Weapon", "Shoes Gloves", "Helmet", "Armor", "Event");

        //----------------
        // Weapon
        //----------------

        //Page 1
        Shop.AddItem("Weapon", 40001);		//Wooden Stick
        Shop.AddItem("Weapon", 40023);		//Gathering Knife
        Shop.AddItem("Weapon", 45001, 20);	//Arrow x20
        Shop.AddItem("Weapon", 45001, 100);	//Arrow x100
        Shop.AddItem("Weapon", 40022);		//Gathering Axe
        Shop.AddItem("Weapon", 45002, 50);	//Bolt x50
        Shop.AddItem("Weapon", 45002, 200);	//Bolt x200
        Shop.AddItem("Weapon", 40027);		//Weeding Hoe
        Shop.AddItem("Weapon", 40003);		//Short Bow
        Shop.AddItem("Weapon", 40002);		//Wooden Blade
        Shop.AddItem("Weapon", 40020);		//Wooden Club
        Shop.AddItem("Weapon", 40006);		//Dagger
        Shop.AddItem("Weapon", 40026);		//Sickle
        Shop.AddItem("Weapon", 40025);		//Pickaxe
        Shop.AddItem("Weapon", 40005);		//Short Sword
        Shop.AddItem("Weapon", 40007);		//Hatchet
        Shop.AddItem("Weapon", 40179);		//Spiked Knuckle
        Shop.AddItem("Weapon", 40024);		//Blacksmith Hammer
        Shop.AddItem("Weapon", 40244);		//Bear Knuckle
        Shop.AddItem("Weapon", 40180);		//Hobnail Knuckle
        Shop.AddItem("Weapon", 40745);		//Basic Control Bar
        Shop.AddItem("Weapon", 46001);		//Round Shield

        //----------------
        // Shoes Gloves
        //----------------

        //Page 1
        Shop.AddItem("Shoes Gloves", 16004);	//Studded Bracelet
        Shop.AddItem("Shoes Gloves", 16008);	//Cores' Thief Gloves
        Shop.AddItem("Shoes Gloves", 16000);	//Leather Gloves
        Shop.AddItem("Shoes Gloves", 17021);	//Lorica Sandles
        Shop.AddItem("Shoes Gloves", 17014);	//Leather Shoes
        Shop.AddItem("Shoes Gloves", 17001);	//Ladies Leather Boots
        Shop.AddItem("Shoes Gloves", 17005);	//Hunter Boots
        Shop.AddItem("Shoes Gloves", 17015);	//Combat Shoes
        Shop.AddItem("Shoes Gloves", 17016);	//Field Combat Shoes
        Shop.AddItem("Shoes Gloves", 17020);	//Thief Shoes
        Shop.AddItem("Shoes Gloves", 16014);	//Lorica Gloves

        //----------------
        // Helmet
        //----------------

        //Page 1
        Shop.AddItem("Helmet", 18503);	//Cuirassier Helm

        //----------------
        // Armor
        //----------------

        //Page 1
        Shop.AddItem("Armor", 14001);	//Light Leather Mail (F)
        Shop.AddItem("Armor", 14010);	//Light Leather Mail (M)
        Shop.AddItem("Armor", 14004);	//Cloth Mail
        Shop.AddItem("Armor", 14008);	//Full Leather Armor Set (F)
        Shop.AddItem("Armor", 14003);	//Studded Cuirassier

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

		MsgSelect(c, "Welcome to my Blacksmith's Shop", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Repair Item", "@repair"), Button("Upgrade Item", "@upgrade"));
		
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
					Button("End Conversation", "@endrepair")
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
					Button("End Conversation", "@endupgrade")
				);
				
				r = Wait();
				
				Msg(c, "If you have something to modify, let me know anytime.");
				End();
			}
		}
	}
}
