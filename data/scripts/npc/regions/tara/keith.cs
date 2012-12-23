using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KeithScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_keith");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 85, eyeColor: 32, lip: 35);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1332, 0x30B558, 0xFAAE51, 0xC5AE6C);
		EquipItem(Pocket.Hair, 0x100A, 0x840C18, 0x840C18, 0x840C18);
		EquipItem(Pocket.Armor, 0x3C22, 0xD7DEE2, 0x606A7A, 0xC17524);

		SetLocation(region: 431, x: 718, y: 2825);

		SetDirection(196);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");
	}
}
