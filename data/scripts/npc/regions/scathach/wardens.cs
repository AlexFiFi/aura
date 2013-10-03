using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _Wardens_baseScript : NPCScript
{
	public override void OnLoad()
	{
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 30824, y: 42563);

		SetDirection(69);

		Phrases.Add("Be on guard.");
		Phrases.Add("Can't...let guard down...");
		Phrases.Add("Cows have it easy.");
		Phrases.Add("Hey! Watch the sudden movements!");
		Phrases.Add("Hm.");
		Phrases.Add("Ho-hum.");
		Phrases.Add("How long has it been?");
		Phrases.Add("Huh? Need a hand?");
		Phrases.Add("Huh? What?");
		Phrases.Add("I could really go for a steak.");
		Phrases.Add("I have a bad feeling about this.");
		Phrases.Add("I should've been born a cow.");
		Phrases.Add("I wonder how they're doing...");
		Phrases.Add("I'm on to you.");
		Phrases.Add("Isn't the captain the best?");
		Phrases.Add("No sudden movements...");
		Phrases.Add("Shh!");
		Phrases.Add("Sigh...");
		Phrases.Add("So much noise! Jeez!");
		Phrases.Add("So...tired...");
		Phrases.Add("Stupid alchemists...");
		Phrases.Add("This is a dangerous place.");
		Phrases.Add("Those beasts at the main gate are such a headache.");
		Phrases.Add("What're you staring at?");
		Phrases.Add("Who goes there? You aren't one of ours, are you?");
		Phrases.Add("Whoa! What's that?");
		Phrases.Add("Yawn...");
		Phrases.Add("You! You're hiding something, aren't you?");
	}
}

public class Wardens_AScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		SetName("_wardens_A");
		SetRace(8002);
		SetFace(skin: 22, eye: 150, eyeColor: 28, lip: 26);

		EquipItem(Pocket.Face, 0x22C4, 0xFEE75F, 0x21003C, 0x2AB1A1);
		EquipItem(Pocket.Hair, 0x1F83, 0x6F3C3B, 0x6F3C3B, 0x6F3C3B);

		SetLocation(region: 4014, x: 30824, y: 42563);

		SetDirection(69);
		SetStand("giant/anim/giant_sit_01");
	}
}

public class Wardens_BScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_B");
		SetRace(10002);
		SetFace(skin: 22, eye: 23, eyeColor: 29, lip: 21);

		EquipItem(Pocket.Face, 0x1324, 0xB5C2, 0x5E000A, 0xFFEBA3);
		EquipItem(Pocket.Hair, 0x100E, 0x367071, 0x367071, 0x367071);

		SetLocation(region: 4014, x: 33891, y: 44839);

		SetDirection(191);
		SetStand("elf/male/anim/elf_npc_granites_stand_friendly");
	}
}

public class Wardens_CScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_C");
		SetRace(8001);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		EquipItem(Pocket.Face, 0x1EDC, 0x5AC56, 0x562488, 0xF79B62);
		EquipItem(Pocket.Hair, 0x1B64, 0xD01D09, 0xD01D09, 0xD01D09);

		SetLocation(region: 4014, x: 33561, y: 42889);

		SetDirection(101);
		SetStand("chapter3/giant/female/anim/giant_female_c3_npc_karpfen");
	}
}

public class Wardens_DScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_D");
		SetRace(10001);
		SetFace(skin: 22, eye: 5, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0x366969, 0xFFA627, 0x730D6C);
		EquipItem(Pocket.Hair, 0xC22, 0x414141, 0x414141, 0x414141);

		SetLocation(region: 4014, x: 33350, y: 44926);

		SetDirection(196);
		SetStand("");
	}
}

public class Hidden_wardens_AScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_A");
		SetRace(8002);
		SetFace(skin: 22, eye: 150, eyeColor: 28, lip: 26);

		EquipItem(Pocket.Face, 0x22C4, 0x576869, 0x9355, 0x18B2D1);
		EquipItem(Pocket.Hair, 0x1F46, 0x1F3C1B, 0x1F3C1B, 0x1F3C1B);

		SetLocation(region: 4014, x: 61500, y: 41200);

		SetDirection(17);
		SetStand("");
	}
}

public class Hidden_wardens_BScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_B");
		SetRace(10002);
		SetFace(skin: 15, eye: 32, eyeColor: 3, lip: 2);

		EquipItem(Pocket.Face, 0x1324, 0x7ECEC7, 0x1CB1B8, 0x6089);
		EquipItem(Pocket.Hair, 0xFB1, 0x70403E, 0x70403E, 0x70403E);

		SetLocation(region: 4014, x: 75400, y: 38100);

		SetDirection(76);
		SetStand("");
	}
}

public class Hidden_wardens_CScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_C");
		SetRace(8001);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		EquipItem(Pocket.Face, 0x1EDC, 0x74238F, 0xD4A463, 0x39247A);
		EquipItem(Pocket.Hair, 0x1B5D, 0x401D69, 0x401D69, 0x401D69);

		SetLocation(region: 4014, x: 66900, y: 53800);

		SetDirection(253);
		SetStand("");
	}
}

public class Hidden_wardens_DScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_D");
		SetRace(10001);
		SetFace(skin: 22, eye: 9, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0xD4EEED, 0xFCD25D, 0x84002C);
		EquipItem(Pocket.Hair, 0xBE0, 0x864349, 0x864349, 0x864349);

		SetLocation(region: 4014, x: 74100, y: 59900);

		SetDirection(75);
		SetStand("");
	}
}

public class Hidden_wardens_EScript : _Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_E");
		SetRace(10001);
		SetFace(skin: 22, eye: 12, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0x87BA5C, 0xFA998E, 0x404863);
		EquipItem(Pocket.Hair, 0xBE6, 0x717141, 0x717141, 0x717141);

		SetLocation(region: 4014, x: 73500, y: 76600);

		SetDirection(233);
		SetStand("");
	}
}
