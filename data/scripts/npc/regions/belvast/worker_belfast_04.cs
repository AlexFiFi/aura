using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Worker_belfast_04Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_worker_belfast_04");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 19, eye: 31, eyeColor: 134, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x380026, 0xFCC157, 0x98198C);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x3BFC, 0x8598BD, 0x525A70, 0x7589B1);
		EquipItem(Pocket.Shoe, 0x4493, 0x6B4D4B, 0xE6D9C8, 0x4A1896);

		SetLocation(region: 4005, x: 54029, y: 22732);

		SetDirection(206);
		SetStand("human/anim/male_natural_sit_01");
	}
}
