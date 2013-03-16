using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class LennoxScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_lennox");
		SetRace(103);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 87, eyeColor: 49, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1361, 0x1CB08E, 0xAB568D, 0x2F8837);
		EquipItem(Pocket.Hair, 0x1015, 0x758289, 0x534E63, 0xFFCA66);
		EquipItem(Pocket.Armor, 0x3CCF, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4771, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 410, x: 26031, y: 6127);

		SetDirection(106);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");

		Phrases.Add("...");
		Phrases.Add("Hm, how old is Leymore this year?");
		Phrases.Add("Hmm...");
		Phrases.Add("My vision is getting fuzzy. I need some rest.");
		Phrases.Add("Oh my, how time flies.");
		Phrases.Add("The study of alchemy never ends...");
	}
}
