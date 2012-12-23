using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_patrol_03Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_castle_patrol_03");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x7C0011, 0x51419B, 0x3EB28F);
		EquipItem(Pocket.Hair, 0xFB0, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.Armor, 0x3C4D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4754, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xB0B0B0, 0x47A8E5, 0x676661);

		SetLocation(region: 410, x: 14148, y: 22118);

		SetDirection(255);
		SetStand("");
	}
}
