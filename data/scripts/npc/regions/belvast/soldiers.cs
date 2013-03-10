// Aura Script
// --------------------------------------------------------------------------
// Soldiers
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _SoldierBelfastBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetColor(0x808080, 0x808080, 0x808080);

		Phrases.Add("If a problem arises, please let me know right away.");
		Phrases.Add("No problems here!");
		Phrases.Add("Please be careful to not lose your belongings.");
		Phrases.Add("The pirates are no more!");
		Phrases.Add("This is Belvast Island, governed by Admiral Owen himself.");
		Phrases.Add("Watch yourself. The monsters at Scathach Beach are no slouches.");
	}
}

public class _SoldierBelfastBase1Script : _SoldierBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetRace(10002);
		SetFace(skin: 19, eye: 31, eyeColor: 134, lip: 2);

		EquipItem(Pocket.Face, 0x1324, 0xB6F9D, 0xA60F46, 0x9DDB);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x36BF, 0x15151A, 0x6F605E, 0xD2D46);
		EquipItem(Pocket.Glove, 0x3EC0, 0x1C2024, 0x424B, 0x399FF2);
		EquipItem(Pocket.Shoe, 0x4337, 0x2B2C35, 0x62404D, 0x66685A);
		EquipItem(Pocket.Head, 0x485D, 0x524F5D, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand1, 0x9D32, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.RightHand2, 0x9C91, 0x546C74, 0x274376, 0x2537C6);
		EquipItem(Pocket.LeftHand1, 0xB3B3, 0x464646, 0x37727F, 0x4E1EF4);
		EquipItem(Pocket.LeftHand2, 0xAFC9, 0x546C74, 0x274376, 0x2537C6);
	}
}

public class _SoldierBelfastBase2Script : _SoldierBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
        SetRace(10001);
		SetBody(height: 0.85f, fat: 0.95f, upper: 1.1f, lower: 0.9f);
		SetFace(skin: 19, eye: 6, eyeColor: 76, lip: 1);

		EquipItem(Pocket.Face, 0xF3C, 0xFDDE5B, 0x389FD8, 0x500000);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x36C4, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Glove, 0x3EC0, 0x828182, 0x6F605E, 0x399FF2);
		EquipItem(Pocket.Shoe, 0x4337, 0x828182, 0x6F605E, 0x66685A);
		EquipItem(Pocket.Head, 0x485D, 0x828182, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand1, 0x9D32, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.RightHand2, 0x9C91, 0x546C74, 0x274376, 0x2537C6);
		EquipItem(Pocket.LeftHand1, 0xB3B3, 0x464646, 0x37727F, 0x4E1EF4);
	}
}

public class SoldierBelfast1Script : _SoldierBelfastBase1Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_soldier_belfast_01");
		SetLocation(4005, 23484, 48333, 224);
	}
}

public class SoldierBelfast2Script : _SoldierBelfastBase1Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_soldier_belfast_02");
		SetLocation(4005, 24072, 48696, 216);
	}
}

public class SoldierBelfast3Script : _SoldierBelfastBase1Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_soldier_belfast_03");
		SetLocation(4005, 26983, 28858, 193);
	}
}

public class SoldierBelfast4Script : _SoldierBelfastBase1Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_soldier_belfast_04");
		SetLocation(4005, 26983, 30470, 63);
	}
}

public class SoldierBelfast5Script : _SoldierBelfastBase2Script
{
	public override void OnLoad()
	{
		base.OnLoad();
        
		SetName("_soldier_belfast_05");
		SetLocation(4005, 27304, 27369);
	}
}

public class SoldierBelfast6Script : _SoldierBelfastBase2Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_soldier_belfast_06");
		SetLocation(4005, 30444, 27551, 127);
	}
}
