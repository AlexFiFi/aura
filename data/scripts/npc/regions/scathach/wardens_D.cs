using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_DScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_D");
		SetRace(10001);
		SetFace(skin: 22, eye: 5, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0x366969, 0xFFA627, 0x730D6C);
		EquipItem(Pocket.Hair, 0xC22, 0x414141, 0x414141, 0x414141);

		SetLocation(region: 4014, x: 33350, y: 44926);

		SetDirection(196);
		SetStand("");
	}
}
