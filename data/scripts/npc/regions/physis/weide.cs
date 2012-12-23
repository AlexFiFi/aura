using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class WeideScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_weide");
		SetRace(29);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0xEDE5D4;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.RightHand1, 0xB3C7, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 287360, y: 222560);

		SetDirection(225);
		SetStand("giant/male/anim/giant_npc_weide");
	}
}
