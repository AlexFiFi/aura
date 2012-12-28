using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class HeulfrynScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_heulfryn");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 239, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x9C0571, 0x60034, 0x6F6253);
		EquipItem(Pocket.Hair, 0xFC7, 0xFFFEE7BC, 0xFFFEE7BC, 0xFFFEE7BC);
		EquipItem(Pocket.Armor, 0x3AB6, 0xFFEEFFFF, 0xFF1A6253, 0xFF0F5E06);
		EquipItem(Pocket.Shoe, 0x4283, 0xFFC97A10, 0xFFF5E79E, 0xFFFAE1A7);

		SetLocation(region: 3001, x: 166090, y: 169957);

		SetDirection(144);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");
        
		Phrases.Add("And if you need some healing, just let me know.");
		Phrases.Add("I hope the good weather continues...");
		Phrases.Add("I sell magic items too.");
		Phrases.Add("I'm gonna have to find some part-time help...");
		Phrases.Add("Maybe I should send home a letter today...");
		Phrases.Add("No matter where you are, you can always use a little blessing");
		Phrases.Add("The mana's effects in this land...");
	}
}
