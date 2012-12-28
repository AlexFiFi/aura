using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Worker_belfast_03Script : Worker_belfast_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_worker_belfast_03");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 19, eye: 26, eyeColor: 196, lip: 1);

		EquipItem(Pocket.Face, 0x1324, 0xF09D4C, 0x7E6936, 0x7B3441);
		EquipItem(Pocket.Hair, 0x1775, 0xCC8D46, 0xCC8D46, 0xCC8D46);
		EquipItem(Pocket.Armor, 0x3BFC, 0x8194, 0xBAC2AE, 0x422FB7);
		EquipItem(Pocket.Shoe, 0x4493, 0x94546F, 0x4D6A8E, 0xA6F4FF);

		SetLocation(region: 4005, x: 54285, y: 22677);

		SetDirection(147);
		SetStand("human/anim/male_natural_sit_02");
	}
}
