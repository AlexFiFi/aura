using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Admiral_owenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_admiral_owen");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 147, eyeColor: 126, lip: 46);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1343, 0x737070, 0xFAB85B, 0x3A54);
		EquipItem(Pocket.Hair, 0x102F, 0xA7A59D, 0xA7A59D, 0xA7A59D);
		EquipItem(Pocket.Armor, 0x3E02, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x4214, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 22638, y: 29322);

		SetDirection(0);
		SetStand("chapter4/human/male/anim/male_c4_npc_owen");

		Phrases.Add("Hmm...this is going to take more work.");
		Phrases.Add("How many ships are coming in today?");
		Phrases.Add("I think a storm may be heading our way.");
		Phrases.Add("Let's handle the important matters first.");
		Phrases.Add("Perhaps, if we were to...");
		Phrases.Add("There's not a cloud in the sky.");
		Phrases.Add("This is a dangerous area. Stay alert.");
	}
}
