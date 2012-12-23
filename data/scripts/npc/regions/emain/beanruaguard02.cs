using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Beanruaguard02Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_beanruaguard02");
		SetRace(10002);
		SetBody(height: 1.26f, fat: 1.09f, upper: 1.26f, lower: 1f);
		SetFace(skin: 15, eye: 4, eyeColor: 32, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x8346, 0xF99F48, 0xF46F3F);
		EquipItem(Pocket.Hair, 0xFC6, 0xAA7840, 0xAA7840, 0xAA7840);
		EquipItem(Pocket.Armor, 0x3AA6, 0x0, 0x0, 0x715B44);
		EquipItem(Pocket.Glove, 0x3E86, 0x2E231F, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4272, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 52, x: 48270, y: 48122);

		SetDirection(155);
		SetStand("");
	}
}
