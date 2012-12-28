using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PartnerScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_partnerdummy");
		SetRace(10000);
		SetLocation(region: 22, x: 6118, y: 5519);

		Shop.AddTabs("General Goods", "First Aid Kits", "Decorative Items", "Event");
		Shop.AddItem("General Goods", 40203);      // Taming Cane
		Shop.AddItem("General Goods", 60059);      // Taming Bait
		Shop.AddItem("General Goods", 60059, 50); 
		Shop.AddItem("General Goods", 60059, 100);
		Shop.AddItem("General Goods", 40097);      // L-Rod
		Shop.AddItem("General Goods", 61501);      // Sketch Paper
		Shop.AddItem("General Goods", 60046);      // Finest Silk Weaving Gloves
		Shop.AddItem("General Goods", 60055);      // Fine Silk Weaving Gloves
		Shop.AddItem("General Goods", 60056);      // Finest Fabric Weaving Gloves
		Shop.AddItem("General Goods", 60057);      // Fine Fabric Weaving Gloves
		Shop.AddItem("General Goods", 40042);      // Cooking Knife
		Shop.AddItem("General Goods", 40043);      // Rolling Pin
		Shop.AddItem("General Goods", 40044);      // Ladle
		Shop.AddItem("General Goods", 46004);      // Cooking Pot
		Shop.AddItem("General Goods", 46005);      // Cooking Table
		Shop.AddItem("General Goods", 18198);      // Monocle
		Shop.AddItem("First Aid Kits", 63000, 10); // Phoenix Feather
		Shop.AddItem("First Aid Kits", 63000, 20); // Phoenix Feather
		Shop.AddItem("First Aid Kits", 60005, 10); // Bandage
		Shop.AddItem("First Aid Kits", 60005, 20); // Bandage
		Shop.AddItem("Decorative Items", 15115);   // Jagged Mini Skirt
		Shop.AddItem("Decorative Items", 15116);   // Wizard Suit for Women
		Shop.AddItem("Decorative Items", 15117);   // Wizard Suit for Men
		Shop.AddItem("Decorative Items", 15119);   // Long Swordsmanship School Uniform (F)
		Shop.AddItem("Decorative Items", 15139);   // Xiao-Lung Juen's Formal Suit (F)
		Shop.AddItem("Decorative Items", 15140);   // Xiao-Lung Juen's Formal Suit (M)
		Shop.AddItem("Decorative Items", 15157);   // Wis' Intelligence Soldier Uniform (M)
		Shop.AddItem("Decorative Items", 15158);   // Wis' Intelligence Soldier Uniform (F)
	}

	public override void OnTalk(WorldClient c)
	{
		OnSelect(c, null);
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case null:
				Msg(c, "Hiya, boss! You don't mind if I call you boss, right?");
				Msg(c, "I'm your new maid. Just don't ask me to work too hard,", "and we'll have a great time, I bet!", "In fact, I can already tell we'll be great friends!");
				Disable(c, Options.FaceAndName);
				Msg(c, "(At least she's friendly...)");
				Enable(c, Options.FaceAndName);
				goto case "#task";
				
			case "#task":
				Disable(c, Options.FaceAndName);
				MsgSelect(c, "(Choose a task.)",
							 "Grant Favor", "@favor",
							 "Gift", "@gift",
							 "Order a Dish", "@cook",
							 "Trade", "@trade",
							 "Repair", "@repair",
							 "Action", "@action",
							 "End Conversation", "@end");
				break;
			
			case "@trade":
				Msg(c, "You need something, boss?", "I don't have much, but take a look.");
				OpenShop(c);
				break;
			
			default:
				Msg(c, "Yea~ no. Ask again later.");
				goto case "#task";
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation.)");
	}
}
