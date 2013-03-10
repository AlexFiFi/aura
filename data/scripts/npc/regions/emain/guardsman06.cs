using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Guardsman06Script : Emain_guardsman_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_guardsman06");
		SetFace(skin: 15, eye: 4, eyeColor: 167, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xADDEDA, 0x434500, 0x9C005A);
		EquipItem(Pocket.Hair, 0xFC9, 0x4040, 0x4040, 0x4040);
		EquipItem(Pocket.Armor, 0x32E1, 0x8C8C8C, 0x808080, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x485A, 0x646464, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xFFFFFF, 0x6C7050, 0xFFFFFF);

		SetLocation(region: 52, x: 32462, y: 48773);

		SetDirection(225);
	}
}
