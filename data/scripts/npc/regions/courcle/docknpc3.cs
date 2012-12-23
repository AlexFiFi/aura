using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc3Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_docknpc3");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1330, 0xFABBCC, 0xFDE25B, 0x117C78);
		EquipItem(Pocket.Hair, 0xFFC, 0x3A322F, 0x3A322F, 0x3A322F);
		EquipItem(Pocket.Armor, 0x3B89, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F8, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 175860, y: 130750);

		SetDirection(48);
		SetStand("");
	}
}
