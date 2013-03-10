using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SionScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_sion");
		SetRace(10002);
		SetBody(height: 0.1000001f, fat: 1f, upper: 1.3f, lower: 1.3f);
		SetFace(skin: 17, eye: 2, eyeColor: 27, lip: 3);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xF58A78, 0xCDD0EA, 0xB5E3E5);
		EquipItem(Pocket.Hair, 0xFA8, 0x2E4830, 0x2E4830, 0x2E4830);
		EquipItem(Pocket.Armor, 0x3AC4, 0x54697A, 0xCAD98C, 0x1F2E26);
		EquipItem(Pocket.Glove, 0x3E80, 0xAFA992, 0xB60659, 0xE1D5E9);
		EquipItem(Pocket.Shoe, 0x4274, 0x676149, 0x71485B, 0xF78A3D);
		EquipItem(Pocket.Head, 0x4668, 0x808000, 0xFFFFFF, 0xAA89C0);
		EquipItem(Pocket.RightHand1, 0x9C59, 0xC0C6BB, 0x8E6D59, 0xC7B0D5);

		SetLocation(region: 31, x: 12093, y: 15062);

		SetDirection(184);
		SetStand("human/anim/tool/Rhand_A/female_tool_Rhand_A02_stand_friendly");
        
		Phrases.Add("Dad should be coming any minute now...");
		Phrases.Add("I want to grow up quickly and be an adult soon.");
		Phrases.Add("I wonder what's for dinner. *Gulp*");
		Phrases.Add("Ibbie... I miss you...");
		Phrases.Add("If you want to activate the switch by the Watermill, let me know!");
		Phrases.Add("If you want to make an ingot, talk to me first!");
		Phrases.Add("If you want to refine ore, you have to come talk to me!");
		Phrases.Add("The Watermill never gets boring...");
		Phrases.Add("The way Gilmore talks is too hard to understand.");
		Phrases.Add("To fire up the furnace, come talk to me!");
		Phrases.Add("Why does Bryce not like me?");
		Phrases.Add("You have to pay. You have to pay to activate the switch!");
	}
}
