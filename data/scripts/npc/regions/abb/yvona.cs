using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class YvonaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_yvona");
		SetRace(9001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 19, eye: 40, eyeColor: 38, lip: 38);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x170C, 0xBCC729, 0x714856, 0xF79827);
		EquipItem(Pocket.Hair, 0x1389, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3E26, 0x202324, 0x725546, 0x34484D);
		EquipItem(Pocket.Glove, 0x3E8A, 0x202324, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4377, 0x202943, 0x2F3C69, 0x6B89F4);
		EquipItem(Pocket.Head, 0x4679, 0x202324, 0x204D76, 0x820202);
		EquipItem(Pocket.RightHand1, 0x9CFE, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CAB, 0x202324, 0xFFFFFF, 0x808080);
		EquipItem(Pocket.LeftHand2, 0xAFC9, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 302, x: 126477, y: 85641);

		SetDirection(170);
		SetStand("darkknight/anim/tool/Bhand_M/darkknight_Bhand_M03_stand_friendly");
        
		Phrases.Add("Can't they leave me alone...?");
		Phrases.Add("He got the theory, but he his music had no soul...");
		Phrases.Add("I knew a guy like that.");
		Phrases.Add("It's not like they're getting paid. Why do they work so hard?");
		Phrases.Add("Tough...");
	}
}
