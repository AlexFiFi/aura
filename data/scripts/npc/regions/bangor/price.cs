using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class PriceScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_price");
		SetRace(10002);
		SetBody(height: 1.29f, fat: 1.12f, upper: 1.32f, lower: 1.06f);
		SetFace(skin: 19, eye: 4, eyeColor: 90, lip: 13);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1325, 0xD8DDF2, 0xEF8A7A, 0x3F4D);
		EquipItem(Pocket.Hair, 0x135B, 0xC1C298, 0xC1C298, 0xC1C298);
		EquipItem(Pocket.Armor, 0x3ACC, 0x986C4B, 0x181E13, 0xC2B39E);
		EquipItem(Pocket.Shoe, 0x4294, 0x74562E, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x4668, 0x4E7271, 0x24312F, 0xFFFFFF);

		SetLocation(region: 31, x: 13840, y: 14746);

		SetDirection(80);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");
	}
}
