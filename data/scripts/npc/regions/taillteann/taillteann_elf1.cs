using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_elf1Script : Taillteann_elf_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_elf1");
		SetFace(skin: 19, eye: 38, eyeColor: 30, lip: 0);

		EquipItem(Pocket.Face, 0x1AF4, 0x628BA4, 0x7B4F3A, 0x64B27A);
		EquipItem(Pocket.Hair, 0x1777, 0x7B8AAD, 0x7B8AAD, 0x7B8AAD);
		EquipItem(Pocket.Armor, 0x3BEE, 0x647692, 0x78829D, 0xBFBFBF);
		EquipItem(Pocket.RightHand1, 0x9D20, 0xD8B77E, 0x0, 0x0);
		EquipItem(Pocket.RightHand2, 0x9D33, 0xFFFFFF, 0x5D4519, 0x5D5537);

		SetLocation(region: 300, x: 227359, y: 192931);

		SetDirection(150);
	}
}
