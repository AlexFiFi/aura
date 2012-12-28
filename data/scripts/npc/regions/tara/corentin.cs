using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CorentinScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_corentin");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 105, eyeColor: 49, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xFCBD54, 0xB70D27, 0xA0D5);
		EquipItem(Pocket.Hair, 0xC29, 0x202020, 0x202020, 0x202020);
		EquipItem(Pocket.Armor, 0x3CC9, 0x808080, 0xCD9048, 0xCD9048);
		EquipItem(Pocket.Shoe, 0x42F6, 0x454545, 0xA6A6A8, 0x7B1A5E);

		SetLocation(region: 421, x: 5692, y: 4294);

		SetDirection(117);
		SetStand("human/anim/uni_natural_stand_straight");

		Phrases.Add("Hm...");
		Phrases.Add("I am Corentin, Chamberlain of the Pontiff's Court.");
		Phrases.Add("May the light of Lymilark be upon you always.");
		Phrases.Add("My prayer is that the hearts of all believers shall feel the warmth of Lymilark's holy fire.");
		Phrases.Add("Oh? Is the donation box full already?");
		Phrases.Add("Please join us in supporting the Church of Lymilark.");
		Phrases.Add("The light of Lymilark has led you here.");
		Phrases.Add("This is holy ground, upon which the light of Lymilark shines.");
		Phrases.Add("Will you heed the call of Lymilark?");
	}
}
