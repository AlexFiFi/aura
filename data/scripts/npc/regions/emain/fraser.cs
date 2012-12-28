using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class FraserScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_fraser");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.3f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 39, lip: 4);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x754045, 0xDDD6EA, 0x5383);
		EquipItem(Pocket.Hair, 0xFC0, 0xDBB0B0, 0xDBB0B0, 0xDBB0B0);
		EquipItem(Pocket.Armor, 0x3AE5, 0xFFFFFF, 0xF5A929, 0xF5A929);
		EquipItem(Pocket.Shoe, 0x4271, 0x352411, 0x696969, 0xE17662);
		EquipItem(Pocket.Head, 0x4685, 0xFFFFFF, 0x824F58, 0x745339);

		SetLocation(region: 52, x: 35257, y: 39473);

		SetDirection(232);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");
        
		Phrases.Add("Ah, my legs are sore.");
		Phrases.Add("Ha! I'm a genius!");
		Phrases.Add("I gotta make sure I keep track of the time on the oven.");
		Phrases.Add("Let's eat first, I'm hungry.");
		Phrases.Add("Mmm, smells good.");
		Phrases.Add("Next time, I'll try using flowers for decoration...");
		Phrases.Add("This should be good enough for the whip cream...");
		Phrases.Add("Time to eat!");
		Phrases.Add("What! I'm out of fruit!");
		Phrases.Add("What's the next step?");
	}
}
