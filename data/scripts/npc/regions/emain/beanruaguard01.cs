using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Beanruaguard01Script : Beanruaguard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanruaguard01");
		SetFace(skin: 15, eye: 9, eyeColor: 167, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xFFFAE0, 0xCBEDF9, 0xB8588C);
		EquipItem(Pocket.Hair, 0xFBE, 0x612314, 0x612314, 0x612314);
		EquipItem(Pocket.Armor, 0x3AA6, 0x0, 0x0, 0x715B44);
		EquipItem(Pocket.Glove, 0x3E86, 0x2E231F, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4272, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetDirection(183);
	}
    
    protected override void Warp(bool visible)
    {
        if (visible)
            Warp(region: 52, x: 47115, y: 47272);
        else
            Warp(15, 500, 0);
    }
}
