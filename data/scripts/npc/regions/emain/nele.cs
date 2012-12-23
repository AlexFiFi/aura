using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class NeleScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_nele");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.2f, lower: 1.2f);
		SetFace(skin: 17, eye: 5, eyeColor: 0, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x6D6161, 0x6F555E, 0x66A2);
		EquipItem(Pocket.Hair, 0x1359, 0xA97534, 0xA97534, 0xA97534);
		EquipItem(Pocket.Armor, 0x3A9A, 0x4040, 0x539540, 0x6A8C91);
		EquipItem(Pocket.Shoe, 0x4294, 0x824D3E, 0x4B4B4B, 0xB80026);
		EquipItem(Pocket.Head, 0x467D, 0x45250C, 0x0, 0xFEF1CB);
		EquipItem(Pocket.Robe, 0x4A3C, 0xFDE8B7, 0x539540, 0x6A8C91);
		EquipItem(Pocket.RightHand1, 0x9C51, 0x67974, 0x31150F, 0x1D5567);

		SetLocation(region: 52, x: 40527, y: 41230);

		SetDirection(217);
		SetStand("");
	}
}
