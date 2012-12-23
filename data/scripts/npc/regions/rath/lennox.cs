using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LennoxScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_lennox");
		SetRace(103);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 87, eyeColor: 49, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1361, 0x1CB08E, 0xAB568D, 0x2F8837);
		EquipItem(Pocket.Hair, 0x1015, 0x758289, 0x534E63, 0xFFCA66);
		EquipItem(Pocket.Armor, 0x3CCF, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4771, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 410, x: 26031, y: 6127);

		SetDirection(106);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
	}
}
