using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CollenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_collen");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.2f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 162, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1335, 0xF8A133, 0x78B3, 0x696E69);
		EquipItem(Pocket.Hair, 0x1005, 0x96793C, 0x96793C, 0x96793C);
		EquipItem(Pocket.Armor, 0x3BE6, 0x2D2F30, 0x585858, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x42AA, 0x585858, 0x594FB9, 0x808080);
		EquipItem(Pocket.LeftHand1, 0x3E8, 0x0, 0x0, 0x0);

		SetLocation(region: 300, x: 222342, y: 198442);

		SetDirection(115);
		SetStand("chapter3/human/male/anim/male_c3_npc_collen");
        
		Phrases.Add("Alchemy, too, must be a part of Lymilark's will.");
		Phrases.Add("At times, religion dictates that we take a conservative stance.");
		Phrases.Add("May Lymilark's blessing be upon you...");
		Phrases.Add("May the blessings of Lymilark fall graciously upon you.");
		Phrases.Add("The letter has finally arrived from the Pontiff's Court.");
		Phrases.Add("There are times when I realize that I am human being before I am a priest.");
	}
}
