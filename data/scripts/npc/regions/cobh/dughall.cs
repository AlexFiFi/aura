using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class DughallScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_dughall");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 82, eyeColor: 0, lip: 30);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x362988, 0xF59D31, 0x2A55);
		EquipItem(Pocket.Hair, 0x1F5E, 0xEF9252, 0xEF9252, 0xEF9252);
		EquipItem(Pocket.Armor, 0x3BF6, 0x241919, 0xB79B8A, 0xA253E);
		EquipItem(Pocket.Shoe, 0x42F1, 0xA253E, 0x1E45AD, 0x89AD0F);
		EquipItem(Pocket.Head, 0x4740, 0xA253E, 0xFFFFFF, 0x316900);

		SetLocation(region: 23, x: 27862, y: 36669);

		SetDirection(133);
		SetStand("");
	}
}
