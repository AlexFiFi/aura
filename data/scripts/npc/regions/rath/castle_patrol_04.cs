using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_patrol_04Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_castle_patrol_04");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x412275, 0xB1AC12, 0xDEF0E9);
		EquipItem(Pocket.Hair, 0xFB0, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.Armor, 0x3C4D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4754, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xB0B0B0, 0x47A8E5, 0x676661);

		SetLocation(region: 410, x: 9580, y: 14114);

		SetDirection(0);
		SetStand("");
	}
}
