using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KristellScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_kristell");
		SetRace(10001);
		SetBody(height: 0.97f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 191, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x136EAA, 0xFEE1D3, 0x7B6C3C);
		EquipItem(Pocket.Hair, 0xBC9, 0xEE937E, 0xEE937E, 0xEE937E);
		EquipItem(Pocket.Armor, 0x3AA1, 0x303133, 0xC6D8EA, 0xDBC741);
		EquipItem(Pocket.Shoe, 0x4277, 0x303133, 0x7BCDB7, 0x6E6565);

		SetLocation(region: 14, x: 34657, y: 42808);

		SetDirection(0);
		SetStand("human/female/anim/female_natural_stand_npc_Kristell");
	}
}
