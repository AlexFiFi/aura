using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NessaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_nessa");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 26, eyeColor: 27, lip: 2);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x530056, 0x26426A, 0xFDE1EA);
		EquipItem(Pocket.Hair, 0xBDA, 0xFF99CC, 0xFF99CC, 0xFF99CC);
		EquipItem(Pocket.Armor, 0x3AAC, 0xEF9252, 0xFFE3B5, 0xEA5C9E);
		EquipItem(Pocket.Shoe, 0x427C, 0xFFFFFF, 0xE5C00E, 0x808080);

		SetLocation(region: 429, x: 1183, y: 2195);

		SetDirection(247);
		SetStand("human/female/anim/female_stand_npc_emain_Rua_02");

		Phrases.Add("I need to get more herbs.");
		Phrases.Add("I need to trim my hair.");
		Phrases.Add("I'm done organizing all the First Aid Kits and Bandages!");
		Phrases.Add("There seems to be a lot of people who've fallen down today.");
	}
}
