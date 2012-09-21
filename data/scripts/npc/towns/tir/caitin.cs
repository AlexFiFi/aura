using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;

public class NewTest : NPCScript
{
	public override void OnLoad()
	{
		SetName("_caitin");
		SetRace(10001);
		SetBody(height: 1.3f);
		SetFace(skin: 16, eye: 2, eyeColor: 39, lip: 0);

		EquipItem(Pocket.Face, 0xF3C, 0x1AB67C, 0xF09D3B, 0x007244); //7C B6 1A  3B 9D F0  44 72 00 
		EquipItem(Pocket.Hair, 0xBBA, 0x683E33, 0x683E33, 0x683E33); //333E6800 333E6800 333E6800
		EquipItem(Pocket.Armor, "Popo's Skirt", 0x708B3D, 0xFBE39B, 0x6D685F); //3D8D70 9BE3FB 5F686D
		EquipItem(Pocket.Shoe, 0x426E, 0x2A2A2A);

		SetLocation(region: 5, x: 1831, y: 1801);

		SetDirection(64);
		SetStand("human/female/anim/female_natural_stand_npc_Caitin");

		Shop.AddTabs("Grocery", "Gift", "Quest", "Event");
		Shop.AddItem("Grocery", "Bread");
		Shop.AddItem("Grocery", "Slice of Cheese");
		Shop.AddItem("Grocery", "Sugar", 1);
		Shop.AddItem("Grocery", "Sugar", 10);
	}

	public override void OnTalk(WorldClient c)
	{
		MsgSelect(c, "What can I do for you?", "Start Conversation", "@talk", "Shop", "@shop");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@shop":
				Msg(c, "Welcome to the Grocery Store.", "There is a variety of fresh food and ingredients for you to choose from.");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Nice to meet you.");
				Msg(c, true, false, "(Caitin is looking in my direction.)");
				ShowKeywords(c);
				break;

			case "personal_info":
				Msg(c, "My grandmother named me.", "I work here at the Grocery Store, so I know one important thing.", "You have to eat to survive!", "Food helps you regain your Stamina.");
				Msg(c, "That doesn't mean you can eat just everything.", "You shouldn't have too much greasy food", "because you could gain a lot of weight.");
				Msg(c, "Huh? You have food with your but don't know how to eat it?", "Okay, open the Inventory and right-click on the food.", "Then, click \"Use\" to eat.", "If you have bread in your Inventory, and your Stamina is low,", "try eating it now.");
				Msg(c, true, false, "(That was a great conversation!)");
				ShowKeywords(c);
				break;

			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}

	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You ended your conversation with Caitin.)");
	}
}