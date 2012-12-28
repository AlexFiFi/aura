using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_EScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_E");
		SetRace(10001);
		SetFace(skin: 22, eye: 12, eyeColor: 8, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0x87BA5C, 0xFA998E, 0x404863);
		EquipItem(Pocket.Hair, 0xBE6, 0x717141, 0x717141, 0x717141);

		SetLocation(region: 4014, x: 73500, y: 76600);

		SetDirection(233);
		SetStand("");
	}
}
