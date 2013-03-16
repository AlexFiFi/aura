using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KusinaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kusina");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 26, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF46, 0x8377, 0xF99C4D, 0xFAB154);
		EquipItem(Pocket.Hair, 0xC1B, 0x37231D, 0x37231D, 0x37231D);
		EquipItem(Pocket.Armor, 0x3B8C, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 258180, y: 188820);

		SetDirection(183);
		SetStand("human/female/anim/female_stand_npc_iria_01");
        
		Phrases.Add("Cor Village is safe from the Goblins.");
		Phrases.Add("I thank the Great Spirit of Irinid for her grace.");
		Phrases.Add("I want to dye this piece of clothing a beautiful color.");
		Phrases.Add("Mr. Voight...");
		Phrases.Add("The sunlight is so warm and bright that I could almost touch it.");
		Phrases.Add("Tupai, did you finish today's homework?");
		Phrases.Add("What color do you think I should use?");
	}
}
