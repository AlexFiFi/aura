using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AlpinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alpin");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 41, eyeColor: 52, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xD8E6F6, 0x6C6F70, 0x815E32);
		EquipItem(Pocket.Hair, 0x1013, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3CC8, 0xECECEC, 0x5C7026, 0xBFCC66);
		EquipItem(Pocket.Glove, 0x3E90, 0xE1D5CB, 0x0, 0x0);
		EquipItem(Pocket.Shoe, 0x42F2, 0x353535, 0x0, 0x0);

		SetLocation(region: 401, x: 116260, y: 121904);

		SetDirection(147);
		SetStand("human/male/anim/male_natural_stand_npc_Malcolm");
	}
}
