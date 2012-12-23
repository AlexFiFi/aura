using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ElenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_elen");
		SetRace(10001);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 25, eye: 3, eyeColor: 54, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x2C6B74, 0xF25CA0, 0xB5901E);
		EquipItem(Pocket.Hair, 0xBBD, 0xFFE680, 0xFFE680, 0xFFE680);
		EquipItem(Pocket.Armor, 0x3AB5, 0xFFFFFF, 0x942370, 0xEFE1C2);
		EquipItem(Pocket.Shoe, 0x427B, 0x2B6280, 0x67676C, 0x5DAA);
		EquipItem(Pocket.Head, 0x4668, 0x7D2224, 0xFFFFFF, 0x88CD);
		EquipItem(Pocket.RightHand1, 0x9C58, 0xFACB5F, 0x4F3C26, 0xFAB052);

		SetLocation(region: 31, x: 11353, y: 12960);

		SetDirection(15);
		SetStand("human/female/anim/female_natural_stand_npc_elen");
	}
}
