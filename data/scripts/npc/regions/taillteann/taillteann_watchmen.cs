using System;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _TaillteannWatchmanBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetColor(0x808080, 0x808080, 0x808080);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
		
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);
	}
}

public class Taillteann_watchman_3Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human3");
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);
		SetLocation(300, 236811, 193025, 0);

		EquipItem(Pocket.Face, 0x1324, 0x769476, 0x770008, 0xD6D6EB);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);
	}
}

public class Taillteann_watchman_4Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human4");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);
		SetLocation(300, 236811, 191827, 0);

		EquipItem(Pocket.Face, 0x1324, 0x296D6E, 0x984B90, 0x70505E);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
	}
}

public class Taillteann_watchman_5Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human5");
		SetFace(skin: 20, eye: 12, eyeColor: 32, lip: 0);
		SetLocation(300, 220698, 178629, 197);

		EquipItem(Pocket.Face, 0x1324, 0x3BA945, 0xCC0066, 0xAB62);
		EquipItem(Pocket.Hair, 0xFCA, 0x211C39, 0x211C39, 0x211C39);
	}
}

public class Taillteann_watchman_6Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human6");
		SetFace(skin: 20, eye: 32, eyeColor: 162, lip: 0);
		SetLocation(300, 219542, 178625, 194);

		EquipItem(Pocket.Face, 0x1324, 0xE0005F, 0xDBC75E, 0x5180);
		EquipItem(Pocket.Hair, 0x135C, 0x663333, 0x663333, 0x663333);
	}
}

public class Taillteann_watchman_7Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human7");
		SetFace(skin: 20, eye: 12, eyeColor: 32, lip: 0);
		SetLocation(300, 202174, 198517, 126);

		EquipItem(Pocket.Face, 0x1324, 0x496375, 0xB5A57F, 0xF21D6E);
		EquipItem(Pocket.Hair, 0xFCA, 0x211C39, 0x211C39, 0x211C39);
	}
}

public class Taillteann_watchman_8Script : _TaillteannWatchmanBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human8");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);
		SetLocation(300, 202173, 199698, 126);

		EquipItem(Pocket.Face, 0x1324, 0xF46369, 0x6E4D9E, 0xFBBC58);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
	}
}
