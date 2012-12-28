using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GalvinScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_galvin");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 25, eye: 6, eyeColor: 29, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xEBA441, 0xEF4F2B, 0x9D5627);
		EquipItem(Pocket.Hair, 0xFC4, 0xF2BB7D, 0xF2BB7D, 0xF2BB7D);
		EquipItem(Pocket.Armor, 0x3ADE, 0x3E3D22, 0x16322C, 0xF5D29E);
		EquipItem(Pocket.Shoe, 0x4294, 0x0, 0x4C1458, 0xAF760B);
		EquipItem(Pocket.Head, 0x467F, 0x7C2703, 0xE7A95F, 0x8B5340);

		SetLocation(region: 52, x: 43924, y: 37003);

		SetDirection(61);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");
        
		Phrases.Add("(*Scratch*)");
		Phrases.Add("Hahaha!");
		Phrases.Add("Hee! Ha! Hee!");
		Phrases.Add("Hello, sir! Right here!");
		Phrases.Add("Hello, there!");
		Phrases.Add("Hey there, cool guy!");
		Phrases.Add("Hey! How you doing there?");
	}
}
