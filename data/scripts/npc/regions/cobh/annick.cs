using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AnnickScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_annick");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 6, eyeColor: 37, lip: 55);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x8ED4, 0xA6D590, 0x5130);
		EquipItem(Pocket.Hair, 0xC39, 0x2D1C12, 0x2D1C12, 0x2D1C12);
		EquipItem(Pocket.Armor, 0x3DF2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 136, x: 939, y: 978);

		SetDirection(253);
		SetStand("chapter4/human/female/anim/female_c4_npc_annick");

		Phrases.Add("Ah, I love the smell of the sea!");
		Phrases.Add("Hahahah! You're a strange one, aren't ye?");
		Phrases.Add("Hey there! What're you looking at?");
		Phrases.Add("Hmm... Storm's comin', I reckon.");
		Phrases.Add("I miss my days on the sea.");
		Phrases.Add("I wonder what Miyir is doing right now...");
		Phrases.Add("I wonder what my men are up to?");
		Phrases.Add("If it wasn't for Admiral Owen, I would never be here!");
		Phrases.Add("This is the start of another day!");
	}
}
