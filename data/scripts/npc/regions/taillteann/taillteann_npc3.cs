using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_npc3Script : Taillteann_npc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_npc3");
		SetRace(10002);
		SetFace(skin: 15, eye: 15, eyeColor: 0, lip: 6);

		EquipItem(Pocket.Face, 0x1324, 0xBED22E, 0xFAB150, 0xA3807D);
		EquipItem(Pocket.Hair, 0xFC8, 0xCC3300, 0xCC3300, 0xCC3300);
		EquipItem(Pocket.Armor, 0x3BF7, 0x545743, 0x6281AA, 0x93494E);
		EquipItem(Pocket.Shoe, 0x42F2, 0x663300, 0x0, 0x0);

		SetLocation(region: 300, x: 223266, y: 195458);

		SetDirection(176);
	}
}
