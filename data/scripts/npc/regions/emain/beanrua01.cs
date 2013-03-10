using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Beanrua01Script : Beanrua_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanrua01");
		SetBody(height: 0.97f, fat: 0.97f, upper: 1.09f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 156, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x901D52, 0x6D6E67, 0xF1E60C);
		EquipItem(Pocket.Hair, 0xBDA, 0x8E132F, 0x8E132F, 0x8E132F);
		EquipItem(Pocket.Armor, 0x3AEC, 0x171211, 0x2E2623, 0x875253);
		EquipItem(Pocket.Head, 0x468D, 0x0, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 57, x: 6383, y: 5308);

		SetDirection(132);
		SetStand("human/female/anim/female_stand_npc_emain_Rua_02");
	}
}
