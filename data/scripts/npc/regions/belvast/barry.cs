using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BarryScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_barry");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 7, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x5B005D, 0x65656A, 0xE0F0DC);
		EquipItem(Pocket.Hair, 0x177A, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3B24, 0x424563, 0xCCCCCC, 0x7B2C10);
		EquipItem(Pocket.Shoe, 0x460B, 0x3F2A25, 0x2B9AF6, 0x10067);
		EquipItem(Pocket.Head, 0x4770, 0x37727F, 0xC4C4C4, 0x67676F);
		EquipItem(Pocket.LeftHand1, 0xA027, 0x0, 0x0, 0x0);

		SetLocation(region: 4005, x: 47004, y: 26750);

		SetDirection(21);
		SetStand("chapter4/human/male/anim/male_c4_npc_drink");

		Phrases.Add("Do you think I'm joking...? Yeah, I'm joking. Heh.");
		Phrases.Add("Don't put words into my mouth.");
		Phrases.Add("I'm not scary. I'm not. I'm NOT.");
		Phrases.Add("Stay away from my daughter.");
		Phrases.Add("Watch what you say...");
		Phrases.Add("Welcome to Barry's Pub! I'm Barry.");
		Phrases.Add("What did you just say?");
		Phrases.Add("What would you like to drink?");
	}
}
