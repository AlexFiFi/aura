using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BanhallenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_banhallen");
		SetRace(10001);
		SetBody(height: 0.5f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 98, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xC08A6B, 0xC8015B, 0xE3AA4B);
		EquipItem(Pocket.Hair, 0xBE3, 0xCC3300, 0xCC3300, 0xCC3300);
		EquipItem(Pocket.Armor, 0x3B71, 0xFF99CC, 0x424563, 0xFFCCCC);
		EquipItem(Pocket.Shoe, 0x42A0, 0x161616, 0x808080, 0xBA9AE2);

		SetLocation(region: 4005, x: 45560, y: 24520);

		SetDirection(12);
		SetStand("chapter4/human/female/anim/female_c4_npc_banallen");
	}
}
