using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GilmoreScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_gilmore");
		SetRace(10002);
		SetBody(height: 0.8000003f, fat: 0.4f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 7, eyeColor: 76, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1327, 0x719235, 0x496D4A, 0xF2A945);
		EquipItem(Pocket.Hair, 0xFBA, 0x896D43, 0x896D43, 0x896D43);
		EquipItem(Pocket.Armor, 0x3A9B, 0xB6CAAA, 0x584232, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xA68DC3, 0x1B24B);
		EquipItem(Pocket.Head, 0x466C, 0x0, 0xC8C6C4, 0xDFE9A7);

		SetLocation(region: 31, x: 10383, y: 10055);

		SetDirection(224);
		SetStand("human/male/anim/male_natural_stand_npc_gilmore");
	}
}
