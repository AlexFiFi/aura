using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ComganScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_comgan");
		SetRace(10002);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 55, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x977726, 0xFFFCDE, 0x35003F);
		EquipItem(Pocket.Hair, 0xFA3, 0xFFFFFFF, 0xFFFFFFF, 0xFFFFFFF);
		EquipItem(Pocket.Armor, 0x3AD4, 0x400000, 0xF0EA9D, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4277, 0x0, 0xF4638B, 0xF9EF64);

		SetLocation(region: 31, x: 15329, y: 12122);

		SetDirection(154);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");
	}
}
