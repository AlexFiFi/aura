using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_watchman_6Script : Taillteann_watchman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human6");
		SetFace(skin: 20, eye: 32, eyeColor: 162, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xE0005F, 0xDBC75E, 0x5180);
		EquipItem(Pocket.Hair, 0x135C, 0x663333, 0x663333, 0x663333);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 219542, y: 178625);

		SetDirection(194);
	}
}
