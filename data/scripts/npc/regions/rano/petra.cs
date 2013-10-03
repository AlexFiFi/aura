using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class PetraScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_petra");
		SetRace(10001);
		SetBody(height: 1.03f, fat: 1f, upper: 0.97f, lower: 1f);
		SetFace(skin: 20, eye: 32, eyeColor: 97, lip: 16);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x6889, 0x960069, 0x892444);
		EquipItem(Pocket.Hair, 0xBCE, 0xFFD7FFE1, 0xFFD7FFE1, 0xFFD7FFE1);
		EquipItem(Pocket.Armor, 0x3AC3, 0xFFB02495, 0xFF340C26, 0xFF340C26);
		EquipItem(Pocket.Shoe, 0x4295, 0xFF477B94, 0xFFC0C0C0, 0xFFFFFFFF);
		EquipItem(Pocket.Head, 0x4872, 0xFF353740, 0xFFBBAEA4, 0xFFFFFFFF);

		SetLocation(region: 3002, x: 17100, y: 16000);

		SetDirection(127);
		SetStand("human/female/anim/female_stand_npc_emain_06");
        
		Phrases.Add("A true fisherman would be able to catch fish no matter where you are.");
		Phrases.Add("Ahhh, is this salt water, or is it tears?");
		Phrases.Add("Alright. There goes another one!");
		Phrases.Add("Augh, I can't seem to focus.");
		Phrases.Add("Did I eat too much for lunch?");
		Phrases.Add("Do we have to turn the ship here... Hmm, I wonder.");
		Phrases.Add("Gosh, it always seems to rain right after we've washed up the decks.");
		Phrases.Add("Hmm, I should start looking for some other fishing grounds.");
		Phrases.Add("Hmm, so boring.");
		Phrases.Add("I remember seeing some blue marlin right around here.");
		Phrases.Add("I should check on the rigging.");
		Phrases.Add("I suppose I should check on the rigging too?");
		Phrases.Add("I think there was a great fishing ground around here...");
		Phrases.Add("I wonder if there's a storm coming...");
		Phrases.Add("I wonder if there's a storm on the way...");
		Phrases.Add("I'm assuming that today's boatride is not your first?");
		Phrases.Add("I'm not looking for pity. Should I lower middle sail?");
		Phrases.Add("Let me give it some thought.");
		Phrases.Add("Maybe I should do some maintenance on the ropes soon.");
		Phrases.Add("Maybe I should do some maintenance on the ropes...");
		Phrases.Add("No efforts go to waste, that's for sure. There's plenty of fish in the sea.");
		Phrases.Add("Oh, my oatmeal cookie! It got all soggy in the seawater.");
		Phrases.Add("Steer right!");
		Phrases.Add("The weather's not too bad today.");
		Phrases.Add("Ugh... Don't give me that sad-looking face!");
		Phrases.Add("Um... Did we catch something?");
		Phrases.Add("What is this? Your first time fishing or something?");
	}
}
