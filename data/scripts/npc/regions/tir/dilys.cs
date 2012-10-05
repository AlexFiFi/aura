using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class DilysScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_dilys");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 27, lip: 48);

		EquipItem(Pocket.Face, 0xF44, 0xFFE170);
		EquipItem(Pocket.Hair, 0xC45, 0x633C31);
		EquipItem(Pocket.Armor, 0x3D25, 0xFFFFFF, 0x61854B, 0xFFFFFF);
		EquipItem(Pocket.Glove, 0x3EE2, 0x61854B, 0x0, 0x0);
		EquipItem(Pocket.Shoe, 0x4385, 0xE8E8E8, 0x0, 0x0);

		SetLocation(region: 6, x: 1107, y: 1050);

		SetDirection(195);
		SetStand("human/female/anim/female_natural_stand_npc_Dilys_retake", "human/female/anim/female_natural_stand_npc_Dilys_talk");

		Shop.AddTabs("Potions", "First Aid Kits", "Etc.");

		Phrases.Add("I wish I could see the stars.");
		Phrases.Add("It's such a hassle to get all those ingredients for just one meal.");
		Phrases.Add("Men are all the same.");
		Phrases.Add("Perhaps I should order a safe this month.");
		Phrases.Add("Should I go to the market?");
		Phrases.Add("What should I cook for dinner tonight?");
	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "A tall, slim lady tinkers with various ointments, herbs, and bandages.",
			"She looks wise beyond her years, maybe because of the green healer dress she's wearing.",
			"Her dark hair is neatly combed, and her gentle brown eyes puts everyone who speaks to her at ease.",
			"She smiles faintly, waiting for you to speak.");
		MsgSelect(c, "Welcome to the Healer's House", "Start Conversation", "@talk", "Shop", "@shop", "Get Treatment", "@heal", "Heal Pet", "@healpet");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@heal":
				if (c.Character.Life == c.Character.LifeMax)
				{
					Msg(c, "You don't have a mark on you! You really shouldn't pretend to be sick... There are people out there that need my help!");
				}
				else
				{
					Msg(c, "Goodness, " + c.Character.Name + "! Are you hurt? I must treat your wounds immediately.<br/>I can't understand why everyone gets injured so much around here...<br/>The fee is 90 Gold but don't think about money right now. What's important is that you get treated.",
						"Recieve Treatment", "@recieveheal", "Decline", "@end");
				}
				break;

			case "@healpet":
				if (c.Character.Vehicle == null)
				{
					Msg(c, "You may want to summon your animal friend first.",
						"If you don't have a pet, then please don't waste my time.");
				}
				else if (c.Character.Vehicle.IsDead())
				{
					Msg(c, "Uh oh, you'll need to revive the pet first.",
						"Use a Phoenix Feather to revive your pet.");
				}
				else if (c.Character.Vehicle.Life == c.Character.Vehicle.LifeMax)
				{
					Msg(c, "Your pet's as healthy as you are!",
						"I don't appreciate pranks.");
				}
				else
				{
					MsgSelect(c, "Oh no! " + c.Character.Name + ", your animal friend is badly hurt and needs to be treated right away.<br/>I don't know why so many animals are getting injured lately. It makes me worry.<br/>The treatment will cost 180 Gold, but don't think of the price. Your pet needs help immediatly",
						"Recieve Treatment", "@recievepet", "Decline the Treatment", "@end");
				}
				break;

			case "@recieveheal":
				if (c.Character.HasGold(90))
				{
					c.Character.RemoveGold(90);
					c.Character.Injuries = 0;
					c.Character.Life = c.Character.Vehicle.LifeMax;
					Msg(c, "Good, I've put on some bandages and your treatment is done.",
						"If you get injured again, don't hesitate to visit me.");
				}
				else
				{
					Msg(c, "Oh no, I don't think you have enough gold.",
						"Hmmm... The gold covers the cost of the bandages and medicine...",
						"HOw about doing a part-time job?");
				}
				break;

			case "@recievepet":
				if (c.Character.HasGold(180))
				{
					c.Character.RemoveGold(180);
					c.Character.Vehicle.Injuries = 0;
					c.Character.Vehicle.Life = c.Character.Vehicle.LifeMax;
					Msg(c, "Okay, all bandaged up and ready to go!",
						"If your dear animal friend gets hurt again, do not hesitate to visit me.");
				}
				else
				{
					Msg(c, "Oh no, I don't think you have enough gold.",
						"Hmmm... The gold covers the cost of the bandages and medicine...",
						"How about doing a part-time job?");
				}
				break;

			case "@shop":
				Msg(c, "What potion do you need?");
				OpenShop(c);
				break;

			case "@talk":
				Msg(c, "Welcome!");
				Msg(c, true, false, "(Dilys is waiting for me to say something.)");
				ShowKeywords(c);
				break;
				
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
