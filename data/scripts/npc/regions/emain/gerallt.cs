using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GeralltScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_gerallt");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 40, eyeColor: 139, lip: 38);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xFAB15C, 0x74C791, 0xFFDF8B);
		EquipItem(Pocket.Hair, 0x177A, 0x632841, 0x632841, 0x632841);
		EquipItem(Pocket.Armor, 0x3B2F, 0x844E4B, 0x243E4B, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x42B4, 0x303030, 0x3D3A3A, 0x808080);
		EquipItem(Pocket.Head, 0x4716, 0x616161, 0x5F5F5F, 0x808080);
		EquipItem(Pocket.RightHand2, 0x4295, 0x2F5256, 0x886654, 0x808080);

		SetLocation(region: 52, x: 30682, y: 43729);

		SetDirection(126);
		SetStand("chapter4/human/anim/cutscene3/noble_boo03");
        
		Phrases.Add("(He grumbles to himself.)");
		Phrases.Add("Harumph!");
		Phrases.Add("Such terrible service!");
		Phrases.Add("The puppets are all so dirty.");
		Phrases.Add("There's nothing special here. Hmph!");
	}
}
