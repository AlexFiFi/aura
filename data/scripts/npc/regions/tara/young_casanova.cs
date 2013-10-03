using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Young_casanovaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_young_casanova");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 1, eyeColor: 0, lip: 13);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0xFFF6D0, 0x5E1775, 0xF3F7E2);
		EquipItem(Pocket.Hair, 0xFB5, 0x2246, 0x2246, 0x2246);
		EquipItem(Pocket.Armor, 0x3B05, 0x2D3C4E, 0x4869CE, 0x7A83A5);
		EquipItem(Pocket.Shoe, 0x42CB, 0x273262, 0x879EAE, 0x808080);

		SetLocation(region: 401, x: 105358, y: 105986);

		SetDirection(227);
		SetStand("chapter3/human/male/anim/male_c3_npc_devi");

		Phrases.Add("Like Eweca, which guides the path of the lost, you've given my heart meaning and direction.");
		Phrases.Add("You've made me a better person.");
	}
}
