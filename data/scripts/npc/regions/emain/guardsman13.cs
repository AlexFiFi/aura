using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Guardsman13Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_guardsman13");
		SetRace(10002);
		SetBody(height: 1.17f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 9, eyeColor: 29, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x63636A, 0x1BBFDA, 0x7FBE52);
		EquipItem(Pocket.Hair, 0xFBE, 0xFCF4D1, 0xFCF4D1, 0xFCF4D1);
		EquipItem(Pocket.Armor, 0x32E1, 0x8C8C8C, 0x808080, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x485A, 0x646464, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xFFFFFF, 0x6C7050, 0xFFFFFF);

		SetLocation(region: 52, x: 31273, y: 49527);

		SetDirection(226);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
