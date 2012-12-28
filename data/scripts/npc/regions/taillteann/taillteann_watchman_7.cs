using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_watchman_7Script : Taillteann_watchman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human7");
		SetFace(skin: 20, eye: 12, eyeColor: 32, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x496375, 0xB5A57F, 0xF21D6E);
		EquipItem(Pocket.Hair, 0xFCA, 0x211C39, 0x211C39, 0x211C39);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 202174, y: 198517);

		SetDirection(126);
	}
}
