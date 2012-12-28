using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_AScript : Wardens_baseScript
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
