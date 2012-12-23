using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Eremon_belfast_03Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_eremon_belfast_03");
		SetRace(10001);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 165, lip: 40);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF53, 0xFC9D59, 0xFFECCD, 0xF87333);
		EquipItem(Pocket.Hair, 0xC40, 0xD2D3D5, 0xD2D3D5, 0xD2D3D5);
		EquipItem(Pocket.Armor, 0x3E2C, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4380, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x48F3, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 53517, y: 35198);

		SetDirection(135);
		SetStand("chapter4/human/female/anim/female_c4_npc_erewon_stand");
	}
}
