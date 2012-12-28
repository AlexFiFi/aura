using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_DScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_D");
		SetRace(10001);
		SetFace(skin: 22, eye: 9, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0xD4EEED, 0xFCD25D, 0x84002C);
		EquipItem(Pocket.Hair, 0xBE0, 0x864349, 0x864349, 0x864349);

		SetLocation(region: 4014, x: 74100, y: 59900);

		SetDirection(75);
		SetStand("");
	}
}
