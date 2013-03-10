using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Beanrua03Script : Beanrua_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanrua03");
		SetBody(height: 0.97f, fat: 0.97f, upper: 1.09f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 113, lip: 1);

		EquipItem(Pocket.Face, 0xF3C, 0xF39E36, 0x1048, 0x8A3D);
		EquipItem(Pocket.Hair, 0xBD0, 0x980A0A, 0x980A0A, 0x980A0A);
		EquipItem(Pocket.Armor, 0x3AEC, 0x171211, 0x2E2623, 0x875253);
		EquipItem(Pocket.Head, 0x468D, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 57, x: 6618, y: 6271);

		SetDirection(157);
		SetStand("human/female/anim/female_stand_npc_emain_05");
	}
}
