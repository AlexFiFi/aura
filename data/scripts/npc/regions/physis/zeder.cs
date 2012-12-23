using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ZederScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_zeder");
		SetRace(8002);
		SetBody(height: 0.6f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 49, eyeColor: 35, lip: 29);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x22C4, 0xF2659C, 0xFA7D2C, 0x916D);
		EquipItem(Pocket.Hair, 0x1F48, 0xC6C3C6, 0xC6C3C6, 0xC6C3C6);
		EquipItem(Pocket.Armor, 0x3B6B, 0xB27878, 0x4B2C1A, 0x8E522E);
		EquipItem(Pocket.Glove, 0x3EA7, 0xC9AF71, 0xCAB1E7, 0x917C08);
		EquipItem(Pocket.Shoe, 0x42BE, 0xB8A069, 0xCAB1E7, 0x917C08);

		SetLocation(region: 3200, x: 291051, y: 228527);

		SetDirection(235);
		SetStand("giant/male/anim/giant_npc_zeder");
	}
}
