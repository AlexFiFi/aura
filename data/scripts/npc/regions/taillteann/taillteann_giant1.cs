using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_giant1Script : Taillteann_giant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_giant1");
		SetFace(skin: 23, eye: 45, eyeColor: 27, lip: 28);

		EquipItem(Pocket.Face, 0x22C4, 0x670061, 0x840F69, 0xCF9461);
		EquipItem(Pocket.Hair, 0x1F51, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		SetLocation(region: 300, x: 205354, y: 191114);

		SetDirection(7);
	}
}
