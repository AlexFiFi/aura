using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Guardsman05Script : Emain_guardsman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_guardsman05");
		SetFace(skin: 15, eye: 4, eyeColor: 167, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xE6A64D, 0xFDD7CF, 0x936);
		EquipItem(Pocket.Hair, 0xFC9, 0x4040, 0x4040, 0x4040);
		EquipItem(Pocket.Armor, 0x32E1, 0x8C8C8C, 0x808080, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x485A, 0x646464, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xFFFFFF, 0x6C7050, 0xFFFFFF);

		SetLocation(region: 52, x: 32032, y: 48355);

		SetDirection(223);
	}
}
