using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LileasScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_lileas");
		SetRace(10001);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 82, eyeColor: 25, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF4E, 0xF35876, 0xFBC614, 0x460004);
		EquipItem(Pocket.Hair, 0xC24, 0xFFCC66, 0xFFCC66, 0xFFCC66);
		EquipItem(Pocket.Armor, 0x3C1B, 0xFFBD33, 0x9C5D42, 0x654834);
		EquipItem(Pocket.Shoe, 0x4318, 0x3C3834, 0x808080, 0x808080);

		SetLocation(region: 428, x: 72730, y: 105460);

		SetDirection(96);
		SetStand("human/female/anim/female_natural_stand_npc_01");
	}
}
