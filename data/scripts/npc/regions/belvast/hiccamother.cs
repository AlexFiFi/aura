using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class HiccamotherScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hiccamother");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1.1f, upper: 1.3f, lower: 1.1f);
		SetFace(skin: 15, eye: 5, eyeColor: 134, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x1ABDC, 0xF29D38, 0x26B673);
		EquipItem(Pocket.Hair, 0xBDA, 0x568B7D, 0x568B7D, 0x568B7D);
		EquipItem(Pocket.Armor, 0x3AB0, 0xD6BA8F, 0x5DBAD8, 0x5A5A6E);
		EquipItem(Pocket.Shoe, 0x426E, 0x7F4F45, 0x52B6F2, 0x808080);

		SetLocation(region: 4005, x: 39157, y: 35537);

		SetDirection(65);
		SetStand("");

		Phrases.Add("Don't think about useless things!");
		Phrases.Add("I'm against it!");
		Phrases.Add("Just bring home a pretty wife.");
		Phrases.Add("Not everyone can become a hero.");
		Phrases.Add("What in the world are you talking about?");
		Phrases.Add("Why don't you listen to your mother?");
		Phrases.Add("You should think it over.");
		Phrases.Add("You want to be like your father also?");
	}
}
