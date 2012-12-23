using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Guardsman15Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_guardsman15");
		SetRace(10002);
		SetBody(height: 1.17f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 9, eyeColor: 29, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xD74A29, 0x993E11, 0xB2B067);
		EquipItem(Pocket.Hair, 0xFBE, 0xFCF4D1, 0xFCF4D1, 0xFCF4D1);
		EquipItem(Pocket.Armor, 0x32E1, 0x8C8C8C, 0x808080, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x485A, 0x646464, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xFFFFFF, 0x6C7050, 0xFFFFFF);

		SetLocation(region: 52, x: 30253, y: 46178);

		SetDirection(32);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
