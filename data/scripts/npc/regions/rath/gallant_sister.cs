using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Gallant_sisterScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_gallant_sister");
		SetRace(10001);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1f, lower: 0.9f);
		SetFace(skin: 15, eye: 10, eyeColor: 8, lip: 12);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x616169, 0xF57F31, 0x402F60);
		EquipItem(Pocket.Hair, 0xBE3, 0x626743, 0x626743, 0x626743);
		EquipItem(Pocket.Armor, 0x3B9D, 0xD5B1C6, 0x7E9AD8, 0x324064);
		EquipItem(Pocket.Shoe, 0x42A0, 0xCE9D9E, 0x2980C3, 0x808080);

		SetLocation(region: 401, x: 123598, y: 122990);

		SetDirection(224);
		SetStand("human/female/anim/female_natural_stand_npc_Eavan");

		Phrases.Add("I can enter the Fashion Competition, too!");
		Phrases.Add("You're mean! I'm not talking to you!");
	}
}
