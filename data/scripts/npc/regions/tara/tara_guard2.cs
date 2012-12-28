using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_guard2Script : Tara_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard2");
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x295473, 0xFFF25B, 0x71AE3C);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);

		SetLocation(region: 401, x: 100337, y: 101515);

		SetDirection(191);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
