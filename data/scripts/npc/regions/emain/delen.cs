using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DelenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_delen");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 113, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x506864, 0xF99CE, 0x2F0045);
		EquipItem(Pocket.Hair, 0xBD7, 0xFAA974, 0xFAA974, 0xFAA974);
		EquipItem(Pocket.Armor, 0x3AEA, 0xFA8D8D, 0xF7B4AC, 0x202020);
		EquipItem(Pocket.Shoe, 0x4290, 0x5E2922, 0x41002C, 0x683D0D);

		SetLocation(region: 52, x: 41198, y: 38978);

		SetDirection(94);
		SetStand("human/female/anim/female_natural_stand_npc_01");
        
		Phrases.Add("Am I doing something wrong...?");
		Phrases.Add("Flowers are a must if you're wooing a lady!");
		Phrases.Add("Flowers!");
		Phrases.Add("Fresh flowers!");
		Phrases.Add("Hi there! Check out these flowers!");
		Phrases.Add("Look at these flowers! They are gorgeous!");
		Phrases.Add("Need flowers?");
		Phrases.Add("Pretty flowers! Pick all you want!");
		Phrases.Add("Surprise your beloved with these beautiful flowers.");
		Phrases.Add("These flowers smell so good!");
		Phrases.Add("What kind of flowers are you looking for?");
	}
}
