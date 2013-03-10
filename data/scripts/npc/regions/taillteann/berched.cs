using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BerchedScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_berched");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 20, eye: 0, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1334, 0xD8CC8, 0x290AC, 0xF65233);
		EquipItem(Pocket.Hair, 0x1004, 0x887B66, 0x887B66, 0x887B66);
		EquipItem(Pocket.Armor, 0x3BE1, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x42CB, 0x1D1818, 0x686868, 0xD6D6D6);

		SetLocation(region: 300, x: 191470, y: 223608);

		SetDirection(188);
		SetStand("chapter3/human/male/anim/male_c3_npc_berched");
        
		Phrases.Add("Even magic can be dull if it's used everyday.");
		Phrases.Add("Forget the past and let go of the rage in your heart.");
		Phrases.Add("Ha. To think that someone is looking for a Druid and not an Alchemist in Taillteann.");
		Phrases.Add("Hehehe...");
		Phrases.Add("I don't have any prejudices against the Vates.");
		Phrases.Add("I must be getting old, even my Mana isn't the same anymore.");
		Phrases.Add("I'm reminded of my granddaughter Lena today.");
		Phrases.Add("There was a time when I had much more hair.");
	}
}
