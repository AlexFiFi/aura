using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class TupayScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tupay");
		SetRace(10002);
		SetBody(height: 0.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 31, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x67C7B7, 0x656A6D, 0xF8A83F);
		EquipItem(Pocket.Hair, 0xFF8, 0x3B2D20, 0x3B2D20, 0x3B2D20);
		EquipItem(Pocket.Armor, 0x3B8B, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 257350, y: 183890);

		SetDirection(115);
		SetStand("");
	}
}
