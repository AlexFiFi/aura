using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Giantguard_belfast_02Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_giantguard_belfast_02");
		SetRace(8002);
		SetBody(height: 1.6f, fat: 1f, upper: 1.3f, lower: 1.2f);
		SetFace(skin: 19, eye: 45, eyeColor: 26, lip: 26);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x5327, 0xEBE92C, 0xF7775B);
		EquipItem(Pocket.Hair, 0x1F53, 0x99CCCC, 0x99CCCC, 0x99CCCC);
		EquipItem(Pocket.Armor, 0x32FD, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Glove, 0x407D, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Shoe, 0x428C, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Head, 0x485D, 0x524F5D, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand2, 0x9CEC, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.LeftHand2, 0xB3C3, 0x7D4827, 0x37727F, 0x809092);

		SetLocation(region: 4005, x: 23520, y: 28760);

		SetDirection(0);
		SetStand("");
	}
}
