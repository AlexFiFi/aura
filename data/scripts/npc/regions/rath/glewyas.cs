using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GlewyasScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_glewyas");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 100, eyeColor: 26, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x133B, 0xFDC0AB, 0x7E0A6C, 0x96D1F1);
		EquipItem(Pocket.Hair, 0x1012, 0xD4B6A0, 0xD4B6A0, 0xD4B6A0);
		EquipItem(Pocket.Armor, 0x3C4C, 0xE5D2C8, 0xFFFFFF, 0x0);
		EquipItem(Pocket.Shoe, 0x42F2, 0x213547, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4753, 0x2F5D74, 0xB6B6B6, 0xC08FD5);

		SetLocation(region: 410, x: 20505, y: 18728);

		SetDirection(230);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
	}
}
