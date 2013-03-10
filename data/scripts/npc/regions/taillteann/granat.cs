using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GranatScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_granat");
		SetRace(9002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 41, eyeColor: 114, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0x5C0056, 0xECF4DB, 0x1F706B);
		EquipItem(Pocket.Hair, 0x1778, 0x9DD0EC, 0x9DD0EC, 0x9DD0EC);
		EquipItem(Pocket.Armor, 0x36C9, 0x4D5F7D, 0x888484, 0xCED7E8);
		EquipItem(Pocket.Shoe, 0x4296, 0x4D5F7D, 0x979797, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9D2C, 0xD7D1AD, 0xEFE3B5, 0x0);
		EquipItem(Pocket.LeftHand2, 0xAFC9, 0x0, 0x0, 0x0);

		SetLocation(region: 300, x: 227290, y: 193377);

		SetDirection(150);
		SetStand("chapter3/elf/male/anim/elf_c3_npc_granat");
        
		Phrases.Add("Elves are the best friends of Humans.");
		Phrases.Add("Have we come too far from the Memory Tower?");
		Phrases.Add("Supply Guards must pay extra heed to taking care of our equipment.");
		Phrases.Add("The Elves of Connous will protect you from the threat of the Shadow Realm.");
		Phrases.Add("This weather is difficult for Desert Elves to get used to.");
		Phrases.Add("We are preparing for a Shadow Realm expedition.");
		Phrases.Add("What is impossible, if only Elves and Humans work together?");
	}
}
