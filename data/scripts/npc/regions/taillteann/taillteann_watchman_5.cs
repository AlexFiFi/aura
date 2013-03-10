using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_watchman_5Script : Taillteann_watchman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human5");
		SetFace(skin: 20, eye: 12, eyeColor: 32, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x3BA945, 0xCC0066, 0xAB62);
		EquipItem(Pocket.Hair, 0xFCA, 0x211C39, 0x211C39, 0x211C39);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 220698, y: 178629);

		SetDirection(197);
	}
}
