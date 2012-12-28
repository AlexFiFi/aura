using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Worker_belfast_01Script : Worker_belfast_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_worker_belfast_01");
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 30, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x29C0BE, 0x8ADAA, 0x9A518F);
		EquipItem(Pocket.Hair, 0xFCB, 0x506A59, 0x506A59, 0x506A59);
		EquipItem(Pocket.Armor, 0x3AC4, 0x698C6D, 0x313A58, 0x4F91F4);
		EquipItem(Pocket.Shoe, 0x4493, 0x9F663F, 0xF6ECE5, 0x5F9CFC);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 54358, y: 23439);

		SetDirection(193);
	}
}
