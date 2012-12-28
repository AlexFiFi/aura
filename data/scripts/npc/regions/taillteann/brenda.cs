using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BrendaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_brenda");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 49, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF48, 0x68962F, 0x558E, 0x1E2C7C);
		EquipItem(Pocket.Hair, 0xC1E, 0x822222, 0x822222, 0x822222);
		EquipItem(Pocket.Armor, 0x3BE7, 0xC46751, 0xC99E92, 0x1C2468);
		EquipItem(Pocket.Shoe, 0x427D, 0x1C2468, 0xC1A201, 0x808080);

		SetLocation(region: 300, x: 212557, y: 189817);

		SetDirection(68);
		SetStand("chapter3/human/female/anim/female_c3_npc_brenda");
        
		Phrases.Add("A shipment of beautiful new clothing is arriving today! Ah, ecstasy...");
		Phrases.Add("I need to change the display window. But I feel so lazy today...");
		Phrases.Add("I think I'm gaining weight! I better eat only berries today.");
		Phrases.Add("I wonder if there are any parties tonight?");
		Phrases.Add("I'm exhausted from dancing all night. The things I have to do to appease my adoring public!");
		Phrases.Add("I've seen a lot of people wearing star-patterned clothes recently. Hmm...");
		Phrases.Add("I've slacked on my Lute lessons for far too long, sigh...");
		Phrases.Add("Maybe I should get some sparkly clothes for the store. Sequins? Glitter? Gems? Hmm!");
		Phrases.Add("Where oh where could my Prince Charming be?");
		Phrases.Add("Why am I so beautiful? Sigh, popularity can be tiring sometimes.");
	}
}
