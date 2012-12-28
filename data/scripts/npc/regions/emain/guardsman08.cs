using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Guardsman08Script : Emain_guardsman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_guardsman08");
		SetFace(skin: 15, eye: 9, eyeColor: 29, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xFCB756, 0x4A004A, 0x9B767B);
		EquipItem(Pocket.Hair, 0xFBE, 0xFCF4D1, 0xFCF4D1, 0xFCF4D1);
		EquipItem(Pocket.Armor, 0x32E1, 0x8C8C8C, 0x808080, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x485A, 0x646464, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xFFFFFF, 0x6C7050, 0xFFFFFF);

		SetLocation(region: 52, x: 41213, y: 58006);

		SetDirection(63);
	}
}
