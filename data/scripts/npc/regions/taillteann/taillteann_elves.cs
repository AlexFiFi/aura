using System;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _TaillteannElfBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(9002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		
		EquipItem(Pocket.Armor, 0x3BEE, 0x647692, 0x78829D, 0xBFBFBF);
		EquipItem(Pocket.RightHand1, 0x9D20, 0xD8B77E, 0x0, 0x0);
		EquipItem(Pocket.RightHand2, 0x9D33, 0xFFFFFF, 0x5D4519, 0x5D5537);

        Phrases.Add("Does Granat never grow tired?");
		Phrases.Add("I wonder if I'll be permitted to go home this holiday.");
		Phrases.Add("I'm quite hungry.");
		Phrases.Add("It's cooler here than in Filia.");
	}
}

public class TaillteannElf1Script : _TaillteannElfBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_elf1");
		SetFace(skin: 19, eye: 38, eyeColor: 30, lip: 0);
		SetLocation(300, 227359, 192931, 150);

		EquipItem(Pocket.Face, 0x1AF4, 0x628BA4, 0x7B4F3A, 0x64B27A);
		EquipItem(Pocket.Hair, 0x1777, 0x7B8AAD, 0x7B8AAD, 0x7B8AAD);
	}
}

public class TaillteannElf2Script : _TaillteannElfBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_elf2");
		SetFace(skin: 18, eye: 43, eyeColor: 31, lip: 0);
		SetLocation(300, 226962, 193788, 150);

		EquipItem(Pocket.Face, 0x1AF4, 0x55146B, 0x7B344A, 0x8D0058);
		EquipItem(Pocket.Hair, 0x1775, 0xFFC7C6, 0xFFC7C6, 0xFFC7C6);
	}
}
