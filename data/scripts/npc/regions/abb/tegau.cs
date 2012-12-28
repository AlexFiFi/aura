using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class TegauScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tegau");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 0.5f, upper: 1.4f, lower: 1.2f);
		SetFace(skin: 23, eye: 7, eyeColor: 27, lip: 35);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3E, 0xF46F85, 0xE5CA8, 0xC9006C);
		EquipItem(Pocket.Hair, 0xC2E, 0x9F936C, 0x9F936C, 0x9F936C);
		EquipItem(Pocket.Armor, 0x3B1B, 0xA2513F, 0xADAEC6, 0x543D3D);
		EquipItem(Pocket.Shoe, 0x4268, 0x543D3D, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4750, 0x808080, 0x714949, 0x808080);

		SetLocation(region: 302, x: 126529, y: 84900);

		SetDirection(113);
		SetStand("human/male/anim/male_natural_stand_npc_gilmore");
        
		Phrases.Add("Listen closely, and you can hear the whispers of the stars.");
		Phrases.Add("My back hurts...or does it?");
		Phrases.Add("My, oh my, oh my...");
		Phrases.Add("No one can see the future...or can they?");
		Phrases.Add("Speak up, dear.");
	}
}
