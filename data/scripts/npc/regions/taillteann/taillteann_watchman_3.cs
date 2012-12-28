using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_watchman_3Script : Taillteann_watchman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human3");
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x769476, 0x770008, 0xD6D6EB);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 236811, y: 193025);

		SetDirection(0);
	}
}
