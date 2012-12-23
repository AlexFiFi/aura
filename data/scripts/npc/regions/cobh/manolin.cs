using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ManolinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_manolin");
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 37, eyeColor: 22, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0x244E72, 0xFFED00, 0x2C5370);
		EquipItem(Pocket.Hair, 0xFF5, 0xC3D0BC, 0xC3D0BC, 0xC3D0BC);
		EquipItem(Pocket.Armor, 0x3BFC, 0xAA5233, 0x101010, 0x501212);
		EquipItem(Pocket.Shoe, 0x435C, 0x47271F, 0x51A26, 0x2A2B2F);
		EquipItem(Pocket.RightHand1, 0x9C6D, 0xAA5233, 0x101010, 0x501212);
		EquipItem(Pocket.RightHand2, 0x9C46, 0xD4D4D4, 0x95BCD2, 0x15C87);

		SetLocation(region: 23, x: 29690, y: 41248);

		SetDirection(4);
		SetStand("human/anim/tool/Bhand_E/uni_tool_Bhand_E01_fishing_waiting_02");
	}
}
