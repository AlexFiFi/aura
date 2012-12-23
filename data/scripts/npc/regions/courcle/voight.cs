using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class VoightScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_voight");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x132E, 0x2C2869, 0xD00031, 0xF58974);
		EquipItem(Pocket.Hair, 0x135B, 0x677276, 0x677276, 0x677276);
		EquipItem(Pocket.Armor, 0x3B8A, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F0, 0x969696, 0xBCBCBC, 0x948C8F);

		SetLocation(region: 3300, x: 251470, y: 183780);

		SetDirection(236);
		SetStand("");
	}
}
