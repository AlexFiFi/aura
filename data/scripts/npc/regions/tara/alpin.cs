using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AlpinScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_alpin");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 41, eyeColor: 52, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0xD8E6F6, 0x6C6F70, 0x815E32);
		EquipItem(Pocket.Hair, 0x1013, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3CC8, 0xECECEC, 0x5C7026, 0xBFCC66);
		EquipItem(Pocket.Glove, 0x3E90, 0xE1D5CB, 0x0, 0x0);
		EquipItem(Pocket.Shoe, 0x42F2, 0x353535, 0x0, 0x0);

		SetLocation(region: 401, x: 116260, y: 121904);

		SetDirection(147);
		SetStand("human/male/anim/male_natural_stand_npc_Malcolm");

		Phrases.Add("Another blister on my finger? Wonder how I get so many of these.");
		Phrases.Add("Hmm, those bushes need to be trimmed. I'll do it tomorrow.");
		Phrases.Add("Now, where did I put that fertilizer?");
		Phrases.Add("Oh no! Insects!");
		Phrases.Add("Oh, almost forgot about Sinead's favor!");
		Phrases.Add("Shh, if you listen closely, you can hear the trees whispering to each other.");
		Phrases.Add("This would be a great gift for Sinead!");
		Phrases.Add("Today wasn't too bad.");
	}
}
