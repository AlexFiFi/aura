using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EdanaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_edana");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 27, eyeColor: 29, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xAC52, 0xFFE05E, 0xB8D63D);
		EquipItem(Pocket.Hair, 0xBC4, 0xBF86AC, 0xBF86AC, 0xBF86AC);
		EquipItem(Pocket.Armor, 0x3C0F, 0xD29E0B, 0xCDEAD7, 0xC5C38B);
		EquipItem(Pocket.Shoe, 0x4290, 0x727C92, 0x67C4E5, 0x808080);

		SetLocation(region: 402, x: 43900, y: 10227);

		SetDirection(43);
		SetStand("human/female/anim/female_natural_stand_npc_Lassar");
        
		Phrases.Add("I better finish cleaning before Eluned returns.");
		Phrases.Add("I wonder how Rumon is doing.");
		Phrases.Add("I wonder if there really is such a thing as the Goddess Water.");
		Phrases.Add("She must have fought with Lezarro again.");
		Phrases.Add("So, this day passes by once again like any other.");
	}
}
