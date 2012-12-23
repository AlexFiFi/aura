using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ManusScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_manus");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 12, eyeColor: 27, lip: 18);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x18B475, 0x87912F, 0x7A4D00);
		EquipItem(Pocket.Hair, 0x1000, 0x2B2822, 0x2B2822, 0x2B2822);
		EquipItem(Pocket.Armor, 0x3AB6, 0xCFD0B5, 0x6600, 0x6600);
		EquipItem(Pocket.Shoe, 0x428B, 0x223846, 0x574662, 0x808080);

		SetLocation(region: 19, x: 881, y: 1194);

		SetDirection(0);
		SetStand("");
	}
}
