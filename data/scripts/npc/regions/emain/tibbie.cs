using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class TibbieScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tibbie");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 127, eyeColor: 162, lip: 41);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x616A6D, 0xFAA950, 0xF8A53E);
		EquipItem(Pocket.Hair, 0xBE8, 0x6C6955, 0x6C6955, 0x6C6955);
		EquipItem(Pocket.Armor, 0x3D1A, 0xE3D5CD, 0xCED7E8, 0xD07B96);
		EquipItem(Pocket.Shoe, 0x426F, 0xDCD2CB, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x488F, 0x5A4D8C, 0xE2DCDC, 0x808080);

		SetLocation(region: 52, x: 30835, y: 43292);

		SetDirection(208);
		SetStand("chapter4/human/anim/cutscene2/3act/3act2ch_juliet_sad");
	}
}
