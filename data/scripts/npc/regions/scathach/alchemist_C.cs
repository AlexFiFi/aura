using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_CScript : Scathach_alchemist_baseScript
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
