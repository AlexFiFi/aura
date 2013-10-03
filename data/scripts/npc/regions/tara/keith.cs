using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KeithScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_keith");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 85, eyeColor: 32, lip: 35);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1332, 0x30B558, 0xFAAE51, 0xC5AE6C);
		EquipItem(Pocket.Hair, 0x100A, 0x840C18, 0x840C18, 0x840C18);
		EquipItem(Pocket.Armor, 0x3C22, 0xD7DEE2, 0x606A7A, 0xC17524);

		SetLocation(region: 431, x: 718, y: 2825);

		SetDirection(196);
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
