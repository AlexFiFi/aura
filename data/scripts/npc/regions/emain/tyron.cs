using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class TyronScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tyron");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 0.97f, upper: 1.2f, lower: 1.03f);
		SetFace(skin: 15, eye: 9, eyeColor: 32, lip: 12);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xB865A9, 0xFDD94D, 0xF23961);
		EquipItem(Pocket.Hair, 0xFBE, 0x1D1712, 0x1D1712, 0x1D1712);
		EquipItem(Pocket.Armor, 0x32D3, 0x0, 0xB6A48B, 0x241D13);
		EquipItem(Pocket.Glove, 0x4077, 0x6F5840, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4464, 0xCBAF9C, 0xCAB8A4, 0xFFFFFF);
		EquipItem(Pocket.RightHand1, 0x9C4B, 0xC0C0C0, 0x304544, 0xFFFFFF);

		SetLocation(region: 52, x: 26103, y: 59831);

		SetDirection(155);
		SetStand("human/anim/male_natural_sit_02.ani");
	}
}
