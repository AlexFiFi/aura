using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ArzhelaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_arzhela");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 101, eyeColor: 27, lip: 35);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF44, 0x169AD, 0x8D91, 0x8A003C);
		EquipItem(Pocket.Hair, 0xC26, 0x9F936C, 0x9F936C, 0x9F936C);
		EquipItem(Pocket.Armor, 0x3C0E, 0x4C3D63, 0xD2E0E4, 0x2E3137);
		EquipItem(Pocket.Shoe, 0x4290, 0x3D190F, 0x808080, 0x808080);

		SetLocation(region: 417, x: 3117, y: 3027);

		SetDirection(193);
		SetStand("chapter3/human/female/anim/female_c3_npc_dorren");

		Phrases.Add("Hm, Buchanan sent me an interesting letter...");
		Phrases.Add("Hmm...");
		Phrases.Add("I can't begin to imagine a world without books!");
		Phrases.Add("My eyes tire so easily these days.");
		Phrases.Add("Nothing is so painful as living life fixated on your past mistakes.");
		Phrases.Add("Oh! I've never seen THIS book before.");
		Phrases.Add("This is the biggest library in all of Erinn.");
		Phrases.Add("Welcome to the Royal Library.");
		Phrases.Add("Why does my vocabulary seem to get smaller as I get older?");
		Phrases.Add("Why, is there a book missing?");
	}
}
