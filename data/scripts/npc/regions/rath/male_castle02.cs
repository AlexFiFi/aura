using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Male_castle02Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_male_castle02");
		SetRace(10002);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 0.9f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xB6002E, 0xFFE38A, 0xAE8D);
		EquipItem(Pocket.Hair, 0x1774, 0x802445, 0x802445, 0x802445);
		EquipItem(Pocket.Armor, 0x3B33, 0x8298B5, 0xDC7D4F, 0x75788B);
		EquipItem(Pocket.Shoe, 0x42E4, 0x34678F, 0x3D00A9, 0x808080);

		SetLocation(region: 410, x: 8058, y: 17918);

		SetDirection(0);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
	}
}
