using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LezardScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_lezard");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 86, eyeColor: 32, lip: 37);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x133B, 0x6C0025, 0x7A6D49, 0x1A95B);
		EquipItem(Pocket.Hair, 0x100B, 0x746E56, 0x746E56, 0x746E56);
		EquipItem(Pocket.Armor, 0x3C1E, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4299, 0x41463B, 0x5A5A5A, 0x808080);

		SetLocation(region: 401, x: 101436, y: 101459);

		SetDirection(206);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");

		Phrases.Add("(Ding! Ding!) Can't that be changed to a more pleasant sound?");
		Phrases.Add("Aah, another weary day has started!");
		Phrases.Add("At this rate, I'm going to end up being late to the ball.");
		Phrases.Add("Do people understand that having too much money can make life boring?");
		Phrases.Add("Errr....Must the president of the bank be doing this...?");
		Phrases.Add("Haha, don't tell me you've fallen in love with my style?");
		Phrases.Add("Haha, welcome to the head office of Erskin Bank!");
		Phrases.Add("Should I lower the bank fee while I'm at it?");
	}
}
