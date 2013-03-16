using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AgnesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_agnes");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 47, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xCD8320, 0xF0097B, 0x6C676B);
		EquipItem(Pocket.Hair, 0xBE1, 0xB5562, 0xB5562, 0xB5562);
		EquipItem(Pocket.Armor, 0x3AE8, 0xFFFCE8, 0xA5C261, 0x275A49);
		EquipItem(Pocket.Shoe, 0x4290, 0x227775, 0x2F2F, 0x576D8D);

		SetLocation(region: 52, x: 42538, y: 46984);

		SetDirection(244);
		SetStand("human/female/anim/female_natural_stand_npc_01");
        
		Phrases.Add("...");
		Phrases.Add("Ah... I'm bored...");
		Phrases.Add("Hmm...we are out of herbal medicine...");
		Phrases.Add("I need to get out of this routine.");
		Phrases.Add("I'm bored...");
		Phrases.Add("Isn't there something fun to do...?");
		Phrases.Add("Maybe I should take care of this another time...");
		Phrases.Add("Next customer!");
		Phrases.Add("Should I clean up...?");
		Phrases.Add("Should I hire an employee...?");
		Phrases.Add("This isn't good...");
		Phrases.Add("This should do it...");
	}
}
