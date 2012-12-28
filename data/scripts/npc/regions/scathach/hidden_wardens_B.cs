using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_BScript : Wardens_baseScript
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
