using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TaunesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taunes");
		SetRace(31);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.RightHand1, 0x24B29, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 297463, y: 217702);

		SetDirection(153);
		SetStand("giant/male/anim/giant_npc_taunes");
        
		Phrases.Add("Every morning, I start the day off by cleaning my sword, Vastian.");
		Phrases.Add("Giants are the most delicate creatures.");
		Phrases.Add("Hey, I need to work right now. I'll talk to you later.");
		Phrases.Add("I am left-handed, the hand that's closer to the heart.");
		Phrases.Add("I did my part for the Seebarsch royal family.");
		Phrases.Add("If you've come here to hear some kind of heroic tale, I'd suggest you leave.");
		Phrases.Add("I'm not interested in political power. I just focus on my job.");
		Phrases.Add("It's been a long time since I left the battlefield. But for some reason, I don't miss it.");
		Phrases.Add("It's not about what kind of sword you have, but how you handle it.");
		Phrases.Add("When I'm focused on work, I tend to forget about the cold.");
	}
}
