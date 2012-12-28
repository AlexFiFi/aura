using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_AScript : Scathach_alchemist_baseScript
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
