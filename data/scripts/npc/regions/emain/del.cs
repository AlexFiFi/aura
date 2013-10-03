using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_del");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 113, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x6D6E69, 0xB3B1, 0xEDA23E);
		EquipItem(Pocket.Hair, 0xBD7, 0xEC7766, 0xEC7766, 0xEC7766);
		EquipItem(Pocket.Armor, 0x3AEA, 0xFA8D8D, 0xF7B4AC, 0x202020);
		EquipItem(Pocket.Shoe, 0x4290, 0x5E2922, 0x41002C, 0x683D0D);

		SetLocation(region: 52, x: 40406, y: 38978);

		SetDirection(38);
		SetStand("human/female/anim/female_natural_stand_npc_01");
        
		Phrases.Add("Do you need flowers?");
		Phrases.Add("Flowers for beautiful ladies!");
		Phrases.Add("Flowers make a perfect gift for a romantic date.");
		Phrases.Add("Fresh flowers!");
		Phrases.Add("Hey! I got flowers.");
		Phrases.Add("Hi. Take a look at these gorgeous flowers!");
		Phrases.Add("How are you doing?");
		Phrases.Add("Look at these beautiful flowers. Choose one!");
		Phrases.Add("Smell these fresh flowers.");
		Phrases.Add("What kinds of flowers are you looking for?");
		Phrases.Add("What's wrong with today's customers...?");
	}
}
