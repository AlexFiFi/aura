using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GilmoreScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_gilmore");
		SetRace(10002);
		SetBody(height: 0.8000003f, fat: 0.4f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 7, eyeColor: 76, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1327, 0x719235, 0x496D4A, 0xF2A945);
		EquipItem(Pocket.Hair, 0xFBA, 0x896D43, 0x896D43, 0x896D43);
		EquipItem(Pocket.Armor, 0x3A9B, 0xB6CAAA, 0x584232, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xA68DC3, 0x1B24B);
		EquipItem(Pocket.Head, 0x466C, 0x0, 0xC8C6C4, 0xDFE9A7);

		SetLocation(region: 31, x: 10383, y: 10055);

		SetDirection(224);
		SetStand("human/male/anim/male_natural_stand_npc_gilmore");
        
		Phrases.Add("Business is slow nowadays. Perhaps I should raise the rent.");
		Phrases.Add("Cheap stuff means cheap quality.");
		Phrases.Add("Get lost unless you are going to buy something!");
		Phrases.Add("I have plenty of goods. As long as you have the Gold.");
		Phrases.Add("If you don't like me, you can buy goods somewhere else.");
		Phrases.Add("My goods don't just grow on trees.");
		Phrases.Add("So you think you can buy goods somewhere else?");
		Phrases.Add("They are so much trouble. Those thieving jerks...");
		Phrases.Add("What a pain. More kids just keep coming to the store.");
		Phrases.Add("Why should I put up with criticism from people who are not even my customers?");
	}
}
