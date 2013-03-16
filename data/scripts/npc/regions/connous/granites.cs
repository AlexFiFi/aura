using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GranitesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_granites");
		SetRace(9002);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 40, eyeColor: 126, lip: 2);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1AF4, 0x395F74, 0x19A78, 0xF40F32);
		EquipItem(Pocket.Hair, 0x1773, 0xABDBD7, 0xABDBD7, 0xABDBD7);
		EquipItem(Pocket.Shoe, 0x4292, 0x6E96CA, 0x5A5A5A, 0x7170B2);
		EquipItem(Pocket.Robe, 0x4A4E, 0x63967, 0x7283C4, 0xD1239);
		EquipItem(Pocket.RightHand1, 0x9D0C, 0xFF808080, 0xFF07497A, 0xFF07497A);
		EquipItem(Pocket.LeftHand1, 0x9C44, 0xFF808080, 0xFF07497A, 0xFF07497A);
        
        NPC.GetItemInPocket(Pocket.Robe).Info.FigureA = 1;

		SetLocation(region: 3100, x: 368232, y: 419384);

		SetDirection(224);
		SetStand("elf/male/anim/elf_npc_granites_stand_friendly");
        
		Phrases.Add("Are you here because you've missed me so? Geez. I can't say that I've felt the same way...");
		Phrases.Add("Even when time passes, not much changes.");
		Phrases.Add("Hey you, just wait over there. Let me tell you the same story once again.");
		Phrases.Add("I don't believe in luck. What kind of idiot do you think I am?");
		Phrases.Add("I know where you're at so...");
		Phrases.Add("I'm not sure. I don't know myself very well either.");
        Phrases.Add("I'm so sick of this...");
		Phrases.Add("I'm starting to get so sick of this work.");
		Phrases.Add("Incredible voice.");
		Phrases.Add("Wherever I turn, all I see is sand, sand and more sand.");
		Phrases.Add("Yeah, I sometimes get pretty depressed, but it doesn't usually last very long.");
	}
}
