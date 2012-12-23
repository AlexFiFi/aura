using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CollenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_collen");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.2f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 162, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1335, 0xF8A133, 0x78B3, 0x696E69);
		EquipItem(Pocket.Hair, 0x1005, 0x96793C, 0x96793C, 0x96793C);
		EquipItem(Pocket.Armor, 0x3BE6, 0x2D2F30, 0x585858, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x42AA, 0x585858, 0x594FB9, 0x808080);
		EquipItem(Pocket.LeftHand1, 0x3E8, 0x0, 0x0, 0x0);

		SetLocation(region: 300, x: 222342, y: 198442);

		SetDirection(115);
		SetStand("chapter3/human/male/anim/male_c3_npc_collen");
	}
}
