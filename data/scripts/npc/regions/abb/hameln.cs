using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class HamelnScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hameln");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 17, eye: 5, eyeColor: 0, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xA4AD23, 0x6B598C, 0xDCA45B);
		EquipItem(Pocket.Hair, 0x1359, 0xFFFFFF, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Armor, 0x3AD4, 0x6E745A, 0x592402, 0x364D43);
		EquipItem(Pocket.Shoe, 0x4335, 0x3B3735, 0x6E7D78, 0xA7E1BB);
		EquipItem(Pocket.Head, 0x467D, 0x767966, 0x557380, 0x353E44);
		EquipItem(Pocket.RightHand1, 0x9C71, 0x466C8E, 0x959595, 0x3C4155);

		SetLocation(region: 302, x: 126020, y: 86134);

		SetDirection(200);
		SetStand("");
	}
}
