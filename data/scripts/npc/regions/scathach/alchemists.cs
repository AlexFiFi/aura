using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _ScathachAlchemistBaseScript : NPCScript
{
	public override void OnLoad()
	{
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

		Phrases.Add("A bit chilly today, isn't it?");
		Phrases.Add("Ah, I'm so hungry.");
		Phrases.Add("Be careful, now.");
		Phrases.Add("Believe me, I'm not here by choice.");
		Phrases.Add("Captain Odran! Yoo-hoo!");
		Phrases.Add("Captain, over here! I'm over here!");
		Phrases.Add("Captain. Captain! CAPTAIN!!");
		Phrases.Add("Dullards everywhere!");
		Phrases.Add("Have you eaten?");
		Phrases.Add("Is it because I'm a Royal Alchemist? Is that why you're ignoring me?");
		Phrases.Add("Is that a bug...? Disgusting!");
		Phrases.Add("My throat hurts.");
		Phrases.Add("Savages.");
		Phrases.Add("Such filth!");
		Phrases.Add("That's cute.");
		Phrases.Add("The patrolmen are so...uncouth.");
		Phrases.Add("Welcome.");
		Phrases.Add("What shall I study today?");
		Phrases.Add("Why must I stay in this terrible place?");
		Phrases.Add("Why must I suffer so?");
		Phrases.Add("You can't even get decent tea out here!");
		Phrases.Add("You can't ignore me forever, captain!");
    }
}

public class Alchemist_AScript : _ScathachAlchemistBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_alchemist_A");
		SetRace(10001);
		SetFace(skin: 17, eye: 140, eyeColor: 168, lip: 53);

		EquipItem(Pocket.Face, 0xF3C, 0x686D51, 0x79B38E, 0x2E3896);
		EquipItem(Pocket.Hair, 0xC37, 0xFCD685, 0xFCD685, 0xFCD685);

		SetLocation(region: 4014, x: 33322, y: 42078);

		SetDirection(118);
	}
}

public class Alchemist_CScript : _ScathachAlchemistBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_alchemist_C");
		SetRace(10002);
		SetFace(skin: 23, eye: 30, eyeColor: 8, lip: 24);

		EquipItem(Pocket.Face, 0x1324, 0x3CA040, 0x55C190, 0x1F63AE);
		EquipItem(Pocket.Hair, 0x1007, 0x414141, 0x414141, 0x414141);

		SetLocation(region: 4014, x: 33951, y: 43279);

		SetDirection(209);
	}
}

public class Alchemist_DScript : _ScathachAlchemistBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_alchemist_D");
		SetRace(9001);
		SetFace(skin: 19, eye: 41, eyeColor: 192, lip: 1);

		EquipItem(Pocket.Face, 0x1713, 0x3D3B, 0x725F48, 0x88488);
		EquipItem(Pocket.Hair, 0x138D, 0xFF9FA4, 0xFF9FA4, 0xFF9FA4);

		SetLocation(region: 4014, x: 33049, y: 42353);

		SetDirection(126);
	}
}
