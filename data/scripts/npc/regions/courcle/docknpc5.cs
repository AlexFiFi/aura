using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc5Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_docknpc5");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1330, 0xA97E79, 0x187544, 0xFCD652);
		EquipItem(Pocket.Hair, 0xFFC, 0x3A322F, 0x3A322F, 0x3A322F);
		EquipItem(Pocket.Armor, 0x3B89, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F8, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 86480, y: 161730);

		SetDirection(200);
		SetStand("");
	}
}
