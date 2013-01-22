using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Beanruaguard02Script : Beanruaguard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanruaguard02");
		SetFace(skin: 15, eye: 4, eyeColor: 32, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x8346, 0xF99F48, 0xF46F3F);
		EquipItem(Pocket.Hair, 0xFC6, 0xAA7840, 0xAA7840, 0xAA7840);
		EquipItem(Pocket.Armor, 0x3AA6, 0x0, 0x0, 0x715B44);
		EquipItem(Pocket.Glove, 0x3E86, 0x2E231F, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4272, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetDirection(155);
	}
    
    protected override void Warp(bool visible)
    {
        if (visible)
            WarpNPC(region: 52, x: 48270, y: 48122);
        else
            WarpNPC(15, 600, 0);
    }
}
