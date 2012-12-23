using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BelitaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_belita");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 24, eyeColor: 119, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x546B18, 0x8A5A02, 0xAF8D1E);
		EquipItem(Pocket.Hair, 0xBCA, 0x2D1E14, 0x2D1E14, 0x2D1E14);
		EquipItem(Pocket.Armor, 0x3BC5, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x42A5, 0x323133, 0x413D42, 0x808080);

		SetLocation(region: 3400, x: 330075, y: 175856);

		SetDirection(172);
		SetStand("human/female/anim/female_natural_stand_npc_Eavan");
	}
}
