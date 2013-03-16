using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GlenisScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_glenis");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 0.3f, upper: 1.4f, lower: 1.2f);
		SetFace(skin: 15, eye: 7, eyeColor: 119, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3E, 0x84C8, 0x323C99, 0xF35D80);
		EquipItem(Pocket.Hair, 0xBCC, 0xBC756C, 0xBC756C, 0xBC756C);
		EquipItem(Pocket.Armor, 0x3AA2, 0x764E63, 0xCCD8ED, 0xE7957A);
		EquipItem(Pocket.Shoe, 0x4274, 0x764E63, 0xFC9C5F, 0xD2CCE5);

		SetLocation(region: 14, x: 37566, y: 41605);

		SetDirection(129);
		SetStand("human/female/anim/female_natural_stand_npc_Glenis");
        
		Phrases.Add("Come buy your food here.");
		Phrases.Add("Flora! Are the ingredients ready?");
		Phrases.Add("Have a nice day today!");
		Phrases.Add("Please come again!");
		Phrases.Add("Thank you for coming!");
		Phrases.Add("This is Glenis' Restaurant.");
		Phrases.Add("This is today's special! Mushroom soup!");
		Phrases.Add("We are serving breakfast now.");
		Phrases.Add("Welcome!");
	}
}
