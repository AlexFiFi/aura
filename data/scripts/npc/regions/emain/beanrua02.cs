using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Beanrua02Script : Beanrua_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanrua02");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 1, eyeColor: 39, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3D, 0xB57C13, 0xFFE171, 0xFA946E);
		EquipItem(Pocket.Hair, 0xBD8, 0x942C12, 0x942C12, 0x942C12);
		EquipItem(Pocket.Armor, 0x3AEC, 0x171211, 0x2E2623, 0x875253);
		EquipItem(Pocket.Head, 0x468D, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 57, x: 6927, y: 6356);

		SetDirection(143);
		SetStand("human/female/anim/female_stand_npc_emain_Rua");
	}
}
