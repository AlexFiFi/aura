using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_watchman_8Script : Taillteann_watchman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human8");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xF46369, 0x6E4D9E, 0xFBBC58);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 202173, y: 199698);

		SetDirection(126);
	}
}
