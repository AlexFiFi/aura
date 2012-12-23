using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_npc3Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_npc3");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 15, eyeColor: 0, lip: 6);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xBED22E, 0xFAB150, 0xA3807D);
		EquipItem(Pocket.Hair, 0xFC8, 0xCC3300, 0xCC3300, 0xCC3300);
		EquipItem(Pocket.Armor, 0x3BF7, 0x545743, 0x6281AA, 0x93494E);
		EquipItem(Pocket.Shoe, 0x42F2, 0x663300, 0x0, 0x0);

		SetLocation(region: 300, x: 223266, y: 195458);

		SetDirection(176);
		SetStand("");
	}
}
