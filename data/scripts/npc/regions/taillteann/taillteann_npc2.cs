using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_npc2Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_npc2");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 0, lip: 14);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x332E, 0xE19A50, 0xD2847B);
		EquipItem(Pocket.Hair, 0xBE3, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3BF7, 0x4A4A61, 0xCC6633, 0xADAEC6);
		EquipItem(Pocket.Shoe, 0x42F2, 0x211C39, 0x0, 0x0);
		EquipItem(Pocket.Head, 0x4657, 0x424563, 0x8472BD, 0x717600);

		SetLocation(region: 300, x: 226148, y: 191802);

		SetDirection(159);
		SetStand("");
	}
}
