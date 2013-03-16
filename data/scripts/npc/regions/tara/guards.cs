// Aura Script
// --------------------------------------------------------------------------
// Guards in Tara
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TaraGuardBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(10002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
        
		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Armor, 0x3BEC, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}

public class TaraGuard1Script : TaraGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard1");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);
		SetLocation(region: 401, x: 82342, y: 122341);
		SetDirection(144);

		EquipItem(Pocket.Face, 0x1324, 0xF4F7D8, 0x594A9F, 0xDEC7E2);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
	}
}

public class TaraGuard2Script : TaraGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard2");
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
		SetLocation(region: 401, x: 100337, y: 101515);
		SetDirection(191);

		EquipItem(Pocket.Face, 0x1324, 0x295473, 0xFFF25B, 0x71AE3C);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);
	}
}

public class TaraGuard3Script : TaraGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard3");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);
		SetLocation(region: 401, x: 101191, y: 101540);
		SetDirection(200);

		EquipItem(Pocket.Face, 0x1324, 0x496D72, 0x486770, 0xECA721);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
	}
}
