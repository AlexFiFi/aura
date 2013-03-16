using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MondScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_mond");
		SetRace(139);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 26, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x808080, 0x0, 0x0);

		EquipItem(Pocket.RightHand1, 0x9E17, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0x9E15, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 42094, y: 22868);

		SetDirection(3);
		SetStand("chapter4/monster/anim/imp/imp_c4_npc_bank");

		Phrases.Add("Hey, I happen to be a respectable Imp! Eeeyah!");
		Phrases.Add("Interest rates? Don't worry about that. Just think, \"free money!\"");
		Phrases.Add("Loan payments due by the end of the month, including interest!");
		Phrases.Add("Mumble, mumble, mumble, mumble, mumble.");
		Phrases.Add("Mumble, mumble, mumble...");
		Phrases.Add("Oh yeah? Prove it!");
		Phrases.Add("Quiet! Banks are serious business.");
		Phrases.Add("Your payments are due! Gimme my money! Eeeyah!");
	}
}
