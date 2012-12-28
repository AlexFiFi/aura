using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PromotionnpcScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_promotionnpc");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 8, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x6F6857, 0x88CC93, 0x7A7DBE);
		EquipItem(Pocket.Hair, 0xFC7, 0xFFB39897, 0xFFB39897, 0xFFB39897);
		EquipItem(Pocket.Armor, 0x3ADB, 0x827EB2, 0x3D4363, 0x713E5E);

		SetLocation(region: 212, x: 2433, y: 3032);

		SetDirection(191);
		SetStand("");
	}
}
