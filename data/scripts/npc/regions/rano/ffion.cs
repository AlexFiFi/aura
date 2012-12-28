using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class FfionScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ffion");
		SetRace(10001);
		SetBody(height: 0.1899999f, fat: 1f, upper: 1.12f, lower: 1.2f);
		SetFace(skin: 19, eye: 24, eyeColor: 4, lip: 13);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xD38722, 0xF39B35, 0x8355);
		EquipItem(Pocket.Hair, 0xBDF, 0xFF492512, 0xFF492512, 0xFF492512);
		EquipItem(Pocket.Armor, 0x3B37, 0xFF88532F, 0xFF6D512E, 0xFFF4E3B7);

		SetLocation(region: 3001, x: 165082, y: 160085);

		SetDirection(31);
		SetStand("human/female/anim/female_stand_npc_emain_06");
	}
}
