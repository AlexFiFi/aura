using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Worker_belfast_02Script : Worker_belfast_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_worker_belfast_02");
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1.4f);
		SetFace(skin: 20, eye: 21, eyeColor: 0, lip: 2);

		EquipItem(Pocket.Face, 0x1324, 0xDA9B59, 0xFA8E3D, 0x93B396);
		EquipItem(Pocket.Hair, 0x1773, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3AC4, 0x9FA9CD, 0x4A6290, 0x6C9368);
		EquipItem(Pocket.Shoe, 0x4493, 0x4792B0, 0x6B5737, 0xD6B2C3);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);
        
		SetLocation(region: 4005, x: 54700, y: 24291);

		SetDirection(63);
	}
}
