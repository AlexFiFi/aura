using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AilionoaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ailionoa");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 114, lip: 14);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xCFE4A7, 0x19EA5, 0xE9E619);
		EquipItem(Pocket.Hair, 0xBE8, 0xFFE2AC, 0xFFE2AC, 0xFFE2AC);
		EquipItem(Pocket.Armor, 0x3AE7, 0xDD6C66, 0x450C1D, 0xFFF2D2);
		EquipItem(Pocket.Shoe, 0x426F, 0x200B04, 0x576D8D, 0xFFFFFF);

		SetLocation(region: 52, x: 48618, y: 44955);

		SetDirection(131);
		SetStand("human/female/anim/female_natural_stand_npc_03");
        
		Phrases.Add("...Annoying.");
		Phrases.Add("...No...Nothing.");
		Phrases.Add("Am I missing something...?");
		Phrases.Add("But...");
		Phrases.Add("I think I've forgot something...");
		Phrases.Add("It doesn't look good...");
		Phrases.Add("No way...");
		Phrases.Add("Not yet...");
		Phrases.Add("That's your problem...");
		Phrases.Add("There's no point in talking like that...");
		Phrases.Add("This should be it...");
		Phrases.Add("What should I do?");
		Phrases.Add("What's wrong with them?");
		Phrases.Add("Why...");
	}
}
