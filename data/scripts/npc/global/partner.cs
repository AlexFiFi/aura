// Aura Script
// --------------------------------------------------------------------------
// Partner - NPC dialog for partner pets
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class PartnerScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_partnerdummy");
		SetRace(10000);
		SetLocation(22, 6118, 5519);

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

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c,
			"Hiya, boss! You don't mind if I call you boss, right?",
			"<p/>I'm your new maid. Just don't ask me to work too hard,",
			"and we'll have a great time, I bet!",
			"In fact, I can already tell we'll be great friends!"
		);
		Msg(c, Options.FaceAndName, "(At least she's friendly...)");
		
	L_Task:
		Disable(c, Options.FaceAndName);
		MsgSelect(c,
			"(Choose a task.)",
			Button("Grant Favor", "@favor"),
			Button("Gift", "@gift"),
			Button("Order a Dish", "@cook"),
			Button("Trade", "@trade"),
			Button("Repair", "@repair"),
			Button("Action", "@action"),
			Button("End Conversation", "@end")
		);
		
		var r = Wait();
		switch (r)
		{
			case "@trade":
			{
				Msg(c, "You need something, boss?<br/>I don't have much, but take a look.");
				OpenShop(c);
				End();
			}
			
			default:
			{
				Msg(c, "Yea~ no. Ask again later.");
				goto L_Task;
			}
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation.)");
	}
}
